using System;
using System.Collections;
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
        
        public EntityProvider(IDictionary<string, EntityContext> entities)
        {
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
        }
        
        public EntityContext this[string entity] => GetEntityContext(entity);

        public bool ContainsEntity(string entity)
        {
            if (string.IsNullOrEmpty(entity))  throw new ArgumentException("Value cannot be null or empty.", nameof(entity));
            
            return Entities.ContainsKey(entity);
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

        public IEnumerator<EntityContext> GetEnumerator() => Entities.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public int Count => Entities.Count;
    }
}