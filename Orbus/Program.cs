/**********************************************************************************
This coding task aims to explore your programming knowledge from the
perspective of the SOLID principles and how you'd apply them to existing
code. 

We want you to demonstrate how you would improve the code. We would like to see a 
new sample of code clearly annotated with what you have changed and the reasons 
for the change.

It's important to note that this code isn't designed to run in any meaningful
way, so please do not spend time trying to get it to work. We do expect you to
submit code that compiles though.
**********************************************************************************/

using System.Xml.Linq;
using Microsoft.Data.SqlClient;

class Program
{
	async Task Main()
	{
		using var cts = new CancellationTokenSource();
		using var httpClient = new HttpClient();
		var bookFeed = new BookFeed(httpClient);
		var bookStore = new BookStore();
		var booksImport = new BooksImport(bookFeed, bookStore);
		await booksImport.Import(cts.Token);
	}
}

// Question: What is the main problem with the BooksImport class?
public class BooksImport
{
	private readonly IBookFeed _bookFeed;
	private readonly IBookStore _bookStore;

	public BooksImport(
		IBookFeed bookFeed,
		IBookStore bookStore)
	{
		_bookFeed = bookFeed;
		_bookStore = bookStore;
	}
	
	public async Task Import(CancellationToken cancellationToken)
	{
		// download books xml from endpoint
		var books = await _bookFeed.FetchBooks(cancellationToken);

		// save books to the database
		await _bookStore.SaveBooks(books, cancellationToken);
	}
}

public interface IBookStore
{
	Task SaveBooks(IEnumerable<object> books, CancellationToken cancellationToken);
}

public class BookStore : IBookStore
{
	public async Task SaveBooks(IEnumerable<object> books, CancellationToken cancellationToken)
	{
		await using var conn = new SqlConnection("connString");
		await using var comm = conn.CreateCommand();
		await using var trans = conn.BeginTransaction();

		foreach (var b in books)
		{
			// build sql or call sp to insert book - this isn't important
			// for this exercise so please don't spend time trying to
			// write code here.
			await comm.ExecuteNonQueryAsync(cancellationToken);
		}

		trans.Commit();
	}
}

public interface IBookFeed
{
	Task<IEnumerable<object>> FetchBooks(CancellationToken cancellationToken);
}

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
