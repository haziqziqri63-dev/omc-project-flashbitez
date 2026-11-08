namespace omc;

public sealed class frmSellerDashboard : WarmForm
{
    private readonly TextBox itemName = new();
    private readonly TextBox price = new();
    private readonly TextBox category = new();
    private readonly TextBox description = new();
    private readonly TextBox imagePath = new();
    private readonly Label status = new() { AutoSize = true, ForeColor = Forest };
    private readonly FlowLayoutPanel orderList = new();

    public frmSellerDashboard()
    {
        Text = "FlashBitez | Seller Dashboard";
        AutoScroll = true;
        AutoScrollMinSize = new Size(0, 900);
        AddNav();

        var header = new Panel { Dock = DockStyle.Top, Height = 145, BackColor = Forest };
        header.Controls.Add(new Label { Text = "KITCHEN PARTNER", Dock = DockStyle.Top, Height = 32, Padding = new Padding(0, 24, 0, 0), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        header.Controls.Add(new Label { Text = "Seller dashboard", Dock = DockStyle.Top, Height = 62, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 31F, FontStyle.Bold | FontStyle.Italic) });
        header.Controls.Add(new Label { Text = "Manage your menu and keep buyers updated.", Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
        header.Controls.SetChildIndex(header.Controls[2], 0);
        header.Controls.SetChildIndex(header.Controls[2], 1);
        Controls.Add(header);
        header.SendToBack();

        var tabs = new TabControl { Location = new Point(55, 235), Size = new Size(990, 510) };
        tabs.TabPages.Add(BuildMenuPage());
        tabs.TabPages.Add(BuildOrdersPage());
        Controls.Add(tabs);
        Controls.Add(BuildFooter());
    }

    private TabPage BuildMenuPage()
    {
        var page = new TabPage("Manage menu") { BackColor = Cream, Padding = new Padding(24) };
        page.Controls.Add(new Label { Text = "Add a food item", Location = new Point(24, 20), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 20F, FontStyle.Bold) });
        AddField(page, "ITEM NAME *", itemName, 24, 62, 420);
        AddField(page, "PRICE (RM) *", price, 24, 125, 190);
        AddField(page, "CATEGORY *", category, 230, 125, 214);
        AddField(page, "DESCRIPTION", description, 24, 188, 420);
        description.Multiline = true;
        description.Height = 64;
        AddField(page, "IMAGE", imagePath, 24, 280, 320);
        var browse = ActionButton("BROWSE", BrowseImage, 90);
        browse.Location = new Point(354, 297);
        browse.Height = 30;
        browse.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        page.Controls.Add(browse);
        var add = ActionButton("ADD ITEM", AddItem, 130);
        add.Location = new Point(24, 365);
        add.Height = 36;
        add.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        page.Controls.Add(add);
        status.Location = new Point(24, 415);
        page.Controls.Add(status);
        return page;
    }

    private TabPage BuildOrdersPage()
    {
        var page = new TabPage("Received orders") { BackColor = Cream, Padding = new Padding(24) };
        page.Controls.Add(new Label { Text = "Orders from buyers", Location = new Point(24, 20), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 20F, FontStyle.Bold) });
        orderList.Location = new Point(24, 65);
        orderList.Size = new Size(920, 390);
        orderList.FlowDirection = FlowDirection.TopDown;
        orderList.WrapContents = false;
        orderList.AutoScroll = true;
        orderList.BackColor = Cream;
        page.Controls.Add(orderList);
        RefreshOrders();
        return page;
    }

    private void RefreshOrders()
    {
        orderList.Controls.Clear();
        if (AppData.Orders.Count == 0)
        {
            orderList.Controls.Add(new Label { Text = "No buyer orders yet.", AutoSize = true, ForeColor = Ink, Font = BodyFont, Margin = new Padding(4, 18, 0, 0) });
            return;
        }

        for (var index = 0; index < AppData.Orders.Count; index++)
            orderList.Controls.Add(BuildOrderRow(AppData.Orders[index], index));
    }

    private Panel BuildOrderRow(Order order, int index)
    {
        var row = CardPanel(875, 132);
        row.Margin = new Padding(0, 0, 0, 12);
        row.Controls.Add(new Label { Text = $"{order.DisplayId}  ·  {order.Status.ToUpperInvariant()}", Location = new Point(16, 12), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        row.Controls.Add(new Label { Text = order.Item, Location = new Point(16, 35), Size = new Size(560, 24), AutoEllipsis = true, ForeColor = Forest, Font = new Font("Georgia", 12F, FontStyle.Bold) });
        row.Controls.Add(new Label { Text = $"Address: {order.DeliveryAddress}\nPayment: {order.PaymentMethod}   ·   Total: RM {order.Total:0.00}", Location = new Point(16, 65), Size = new Size(560, 48), ForeColor = Ink, Font = new Font("Segoe UI", 8F) });
        row.Controls.Add(new Label { Text = $"Phone: {order.Phone}", Location = new Point(600, 38), Size = new Size(240, 22), AutoEllipsis = true, ForeColor = Ink, Font = new Font("Segoe UI", 8F) });
        var statusBox = new ComboBox { Location = new Point(600, 72), Size = new Size(220, 30), DropDownStyle = ComboBoxStyle.DropDownList };
        statusBox.Items.AddRange(["Pending", "Preparing", "Delivered"]);
        statusBox.SelectedItem = statusBox.Items.Contains(order.Status) ? order.Status : "Pending";
        statusBox.SelectedIndexChanged += (_, _) => UpdateOrderStatus(index, statusBox.SelectedItem?.ToString() ?? "Pending");
        row.Controls.Add(statusBox);
        return row;
    }

    private void UpdateOrderStatus(int index, string statusValue)
    {
        if (index < 0 || index >= AppData.Orders.Count || AppData.Orders[index].Status == statusValue)
            return;
        AppData.Orders[index] = AppData.Orders[index] with { Status = statusValue };
        RefreshOrders();
    }

    private void AddItem(object? sender, EventArgs e)
    {
        status.ForeColor = Rust;
        if (string.IsNullOrWhiteSpace(itemName.Text) || string.IsNullOrWhiteSpace(category.Text) || !decimal.TryParse(price.Text, out var itemPrice) || itemPrice <= 0)
        {
            status.Text = "Enter an item name, category, and a valid price.";
            return;
        }

        var sellerName = AppData.CurrentUser?.Name ?? "FlashBitez Kitchen";
        AppData.Dishes.Add(new Dish(itemName.Text.Trim(), sellerName, itemPrice, itemPrice, category.Text.Trim(), description.Text.Trim(), imagePath.Text.Trim()));
        itemName.Clear();
        price.Clear();
        category.Clear();
        description.Clear();
        imagePath.Clear();
        status.ForeColor = Forest;
        status.Text = "Menu item added successfully.";
    }

    private void BrowseImage(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog { Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp|All files|*.*", Title = "Choose food image" };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            imagePath.Text = dialog.FileName;
    }

    private static void AddField(Control parent, string labelText, TextBox input, int x, int y, int width)
    {
        parent.Controls.Add(new Label { Text = labelText, Location = new Point(x, y), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        input.Location = new Point(x, y + 18);
        input.Size = new Size(width, 32);
        input.BorderStyle = BorderStyle.FixedSingle;
        input.Font = BodyFont;
        parent.Controls.Add(input);
    }

    private static Panel BuildFooter() => new()
    {
        Location = new Point(0, 770),
        Size = new Size(1100, 70),
        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
        BackColor = Ink,
        Controls = { new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) }, new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) } }
    };
}
