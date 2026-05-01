using System.Diagnostics;

namespace FootballRadar.DataCollector.Kaggle
{
	internal static class KaggleDownloader
	{
		public static void DownloadDataset(string datasetSlug, string outputPath, string apiToken)
		{
			Directory.CreateDirectory(outputPath);

			var psi = new ProcessStartInfo
			{
				FileName = "python",
				Arguments = $"-c \"from kaggle.api.kaggle_api_extended import KaggleApi; api = KaggleApi(); api.authenticate(); api.dataset_download_files('{datasetSlug}', path=r'{outputPath}', unzip=True)\"",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(psi)!;
			process.WaitForExit();
			Console.WriteLine(process.StandardOutput.ReadToEnd());
		}

		static List<TransferRecord> DownloadAndImport(string datasetSlug, string csvFileName, string outputPath, string apiToken)
		{
			DownloadDataset(datasetSlug, outputPath, apiToken);
			var csvPath = Path.Combine(outputPath, csvFileName);
			return CsvImporter.Import(csvPath);
		}
	}
}
