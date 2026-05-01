using System.Globalization;
using System.Text;

namespace FootballRadar.DataCollector.Kaggle
{

    public static class CsvImporter
    {
        private const string Separator = ",";
        public static List<TransferRecord> Import(string filePath)
        {
            var records = new List<TransferRecord>();

            using var reader = new StreamReader(filePath, Encoding.GetEncoding(1252), detectEncodingFromByteOrderMarks: true);

            // Read header
            var headerLine = reader.ReadLine();
            if (headerLine == null)
                return records;

            var headers = headerLine.Split(Separator);

            var map = BuildColumnMap(headers);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var cols = line.Split(Separator);

                try
                {
                    var record = new TransferRecord
                    {
                        PlayerId = ParseInt(cols, map["player_id"]),
                        TransferDate = ParseDate(cols, map["transfer_date"]),
                        TransferSeason = Get(cols, map["transfer_season"]),

                        FromClubId = ParseInt(cols, map["from_club_id"]),
                        ToClubId = ParseInt(cols, map["to_club_id"]),

                        FromClubName = Get(cols, map["from_club_name"]),
                        ToClubName = Get(cols, map["to_club_name"]),

                        TransferFee = ParseDecimal(cols, map["transfer_fee"]),
                        MarketValueInEur = ParseDecimal(cols, map["market_value_in_eur"]),

                        PlayerName = Get(cols, map["player_name"]),
                    };

                    if (record.TransferFee > 0)
                        records.Add(record);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Skipping row: {ex.Message}");
                }
            }

            return records;
        }

        private static Dictionary<string, int> BuildColumnMap(string[] headers)
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);


            for (int i = 0; i < headers.Length; i++)
            {
                map[headers[i].Trim()] = i;
            }

            return map;
        }

        private static string Get(string[] cols, int index)
        {
            if (index < 0 || index >= cols.Length)
                return null!;

            return cols[index].Trim();
        }

        private static int ParseInt(string[] cols, int index)
        {
            var val = Get(cols, index);
            return int.TryParse(val, out var result) ? result : 0;
        }

        private static decimal ParseDecimal(string[] cols, int index)
        {
            var val = Get(cols, index);

            return decimal.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0;
        }

        private static DateTimeOffset ParseDate(string[] cols, int index)
        {
            var val = Get(cols, index);

            var formats = new[]
            {
        "dd.MM.yyyy",
        "yyyy-MM-dd"
    };

            if (DateTime.TryParseExact(
                    val,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                return new DateTimeOffset(date);
            }

            throw new Exception($"Invalid date: {val}");
        }
    }
}