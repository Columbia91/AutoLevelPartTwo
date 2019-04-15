using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Configuration;

namespace AutoLevelPartTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = ConfigurationManager.ConnectionStrings["appConnection"];
            var providerName = configuration.ProviderName;
            var connectionString = configuration.ConnectionString;

            DbProviderFactory providerFactory = DbProviderFactories.GetFactory(providerName);
            using(var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                var dataSet = new DataSet("users");
                DbDataAdapter dataAdapter = providerFactory.CreateDataAdapter();

                var selectUsersCommand = connection.CreateCommand();
                selectUsersCommand.CommandText = "select * from Users";
                dataAdapter.SelectCommand = selectUsersCommand;

                dataAdapter.Fill(dataSet, "Users");

                var commandBuilder = providerFactory.CreateCommandBuilder();
                commandBuilder.DataAdapter = dataAdapter;

                var usersTable = dataSet.Tables["Users"];
                var row = usersTable.Rows[0];
                row.BeginEdit();
                row["Login"] = "superUser";
                row.EndEdit();

                dataAdapter.Update(dataSet, "Users");
            }
        }
    }
}
