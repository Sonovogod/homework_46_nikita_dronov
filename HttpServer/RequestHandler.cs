using System.Net;
using HttpServer.resolvers;
using Encoding = System.Text.Encoding;

namespace HttpServer;

public class RequestHandler
{
    private readonly string _siteDirectory;
    private readonly HttpListenerContext _context;
    private readonly ContentTypeResolver _contentTypeResolver;


    public RequestHandler(
        string siteDirectory,
        HttpListenerContext context,
        ContentTypeResolver contentTypeResolver)
    {
        _siteDirectory = siteDirectory;
        _context = context;
        _contentTypeResolver = contentTypeResolver;
    }

    public void Handle(string content, string? filename)
    {
        try
        {
            byte[] htmlBytes = Encoding.UTF8.GetBytes(content); 
            using Stream stream = new MemoryStream(htmlBytes);
            _context.Response.ContentType = _contentTypeResolver.ResolveContentType(filename);
            _context.Response.ContentLength64 = stream.Length;
            
            byte[] buffer = new byte[64 * 1024];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                _context.Response.OutputStream.Write(buffer, 0, bytesRead);
        }
        catch (Exception ex)
        {
            _context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.WriteLine(ex.Message);
            _context.Response.OutputStream.Close();
        }
    }

}