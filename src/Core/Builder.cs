using System;
using System.Collections.Generic;
using System.Linq;

namespace Schematics.Core
{
    public class EntityBuilder
    {
        public IFeatureCollection EntitySource { get; private set; }
        public string EntityName { get; private set; }
        public PropertyInfo Key { get; private set; }
        public StringComparer PropertyComparer { get; private set; } = StringComparer.OrdinalIgnoreCase;
        public string EntityVersion { get; private set; }
        public string EntityAlias { get; private set; }

        public IReadOnlyCollection<PropertyInfo> Properties => _properties;
        private List<PropertyInfo> _properties;

        public EntityBuilder()
        {
            _properties = new List<PropertyInfo>();
        }


        public EntityBuilder Source(IDataSource source)
        {
            EntitySource = source.Features;
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

            if (type.Kind != TypeKind.Scalar)
            {
                throw new EntityConfigurationException("Id property must be scalar.");
            }
            
            var property = new PropertyInfo(name, type);
            
            _properties.Add(property);
            Key = property;
            
            return this;
        }

        public EntityBuilder Alias(string alias)
        {
            if (string.IsNullOrEmpty(alias)) throw new ArgumentException("Value cannot be null or empty.", nameof(alias));
            
            EntityAlias = alias;
            return this;
        }

        public EntityBuilder Version(string version)
        {
            if (string.IsNullOrEmpty(version)) throw new ArgumentException("Value cannot be null or empty.", nameof(version));

            EntityVersion = version;
            return this;
        }
        
        public EntityBuilder AddProperty(string name, IType type)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (type == null) throw new ArgumentNullException(nameof(type));
            
            var property = new PropertyInfo(name, type);
            
            _properties.Add(property);
            
            return this;
        }

        public EntityContext Build()
        {
            if (string.IsNullOrEmpty(EntityName))
            {
                throw new EntityConfigurationException("Entity name cannot be null or empty.");
            }            
            if (EntitySource == null)
            {
                throw new DataSourceNotDefinedException(EntityName);
            }

            var entity = new Entity
            {
                Name = EntityName,
                Properties = new PropertyInfoCollection(_properties, PropertyComparer),
                Key = Key
            };
            var features = EntitySource;
            
            return new EntityContext(entity, features);
        }

        public static EntityBuilder FromEntity(EntityContext context)
        {
            var entity = context.Metadata;
            return new EntityBuilder
            {
                EntityName = entity.Name,
                EntitySource = context.Features,
                Key = entity.Key,
                _properties = entity.Properties.Values?.ToList()
            };
        }

        public static EntityBuilder FromEntity(EntityBuilder builder)
        {
            return new EntityBuilder
            {
                _properties = builder._properties,
                EntityName = builder.EntityName,
                EntitySource = builder.EntitySource,
                Key = builder.Key,
                PropertyComparer = builder.PropertyComparer
            };
        }
    }

    public class SchemaBuilder
    {
        public IServiceProvider ServiceProvider { get; }
        public IDictionary<string, EntityBuilder> Entities { get; }
        public IEntityComparerProvider ComparerProvider { get; }
        public IDataSource DefaultSource { get; internal set; }
        
        public SchemaBuilder(IServiceProvider serviceProvider, IEntityComparerProvider comparerProvider)
        {
            ServiceProvider = serviceProvider;
            ComparerProvider = comparerProvider;
            Entities = new Dictionary<string, EntityBuilder>(comparerProvider.EntityNameComparer);
        }
    }

    public static class SchemaBuilderExtensions
    {
        public static TSource GetSource<TSource>(this SchemaBuilder builder) where TSource : IDataSource
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var source = builder.ServiceProvider.GetService(typeof(TSource));

            if (source == null)
            {
                throw new DataSourceNotFoundException(typeof(TSource));
            }

            return (TSource) source;
        }
        
        public static SchemaBuilder DefaultSource<TSource>(this SchemaBuilder builder) where TSource : IDataSource
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var source = GetSource<TSource>(builder);
            
            builder.DefaultSource = source;

            return builder;
        }
        

        public static SchemaBuilder Entity(this SchemaBuilder builder, EntityContext entity)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var entityBuilder = EntityBuilder.FromEntity(entity);
            
            builder.Entities.Add(entityBuilder.EntityName, entityBuilder);

            return builder;
        }
        
        public static SchemaBuilder Entity(this SchemaBuilder builder, Func<EntityBuilder, EntityBuilder> configure)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            
            var entityBuilder = configure(new EntityBuilder());

            builder.Entities.Add(entityBuilder.EntityName, entityBuilder); 
            
            return builder;
        }
        
        public static SchemaBuilder AddEntitiesFromSource(this SchemaBuilder builder, IDataSource source)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var hasMetadata = source.Features.TryGetFeature(out IMetadataFeature metadataProvider);
            
            if (hasMetadata)
            {
                var entities = metadataProvider
                    .GetAvailableEntities()
                    .Select(x => metadataProvider.GetEntity(x));
                AddEntities(builder, source.Features, entities);
            }
            
            return builder;
        }

        public static SchemaBuilder AddEntitiesFromSource(this SchemaBuilder builder, IDataSource source, Func<IEntity, bool> filter)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            
            var hasMetadata = source.Features.TryGetFeature(out IMetadataFeature metadataProvider);
            
            if (hasMetadata)
            {
                var entities = metadataProvider
                    .GetAvailableEntities()
                    .Select(x => metadataProvider.GetEntity(x))
                    .Where(filter);
                AddEntities(builder, source.Features, entities);
            }
            
            return builder;
        }

        private static void AddEntities(SchemaBuilder builder, IFeatureCollection features, IEnumerable<IEntity> entities)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (features == null) throw new ArgumentNullException(nameof(features));
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            
            foreach (var entity in entities)
            {
                var ctx = new EntityContext(entity, features);
                Entity(builder, ctx);
            }
        }

        public static IEntityProvider Build(this SchemaBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var dictionary = new Dictionary<string, EntityContext>(builder.ComparerProvider.EntityNameComparer);

            foreach (var pair in builder.Entities)
            {
                var entity = pair.Value;
                var key = entity.EntityAlias ?? pair.Key;
                var value = BuildEntity(builder, entity);

                if (dictionary.ContainsKey(key))
                {
                    throw new DuplicateEntitiesException(key);
                }
                
                dictionary.Add(key, value);
            }

            return new EntityProvider(dictionary);
        }

        private static EntityContext BuildEntity(SchemaBuilder schemaBuilder, EntityBuilder entityBuilder)
        {
            if (entityBuilder.EntitySource == null)
            {
                if (schemaBuilder.DefaultSource == null)
                {
                    throw new DataSourceNotDefinedException(entityBuilder.EntityName);
                }

                entityBuilder.Source(schemaBuilder.DefaultSource);
            }

            if (entityBuilder.PropertyComparer == null)
            {
                var globalComparer = schemaBuilder.ComparerProvider.PropertyNameComparer;

                if (globalComparer != null)
                {
                    entityBuilder.PropertyNameComparer(globalComparer);                    
                }
            }

            return entityBuilder.Build();
        }
    }
}