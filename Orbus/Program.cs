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

using System.Net;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;

class Program
{
	async Task Main()
	{
		using var cts = new CancellationTokenSource();
		var booksImport = new BooksImport();
		await booksImport.Import(cts.Token);
	}
}

// Question: What is the main problem with the BooksImport class?
public class BooksImport
{
	public async Task Import(CancellationToken cancellationToken)
	{
		// download books xml from endpoint
		string xml;
		using (var client = new HttpClient())
		{
			var url = "https://www.w3schools.com/xml/books.xml";
			var response = await client.GetAsync(url, cancellationToken);
			xml = await response.Content.ReadAsStringAsync(cancellationToken);
		}
		
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
		
		// save books to the database
		await using (var conn = new SqlConnection("connString"))
		await using (var comm = conn.CreateCommand())
		await using (var trans = conn.BeginTransaction())
		{
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
}
