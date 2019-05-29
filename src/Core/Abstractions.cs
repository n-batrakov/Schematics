using System;
using System.Collections.Generic;

namespace Schematics.Core
{
    public interface IEntityProvider
    {
        EntityContext this[string entity] { get; }
        bool ContainsEntity(string entity);
    }
    
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
    
    public interface IFeatureCollection : IReadOnlyCollection<IFeature>
    {
        object GetFeature(Type feature);
        bool HasFeature(Type featureType);
    }

    
    public interface IDataSource
    {
        IFeatureCollection Features { get; }
    }
    
    public interface IFeature  {  }

    public interface IEntityComparerProvider
    {
        StringComparer EntityNameComparer { get; }
        StringComparer PropertyNameComparer { get; }
    }
}