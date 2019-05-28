using System;

namespace Schematics.Core
{
    public class EntityBuilder
    {
        public EntityBuilder Source<TSource>() where TSource : IDataSource
        {
            return this;
        }

        public EntityBuilder Version(string version)
        {
            return this;
        }

        public EntityBuilder Name(string entity)
        {
            return this;
        }

        public EntityBuilder WithProperty(string name, IType type)
        {
            return this;
        }

        public EntitySource Build()
        {
            throw new NotImplementedException();
        }
        
        // TODO: Implementation, Extensions
    }
    
    public class EntityProviderBuilder
    {
        public EntityProviderBuilder WithEntity(Action<EntityBuilder> configure)
        {
            return this;
        }

        public IEntityProvider Build()
        {
            throw new NotImplementedException();
        }
        
        // TODO: Implementation
    }
}