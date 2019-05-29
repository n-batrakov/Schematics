using System;

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

    public static class TypeSystem
    {
        public static readonly IType String = new StringType();
        
        public static readonly IType Byte = new NumberType(isInteger:true, 0, 255);
        public static readonly IType Integer = new NumberType(isInteger:true, int.MinValue, int.MaxValue);
        public static readonly IType Float = new NumberType();
        
        public static IType Reference(string entity) => new ReferenceType(entity);
    }

    public class StringType : IType, IEquatable<StringType>
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

        public override string ToString()
        {
            return MaxLength == UnrestrictedMaxLength ? "String" : $"String({MaxLength})";
        }

        public bool Equals(StringType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MaxLength == other.MaxLength;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StringType) obj);
        }

        public override int GetHashCode()
        {
            return MaxLength;
        }

        public static bool operator ==(StringType left, StringType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StringType left, StringType right)
        {
            return !Equals(left, right);
        }
    }

    public class NumberType : IType, IEquatable<NumberType>
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

        public override string ToString()
        {
            var range = $"{MinValue},{MaxValue}";
            return IsInteger ? $"Integer({range})" : $"Float({range})";
        }

        public bool Equals(NumberType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsInteger == other.IsInteger && MaxValue.Equals(other.MaxValue) && MinValue.Equals(other.MinValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((NumberType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsInteger.GetHashCode();
                hashCode = (hashCode * 397) ^ MaxValue.GetHashCode();
                hashCode = (hashCode * 397) ^ MinValue.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(NumberType left, NumberType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NumberType left, NumberType right)
        {
            return !Equals(left, right);
        }
    }

    public class ReferenceType : IType, IEquatable<ReferenceType>
    {
        public TypeKind Kind => TypeKind.Reference;
        
        public string Entity { get; }

        public override string ToString()
        {
            return $"Reference({Entity})";
        }

        public ReferenceType(string entity)
        {
            Entity = entity;
        }

        public bool Equals(ReferenceType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Entity, other.Entity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ReferenceType) obj);
        }

        public override int GetHashCode()
        {
            return (Entity != null ? Entity.GetHashCode() : 0);
        }

        public static bool operator ==(ReferenceType left, ReferenceType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReferenceType left, ReferenceType right)
        {
            return !Equals(left, right);
        }
    }
    
    // TODO: Boolean, DateTime, Uuid, Reference, Union, Collection, Enumeration, etc.
}