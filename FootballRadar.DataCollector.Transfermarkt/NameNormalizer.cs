namespace FootballRadar.DataCollector.Kaggle
{
    using System.Globalization;
    using System.Text;

    namespace FootballRadar.DataCollector.Kaggle
    {
        public static class NameNormalizer
        {
            public static string Normalize(string name)
            {
                if (string.IsNullOrWhiteSpace(name)) return string.Empty;

                var normalized = name.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (var c in normalized)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        sb.Append(c);
                }
                return sb.ToString()
                    .Normalize(NormalizationForm.FormC)
                    .ToLowerInvariant()
                    .Trim();
            }
        }
    }
}
