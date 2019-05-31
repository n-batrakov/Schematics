using System;
using System.Linq;
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
            var entityContext = new EntityContext(new Entity("Test"), NullDataSource.Instance);
            
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
            
            var actual = sut.DefaultSource(NullDataSource.Instance).DefaultSource;
            
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void CanSetDefaultSourceGeneric()
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
        public void CanBuildEmptySchema()
        {
            var sut = DefaultBuilder.Build();
            
            var expected = new EntityContext[0];
            
            var actual = sut.ToArray();
            
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanBuildSchema()
        {
            var ctx = new EntityContext(new Entity("Test"), NullDataSource.Instance);
            var sut = DefaultBuilder.Entity(ctx);
            
            var expected = new[] {ctx};
            
            var actual = sut.Build().ToArray();
            
            Assert.Equal(expected, actual, EntityContextComparer.Instance);
        }

        [Fact]
        public void CanBuildWithDefaultProvider()
        {
            var expected = new[]
            {
                new EntityContext(new Entity("Test"), NullDataSource.Instance)
            };
            
            var sut = DefaultBuilder.DefaultSource(NullDataSource.Instance).Entity(x => x.Name("Test"));
            
            var actual = sut.Build().ToArray();
            
            Assert.Equal(expected, actual, EntityContextComparer.Instance);
        }

        [Fact]
        public void CanBuildWithAlias()
        {
            var expectedContext = new EntityContext(new Entity("Test"), NullDataSource.Instance);

            var sut = DefaultBuilder.Entity(x => x
                .Source(NullDataSource.Instance)
                .Name("Test")
                .Alias("Alias"));
            
            var actual = sut.Build();
            
            Assert.True(actual.ContainsEntity("Alias"), "IEntityProvider does not contain Alias entity.");
            Assert.False(actual.ContainsEntity("Test"), "IEntityProvider still has Test entity.");

            var actualContext = actual["Alias"];
            Assert.Equal(expectedContext, actualContext, EntityContextComparer.Instance);
        }

        [Fact]
        public void CanBuildWithGlobalPropertyComparer()
        {
            var sut = new SchemaBuilder(NullServiceProvider.Instance,
                new EntityComparerProvider(StringComparer.OrdinalIgnoreCase));

            sut.Entity(x => x.Source(NullDataSource.Instance).Name("Test").AddProperty("Id", TypeSystem.Integer));
            
            var actual = sut.Build();

            Assert.True(actual.ContainsEntity("test"));
            Assert.True(actual["test"].Metadata.Properties.ContainsKey("id"));
        }

        [Fact]
        public void ThrowOnDuplicateEntities()
        {
            var context = new EntityContext(new Entity("Test"), NullDataSource.Instance);
            var sut = DefaultBuilder.Entity(context);

            Assert.Throws<DuplicateEntitiesException>(() => sut.Entity(context).Build());
        }
    }
}