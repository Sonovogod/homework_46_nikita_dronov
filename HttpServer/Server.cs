using System.Collections.Specialized;
using System.Net;
using System.Web;
using HttpServer.resolvers;
using HttpServer.services;
using HttpServer.ViewModels;


namespace HttpServer;

public class Server
{
    private readonly Thread _serverThread;
    private readonly string _siteDirectory;
    private readonly HttpListener _listener;
    private readonly int _port;
    private readonly ContentTypeResolver _contentTypeResolver;
    private readonly string _route;
    private HttpListenerContext _context;
    private readonly HtmlBuilder<CatViewModel> _htmlBuilder;
    private readonly FileManager _fileManager;
    private CatManager _catManager;
    private CatViewModel _catViewModel;
    private Cat? _cat;

    public Server(
        string siteDirectory,
        HttpListener listener,
        int port,
        ContentTypeResolver contentTypeResolver, 
        HtmlBuilder<CatViewModel> htmlBuilder, 
        FileManager fileManager)
    {
        _siteDirectory = siteDirectory;
        _listener = listener;
        _port = port;
        _contentTypeResolver = contentTypeResolver;
        _htmlBuilder = htmlBuilder;
        _fileManager = fileManager;
        _route = $"http://localhost:{_port}";
        _siteDirectory = siteDirectory;
        _catManager = new CatManager();
        _catManager.GetJsonCats();
        _catViewModel = new CatViewModel();
        _serverThread = new Thread(Listen);
    }
    
    public void Start()
    {
        _serverThread.Start();
        Console.WriteLine($"Сервер запущен на порту: {_port}");
        Console.WriteLine($"Файлы сайта лежат в папке:{_siteDirectory}");
    }

    private void Listen()
    {
        _listener.Prefixes.Add(_route + "/");
        Console.WriteLine($"Роут на главную страницу: {_route}/index.html");
        _listener.Start();
        while (true)
        {
            try
            {
                _context = _listener.GetContext();
                
                string content;
                string? fileName = _context.Request.Url?.AbsolutePath;
                Console.WriteLine(fileName);
                fileName = fileName?.Trim('/');
                string filePath = !string.IsNullOrEmpty(fileName)? Path.Combine(_siteDirectory, fileName) : string.Empty;
                string redirectUrl = $"{_route}/cat_states.html";
                string? queryAction = _context.Request.QueryString.HasKeys()
                    ? _context.Request.QueryString[0]
                    : string.Empty;

                if (_context.Request.HasEntityBody)
                {
                    var inputStream = _context.Request.InputStream;
                    var reader = new StreamReader(inputStream, _context.Request.ContentEncoding);
                    string data = reader.ReadToEnd();
                    Console.WriteLine(data);
                    NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(data);
                    var catNameQuery = nameValueCollection["cat_name"];

                    if(!string.IsNullOrEmpty(catNameQuery))
                    {
                        _catManager.CreteNewCat(catNameQuery, out bool isCatExist);

                        _cat = _catManager.Cats.Find(cat => 
                            string.Equals(cat?.Name, catNameQuery, StringComparison.InvariantCultureIgnoreCase));

                        if (_cat != null)
                        {
                            _cat.Message = isCatExist
                                ? $"Кот по имени {catNameQuery} уже есть, открываем карточку"
                                : $"Вы завели кота по имени {catNameQuery}";

                            _catViewModel.Cat = _cat;
                        }

                        _context.Response.Redirect(redirectUrl);
                        fileName = "cat_states.html";
                        filePath = Path.Combine(_siteDirectory, fileName);
                    }

                    content = _htmlBuilder.BuildHtml(fileName, filePath, _siteDirectory, _catViewModel);
                }
                else
                {
                    if (!string.IsNullOrEmpty(queryAction))
                    {
                        if (string.Equals("play", queryAction, StringComparison.InvariantCultureIgnoreCase))
                            _cat = _cat.CatState.ToGame(_cat);
                        
                        if (string.Equals("feed", queryAction, StringComparison.InvariantCultureIgnoreCase))
                            _cat = _cat.CatState.ToEat(_cat);

                        if (string.Equals("sleep", queryAction, StringComparison.InvariantCultureIgnoreCase))
                            _cat = _cat.CatState.ToSleep(_cat);
                     
                        _catManager.PushJsonCats(_cat);
                        _catViewModel.Cat = _cat;
                    }
                    
                    content = fileName.Contains("html")
                    ? _htmlBuilder.BuildHtml(fileName, filePath, _siteDirectory, _catViewModel)
                    : _fileManager.GetContent(filePath);
                }
                
                var handler = new RequestHandler(_siteDirectory, _context, _contentTypeResolver);
                handler.Handle(content, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}