namespace Hephaestus.Repository.MongoDB.Configure
{
    public class MongoDbConfig
    {
        public MongoDbConfig()
        {

        }
        public MongoDbConfig(string connectionString, string databaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = databaseName;
        }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
