namespace omc;

public sealed class frmUserProfile : WarmForm
{
    public frmUserProfile()
    {
        Text = "FlashBitez | Profile";
        AddNav();
        var header = new Panel { Dock = DockStyle.Top, Height = 145, BackColor = Forest };
        header.Controls.Add(new Label { Text = "YOUR ACCOUNT", Dock = DockStyle.Top, Height = 30, Padding = new Padding(0, 24, 0, 0), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        header.Controls.Add(new Label { Text = "Profile", Dock = DockStyle.Top, Height = 65, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 32F, FontStyle.Bold | FontStyle.Italic) });
        header.Controls.SetChildIndex(header.Controls[1], 0);
        Controls.Add(header);
        header.SendToBack();

        var user = AppData.CurrentUser;
        var card = CardPanel(520, 245);
        card.Location = new Point(290, 245);
        card.Controls.Add(new Label { Text = "Personal details", Location = new Point(28, 24), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 20F, FontStyle.Bold) });
        AddDetail(card, "NAME", user?.Name ?? "Guest", 28, 78);
        AddDetail(card, "EMAIL", user?.Email ?? "Not signed in", 28, 122);
        AddDetail(card, "ROLE", user?.Role ?? "Buyer", 28, 166);
        Controls.Add(card);
        Controls.Add(BuildFooter());
    }

    private static void AddDetail(Panel panel, string label, string value, int x, int y)
    {
        panel.Controls.Add(new Label { Text = label, Location = new Point(x, y), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
        panel.Controls.Add(new Label { Text = value, Location = new Point(x + 115, y - 2), Size = new Size(330, 24), AutoEllipsis = true, ForeColor = Ink, Font = BodyFont });
    }

    private static Panel BuildFooter() => new()
    {
        Location = new Point(0, 645),
        Size = new Size(1100, 70),
        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
        BackColor = Ink,
        Controls = { new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) }, new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) } }
    };
}
