using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Schematics.Core
{
    public interface IEntity
    {
        string Name { get; }
        PropertyInfoCollection Properties { get; }
        IReadOnlyCollection<PropertyInfo> Key { get; }
        
        // TODO: Extensions, Versioning
    }
    
    public class PropertyInfoCollection : ReadOnlyDictionary<string, PropertyInfo>
    {
        public PropertyInfoCollection() : base(new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase))
        {
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
