using System;

namespace Schematics.Core
{
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