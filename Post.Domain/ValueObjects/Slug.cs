using System.Text;
using System.Text.RegularExpressions;

namespace Post.Domain.ValueObjects
{
    public partial class Slug : IEquatable<Slug>
    {
        public string Value { get; }

        private Slug(string value)
        {
            Value = value;
        }

        public static Slug Create(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Slug text cannot be empty.", nameof(text));

            var slug = GenerateSlug(text);
            return new Slug(slug);
        }

        public static string GenerateSlug(string text)
        {
            // Convert to lowercase and trim
            var slugText = text.ToLowerInvariant().Trim();

            // Remove common diacritics by normalizing and replacing
            slugText = RemoveDiacritics(slugText);

            // Remove invalid characters - keep only alphanumeric and hyphens
            slugText = MyRegex().Replace(slugText, "");

            // Replace spaces with hyphens
            slugText = Regex.Replace(slugText, @"\s+", "-");

            // Replace multiple hyphens with single hyphen
            slugText = Regex.Replace(slugText, @"-+", "-");

            // Remove leading/trailing hyphens
            slugText = slugText.Trim('-');

            return slugText;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalizedText)
            {
                var category = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) => Equals(obj as Slug);

        public bool Equals(Slug? other) => other is not null && Value == other.Value;

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(Slug? left, Slug? right) =>
            left is null ? right is null : left.Equals(right);

        public static bool operator !=(Slug? left, Slug? right) =>
            !(left == right);

        [GeneratedRegex(@"[^a-z0-9\s-]")]
        private static partial Regex MyRegex();
    }
}
