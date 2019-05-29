using System;
using Schematics.Core;
using Schematics.UnitTests.Utils;
using Xunit;

namespace Schematics.UnitTests
{
    public class SchemaBuilderTests
    {
        private static SchemaBuilder DefaultBuilder =>
            new SchemaBuilder(NullServiceProvider.Instance, InvariantIgnoreCaseComparerProvider.Instance);
        
        [Fact]
        public void CanAddEntityConfiguration()
        {
            var expected = DefaultBuilder;
            expected.Entities.Add("Test", new EntityBuilder().Name("Test").Source(NullDataSource.Instance));
            
            var actual = DefaultBuilder.Entity(x => x.Name("Test").Source(NullDataSource.Instance));
            
            Assert.Equal(expected, actual, SchemaBuilderComparer.Instance);
        }

        [Fact]
        public void CanAddEntityContext()
        {
            var entityContext = new EntityContext(new Entity("Test"), NullDataSource.Instance.Features);
            
            var expected = DefaultBuilder;
            expected.Entities.Add("Test", EntityBuilder.FromEntity(entityContext));

            var actual = DefaultBuilder.Entity(entityContext);
            
            Assert.Equal(expected, actual, SchemaBuilderComparer.Instance);
        }
        
        [Fact]
        public void CanAddEntitiesFromSource()
        {
            var entity = new Entity("Test");
            var source = new MetadataSource(entity);

            var expected = DefaultBuilder;
            expected.Entities.Add("Test", new EntityBuilder().Name("Test").Source(source));
            
            var actual = DefaultBuilder.AddEntitiesFromSource(source);
            
            
            Assert.Equal(expected, actual, SchemaBuilderComparer.Instance);
        }

        [Fact]
        public void CanAddEntitiesFromSourceWithFilter()
        {
            var entity1 = new Entity("Test1");
            var entity2 = new Entity("Test2");
            var source = new MetadataSource(entity1, entity2);

            var expected = DefaultBuilder;
            expected.Entities.Add("Test1", new EntityBuilder().Name("Test1").Source(source));
            
            var actual = DefaultBuilder.AddEntitiesFromSource(source, x => x.Name != "Test2");
            
            
            Assert.Equal(expected, actual, SchemaBuilderComparer.Instance);
        }

        [Fact]
        public void CanSetDefaultSource()
        {
            var expected = NullDataSource.Instance;
            
            var serviceProvider = new SingleServiceProvider(expected);
            var sut = new SchemaBuilder(serviceProvider, InvariantIgnoreCaseComparerProvider.Instance);
            
            var actual = sut.DefaultSource<NullDataSource>().DefaultSource;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanGetSource()
        {
            var expected = NullDataSource.Instance;

            var serviceProvider = new SingleServiceProvider(expected);
            var sut = new SchemaBuilder(serviceProvider, InvariantIgnoreCaseComparerProvider.Instance);

            var actual = sut.GetSource<NullDataSource>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanBuildSchema()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanBuildWithDefaultProvider()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanBuildWithAlias()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void CanBuildWithGlobalPropertyComparer()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void ThrowOnDuplicateEntities()
        {
            throw new NotImplementedException();
        }
        
        
    }
}