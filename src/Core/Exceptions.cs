using System;

namespace Schematics.Core
{
    public class SchemaException : Exception
    {
        public SchemaException(string message) : base($"An exception occured while constructing schema. {message}")
        {
            
        }
    }

    public class EntityConfigurationException : SchemaException
    {
        public EntityConfigurationException(string message) : base(message)
        {
        }
    }
    
    public class DuplicateFeaturesException : SchemaException
    {
        public DuplicateFeaturesException() : base(
            "Unable to initialize features collection as there are duplicate features. Make sure you not trying to add the same feature twice.")
        {
        }
    }
    
    public class DuplicatePropertiesException : EntityConfigurationException
    {
        public DuplicatePropertiesException(string prop) : base(
            $"One of the entities contains properties with same name ({prop}). Remove duplicate properties names or consider a different StringComparer.")
        {
        }
    }

    public class DuplicateEntitiesException : EntityConfigurationException
    {
        public DuplicateEntitiesException(string entity) : base(
            $"Schema contains entities with same name ({entity}). Remove duplicate entities or consider a different StringComparer.")
        {

        }
    }
    
    public class DataSourceNotDefinedException : EntityConfigurationException
    {
        public DataSourceNotDefinedException(string entity) : base(
            $"No data source is assigned to entity '{entity}'. Configure entity data source through EntityBuilder or use default source in SchemaBuilder.")
        {
        }
    }


    
    
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entity) : base($"Unable to find entity named '{entity}'.")
        {
            
        }        
    }

    public class FeatureNotFoundException : Exception
    {
        public FeatureNotFoundException(Type feature) : base(
            $"Unable to find feature {feature}. Feature is either not registered for entity or not implemented by Source.")
        {
        }
    }

    

    public class DataSourceNotFoundException : Exception
    {
        public DataSourceNotFoundException(Type t) : base($"Unable to find Source {t}. Make sure the Source is registered.")
        {
            
        }
    }
}