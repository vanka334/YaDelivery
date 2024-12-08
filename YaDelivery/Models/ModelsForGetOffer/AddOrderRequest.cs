namespace YaDelivery.Models.ModelsForGetOffer
{
    public class AddOrderRequest
    {
        public BillingInfo billing_info {  get; set; }
        public DestinationRequestNode destination { get; set; }
        public RequestInfo info { get; set; }
        public List<RequestResourceItem> items { get; set; }
        public string last_mile_policy {  get; set; }//time_interval(до двери),  self_pickup(пвз) - только такие выборы доставки
        public List<ResourcePlace> places { get; set; }
        public Contact recipient_info { get; set; }
        public  SourceRequestNode source { get; set; }
    }
}
