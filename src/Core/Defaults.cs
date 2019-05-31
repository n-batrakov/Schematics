using System;

namespace Schematics.Core
{
    public class EntityComparerProvider : IEntityComparerProvider
    {
        public StringComparer EntityNameComparer { get; }
        public StringComparer PropertyNameComparer { get; }

        public EntityComparerProvider(StringComparer stringComparer)
        {
            EntityNameComparer = stringComparer;
            PropertyNameComparer = stringComparer;
        }

        public EntityComparerProvider(StringComparer entityNameComparer, StringComparer propertyNameComparer)
        {
            EntityNameComparer = entityNameComparer;
            PropertyNameComparer = propertyNameComparer;
        }
    }
    
    public class InvariantIgnoreCaseComparerProvider : IEntityComparerProvider
    {
        public static readonly IEntityComparerProvider Instance = new InvariantIgnoreCaseComparerProvider();

        private InvariantIgnoreCaseComparerProvider()
        {
        }
        
        public StringComparer EntityNameComparer => StringComparer.InvariantCultureIgnoreCase;
        public StringComparer PropertyNameComparer => StringComparer.InvariantCultureIgnoreCase;
    }
}