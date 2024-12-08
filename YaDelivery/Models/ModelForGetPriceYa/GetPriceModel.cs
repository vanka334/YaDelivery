namespace YaDelivery.Models.ModelForGetPriceYa
{
    public class GetPriceModel
    {
        public PricingDestinationNode destination { get; set; }
        public PricingSourceNode source { get; set; }
        public string tariff { get; set; }// time_interval(к двери), self_pickup(пвз) - варианты отправки
        public int total_weight { get; set; }
        public List<PricingResourcePlace> places { get; set; }


    }
}
