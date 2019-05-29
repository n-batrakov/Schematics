using System.Linq;
using Schematics.Core;

namespace Schematics.UnitTests.Utils
{
    public class NullDataSource : IDataSource
    {
        public static readonly NullDataSource Instance = new NullDataSource();
        
        public IFeatureCollection Features => new FeatureCollection(Enumerable.Empty<IFeature>());
    }
}