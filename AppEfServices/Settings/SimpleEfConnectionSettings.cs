using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEfServices.Settings
{
    /// <summary>
    /// Sql Server connection authentication type
    /// </summary>
    public enum AuthenticationType
    {
        [System.ComponentModel.Description("Windows authentication")]
        WindowsAuthentication = 0,

        [System.ComponentModel.Description("Server authentication")]
        ServerAuthentication = 1
    }

    /// <summary>
    /// Sql Server connection settings
    /// </summary>
    public class SimpleEfConnectionSettings : IEfConnectionSettings
    {
        private readonly string efDataModelName;

        public SimpleEfConnectionSettings()
        {
            Authentication = AuthenticationType.WindowsAuthentication;
            Server = "";
            Database = "";
            UserId = "";
            Password = "";
            efDataModelName = "";
        }

        public SimpleEfConnectionSettings(AuthenticationType authentication, string server, string database, string efDataModelName)
        {
            Authentication = authentication;
            Server = server;
            Database = database;
            UserId = "";
            Password = "";
            this.efDataModelName = efDataModelName;
        }

        public SimpleEfConnectionSettings(AuthenticationType authentication, string server,
            string database, string efDataModelName, string userId, string password)
            : this(authentication, server, database, efDataModelName)
        {
            UserId = userId;
            Password = password;
        }

        #region Properties

        public string Server { get; set; }
        public string Database { get; set; }
        public AuthenticationType Authentication { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Gets ready to use connection string 
        /// </summary>
        public string ConnectionString
        {
            get
            {
                var sqlBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
                sqlBuilder.DataSource = Server;
                sqlBuilder.InitialCatalog = Database;
                sqlBuilder.MultipleActiveResultSets = true;
                if (Authentication == AuthenticationType.WindowsAuthentication)
                {
                    sqlBuilder.IntegratedSecurity = true;
                }
                else
                {
                    sqlBuilder.UserID = UserId;
                    sqlBuilder.Password = Password;
                }

                return sqlBuilder.ToString();
            }
        }

        public string EfConnectionString
        {
            get
            {
                var entityBuilder = new System.Data.EntityClient.EntityConnectionStringBuilder();
                entityBuilder.Provider = "System.Data.SqlClient";
                entityBuilder.ProviderConnectionString = ConnectionString;
                entityBuilder.Metadata = "res://*/" + efDataModelName + ".csdl|res://*/" + efDataModelName +
                    ".ssdl|res://*/" + efDataModelName +".msl";

                return entityBuilder.ToString();
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Tests database connection availability
        /// </summary>
        public void TestConnection()
        {
            System.Data.SqlClient.SqlConnection objConnection = null;
            try
            {
                // create and open connection
                objConnection = new System.Data.SqlClient.SqlConnection(ConnectionString);
                objConnection.Open();
                // make sure we are logged into the right database
                if (string.Compare(objConnection.Database, Database, true) != 0)
                    throw new Exception(string.Format("Cannot open database requested in login '{0}'.", Database));
            }
            catch (Exception ex)
            {
                throw new Exception("Database connection failed.", ex);
            }
            finally
            {
                // cleanup 
                if (objConnection != null)
                    objConnection.Dispose();
            }
        }

        #endregion
    }
}
