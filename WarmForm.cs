namespace omc;

public abstract class WarmForm : Form
{
    public static readonly Color Cream = Color.FromArgb(251, 243, 220);
    public static readonly Color Tan = Color.FromArgb(214, 185, 140);
    public static readonly Color Forest = Color.FromArgb(47, 74, 43);
    public static readonly Color Rust = Color.FromArgb(168, 70, 31);
    public static readonly Color Ink = Color.FromArgb(31, 43, 28);
    public static readonly Font HeadingFont = new("Georgia", 24F, FontStyle.Bold);
    public static readonly Font NavFont = new("Georgia", 10F, FontStyle.Bold);
    public static readonly Font BodyFont = new("Segoe UI", 10F);

    protected WarmForm()
    {
        Text = AppData.BusinessName;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1100, 740);
        MinimumSize = new Size(1100, 740);
        MaximumSize = new Size(1100, 740);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Cream;
        ForeColor = Ink;
        Font = BodyFont;
    }

    protected void AddNav()
    {
        Controls.Add(new ucNavBar { Dock = DockStyle.Top, Height = 68 });
    }

    protected override void OnLoad(EventArgs e)
    {
        ClientSize = new Size(1100, 740);
        MinimumSize = new Size(1100, 740);
        MaximumSize = new Size(1100, 740);
        base.OnLoad(e);
    }

    protected Image? LoadLogo()
    {
        var paths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "logo.png"),
            Path.Combine(Environment.CurrentDirectory, "logo.png")
        };

        foreach (var path in paths.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!File.Exists(path))
                continue;

            using var source = Image.FromFile(path);
            return new Bitmap(source);
        }

        return null;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        Controls.OfType<ucNavBar>().FirstOrDefault()?.BringToFront();
    }

    protected Label Heading(string text, int x = 56, int y = 110, int width = 900)
    {
        return new Label { Text = text, Font = HeadingFont, ForeColor = Forest, Location = new(x, y), Size = new(width, 48) };
    }

    protected Label Body(string text, int x, int y, int width = 700, int height = 60)
    {
        return new Label { Text = text, Font = BodyFont, ForeColor = Ink, Location = new(x, y), Size = new(width, height) };
    }

    protected Button ActionButton(string text, EventHandler click, int width = 160)
    {
        var button = new Button
        {
            Text = text,
            Width = width,
            Height = 42,
            BackColor = Forest,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += click;

        var normalBackColor = button.BackColor;
        var normalForeColor = button.ForeColor;
        var colorsCaptured = false;
        System.Windows.Forms.Timer? transition = null;

        void Animate(Color targetBackColor, Color targetForeColor)
        {
            transition?.Stop();
            var startBackColor = button.BackColor;
            var startForeColor = button.ForeColor;
            var elapsed = 0;
            transition = new System.Windows.Forms.Timer { Interval = 20 };
            transition.Tick += (_, _) =>
            {
                elapsed += transition.Interval;
                var progress = Math.Min(1D, elapsed / 200D);
                button.BackColor = Blend(startBackColor, targetBackColor, progress);
                button.ForeColor = Blend(startForeColor, targetForeColor, progress);
                if (progress >= 1D)
                    transition.Stop();
            };
            transition.Start();
        }

        button.MouseEnter += (_, _) =>
        {
            if (!colorsCaptured)
            {
                normalBackColor = button.BackColor;
                normalForeColor = button.ForeColor;
                colorsCaptured = true;
            }

            var hoverBackColor = normalBackColor == Tan ? Color.FromArgb(232, 204, 157) : Rust;
            var hoverForeColor = normalBackColor == Tan ? Forest : Color.White;
            Animate(hoverBackColor, hoverForeColor);
        };
        button.MouseLeave += (_, _) => Animate(normalBackColor, normalForeColor);
        button.GotFocus += (_, _) => button.FlatAppearance.BorderSize = 2;
        button.LostFocus += (_, _) => button.FlatAppearance.BorderSize = 0;
        return button;
    }

    private static Color Blend(Color from, Color to, double progress) => Color.FromArgb(
        (int)(from.A + (to.A - from.A) * progress),
        (int)(from.R + (to.R - from.R) * progress),
        (int)(from.G + (to.G - from.G) * progress),
        (int)(from.B + (to.B - from.B) * progress));

    protected Panel CardPanel(int width, int height)
    {
        var card = new Panel
        {
            BackColor = Color.FromArgb(255, 253, 247),
            Size = new(width, height),
            Margin = new(0, 0, 18, 18),
            Padding = new Padding(18)
        };
        card.Paint += (_, e) =>
        {
            using var border = new Pen(Color.FromArgb(229, 213, 181));
            e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
        };
        card.MouseEnter += (_, _) => card.BackColor = Color.FromArgb(255, 248, 231);
        card.MouseLeave += (_, _) => card.BackColor = Color.FromArgb(255, 253, 247);
        return card;
    }

    protected TextBox Input(string placeholder, bool password = false)
    {
        var input = new TextBox { PlaceholderText = placeholder, Font = BodyFont, Height = 34, BorderStyle = BorderStyle.FixedSingle };
        if (password) input.UseSystemPasswordChar = true;
        return input;
    }

    protected void Go(Form target) => Nav.GoTo(target, this);
}
