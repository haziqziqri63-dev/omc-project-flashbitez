namespace omc;

public sealed class frmCartPage : WarmForm
{
    private readonly FlowLayoutPanel rows = new()
    {
        Location = new Point(75, 255),
        Size = new Size(950, 330),
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true,
        BackColor = Cream,
        Padding = new Padding(0, 0, 8, 8)
    };
    private readonly Label total = new() { ForeColor = Forest, Font = new Font("Georgia", 18F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleRight };

    public frmCartPage()
    {
        AutoScroll = true;
        AutoScrollMinSize = new Size(1100, 770);
        AddNav();

        var header = new Panel { Location = new Point(0, 68), Size = new Size(1100, 170), BackColor = Forest, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
        header.Controls.Add(new Label { Text = "YOUR RESCUE BASKET", Location = new Point(0, 38), Size = new Size(1100, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        header.Controls.Add(new Label { Text = "Cart", Location = new Point(0, 65), Size = new Size(1100, 55), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 32F, FontStyle.Italic) });
        header.Controls.Add(new Label { Text = "Review your rescued meals before checkout.", Location = new Point(0, 125), Size = new Size(1100, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
        Controls.Add(header);
        Controls.Add(rows);

        total.Location = new Point(640, 600);
        total.Size = new Size(215, 36);
        Controls.Add(total);
        var checkout = ActionButton("CHECKOUT", (_, _) => Go(new frmCheckoutPage()), 130);
        checkout.Location = new Point(875, 600);
        checkout.Height = 36;
        checkout.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        Controls.Add(checkout);
        Controls.Add(BuildFooter());

        CartService.Instance.CartChanged += CartChanged;
        FormClosed += (_, _) => CartService.Instance.CartChanged -= CartChanged;
        RefreshCart();
    }

    private void CartChanged(object? sender, EventArgs e)
    {
        if (IsDisposed)
            return;
        if (InvokeRequired)
        {
            BeginInvoke(RefreshCart);
            return;
        }
        RefreshCart();
    }

    private void RefreshCart()
    {
        rows.SuspendLayout();
        rows.Controls.Clear();
        var cart = CartService.Instance;
        if (cart.Items.Count == 0)
        {
            var empty = new Panel { Size = new Size(920, 180), BackColor = Cream };
            empty.Controls.Add(new Label { Text = "Your cart is empty", Location = new Point(0, 35), Size = new Size(920, 35), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Forest, Font = new Font("Georgia", 20F, FontStyle.Bold) });
            empty.Controls.Add(new Label { Text = "Rescue a meal from the menu to get started.", Location = new Point(0, 78), Size = new Size(920, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Ink, Font = BodyFont });
            var menu = ActionButton("BACK TO MENU", (_, _) => Go(new frmMenuPage()), 130);
            menu.Location = new Point(395, 120);
            menu.Height = 34;
            menu.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            empty.Controls.Add(menu);
            rows.Controls.Add(empty);
        }
        else
        {
            foreach (var item in cart.Items)
                rows.Controls.Add(BuildRow(item));
        }
        total.Text = $"Total: RM {cart.GetTotal():0.00}";
        rows.ResumeLayout();
    }

    private Panel BuildRow(CartItem item)
    {
        var row = new Panel { Size = new Size(920, 72), BackColor = Cream, Margin = new Padding(0, 0, 0, 10), Padding = new Padding(16, 10, 16, 10) };
        row.Paint += (_, e) =>
        {
            using var border = new Pen(Color.FromArgb(229, 213, 181));
            e.Graphics.DrawRectangle(border, 0, 0, row.Width - 1, row.Height - 1);
        };
        row.Controls.Add(new Label { Text = item.Name, Location = new Point(16, 12), Size = new Size(300, 24), ForeColor = Forest, Font = new Font("Georgia", 13F, FontStyle.Bold) });
        row.Controls.Add(new Label { Text = $"RM {item.Price:0.00} each", Location = new Point(16, 39), AutoSize = true, ForeColor = Ink, Font = new Font("Segoe UI", 8F) });

        var minus = SmallButton("−");
        minus.Location = new Point(525, 18);
        minus.Click += (_, _) => CartService.Instance.UpdateQuantity(item.MenuItemId, item.Quantity - 1);
        row.Controls.Add(minus);
        row.Controls.Add(new Label { Text = item.Quantity.ToString(), Location = new Point(565, 19), Size = new Size(28, 24), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Forest, Font = new Font("Segoe UI", 10F, FontStyle.Bold) });
        var plus = SmallButton("+");
        plus.Location = new Point(605, 18);
        plus.Click += (_, _) => CartService.Instance.UpdateQuantity(item.MenuItemId, item.Quantity + 1);
        row.Controls.Add(plus);
        row.Controls.Add(new Label { Text = $"RM {item.LineTotal:0.00}", Location = new Point(690, 20), Size = new Size(105, 22), TextAlign = ContentAlignment.MiddleRight, ForeColor = Forest, Font = new Font("Segoe UI", 9F, FontStyle.Bold) });
        var remove = SmallButton("REMOVE", 70);
        remove.Location = new Point(815, 18);
        remove.Click += (_, _) => CartService.Instance.RemoveItem(item.MenuItemId);
        row.Controls.Add(remove);
        return row;
    }

    private static Button SmallButton(string text, int width = 32) => new()
    {
        Text = text,
        Width = width,
        Height = 28,
        FlatStyle = FlatStyle.Flat,
        UseVisualStyleBackColor = false,
        BackColor = Forest,
        ForeColor = Color.White,
        Font = new Font("Segoe UI", 8F, FontStyle.Bold),
        TextAlign = ContentAlignment.MiddleCenter,
        Cursor = Cursors.Hand
    };

    private static Panel BuildFooter()
    {
        var footer = new Panel { Location = new Point(0, 700), Size = new Size(1100, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Ink };
        footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
        footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
        footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(810, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
        return footer;
    }
}
