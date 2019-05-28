using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Schematics.Core
{
    public interface IEntity
    {
        string Name { get; }
        PropertyInfoCollection Properties { get; }
        PropertyInfo Key { get; }
        
        // TODO: Extensions, Versioning
    }
    
    public class PropertyInfoCollection : ReadOnlyDictionary<string, PropertyInfo>
    {
        public PropertyInfoCollection(IEnumerable<PropertyInfo> props, StringComparer comparer) : base(CreateDictionary(props, comparer))
        {
        }

        internal void AddProperty(PropertyInfo property)
        {
            AddPropertyToDictionary(Dictionary, property);
        }

        private static IDictionary<string, PropertyInfo> CreateDictionary(IEnumerable<PropertyInfo> props, StringComparer comparer)
        {
            var dictionary = new Dictionary<string, PropertyInfo>(comparer);

            foreach (var prop in props)
            {
                AddPropertyToDictionary(dictionary, prop);
            }

            return dictionary;
        }

        private static void AddPropertyToDictionary(IDictionary<string, PropertyInfo> dictionary, PropertyInfo prop)
        {
            if (dictionary.ContainsKey(prop.Name))
            {
                throw new DuplicatePropertiesException(prop.Name);
            }
            
            dictionary.Add(prop.Name, prop);
        }
    }
    
    public class PropertyInfo
    {
        public string Name { get; }
        public IType Type { get; }

        public PropertyInfo(string name, IType type)
        {
            Name = name;
            Type = type;
        }
        
        // TODO: Overrides, Extensions
    }
    
    
    
    public class Instance
    {
        public IEntity Entity { get; }
        public PropertyValueCollection Properties { get; }

        public Instance(IEntity entity, PropertyValueCollection properties)
        {
            Entity = entity;
            Properties = properties;
        }
        
        // TODO: Overrides
    }
    public class PropertyValueCollection : ReadOnlyDictionary<string, PropertyValue>
    {
        public PropertyValueCollection() : base(new Dictionary<string, PropertyValue>(StringComparer.OrdinalIgnoreCase))
        {
        }
    }

    public class PropertyValue
    {
        public PropertyInfo Property { get; }
        public object Value { get; }

        public PropertyValue(PropertyInfo property, object value)
        {
            Property = property;
            Value = value;
        }
        
        // TODO: Overrides
    }
}
