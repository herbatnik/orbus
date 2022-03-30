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

namespace Orbus;

class Program
{
	public static async Task Main()
	{
		// if project grows use Microsoft's Host builder and facilitate its dependency injection 
		using var cts = new CancellationTokenSource();
		using var httpClient = new HttpClient();
		var bookFeed = new BookFeed(httpClient);
		var bookStore = new BookStore("connString"); // the connString should come from either env variable or configuration file
		var booksImport = new BooksImporter(bookFeed, bookStore);
		await booksImport.Import(cts.Token);
	}
}
