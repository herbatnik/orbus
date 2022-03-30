using System.Xml.Linq;

namespace Orbus;

public class BookFeed : IBookFeed
{
    private readonly HttpClient _httpClient;

    public BookFeed(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Book>> FetchBooks(CancellationToken cancellationToken)
    {
        var url = "https://www.w3schools.com/xml/books.xml";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        // parse books xml to a books collection
        var document = await XDocument.LoadAsync(
            await response.Content.ReadAsStreamAsync(cancellationToken),
            LoadOptions.None,
            cancellationToken);

        var books = document.Descendants("book")
            .Select(bookXml => new Book
            {
                Category = bookXml.Attribute("category")?.Value,
                Title = bookXml.Element("title")?.Value,
                Authors = bookXml.Elements("author").Select(e => e.Value),
                Price = decimal.TryParse(bookXml.Element("price")?.Value, out var parsedPrice) ? parsedPrice : null,
            });
        return books;
    }
}