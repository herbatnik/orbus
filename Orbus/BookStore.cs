using Microsoft.Data.SqlClient;

namespace Orbus;

public class BookStore : IBookStore
{
    private readonly string _connectionString;

    public BookStore(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task SaveBooks(IEnumerable<Book> books, CancellationToken cancellationToken)
    {
        await using var conn = new SqlConnection(_connectionString);
        await using var comm = conn.CreateCommand();
        await using var trans = conn.BeginTransaction();

        foreach (var book in books)
        {
            // build sql or call sp to insert book - this isn't important
            // for this exercise so please don't spend time trying to
            // write code here.
            await comm.ExecuteNonQueryAsync(cancellationToken);
        }

        trans.Commit();
    }
}