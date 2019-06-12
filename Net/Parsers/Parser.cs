using MySql.Data.MySqlClient;

namespace dotnet
{
    public interface IParser
    {
        void Parse(MySqlConnection mysql_connection);
    }
}
