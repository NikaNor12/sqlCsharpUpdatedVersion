namespace sqlCsharp
{
    public interface IConnectionToSQL : IDisposable
    {
        void Connection();
    }
}