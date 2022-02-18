using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Elasticsearch;
using Hephaestus.Repository.Elasticsearch.Configure;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace Hphaestus.Elasticsearch.tests
{
    public class ElasticDbContextUnitTest
    {
        private readonly ElasticDbContext _dbContext;
        public ElasticDbContextUnitTest()
        {
            var option = new Mock<IOptions<ElasticsearchConfig>>();
            _dbContext = new ElasticDbContext(option.Object);
        }

        [Fact]
        public async void InsertObject_probably_works_coeewctly()
        {
            var testEntity = new TestEntity();
            _dbContext.AddDocument<TestEntity, long>(testEntity);
            await _dbContext.SaveChangesAsync();

        }
    }

    public class TestEntity : Entity
    {

    }
}
