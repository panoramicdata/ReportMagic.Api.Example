using Newtonsoft.Json;
using PanoramicData.ReportMagic.Data;
using PanoramicData.ReportMagic.Data.RemoteSystems;
using System;
using System.IO;
using System.Linq;

namespace ReportMagic.Api.Example
{
	static class Program
	{
		static async System.Threading.Tasks.Task Main(string[] args)
		{
			var reportMagicClientOptions = JsonConvert.DeserializeObject<ReportMagicClientOptions>(
				File.ReadAllText("config.json"),
				new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Error }
				);

			using (var client = new ReportMagicClient(reportMagicClientOptions))
			{
				var schedules = (await client.GetAllAsync<ReportSchedule>().ConfigureAwait(false)).ToList();
				Console.WriteLine($"Found {schedules.Count} schedules.");

				var reportSchedule = await client.CreateAsync(
					new ReportSchedule
					{
						Name = "ReportSchedule1",
						RemoteSystemProtocol = RemoteSystemProtocol.File,
						RemoteSystemInputPath = "/",
						RemoteSystemOutputPath = "/",
						CronSchedule = "0 0 0 * * ?",
						IsEnabled = false,
						DocxOutputIsEnabled = false,
						PdfOutputIsEnabled = false,
						HtmlOutputIsEnabled = false,
						XlsxOutputIsEnabled = false,
						ReportScheduleType = ReportScheduleType.HtmlDocumentJob,
						LastReportBatchJobExecutionResult = ExecutionResult.NeverRun,
						DatabaseOutputIsEnabled = false,
						OutputToMonthlySubFolder = true,
						RenderOption = RenderOption.Default,
						IsDeleted = false
					}
				).ConfigureAwait(false);
				Console.WriteLine($"A new reportSchedule has been created with the id: {reportSchedule.Id}");

				schedules = (await client.GetAllAsync<ReportSchedule>().ConfigureAwait(false)).ToList();
				Console.WriteLine($"Found {schedules.Count} schedules.");
			}
		}
	}
}
