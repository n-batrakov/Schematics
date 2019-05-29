using System.Collections.Generic;
using System.Linq;
using Schematics.Core;

namespace Schematics.UnitTests.Utils
{
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
}