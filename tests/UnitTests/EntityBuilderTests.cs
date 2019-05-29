using System;
using Schematics.Core;
using Schematics.UnitTests.Utils;
using Xunit;

namespace Schematics.UnitTests
{
    public class EntityBuilderTests
    {
        [Fact]
        public void ThrowsOnEntityWithoutName()
        {
            var sut = new EntityBuilder();

            Assert.Throws<EntityConfigurationException>(() => sut.Build());
        }

        [Fact]
        public void ThrowsOnEntityWithoutSource()
        {
            var sut = new EntityBuilder().Name("Test");

            Assert.Throws<DataSourceNotDefinedException>(() => sut.Build());
        }

        [Fact]
        public void CanBuildBasicEntity()
        {
            var expected = new EntityContext(new Entity("Test"), NullDataSource.Instance.Features);
            
            var actual = new EntityBuilder().Name("Test").Source(NullDataSource.Instance).Build();
            
            Assert.Equal(expected, actual, EntityContextComparer.Instance);
        }

        [Fact]
        public void CanBuildEntityWithProperties()
        {
            var expected = CreateContext(new Entity("Test", null, DefaultProps));
            
            var actual = new EntityBuilder()
                .Name("Test")
                .Source(NullDataSource.Instance)
                .AddProperty("Id", TypeSystem.Integer)
                .AddProperty("Name", TypeSystem.String)
                .Build();
            
            Assert.Equal(expected, actual, EntityContextComparer.Instance);
        }
        
        [Fact]
        public void ThrowOnDuplicateProperties()
        {
            var sut = new EntityBuilder()
                .Name("Test")
                .Source(NullDataSource.Instance)
                .AddProperty("Name", TypeSystem.Integer)
                .AddProperty("Name", TypeSystem.String);

            Assert.Throws<DuplicatePropertiesException>(() => sut.Build());
        }

        [Fact]
        public void CanBuildEntityWithKey()
        {
            var expected = CreateContext(new Entity("Test", DefaultProps["Id"], DefaultProps));
            
            var actual = new EntityBuilder()
                .Name("Test")
                .Source(NullDataSource.Instance)
                .Id("Id", TypeSystem.Integer)
                .AddProperty("Name", TypeSystem.String)
                .Build();
            
            Assert.Equal(expected, actual, EntityContextComparer.Instance);
        }

        [Fact]
        public void ThrowWhenKeyDuplicateProperty()
        {
            var sut = new EntityBuilder()
                .Name("Test")
                .Source(NullDataSource.Instance)
                .Id("Id", TypeSystem.Integer)
                .AddProperty("Id", TypeSystem.Integer);
            
            Assert.Throws<DuplicatePropertiesException>(() => sut.Build());
        }

        [Fact]
        public void ThrowOnNonScalarKey()
        {
            Assert.Throws<EntityConfigurationException>(() =>
                new EntityBuilder()
                    .Name("Test")
                    .Source(NullDataSource.Instance)
                    .Id("Id", TypeSystem.Reference("Ref")));
        }

        [Fact]
        public void CanSetAlias()
        {
            var expected = "Alias";
            
            var sut = new EntityBuilder().Alias("Alias");

            var actual = sut.EntityAlias;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanSetVersion()
        {
            var expected = "Version";
            
            var sut = new EntityBuilder().Version("Version");

            var actual = sut.EntityVersion;
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCreateBuilderFromEntityContext()
        {
            var expected = CreateContext(new Entity("Test", DefaultProps["Id"], DefaultProps));

            var actual = EntityBuilder.FromEntity(expected).Build();
            
            Assert.Equal(expected, actual, EntityContextComparer.Instance);
        }

        [Fact]
        public void CanCloneBuilder()
        {
            var expected = new EntityBuilder()
                .Name("Test")
                .Source(NullDataSource.Instance)
                .Id("Id", TypeSystem.Integer)
                .AddProperty("Name", TypeSystem.String)
                .Alias("Alias")
                .Version("Version")
                .PropertyNameComparer(StringComparer.CurrentCulture);

            var actual = EntityBuilder.FromEntity(expected);
            
            Assert.Equal(expected.Build(), actual.Build(), EntityContextComparer.Instance);
        }
        
        
        private static EntityContext CreateContext(IEntity entity)
        {
            return new EntityContext(entity, NullDataSource.Instance.Features);
        }

        private static PropertyInfoCollection DefaultProps =>
            new PropertyInfoCollection(new[]
            {
                new PropertyInfo("Id", TypeSystem.Integer),
                new PropertyInfo("Name", TypeSystem.String)
            }, StringComparer.InvariantCultureIgnoreCase);
    }
}