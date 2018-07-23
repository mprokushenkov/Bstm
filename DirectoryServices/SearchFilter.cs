using System;
using System.Collections.Generic;
using System.Text;
using Bstm.Common;
using JetBrains.Annotations;

namespace Bstm.DirectoryServices
{
    public sealed class SearchFilter : IEquatable<SearchFilter>
    {
        private readonly string attributeName;
        private readonly string attributeValue;
        private readonly FilterType filterType;
        private readonly List<SearchFilter> innerFilters = new List<SearchFilter>();
        private string value;

        private SearchFilter(
            string attributeName,
            string attributeValue,
            FilterType filterType)
        {
            this.attributeName = Guard.CheckNull(attributeName, nameof(attributeName));
            this.attributeValue = Guard.CheckNull(attributeValue, nameof(attributeValue));
            this.filterType = filterType;
        }

        private SearchFilter(FilterType filterType, params SearchFilter[] innerFilters)
        {
            this.filterType = filterType;
            this.innerFilters.AddRange(innerFilters);
        }

        public string Value => value ?? (value = CreateValue());

        [NotNull]
        public static SearchFilter Equality(
            [NotNull] string attributeName,
            [NotNull] string attributeValue)
        {
            return new SearchFilter(attributeName, attributeValue, FilterType.Equality);
        }

        [NotNull]
        public static SearchFilter Negation(
            [NotNull] string attributeName,
            [NotNull] string attributeValue)
        {
            return new SearchFilter(attributeName, attributeValue, FilterType.Negation);
        }

        [NotNull]
        public static SearchFilter Presence([NotNull] string attributeName)
        {
            return new SearchFilter(attributeName, "*", FilterType.Presence);
        }

        [NotNull]
        public static SearchFilter Absence([NotNull] string attributeName)
        {
            return new SearchFilter(attributeName, "*", FilterType.Absence);
        }

        [NotNull]
        public static SearchFilter GreaterThanOrEqual( 
            [NotNull] string attributeName,
            [NotNull] string attributeValue)
        {
            return new SearchFilter(attributeName, attributeValue, FilterType.GreaterThanOrEqual);
        }

        [NotNull]
        public static SearchFilter LessThanOrEqual(
            [NotNull] string attributeName,
            [NotNull] string attributeValue)
        {
            return new SearchFilter(attributeName, attributeValue, FilterType.LessThanOrEqual);
        }

        [NotNull]
        public SearchFilter And(
            [NotNull] SearchFilter filter)
        {
            if (filterType != FilterType.And)
            {
                return new SearchFilter(FilterType.And, this, filter);
            }

            innerFilters.Add(filter);
            return this;
        }

        [NotNull]
        public SearchFilter Or(
            [NotNull] SearchFilter filter)
        {
            if (filterType != FilterType.Or)
            {
                return new SearchFilter(FilterType.Or, this, filter);
            }

            innerFilters.Add(filter);
            return this;
        }

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(SearchFilter other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
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

            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((SearchFilter) obj);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }

        public static implicit operator string(SearchFilter component)
        {
            return component.ToString();
        }

        private string CreateValue()
        {
            if (innerFilters.Count == 0)
            {
                var template = filterType == FilterType.Negation || filterType == FilterType.Absence
                    ? "(!({0}))"
                    : "({0})";

                var result = string.Format(template, $"{attributeName}{filterType.Sign}{attributeValue}");

                return result;
            }

            var sb = new StringBuilder();
            innerFilters.ForEach(innerFilter => sb.Append(innerFilter.Value));

            return $"({filterType.Sign}{sb})";
        }

        private class FilterType : Enumeration
        {
            private FilterType(string value) : base(value)
            {
            }

            private FilterType(string value, string sign) : this(value)
            {
                Sign = sign;
            }

            // ReSharper disable MemberHidesStaticFromOuterClass
            public static FilterType Equality { get; } = new FilterType("Equality", "=");

            public static FilterType Negation { get; } = new FilterType("Negation", "=");

            public static FilterType Presence { get; } = new FilterType("Presence", "=");

            public static FilterType Absence { get; } = new FilterType("Absence", "=");

            public static FilterType GreaterThanOrEqual { get; } = new FilterType("GreaterThanOrEqual", ">=");

            public static FilterType LessThanOrEqual { get; } = new FilterType("LessThanOrEqual", "<=");

            public static FilterType And { get; } = new FilterType("And", "&");

            public static FilterType Or { get; } = new FilterType("Or", "|");

            public string Sign { get; }
        }
    }
}