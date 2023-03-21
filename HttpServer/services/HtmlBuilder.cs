using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using HttpServer.json;
using HttpServer.ViewModels;
using RazorEngine;
using RazorEngine.Templating;

namespace HttpServer.services;

public class HtmlBuilder<T>
{
    public string BuildHtml(string? fileName, string filePath, string siteDir, T viewModel)
    {
        Console.WriteLine(filePath);
        string layoutPath = Path.Combine(siteDir, "layout.html");
        var razor = Engine.Razor;
        
        if (!razor.IsTemplateCached("layout", typeof(T)))
            razor.AddTemplate("layout", File.ReadAllText(layoutPath));
        
        if (!razor.IsTemplateCached(fileName, typeof(T)))
        {
            razor.AddTemplate(fileName, File.ReadAllText(filePath));
            razor.Compile(fileName, typeof(T));
        }

        var key = razor.GetKey(fileName);

        var html = razor.Run(key.Name, typeof(T), viewModel);

        return html;
    }

    
}