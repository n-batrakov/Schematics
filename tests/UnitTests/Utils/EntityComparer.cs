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
}