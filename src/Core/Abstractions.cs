using System;
using System.Collections.Generic;

namespace Schematics.Core
{
    public interface IEntityProvider : IReadOnlyCollection<EntityContext>
    {
        EntityContext this[string entity] { get; }
        bool ContainsEntity(string entity);
    }
    
    public class EntityContext
    {
        public IEntity Metadata { get; }
        public IDataSource Source { get; }

        public EntityContext(IEntity metadata, IDataSource source)
        {
            Metadata = metadata;
            Source = source;
        }

        public override string ToString()
        {
            return $"{Source.GetType().Name}::{Metadata.Name}";
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