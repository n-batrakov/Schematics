using System.Collections.Generic;

namespace Schematics.Core
{
    public class EntityContext
    {
        public IEntity Metadata { get; }
        public IFeatureCollection Features { get; }

        public EntityContext(IEntity metadata, IFeatureCollection features)
        {
            Metadata = metadata;
            Features = features;
        }
    }
    
    public interface IEntityProvider
    {
        EntityContext this[string entity] { get; }
        bool ContainsEntity(string entity);
    }

    public interface IDataSource
    {
        IFeatureCollection Features { get; }
        IMetadataProvider MetadataProvider { get; }
    }
    
    public interface IFeatureCollection : IReadOnlyCollection<IFeature>
    {
    }
}