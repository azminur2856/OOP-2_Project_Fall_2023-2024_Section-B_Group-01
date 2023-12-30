namespace Pastry_Shop_Management_System
{
    internal class CartItem
    {
        public int SerialNumber { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }

        public CartItem(int serialNumber, string itemId, string itemName, double itemPrice, int quantity)
        {
            this.SerialNumber = serialNumber;
            this.ItemId = itemId;
            this.ItemName = itemName;
            this.ItemPrice = itemPrice;
            this.Quantity = quantity;
            this.TotalPrice = itemPrice * quantity; // Calculate total price based on item price and quantity
        }
    }
}