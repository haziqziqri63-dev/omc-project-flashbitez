namespace omc;

public sealed class ucNavBar : UserControl
{
    private readonly FlowLayoutPanel left = new() { Dock = DockStyle.Fill, WrapContents = false, Padding = new(164, 24, 0, 0) };
    private readonly FlowLayoutPanel right = new() { Dock = DockStyle.Right, AutoSize = false, Width = 285, WrapContents = false, Padding = new(0, 20, 10, 0) };
    private Label? cartBadge;

    public ucNavBar()
    {
        BackColor = WarmForm.Cream;
        Height = 68;
        Padding = new Padding(0, 0, 0, 1);
        Paint += (_, e) =>
        {
            using var accent = new Pen(WarmForm.Tan, 1);
            e.Graphics.DrawLine(accent, 0, Height - 1, Width, Height - 1);
        };
        Controls.Add(right);
        Controls.Add(left);
        AddLogo();
        AddLink("home", () => Open(new frmHomePage()));
        AddLink("menu", () => Open(new frmMenuPage()));
        AddLink("deals", () => Open(new frmDealsPage()));
        AddLink("donate", () => Open(new frmDonatePage()));
        AddLink("history", () => Open(new frmHistoryPage()));
        AddLink("about us", () => Open(new frmAboutUs()));
        AddLink("contact us", () => Open(new frmContactUs()));
        RefreshAccountLinks();
        CartService.Instance.CartChanged += CartChanged;
    }

    private void AddLink(string text, Action action)
    {
        var link = CreateLink(text, 13);
        link.LinkClicked += (_, _) => action();
        left.Controls.Add(link);
    }

    private void AddLogo()
    {
        var logo = new Panel
        {
            Dock = DockStyle.Left,
            Size = new Size(155, 67),
            BackColor = WarmForm.Cream,
            Margin = new Padding(8, 0, 0, 0)
        };
        logo.Controls.Add(new Label { Text = "✦", AutoSize = true, Font = new Font("Georgia", 11F, FontStyle.Bold), ForeColor = WarmForm.Rust, Location = new Point(16, 22) });
        logo.Controls.Add(new Label { Text = "FlashBitez", AutoSize = true, Font = new Font("Georgia", 14F, FontStyle.Regular), ForeColor = WarmForm.Forest, Location = new Point(33, 20) });
        Controls.Add(logo);
        logo.BringToFront();
    }

    private void RefreshAccountLinks()
    {
        right.Controls.Clear();
        AddCartButton();
        if (AppData.CurrentUser is null)
        {
            AddRightButton("LOG IN", () => Open(new frmLogInPage()));
            AddRightButton("SIGN UP", () => Open(new frmSignInPagecs()));
            AddAccountButton();
        }
        else
        {
            AddRightLink($"hi, {AppData.CurrentUser.Name}", () => { });
            AddRightButton("LOG OUT", () => Nav.Logout(FindForm()!));
            AddAccountButton();
        }
    }

    private void AddCartButton()
    {
        var holder = new Panel { Width = 42, Height = 28, Margin = new Padding(0, 0, 10, 0) };
        var cart = new Button
        {
            Text = "🛒",
            Width = 32,
            Height = 28,
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false,
            BackColor = WarmForm.Cream,
            ForeColor = WarmForm.Forest,
            Font = new Font("Segoe UI Emoji", 12F),
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            AccessibleName = "Open cart"
        };
        cart.FlatAppearance.BorderSize = 0;
        cart.Click += (_, _) => Open(new frmCartPage());
        cart.MouseEnter += (_, _) => cart.ForeColor = WarmForm.Rust;
        cart.MouseLeave += (_, _) => cart.ForeColor = WarmForm.Forest;
        holder.Controls.Add(cart);

        cartBadge = new Label
        {
            Location = new Point(22, -5),
            Size = new Size(18, 18),
            BackColor = WarmForm.Rust,
            ForeColor = WarmForm.Cream,
            Font = new Font("Segoe UI", 7F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle
        };
        holder.Controls.Add(cartBadge);
        UpdateCartBadge();
        right.Controls.Add(holder);
    }

    private void CartChanged(object? sender, EventArgs e)
    {
        if (IsDisposed)
            return;
        if (InvokeRequired)
        {
            BeginInvoke(UpdateCartBadge);
            return;
        }
        UpdateCartBadge();
    }

    private void UpdateCartBadge()
    {
        if (cartBadge is null)
            return;
        var count = CartService.Instance.GetItemCount();
        cartBadge.Text = count > 99 ? "99+" : count.ToString();
        cartBadge.Visible = count > 0;
    }

    private void AddRightLink(string text, Action action)
    {
        var link = CreateLink(text, 14);
        link.LinkClicked += (_, _) => action();
        right.Controls.Add(link);
    }

    private static LinkLabel CreateLink(string text, int rightMargin)
    {
        var link = new LinkLabel
        {
            Text = text,
            AutoSize = true,
            LinkColor = WarmForm.Forest,
            ActiveLinkColor = WarmForm.Rust,
            VisitedLinkColor = WarmForm.Forest,
            LinkBehavior = LinkBehavior.HoverUnderline,
            Font = new Font("Georgia", 9F),
            Margin = new Padding(0, 4, rightMargin, 0),
            Cursor = Cursors.Hand,
            BackColor = WarmForm.Cream
        };
        link.MouseEnter += (_, _) => link.ForeColor = WarmForm.Rust;
        link.MouseLeave += (_, _) => link.ForeColor = WarmForm.Forest;
        return link;
    }

    private void AddRightButton(string text, Action action)
    {
        var button = new Button
        {
            Text = text,
            AutoSize = false,
            Width = 70,
            Height = 28,
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false,
            BackColor = WarmForm.Forest,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 7F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 10, 0)
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += (_, _) => action();
        button.MouseEnter += (_, _) => button.BackColor = WarmForm.Rust;
        button.MouseLeave += (_, _) => button.BackColor = WarmForm.Forest;
        right.Controls.Add(button);
    }

    private void AddAccountButton()
    {
        var account = new Button
        {
            Text = "",
            Width = 28,
            Height = 28,
            FlatStyle = FlatStyle.Flat,
            BackColor = WarmForm.Tan,
            ForeColor = WarmForm.Forest,
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 0, 0)
        };
        account.FlatAppearance.BorderSize = 0;
        account.Paint += (_, e) =>
        {
            using var brush = new SolidBrush(WarmForm.Forest);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(brush, 10, 5, 8, 8);
            e.Graphics.FillEllipse(brush, 7, 14, 14, 8);
        };
        account.AccessibleName = "User profile";
        account.Click += (_, _) => ShowProfile();
        right.Controls.Add(account);
    }

    private void ShowProfile()
    {
        if (AppData.CurrentUser is null)
        {
            Open(new frmLogInPage());
            return;
        }

        var user = AppData.CurrentUser;
        Open(new frmUserProfile());
    }

    private void Open(Form target) => Nav.GoTo(target, FindForm()!);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            CartService.Instance.CartChanged -= CartChanged;
        base.Dispose(disposing);
    }
}
