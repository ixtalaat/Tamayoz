using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Tamayoz.Services;

namespace Tamayoz.Controllers;

public class SitemapController(IServiceCatalogService catalog) : Controller
{
    [HttpGet]
    [Route("/sitemap.xml")]
    [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Sitemap()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        var root = new XElement(ns + "urlset");

        // Static Key Pages
        AddUrl(root, ns, $"{baseUrl}/", "1.0", "weekly", DateTime.UtcNow);
        AddUrl(root, ns, $"{baseUrl}/Home/About", "0.8", "monthly", DateTime.UtcNow);
        AddUrl(root, ns, $"{baseUrl}/Services", "0.9", "daily", DateTime.UtcNow);
        AddUrl(root, ns, $"{baseUrl}/Requests/Track", "0.7", "monthly", DateTime.UtcNow);
        AddUrl(root, ns, $"{baseUrl}/Contact", "0.7", "monthly", DateTime.UtcNow);
        AddUrl(root, ns, $"{baseUrl}/Home/Privacy", "0.5", "yearly", DateTime.UtcNow);

        // Dynamic Active Services
        var services = await catalog.GetActiveAsync();
        foreach (var service in services)
        {
            AddUrl(root, ns, $"{baseUrl}/Services/Details/{service.Id}", "0.85", "weekly", service.CreatedAt);
        }

        var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
        return Content(document.ToString(), "application/xml", Encoding.UTF8);
    }

    [HttpGet]
    [Route("/robots.txt")]
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any)]
    public IActionResult Robots()
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var sb = new StringBuilder();
        sb.AppendLine("User-agent: *");
        sb.AppendLine("Allow: /");
        sb.AppendLine("Disallow: /Admin/");
        sb.AppendLine("Disallow: /Identity/");
        sb.AppendLine();
        sb.AppendLine($"Sitemap: {baseUrl}/sitemap.xml");

        return Content(sb.ToString(), "text/plain", Encoding.UTF8);
    }

    private static void AddUrl(XElement root, XNamespace ns, string location, string priority, string changeFreq, DateTime lastMod)
    {
        root.Add(new XElement(ns + "url",
            new XElement(ns + "loc", location),
            new XElement(ns + "lastmod", lastMod.ToString("yyyy-MM-dd")),
            new XElement(ns + "changefreq", changeFreq),
            new XElement(ns + "priority", priority)
        ));
    }
}
