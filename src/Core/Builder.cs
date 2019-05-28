using System;
using System.Collections.Generic;

namespace Schematics.Core
{
    public class EntityBuilder
    {
        private IServiceProvider ServiceProvider { get; }
        
        private IDataSource EntitySource { get; set; }
        private string EntityName { get; set; }
        private PropertyInfo Key { get; set; }
        private StringComparer PropertyComparer { get; set; } = StringComparer.OrdinalIgnoreCase;
        private IList<PropertyInfo> Properties { get; }

        public EntityBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Properties = new List<PropertyInfo>();
        }


        public EntityBuilder Source<TSource>() where TSource : IDataSource
        {
            var source = ServiceProvider.GetService(typeof(TSource));
            EntitySource = source as IDataSource;
            return this;
        }

        public EntityBuilder Name(string entity)
        {
            EntityName = entity;
            return this;
        }

        public EntityBuilder PropertyNameComparer(StringComparer comparer)
        {
            PropertyComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public EntityBuilder Id(string name, IType type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var property = new PropertyInfo(name, type);
            
            Properties.Add(property);
            Key = property;
            
            return this;
        }
        
        public EntityBuilder AddProperty(string name, IType type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var property = new PropertyInfo(name, type);
            
            Properties.Add(property);
            
            return this;
        }

        public EntityContext Build()
        {
            var entity = new Entity
            {
                Name = EntityName,
                Properties = new PropertyInfoCollection(Properties, PropertyComparer),
                Key = Key
            };
            var features = EntitySource.Features;
            
            return new EntityContext(entity, features);
        }
    }
    
    public class EntityProviderBuilder
    {
        private IServiceProvider ServiceProvider { get; }
        
        private StringComparer EntityComparer { get; set; } = StringComparer.OrdinalIgnoreCase;
        private IList<EntityContext> Entities { get; }

        public EntityProviderBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Entities = new List<EntityContext>();
        }

        public EntityProviderBuilder EntityNameComparer(StringComparer comparer)
        {
            EntityComparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            return this;
        }

        public EntityProviderBuilder AddEntity(Func<EntityBuilder, EntityBuilder> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            
            var builder = configure(new EntityBuilder(ServiceProvider));

            var entityContext = builder.Build();
            
            Entities.Add(entityContext);
            
            return this;
        }

        public IEntityProvider Build()
        {
            return new EntityProvider(Entities, EntityComparer);
        }
    }
}