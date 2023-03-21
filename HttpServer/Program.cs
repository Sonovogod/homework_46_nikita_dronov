using System.Net;
using HttpServer;
using HttpServer.resolvers;
using HttpServer.services;
using HttpServer.ViewModels;

string currentDir = Directory.GetCurrentDirectory();
string pathToSite = currentDir + @"\site";
Server server = new Server(
    pathToSite, 
    new HttpListener(), 
    8000, 
    new ContentTypeResolver(),
    new HtmlBuilder<CatViewModel>(),
    new FileManager());
server.Start();