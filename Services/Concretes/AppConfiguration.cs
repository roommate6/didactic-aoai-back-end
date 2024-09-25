using API.Services.Interfaces;

namespace API.Services.Concretes
{
    public class AppConfiguration : IAppConfiguration
    {
        public required string IoTDeviceName { get; set; }
    }
}