using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace CVRecognizingService.Domain.ValueObjects.VODerivatives
{
    public class FullName : ValueObject
    {
        private static readonly Regex ValidationRegex = new Regex(
        @"^[\p{L}\p{M}\p{N}]{1,100}\z",
        RegexOptions.Singleline | RegexOptions.Compiled);
        public string Value { get; }

        private FullName(string value)
        {
            Value = value;
        }

        public static FullName? Create(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && ValidationRegex.IsMatch(value) ? new FullName(value) : null;
        }

        public override bool Equals(object obj)
        {
            return obj is FullName other &&
                   StringComparer.Ordinal.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(Value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
