namespace Schematics.Core
{
    public enum TypeKind
    {
        Scalar,
        Reference,
        Collection,
        Union,
        Any,
    }

    public interface IType
    {
        TypeKind Kind { get; }
    }

    public class StringType : IType
    {
        private const int UnrestrictedMaxLength = 4000;

        public TypeKind Kind => TypeKind.Scalar;
        public int MaxLength { get; }

        public StringType()
        {
            MaxLength = UnrestrictedMaxLength;
        }
        
        public StringType(int maxLength)
        {
            MaxLength = maxLength;
        }
        
        // TODO: Overrides
    }

    public class NumberType : IType
    {
        public TypeKind Kind => TypeKind.Scalar;
        public bool IsInteger { get; }
        public double MaxValue { get; }
        public double MinValue { get; }

        public NumberType(bool isInteger = false, double maxValue = double.MinValue, double minValue = double.MaxValue)
        {
            IsInteger = isInteger;
            MaxValue = maxValue;
            MinValue = minValue;
        }
        
        // TODO: Factory methods, Overrides
    }
    
    // TODO: Boolean, DateTime, Uuid, Reference, Union, Collection, Enumeration, etc.
}