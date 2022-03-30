using Microsoft.Data.SqlClient;

namespace Orbus;

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