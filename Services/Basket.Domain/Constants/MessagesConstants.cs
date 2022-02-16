namespace Basket.Domain.Constants
{
    //Added for escape the magic string.
    public static class MessagesConstants
    {
        public const string CreatedItem = "Created Item";
        public const string CreationFailed = "Creation Failed";
        public const string Anonymous = "Anonymous";
        public const string CouldNotSentToQueue = "Item Committed To Database But Couldn't Send To Queue";
        public const string StockNotEnough = "Stock Not Enough This Item";
        public const string BasketCannotBeEmpty = "Basket cannot be empty.";
    }
}