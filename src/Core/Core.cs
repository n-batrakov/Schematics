namespace Schematics.Core
{
    public class EntitySource
    {
        public IEntity Metadata { get; }
        public IFeatureCollection Features { get; }

        public EntitySource(IEntity metadata, IFeatureCollection features)
        {
            Metadata = metadata;
            Features = features;
        }
    }
    
    public interface IEntityProvider
    {
        EntitySource this[string entity] { get; }
        bool ContainsEntity(string entity);
    }
}