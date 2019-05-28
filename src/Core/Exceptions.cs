using System;

namespace Schematics.Core
{
    public class SchemaException : Exception
    {
        public SchemaException(string message) : base($"An exception occured while constructing schema. {message}")
        {
            
        }
    }

    public class DuplicatePropertiesException : SchemaException
    {
        public DuplicatePropertiesException(string prop) : base(
            $"One of the entities contains properties with same name ({prop}). Remove duplicate properties names or consider a different StringComparer.")
        {
        }
    }

    public class DuplicateEntitiesException : SchemaException
    {
        public DuplicateEntitiesException(string entity) : base(
            $"Schema contains entities with same name ({entity}). Remove duplicate entities or consider a different StringComparer")
        {

        }
    }


    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entity) : base($"Unable to find entity named '{entity}'")
        {
            
        }        
    }
}