namespace YaDelivery.Models.ModelsForGetOffer
{
    public class RequestResourceItem
    {
        public string article {  get; set; }
        public ItemBillingDetails billing_details { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public string place_barcode { get; set; }//штрихкод короьбки к оторой относится товар
        
    }
}
