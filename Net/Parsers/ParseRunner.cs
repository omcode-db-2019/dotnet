using MySql.Data.MySqlClient;
using System;
using System.Timers;

namespace DotNet.Parsers
{
    public class ParseRunner
    {
        private static object locker = new object();
        private Timer timer;
        private string Connect;
        public event Action<MySqlConnection> StartParse;

        public ParseRunner(IParser[] parsers, Timer timer, Connect connect)
        {
            foreach (var parser in parsers)
                StartParse += parser.Parse;
            Connect = connect.Value;
            this.timer = timer;
        }

        public void Run()
        {
            ParseAll();
            timer.Elapsed += (o, e) => ParseAll();
            timer.Start();
        }

        private void ParseAll()
        {
            lock (locker)
            {
                MySqlConnection mysql_connection = new MySqlConnection(Connect);
                mysql_connection.Open();
                StartParse?.Invoke(mysql_connection);
                mysql_connection.Close();
            }
        }
    }
}
