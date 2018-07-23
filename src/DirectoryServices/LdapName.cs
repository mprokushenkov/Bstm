using System;
using System.Linq;

namespace Bstm.DirectoryServices
{
    public sealed class LdapName : IEquatable<LdapName>
    {
        private const char space = ' ';
        private const char slash = '\\';
        private const string specialCharacters = ",\\#+<>;\"=";

        private readonly string value;
        private string normalizedValue;

        public LdapName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(
                    null,
                    ErrorMessages.LdapName_ArgumentOutOfRangeException_InvalidValue);
            }

            this.value = value;
        }

        public static implicit operator string(LdapName ldapName)
        {
            return ldapName?.ToString();
        }

        public static implicit operator LdapName(string ldapName)
        {
            return new LdapName(ldapName);
        }

        public static bool operator ==(LdapName left, LdapName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LdapName left, LdapName right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return normalizedValue ?? (normalizedValue = Normalize(value));
        }

        public bool Equals(LdapName other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var casted = obj as LdapName;

            return casted != null && Equals(casted);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        private static string Normalize(string value)
        {
            var ar = value.ToCharArray();
            TryEscapeSpecialCharacters(ref ar);
            TryEscapeLeadingSpace(ref ar);
            TryEscapeTrailingSpace(ref ar);

            return new string(ar);
        }

        private static void TryEscapeSpecialCharacters(ref char[] characters)
        {
            if (!characters.Any(c => specialCharacters.Contains(c)))
            {
                return;
            }

            var newCharacters = new char[0];

            for (var i = 0; i < characters.Length; i++)
            {
                var character = characters[i];

                var isSpecialCharacter = specialCharacters.Contains(character);

                var hasNoPrecedingSlash = i == 0 || characters[i - 1] != slash;

                var isEscapeSequence =
                    character == slash
                    && i != characters.Length - 1
                    && specialCharacters.Contains(characters[i + 1]);

                if (isSpecialCharacter && hasNoPrecedingSlash && !isEscapeSequence)
                {
                    Array.Resize(ref newCharacters, newCharacters.Length + 1);
                    newCharacters[newCharacters.Length - 1] = slash;
                }

                Array.Resize(ref newCharacters, newCharacters.Length + 1);
                newCharacters[newCharacters.Length - 1] = character;
            }

            characters = newCharacters;
        }

        private static void TryEscapeLeadingSpace(ref char[] characters)
        {
            if (characters[0] != space)
            {
                return;
            }

            var length = characters.Length;
            var newCharacters = new char[length + 1];
            Array.Copy(characters, 0, newCharacters, 1, length);
            newCharacters[0] = slash;
            characters = newCharacters;
        }

        private static void TryEscapeTrailingSpace(ref char[] characters)
        {
            var length = characters.Length;

            if (characters[length - 1] != space)
            {
                return;
            }

            var newCharacters = new char[length + 1];
            Array.Copy(characters, newCharacters, length - 1);
            newCharacters[length - 1] = slash;
            newCharacters[length] = space;
            characters = newCharacters;
        }
    }
}