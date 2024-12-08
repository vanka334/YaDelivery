namespace YaDelivery.Models.ModelsForGetOffer
{
    public class DestinationRequestNode
    {
        public string type { get; set; } // platform_station, custom_location
        public CustomLocation custom_location { get; set; }
      
        public PlatformStation platform_station { get; set; }
        public TimeIntervalUTC interval { get; set; }

    }
}
