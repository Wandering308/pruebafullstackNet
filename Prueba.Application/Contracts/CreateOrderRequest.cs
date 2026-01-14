using DocumentFormat.OpenXml.Bibliography;

namespace Prueba.WebApi.Contracts
{
    public class CreateOrderRequest{
        public string Customer {get; set;} = string.Empty;
        public string Product {get; set;} = string.Empty;
        public int Quantity {get; set;}
        public double OriginLat {get; set;}
        public double OriginLon {get; set;}
        public double DestinationLat {get; set;}
        public double DestinationLon {get; set;}
    }
    
}

