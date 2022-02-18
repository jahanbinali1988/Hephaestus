namespace Hephaestus.Repository.Elasticsearch.Configure
{
    public class ElasticsearchConfig
    {
        public ElasticsearchConfig()
        {

        }
        public ElasticsearchConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}
