namespace AutoService.WEB.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string UserId { get; set; }

        public BasketItem()
        {
        }

        public BasketItem(int serviceId, string userId)
        {
            ServiceId = serviceId;
            UserId = userId;
        }
    }
}