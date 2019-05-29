using System;

namespace Schematics.Core
{
    public class InvariantIgnoreCaseComparerProvider : IEntityComparerProvider
    {
        public StringComparer EntityNameComparer => StringComparer.InvariantCultureIgnoreCase;
        public StringComparer PropertyNameComparer => StringComparer.InvariantCultureIgnoreCase;
    }
}