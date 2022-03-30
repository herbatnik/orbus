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
class Program
{
	void Main()
	{
		var booksImport = new BooksImport();
		booksImport.Import();
	}
}

// Question: What is the main problem with the BooksImport class?
public class BooksImport
{
	public void Import()
	{
		// download books xml from endpoint
		string xml;
		using (var client = new WebClient())
		{
			var url = "https://www.w3schools.com/xml/books.xml";
			xml = client.DownloadString(url);
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
		using (var conn = new SqlConnection("connString"))
		using (var comm = conn.CreateCommand())
		{
			using (var trans = conn.BeginTransaction())
			{
				foreach (var b in books)
				{
					// build sql or call sp to insert book - this isn't important
					// for this exercise so please don't spend time trying to
					// write code here.
					comm.ExecuteNonQuery();
				}
				
				trans.Commit();
			}
		}
	}
}
