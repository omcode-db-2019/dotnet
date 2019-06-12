using MySql.Data.MySqlClient;

namespace DotNet
{
    public interface IParser
    {
        void Parse(MySqlConnection mysql_connection);
    }
}
