using DebitOrderProcessor.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitOrderProcessor.Services
{
    public class DebitOrderProcessService
    {
        private readonly string _outputDirectory;
        private readonly ILogger<DebitOrderProcessService> _logger;
        private readonly IConfiguration _configuration;
        public DebitOrderProcessService(ILogger<DebitOrderProcessService> logger, IConfiguration configuration)
        {
            _outputDirectory = configuration["OutputDirectory"]; 
            _logger = logger;
            _configuration = configuration;
        }

        public void CreateOutputFiles(List<DebitOrder> debitOrders)
        {
            var groups = debitOrders.GroupBy(x => x.BankName);

            foreach (var group in groups)
            {
                try
                {
                    var fileName = $"{group.Key.ToUpper()}.TXT";
                    var filePath = Path.Combine(_outputDirectory, fileName);

                    var headerRecord = CreateHeaderRecord(group);

                    var detailRecords = group
                        .OrderBy(x => x.Amount)
                        .ThenBy(x => x.AccountHolder.Split(' ').Last())
                        .Select(x => CreateDetailRecord(x))
                        .ToList();

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    File.WriteAllLines(filePath, new[] { headerRecord }.Concat(detailRecords));
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to create file :{group}");
                    continue;
                }
            }
        }

        private string CreateHeaderRecord(IGrouping<string, DebitOrder> group)
        {
            var recordCount = group.Count().ToString("D3");
            var totalValue = group.Sum(x => x.Amount).ToString("0.00").Replace(".", "").PadLeft(10, '0');


            return $"{group.Key.ToUpper().PadRight(16)}{recordCount} {totalValue}0029";
        }

        private string CreateDetailRecord(DebitOrder debitOrder)
        {
            var initials = debitOrder.AccountHolder.Split(' ').First().Substring(0, 1);
            var surname = debitOrder.AccountHolder.Split(' ').Last().Replace(" ", "").PadRight(15);
            var accountNumber = debitOrder.AccountNumber.PadRight(14);
            var accountType = GetAccountType(debitOrder.AccountType).PadRight(3);
            var branch = debitOrder.Branch.PadRight(10);
            var amount = (debitOrder.Amount * 100);
            var date = debitOrder.Date;

            return $"{initials}{surname}{accountNumber}{accountType}{branch}{amount}{date}";
        }

        private string GetAccountType(string accountType)
        {
            switch (accountType.ToUpper())
            {
                case "CH":
                    return "CH";
                case "SAV":
                    return "SAV";
                case "CR":
                    return "CC";
                default:
                    return "OTH";
            }
        }
    }
}
