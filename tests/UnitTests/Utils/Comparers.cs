using System.Collections.Generic;
using System.Linq;
using Schematics.Core;

namespace Schematics.UnitTests.Utils
{
    public class EntityComparer : IEqualityComparer<IEntity>
    {
        public static readonly IEqualityComparer<IEntity> Instance = new EntityComparer();

        private EntityComparer()
        {
                
        }
            
        public bool Equals(IEntity a, IEntity b)
        {
            if (a.Equals(b))
            {
                return true;
            }

            if (!a.Name.Equals(b.Name))
            {
                return false;
            }

            if (a.Key != null && !a.Key.Equals(b.Key))
            {
                return false;
            }

            if (a.Properties.Count != b.Properties.Count || a.Properties.Any(NotContainedByOther))
            {
                return false;
            }

            return true;

            bool NotContainedByOther(KeyValuePair<string, PropertyInfo> pair) =>
                !(b.Properties.TryGetValue(pair.Key, out var bValue) && bValue.Equals(pair.Value));
        }

        public int GetHashCode(IEntity obj)
        {
            return obj.GetHashCode();
        }
    }
    
    public class EntityContextComparer : IEqualityComparer<EntityContext>
    {
        public static readonly IEqualityComparer<EntityContext> Instance = new EntityContextComparer();

        private EntityContextComparer()
        {
        }
            
        public bool Equals(EntityContext a, EntityContext b)
        {
            if (a.Equals(b))
            {
                return true;
            }

            if (a.Features.Equals(b.Features) && a.Metadata.Equals(b.Metadata))
            {
                return true;
            }
            
            if (!a.Features.Equals(b.Features))
            {
                if (a.Features.Count != b.Features.Count || a.Features.Any(x => !b.Features.Contains(x)))
                {
                    return false;                    
                }
                
            }

            return EntityComparer.Instance.Equals(a.Metadata, b.Metadata);
        }

        public int GetHashCode(EntityContext obj)
        {
            return obj.GetHashCode();
        }
    }
    
    public class SchemaBuilderComparer : IEqualityComparer<SchemaBuilder>
    {
        public static readonly SchemaBuilderComparer Instance = new SchemaBuilderComparer();

        private SchemaBuilderComparer()
        {
            
        }
        
        public bool Equals(SchemaBuilder x, SchemaBuilder y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            var serviceProviderEqual = Equals(x.ServiceProvider, y.ServiceProvider);
            var entitiesEqual = EqualEntities(x.Entities, y.Entities);
            var comparerProviderEqual = Equals(x.ComparerProvider, y.ComparerProvider);
            var defaultSourceEqual = Equals(x.DefaultSource, y.DefaultSource);

            return serviceProviderEqual & entitiesEqual & comparerProviderEqual & defaultSourceEqual;
        }

        private static bool EqualEntities(IDictionary<string, EntityBuilder> x, IDictionary<string, EntityBuilder> y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x.Count != y.Count) return false;

            foreach (var (key, xValue) in x)
            {
                if (y.TryGetValue(key, out var yValue) && EntityBuilderComparer.Instance.Equals(xValue, yValue))
                {
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(SchemaBuilder obj)
        {
            unchecked
            {
                var hashCode = (obj.ServiceProvider != null ? obj.ServiceProvider.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Entities != null ? obj.Entities.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.ComparerProvider != null ? obj.ComparerProvider.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.DefaultSource != null ? obj.DefaultSource.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
    
    public class EntityBuilderComparer : IEqualityComparer<EntityBuilder>
    {
        public static readonly EntityBuilderComparer Instance = new EntityBuilderComparer();

        private EntityBuilderComparer()
        {
            
        }
        
        public bool Equals(EntityBuilder x, EntityBuilder y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;

            var propsEqual = x.Properties.SequenceEqual(y.Properties);
            var sourceEqual = Equals(x.EntitySource, y.EntitySource);
            var keyEqual = Equals(x.Key, y.Key);
            var comparerEqual = Equals(x.PropertyComparer, y.PropertyComparer);
            var nameEqual = string.Equals(x.EntityName, y.EntityName);
            var versionEqual = string.Equals(x.EntityVersion, y.EntityVersion);
            var aliasEqual = string.Equals(x.EntityAlias, y.EntityAlias);

            return propsEqual && sourceEqual && keyEqual && comparerEqual && nameEqual && versionEqual && aliasEqual;
        }

        public int GetHashCode(EntityBuilder obj)
        {
            unchecked
            {
                var hashCode = (obj.Properties != null ? obj.Properties.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.EntitySource != null ? obj.EntitySource.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.EntityName != null ? obj.EntityName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.Key != null ? obj.Key.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.PropertyComparer != null ? obj.PropertyComparer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.EntityVersion != null ? obj.EntityVersion.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (obj.EntityAlias != null ? obj.EntityAlias.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}