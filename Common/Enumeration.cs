using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bstm.Common
{
    public class Enumeration : IEquatable<Enumeration>
    {
        public Enumeration(string name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentOutOfRangeException() : name;
        }

        public string Name { get; }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return (from property in properties select property.GetValue(null)).OfType<T>();
        }

        public static T FromName<T>(string name) where T : Enumeration
        {
            var result = GetAll<T>().FirstOrDefault(c => c.Name == name);

            if (result != null)
            {
                return result;
            }

            var message = $"'{(!string.IsNullOrEmpty(name) ? name : "Null or white space string")}' is not a valid name in {typeof(T)}.";
            throw new ArgumentOutOfRangeException(null, message);
        }

        public sealed override string ToString()
        {
            return Name;
        }

        public sealed override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public sealed override bool Equals(object obj)
        {
            return Equals(obj as Enumeration);
        }

        public bool Equals(Enumeration other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Name?.Equals(other.Name) ?? false;
        }

        public static implicit operator string(Enumeration value)
        {
            return value?.ToString();
        }
    }
}