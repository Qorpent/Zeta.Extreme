using System.Data;
using Qorpent.Applications;
using Qorpent.Data;
using Qorpent.Data.Connections;
using Qorpent.IoC;
using Zeta.Extreme.Form.DbfsAttachmentSource;
using Zeta.Extreme.MongoDB.Integration;

namespace DbfsToMongo.Connector {

    /// <summary>
    /// Connector to establysh connection between DBFS and MongoDB
    /// </summary>
    public class DbfsToMongoConnector {
        private const string DBFS_DEFAULT_CONNECTION_NAME = "_dbfs_test";
        private const string DBFS_DEFAULT_CONNECTION_STRING = "Data Source=192.168.26.137;Initial Catalog=dbfs;Persist Security Info=True;User ID=sfo_home;Password=rhfcysq$0;Min Pool Size=5;Application Name=local-debug";

        /// <summary>
        /// DBFS connector
        /// </summary>
        public DbfsAttachmentStorage Dbfs;

        /// <summary>
        /// MongoDB Connector
        /// </summary>
        public MongoDbAttachmentSource MongoDb;

        /// <summary>
        /// 
        /// </summary>
        public DbfsToMongoConnector() {
            MongoDb = new MongoDbAttachmentSource();
            Dbfs = new DbfsAttachmentStorage {
                ConnectionName = DBFS_DEFAULT_CONNECTION_NAME
            };

            if (null == Application.Current.DatabaseConnections) {
                Application.Current.Container.Register(
                    new BasicComponentDefinition {
                        Lifestyle = Lifestyle.Singleton,
                        ImplementationType = typeof(DatabaseConnectionProvider),
                        ServiceType = typeof(IDatabaseConnectionProvider)
                    }
                );
            }

            if (
                !Application.Current.DatabaseConnections.Exists(
                    DBFS_DEFAULT_CONNECTION_NAME
                )
            ) {
                Application.Current.DatabaseConnections.Register(
                    new ConnectionDescriptor {
                        ConnectionString = DBFS_DEFAULT_CONNECTION_STRING,
                        Name = DBFS_DEFAULT_CONNECTION_NAME
                    },
                    false
                );
            }
        }

        private string GetConnectionName() {
            return Dbfs.ConnectionName ?? "Default";
        }

        private string GetDatabaseName() {
            if (
                string.IsNullOrWhiteSpace(Dbfs.DatabaseName) && "Default" == GetConnectionName()
            ) {
                return "dbfs";
            }

            return Dbfs.DatabaseName ?? "dbfs";
        }

        /// <summary>
        /// Openin connection to DBFS
        /// </summary>
        /// <returns></returns>
        public IDbConnection OpenConnection() {
            var connection = Application.Current.DatabaseConnections.GetConnection(
                GetConnectionName()
            );

            connection.Open();
            connection.ChangeDatabase(GetDatabaseName());

            return connection;
        }
    }
}