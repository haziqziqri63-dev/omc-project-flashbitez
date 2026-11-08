namespace omc;

public sealed class frmCheckoutPage : WarmForm
{
    private const decimal DeliveryFee = 4.00m;

    private readonly TextBox street = new();
    private readonly TextBox unit = new();
    private readonly TextBox city = new();
    private readonly TextBox postalCode = new();
    private readonly TextBox phone = new();
    private readonly TextBox cardNumber = new();
    private readonly TextBox expiry = new();
    private readonly TextBox cvv = new();
    private readonly ComboBox bank = new();
    private readonly RadioButton cod = new() { Text = "Cash on delivery (COD)", AutoSize = true, Checked = true };
    private readonly RadioButton card = new() { Text = "Card", AutoSize = true };
    private readonly RadioButton fpx = new() { Text = "FPX online banking", AutoSize = true };
    private readonly Panel cardPanel = new();
    private readonly Panel fpxPanel = new();
    private readonly Label subtotalValue = new();
    private readonly Label deliveryValue = new();
    private readonly Label totalValue = new();
    private readonly Label status = new() { AutoSize = false, Size = new Size(420, 42), ForeColor = Rust };
    private readonly ErrorProvider errors = new();

    public frmCheckoutPage()
    {
        Text = "FlashBitez | Checkout";
        AutoScroll = true;
        AutoScrollMinSize = new Size(1100, 800);
        AddNav();

        var header = new Panel { Dock = DockStyle.Top, Height = 145, BackColor = Forest };
        header.Controls.Add(new Label { Text = "ALMOST THERE", Dock = DockStyle.Top, Height = 28, Padding = new Padding(0, 28, 0, 0), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        header.Controls.Add(new Label { Text = "Checkout", Dock = DockStyle.Top, Height = 65, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 32F, FontStyle.Italic) });
        header.Controls.Add(new Label { Text = "Tell us where to deliver your rescued meals.", Dock = DockStyle.Top, Height = 30, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
        header.Controls.SetChildIndex(header.Controls[2], 0);
        header.Controls.SetChildIndex(header.Controls[2], 1);
        Controls.Add(header);
        header.SendToBack();

        var content = new Panel { Location = new Point(55, 235), Size = new Size(990, 500), BackColor = Cream };
        content.Controls.Add(BuildDetailsCard());
        content.Controls.Add(BuildSummaryCard());
        Controls.Add(content);
        Controls.Add(BuildFooter());

        cod.CheckedChanged += PaymentChanged;
        card.CheckedChanged += PaymentChanged;
        fpx.CheckedChanged += PaymentChanged;
        PaymentChanged(this, EventArgs.Empty);
    }

    private Panel BuildDetailsCard()
    {
        var panel = CardPanel(540, 500);
        panel.Location = new Point(0, 0);
        panel.Controls.Add(new Label { Text = "Delivery details", Location = new Point(24, 20), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 20F, FontStyle.Bold) });
        AddField(panel, "STREET ADDRESS *", street, 24, 62, 468);
        AddField(panel, "UNIT / FLOOR (OPTIONAL)", unit, 24, 125, 225);
        AddField(panel, "CITY *", city, 267, 125, 225);
        AddField(panel, "POSTAL CODE *", postalCode, 24, 188, 225);
        AddField(panel, "PHONE *", phone, 267, 188, 225);

        panel.Controls.Add(new Label { Text = "PAYMENT METHOD", Location = new Point(24, 252), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        cod.Location = new Point(24, 278);
        card.Location = new Point(215, 278);
        fpx.Location = new Point(300, 278);
        panel.Controls.Add(cod);
        panel.Controls.Add(card);
        panel.Controls.Add(fpx);

        cardPanel.Location = new Point(24, 310);
        cardPanel.Size = new Size(468, 105);
        cardPanel.BackColor = Color.Transparent;
        AddField(cardPanel, "CARD NUMBER *", cardNumber, 0, 0, 220);
        AddField(cardPanel, "EXPIRY *", expiry, 230, 0, 105);
        AddField(cardPanel, "CVV *", cvv, 345, 0, 105);
        panel.Controls.Add(cardPanel);

        fpxPanel.Location = new Point(24, 310);
        fpxPanel.Size = new Size(468, 105);
        fpxPanel.BackColor = Color.Transparent;
        AddField(fpxPanel, "BANK *", bank, 0, 0, 300);
        bank.DropDownStyle = ComboBoxStyle.DropDownList;
        bank.Items.AddRange(["Maybank", "CIMB", "Public Bank", "RHB", "Hong Leong", "Bank Islam"]);
        panel.Controls.Add(fpxPanel);

        status.Location = new Point(24, 422);
        panel.Controls.Add(status);
        return panel;
    }

    private Panel BuildSummaryCard()
    {
        var panel = CardPanel(420, 500);
        panel.Location = new Point(570, 0);
        panel.Controls.Add(new Label { Text = "Order summary", Location = new Point(24, 20), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 20F, FontStyle.Bold) });
        var rows = new FlowLayoutPanel { Location = new Point(24, 65), Size = new Size(370, 245), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Color.Transparent };
        foreach (var item in CartService.Instance.Items)
            rows.Controls.Add(new Label { Text = $"{item.Name}  × {item.Quantity}\nRM {item.LineTotal:0.00}", AutoSize = false, Size = new Size(345, 48), ForeColor = Ink, Font = BodyFont, Padding = new Padding(0, 4, 0, 0) });
        if (CartService.Instance.Items.Count == 0)
            rows.Controls.Add(new Label { Text = "Your cart is empty.", AutoSize = true, ForeColor = Rust, Font = BodyFont });
        panel.Controls.Add(rows);

        AddTotalLine(panel, "Subtotal", subtotalValue, 330);
        AddTotalLine(panel, "Delivery fee", deliveryValue, 358);
        AddTotalLine(panel, "Grand total", totalValue, 395, true);

        var place = ActionButton("PLACE ORDER", PlaceOrder, 180);
        place.Name = "placeOrder";
        place.Location = new Point(24, 445);
        place.Height = 36;
        place.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        panel.Controls.Add(place);
        UpdateTotals();
        return panel;
    }

    private void AddField(Control parent, string labelText, Control input, int x, int y, int width)
    {
        parent.Controls.Add(new Label { Text = labelText, Location = new Point(x, y), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 7F, FontStyle.Bold) });
        input.Location = new Point(x, y + 17);
        input.Size = new Size(width, 30);
        input.Font = BodyFont;
        input.BackColor = Color.White;
        if (input is TextBox textBox)
            textBox.BorderStyle = BorderStyle.FixedSingle;
        parent.Controls.Add(input);
    }

    private static void AddTotalLine(Panel panel, string label, Label value, int y, bool emphasized = false)
    {
        panel.Controls.Add(new Label { Text = label, Location = new Point(24, y), AutoSize = true, ForeColor = emphasized ? Forest : Ink, Font = emphasized ? new Font("Segoe UI", 10F, FontStyle.Bold) : BodyFont });
        value.Location = new Point(250, y);
        value.Size = new Size(140, 22);
        value.TextAlign = ContentAlignment.MiddleRight;
        value.ForeColor = Forest;
        value.Font = emphasized ? new Font("Segoe UI", 12F, FontStyle.Bold) : BodyFont;
        panel.Controls.Add(value);
    }

    private void PaymentChanged(object? sender, EventArgs e)
    {
        cardPanel.Visible = card.Checked;
        fpxPanel.Visible = fpx.Checked;
    }

    private void UpdateTotals()
    {
        var subtotal = CartService.Instance.GetTotal();
        subtotalValue.Text = $"RM {subtotal:0.00}";
        deliveryValue.Text = $"RM {(subtotal > 0 ? DeliveryFee : 0):0.00}";
        totalValue.Text = $"RM {(subtotal > 0 ? subtotal + DeliveryFee : 0):0.00}";
    }

    private void PlaceOrder(object? sender, EventArgs e)
    {
        errors.Clear();
        var valid = true;
        valid &= Required(street, "Street address is required.");
        valid &= Required(city, "City is required.");
        valid &= Required(postalCode, "Postal code is required.");
        valid &= Required(phone, "Phone number is required.");
        if (!postalCode.Text.Trim().All(char.IsDigit) || postalCode.Text.Trim().Length < 5)
        {
            errors.SetError(postalCode, "Enter a valid postal code.");
            valid = false;
        }
        if (phone.Text.Trim().Length < 8)
        {
            errors.SetError(phone, "Enter a valid phone number.");
            valid = false;
        }
        if (CartService.Instance.Items.Count == 0)
        {
            status.Text = "Add at least one meal before placing an order.";
            valid = false;
        }
        if (card.Checked)
        {
            valid &= Required(cardNumber, "Card number is required.");
            valid &= Required(expiry, "Expiry is required.");
            valid &= Required(cvv, "CVV is required.");
            if (cardNumber.Text.Count(char.IsDigit) < 12) { errors.SetError(cardNumber, "Enter a valid card number."); valid = false; }
            if (cvv.Text.Trim().Length is < 3 or > 4) { errors.SetError(cvv, "Enter a valid CVV."); valid = false; }
        }
        if (fpx.Checked && bank.SelectedIndex < 0)
        {
            errors.SetError(bank, "Select a bank.");
            valid = false;
        }
        if (!valid)
        {
            status.Text = "Please correct the highlighted fields.";
            return;
        }

        var cart = CartService.Instance;
        var subtotal = cart.GetTotal();
        var paymentMethod = cod.Checked ? "COD" : card.Checked ? "Card" : "FPX";
        var paymentReference = card.Checked ? MaskCardNumber(cardNumber.Text) : fpx.Checked ? bank.Text : string.Empty;
        var itemSummary = string.Join("; ", cart.Items.Select(item => $"{item.Name} x{item.Quantity} (RM {item.LineTotal:0.00})"));
        var address = string.Join(", ", new[] { street.Text.Trim(), unit.Text.Trim(), city.Text.Trim(), postalCode.Text.Trim() }.Where(value => !string.IsNullOrWhiteSpace(value)));
        var sellerEmail = AppData.Users.FirstOrDefault(user => user.Role.Equals("Seller", StringComparison.OrdinalIgnoreCase))?.Email ?? string.Empty;
        AppData.Orders.Add(new Order(DateTime.Now, itemSummary, subtotal + DeliveryFee, AppData.CurrentUser?.Email ?? "guest@flashbitez.local", address, phone.Text.Trim(), paymentMethod, paymentReference, subtotal, DeliveryFee, $"FB-{DateTime.Now:yyyyMMddHHmmssfff}", "Pending", sellerEmail));
        cart.Clear();
        status.ForeColor = Forest;
        status.Text = "Order placed successfully. Your meals are on the way!";
        if (sender is Button button)
            button.Enabled = false;
        MessageBox.Show("Thanks for rescuing a meal! Your order has been placed.", "Order confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private bool Required(Control input, string message)
    {
        if (!string.IsNullOrWhiteSpace(input.Text))
            return true;
        errors.SetError(input, message);
        return false;
    }

    private static string MaskCardNumber(string value)
    {
        var digits = new string(value.Where(char.IsDigit).ToArray());
        return digits.Length <= 4 ? digits : $"•••• {digits[^4..]}";
    }

    private static Panel BuildFooter() => new()
    {
        Location = new Point(0, 755),
        Size = new Size(1100, 70),
        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
        BackColor = Ink,
        Controls = { new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) }, new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) } }
    };
}
