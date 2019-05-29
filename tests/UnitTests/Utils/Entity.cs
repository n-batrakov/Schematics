using System;
using System.Linq;
using Schematics.Core;

namespace Schematics.UnitTests.Utils
{
    public class Entity : IEntity
    {
        public string Name { get; set; }
        public PropertyInfo Key { get; set; }
        public PropertyInfoCollection Properties { get; set; }

        public Entity(string name)
        {
            Name = name;
            Key = null;
            Properties = new PropertyInfoCollection(Enumerable.Empty<PropertyInfo>(),
                StringComparer.InvariantCultureIgnoreCase);
        }

        public Entity(string name, PropertyInfo key, PropertyInfoCollection properties)
        {
            Name = name;
            Key = key;
            Properties = properties;
        }
    }
}