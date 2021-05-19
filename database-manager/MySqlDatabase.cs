using System;
using MySqlConnector; // https://github.com/mysql-net/MySqlConnector
using System.Threading.Tasks;

namespace MySqlLib
{
    public class MySqlDatabase
    {
        private string _mySqlString;
        private MySqlConnection _connection;
        public string MySqlString { get => _mySqlString; set => _mySqlString = value; }

        public MySqlDatabase()
        {
            _connection = null;
            _mySqlString = "server=localhost;database=redm;userid=root;";
        }

        public async Task<bool> Connect()
        {
            _connection = new MySqlConnection(MySqlString);

            try
            {
                await _connection.OpenAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("");
                return false;
                throw;
            }
        }

        public async void Disconnect()
        {
            await _connection.DisposeAsync();
        }

        //TODO: Find a better way to insert data 
        // https://dev.mysql.com/doc/dev/connector-net/8.0/html/P_MySql_Data_MySqlClient_MySqlCommand_Parameters.htm
        // https://stackoverflow.com/questions/20492019/update-statement-in-mysql-using-c-sharp

        public async void InsertData(string table, string[] rows, string[] values)
        {
            if (rows.Length != values.Length) return;

            MySqlParameter[] myParamArray = new MySqlParameter[rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                myParamArray[i] = new MySqlParameter(rows[i], values[i]);
            }

            await InsertData(table, myParamArray);
        }

        private async Task InsertData(string table, MySqlParameter[] myParamArray)
        {
            //TODO: This should be a function that encapsulates data into tuples with the ( T1, T2, Tn.. )
            // transform this into a UTIL and make a library of utilities;
            string c = "(";
            string v = "(";
            
            for (int i = 0; i < myParamArray.Length; i++)
            {
                c += myParamArray[i].ParameterName + (i == myParamArray.Length - 1 ? ")" : ", ");
                v += $"@{myParamArray[i].ParameterName}" + (i == myParamArray.Length - 1 ? ")" : ", ");
            }

            string cmd = "INSERT INTO " + table + c + " VALUES" + v;
            MySqlCommand command = new MySqlCommand(cmd, _connection);

            for (int i = 0; i < myParamArray.Length; i++)
            {
                command.Parameters.Add(myParamArray[i]);
            }

            await command.ExecuteNonQueryAsync();
        }

        //TODO: REMOVE, UPDATE, SELECT METHODS 

        // rename to more gyneric thing?
        public async Task<int> RowCount(string table, string row, string value)
        {
            string cmd = $"SELECT COUNT(*) from {table} WHERE {row} = @value";
            MySqlCommand command = new MySqlCommand(cmd, _connection);
            command.Parameters.AddWithValue("@value", value);
            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

    }
}
