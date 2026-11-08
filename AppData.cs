namespace omc;

public sealed record Dish(
    string Name,
    string Restaurant,
    decimal Price,
    decimal DealPrice,
    string Category,
    string Description = "",
    string ImagePath = "");

public sealed record User(string Name, string Email, string Password, string Role = "Buyer");
public sealed record Order(
    DateTime Date,
    string Item,
    decimal Total,
    string Email,
    string DeliveryAddress = "",
    string Phone = "",
    string PaymentMethod = "",
    string PaymentReference = "",
    decimal Subtotal = 0,
    decimal DeliveryFee = 0,
    string OrderId = "",
    string Status = "Pending",
    string SellerEmail = "")
{
    public string DisplayId => string.IsNullOrWhiteSpace(OrderId)
        ? $"FB-{Date:yyyyMMddHHmmss}"
        : OrderId;
}

public static class AppData
{
    public const string BusinessName = "FlashBitez";
    public const string BusinessEmail = "hello@flashbitez.com";
    public const string BusinessPhone = "+60 12-345 6789";
    public const string BusinessAddress = "Kuala Lumpur, Malaysia";

    public static List<Dish> Dishes { get; } =
    [
        new("Garden Harvest Salad", "The Green Table", 18.00m, 13.50m, "Salads"),
        new("Smoky Teriyaki Bowl", "Bowl & Co.", 22.00m, 16.50m, "Bowls"),
        new("Herb Roasted Chicken", "The Daily Plate", 25.00m, 18.75m, "Mains"),
        new("Creamy Mushroom Pasta", "Little Spoon", 21.00m, 15.75m, "Mains"),
        new("Coconut Chia Pudding", "Sweet Roots", 12.00m, 9.00m, "Sweets"),
        new("Mango Sticky Rice", "Sunset Kitchen", 14.00m, 10.50m, "Sweets")
    ];

    public static List<User> Users { get; } =
    [
        new("Demo User", "demo@flashbitez.com", "flash123", "Buyer"),
        new("Demo Seller", "seller@flashbitez.com", "seller123", "Seller")
    ];

    public static List<Order> Orders { get; } = [];
    public static User? CurrentUser { get; set; }

    // TODO: Replace these in-memory collections with database repositories.
    // TODO: Load dishes, users, and orders from the database during application startup.
}
