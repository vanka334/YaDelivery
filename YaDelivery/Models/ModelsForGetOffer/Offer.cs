namespace YaDelivery.Models.ModelsForGetOffer
{
    public class Offer
    {

        public object expires_at { get; set; } // string or integer
        public OfferDetails offer_details { get; set; }
        public string offer_id { get; set; }
        public string pricing { get; set; } // Example: 192.15 RUB
    }
}
