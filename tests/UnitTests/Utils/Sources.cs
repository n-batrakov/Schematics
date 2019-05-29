using System.Collections.Generic;
using System.Linq;
using Schematics.Core;

namespace Schematics.UnitTests.Utils
{
    public class MetadataSource : IDataSource
    {
        public IFeatureCollection Features { get; set; }
        
        public MetadataSource(params IEntity[] entities)
        {
            Features = new FeatureCollection(new []
            {
                new MetadataFeature(entities.ToArray())
            });
        }

        private class MetadataFeature : IMetadataFeature
        {
            private IReadOnlyCollection<string> AvailableEntities { get; }
            private IDictionary<string, IEntity> Entities { get; }

            public MetadataFeature(IReadOnlyCollection<IEntity> entities)
            {
                Entities = entities.ToDictionary(x => x.Name);
                AvailableEntities = entities.Select(x => x.Name).ToArray();
            }
            
            public IEnumerable<string> GetAvailableEntities()
            {
                return AvailableEntities;
            }

            public IEntity GetEntity(string name)
            {
                return Entities[name];
            }
        }
    }
    
    public class NullDataSource : IDataSource
    {
        public static readonly NullDataSource Instance = new NullDataSource();
        
        public IFeatureCollection Features { get; }

        private NullDataSource()
        {
            Features = new FeatureCollection(Enumerable.Empty<IFeature>());
        }
    }
}