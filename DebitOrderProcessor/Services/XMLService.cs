using DebitOrderProcessor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DebitOrderProcessor.Services
{
   
    public class XmlService
    {
        private readonly ILogger<XmlService> _logger;
        private readonly IConfiguration _configuration;

        public XmlService(ILogger<XmlService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

        }
        public List<DebitOrder> ReadXml()
        {
            var fileName = _configuration["FileName"];
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "DebitOrders", fileName);
            //XmlSerializer serializer = new XmlSerializer(typeof(List<DebitOrder>));
            XmlSerializer serializer = new XmlSerializer(typeof(DebitOrders));
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    DebitOrders debitOrders = (DebitOrders)serializer.Deserialize(stream);
                    List<DebitOrder> deductions = debitOrders.Deductions;
                    return deductions;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing XML file");
                throw;
            }
        }
    }
}
