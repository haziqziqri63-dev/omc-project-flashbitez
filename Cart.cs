using System.ComponentModel;

namespace omc;

public sealed record MenuItem(string Id, string Name, string Description, decimal Price, string ImageUrl)
{
    public static MenuItem FromDish(Dish dish, int index) => new(
        $"dish-{index + 1}",
        dish.Name,
        $"Fresh surplus from {dish.Restaurant}.",
        dish.DealPrice,
        string.Empty);
}

public sealed class CartItem
{
    public string MenuItemId { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Quantity { get; private set; }
    public decimal LineTotal => Price * Quantity;

    public CartItem(MenuItem item, int quantity = 1)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (string.IsNullOrWhiteSpace(item.Id))
            throw new ArgumentException("A menu item must have an ID.", nameof(item));
        if (item.Price < 0)
            throw new ArgumentOutOfRangeException(nameof(item), "A menu item price cannot be negative.");
        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least one.");

        MenuItemId = item.Id;
        Name = item.Name;
        Price = item.Price;
        Quantity = quantity;
    }

    public void SetQuantity(int quantity)
    {
        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least one.");
        Quantity = quantity;
    }
}

public sealed class CartService
{
    private static readonly Lazy<CartService> LazyInstance = new(() => new CartService());
    private readonly BindingList<CartItem> items = [];

    private CartService()
    {
        items.ListChanged += (_, _) => CartChanged?.Invoke(this, EventArgs.Empty);
    }

    public static CartService Instance => LazyInstance.Value;
    public IReadOnlyList<CartItem> Items => items;
    public event EventHandler? CartChanged;

    public void AddItem(MenuItem item, int quantity = 1)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least one.");

        var existing = items.FirstOrDefault(x => x.MenuItemId == item.Id);
        if (existing is null)
            items.Add(new CartItem(item, quantity));
        else
            existing.SetQuantity(existing.Quantity + quantity);

        CartChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool RemoveItem(string menuItemId)
    {
        var item = Find(menuItemId);
        return item is not null && items.Remove(item);
    }

    public bool UpdateQuantity(string menuItemId, int quantity)
    {
        var item = Find(menuItemId);
        if (item is null)
            return false;
        if (quantity < 1)
            return RemoveItem(menuItemId);

        item.SetQuantity(quantity);
        CartChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public void Clear() => items.Clear();
    public decimal GetTotal() => items.Sum(item => item.LineTotal);
    public int GetItemCount() => items.Sum(item => item.Quantity);

    private CartItem? Find(string menuItemId) => string.IsNullOrWhiteSpace(menuItemId)
        ? null
        : items.FirstOrDefault(item => item.MenuItemId == menuItemId);
}
