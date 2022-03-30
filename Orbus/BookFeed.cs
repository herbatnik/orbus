using System.Xml.Linq;

namespace Orbus;

public class BookFeed : IBookFeed
{
    private readonly HttpClient _httpClient;

    public BookFeed(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<object>> FetchBooks(CancellationToken cancellationToken)
    {
        var url = "https://www.w3schools.com/xml/books.xml";
        var response = await _httpClient.GetAsync(url, cancellationToken);
        var xml = await response.Content.ReadAsStringAsync(cancellationToken);

        // parse books xml to a books collection
        var document = XDocument.Parse(xml);
        var books = (from b in document.Descendants("book")
            select new
            {
                Category = b.Attribute("category").Value,
                Title = b.Element("title").Value,
                Authors = b.Elements("author").Select(e => e.Value),
                Price = decimal.Parse(b.Element("price").Value)
            });
        return books;
    }
}