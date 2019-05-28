using System;
using System.Collections.Generic;

namespace Schematics.Core
{
    internal class Entity : IEntity
    {
        public string Name { get; set; }
        public PropertyInfoCollection Properties { get; set; }
        public PropertyInfo Key { get; set; }
    }
    
    internal class EntityProvider : IEntityProvider
    {
        private IDictionary<string, EntityContext> Entities { get; }
        
        public EntityProvider(IEnumerable<EntityContext> contexts, StringComparer comparer)
        {
            if (contexts == null) throw new ArgumentNullException(nameof(contexts));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            
            Entities = CreateDictionary(contexts, comparer);
        }
        
        public EntityContext this[string entity] => GetEntityContext(entity);

        public bool ContainsEntity(string entity)
        {
            if (string.IsNullOrEmpty(entity))  throw new ArgumentException("Value cannot be null or empty.", nameof(entity));
            
            return Entities.ContainsKey(entity);
        }

        internal void AddEntity(EntityContext context)
        {
            AddToDictionary(Entities, context);
        }

        private EntityContext GetEntityContext(string entity)
        {
            if (string.IsNullOrEmpty(entity)) throw new ArgumentException("Value cannot be null or empty.", nameof(entity));
            
            var isEntityExists = Entities.TryGetValue(entity, out var context);

            if (isEntityExists)
            {
                return context;
            }
            else
            {
                throw new EntityNotFoundException(entity);
            }
        }

        private static IDictionary<string, EntityContext> CreateDictionary(
            IEnumerable<EntityContext> contexts,
            StringComparer comparer)
        {
            var entities = new Dictionary<string, EntityContext>(comparer);
            foreach (var entityContext in contexts)
            {
                AddToDictionary(entities, entityContext);
            }

            return entities;
        }

        private static void AddToDictionary(IDictionary<string, EntityContext> dictionary, EntityContext entityContext)
        {
            var entityName = entityContext.Metadata.Name;

            if (dictionary.ContainsKey(entityName))
            {
                throw new DuplicateEntitiesException(entityName);                    
            }
                
            dictionary.Add(entityName, entityContext);
        }
    }
}