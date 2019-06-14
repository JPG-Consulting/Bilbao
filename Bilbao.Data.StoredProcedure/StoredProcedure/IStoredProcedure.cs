namespace Bilbao.Data.StoredProcedure
{
    public interface IStoredProcedure<TResponse, TRequest>
    {
        DbConnection DbConnection { get; }

        DbTransaction DbTransaction { get; }

        TRequest Parameters { get; }

        IEnumerable<TResponse> ExecuteReader();

        void ExecuteNonQuery();

        
    }
}
