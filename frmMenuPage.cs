using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmMenuPage : WarmForm
    {
        private readonly FlowLayoutPanel grid = new() { Location = new(55, 315), Size = new(990, 410), AutoScroll = true, WrapContents = true, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 8, 0, 10) };

        public frmMenuPage()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 830);
            HorizontalScroll.Enabled = false;
            HorizontalScroll.Visible = false;
            AddNav();
            var header = new Panel { Dock = DockStyle.Top, Height = 170, BackColor = Forest };
            header.Controls.Add(new Label { Text = "OUR KITCHEN PARTNERS", Location = new Point(0, 35), Size = new Size(1100, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "Menu", Location = new Point(0, 60), Size = new Size(1100, 55), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Georgia", 32F, FontStyle.Italic) });
            header.Controls.Add(new Label { Text = "Fresh, seasonal dishes from local kitchens, ready for rescue.", Location = new Point(0, 125), Size = new Size(1100, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
            Controls.Add(header);
            header.SendToBack();

            var chips = new FlowLayoutPanel { Location = new(55, 255), Size = new(990, 44), AutoSize = false, BackColor = Cream };
            foreach (var category in new[] { "All", "Salads", "Bowls", "Mains", "Sweets" })
            {
                var chip = new Button { Text = category, Width = 70, Height = 34, FlatStyle = FlatStyle.Flat, BackColor = category == "All" ? Forest : Cream, ForeColor = category == "All" ? Color.White : Ink, Cursor = Cursors.Hand, Margin = new(0, 0, 10, 0), Font = new Font("Segoe UI", 8F) };
                chip.FlatAppearance.BorderColor = Tan;
                chip.FlatAppearance.BorderSize = 1;
                chip.Click += (_, _) => ShowCategory(category);
                chips.Controls.Add(chip);
            }
            Controls.Add(chips);
            Controls.Add(grid);
            var footer = BuildFooter();
            Controls.Add(footer);
            ClientSizeChanged += (_, _) => ResizePageBands(header, footer);
            ResizePageBands(header, footer);
            ShowCategory("All");
        }

        private void ResizePageBands(Panel header, Panel footer)
        {
            var width = Math.Max(1, ClientSize.Width);
            footer.Width = width;

            foreach (Control control in header.Controls)
                control.Width = width;

            footer.Controls[1].Left = Math.Max(35, (width - footer.Controls[1].Width) / 2);
            footer.Controls[2].Left = Math.Max(35, width - footer.Controls[2].Width - 20);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            BeginInvoke(ResetScrollPosition);
        }

        private void ResetScrollPosition()
        {
            SuspendLayout();
            AutoScrollPosition = Point.Empty;
            HorizontalScroll.Value = HorizontalScroll.Minimum;
            if (VerticalScroll.Visible)
                VerticalScroll.Value = VerticalScroll.Minimum;
            ResumeLayout(true);
        }

        private Panel BuildFooter()
        {
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Ink };
            var brand = new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) };
            var message = new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(350, 28), Size = new Size(400, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 8F) };
            var links = new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(780, 25), Size = new Size(285, 22), Anchor = AnchorStyles.Top | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight, ForeColor = Cream, Font = new Font("Segoe UI", 8F) };
            footer.Controls.Add(brand);
            footer.Controls.Add(message);
            footer.Controls.Add(links);
            return footer;
        }

        private void ShowCategory(string category)
        {
            grid.Controls.Clear();
            foreach (var dishWithIndex in AppData.Dishes.Select((dish, index) => (dish, index)).Where(x => category == "All" || x.dish.Category == category))
            {
                var dish = dishWithIndex.dish;
                var card = new Panel { Size = new Size(300, 190), Margin = new Padding(0, 0, 18, 10), BackColor = Cream };
                var photo = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Tan };
                photo.Controls.Add(new Label { Text = GetDishIcon(dish), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI Emoji", 30F), Location = new Point(127, 20) });
                photo.Controls.Add(new Label { Text = "FRESH TODAY", AutoSize = true, ForeColor = Cream, BackColor = Forest, Font = new Font("Segoe UI", 7F, FontStyle.Bold), Location = new Point(10, 10), Padding = new Padding(4, 2, 4, 2) });
                photo.Controls.Add(new Label { Text = "♡", AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 14F), Location = new Point(270, 5) });
                card.Controls.Add(photo);
                card.Controls.Add(new Label { Text = dish.Restaurant.ToUpperInvariant(), ForeColor = Rust, Font = new Font("Segoe UI", 7F, FontStyle.Bold), Location = new Point(14, 100), Size = new Size(270, 16) });
                card.Controls.Add(new Label { Text = dish.Name, ForeColor = Forest, Font = new Font("Georgia", 14F, FontStyle.Bold), Location = new Point(14, 118), Size = new Size(270, 34), AutoEllipsis = false });
                var actions = new Panel { Location = new Point(14, 158), Size = new Size(272, 28), BackColor = Color.Transparent };
                actions.Controls.Add(new Label { Text = $"RM {dish.DealPrice:0.00}", ForeColor = Ink, Font = new Font("Segoe UI", 8F), Location = new Point(0, 6), Size = new Size(78, 20) });
                var view = ActionButton("VIEW", (_, _) => MessageBox.Show($"{dish.Name}\n{dish.Restaurant}\nRM {dish.DealPrice:0.00}", "Dish details"), 70);
                view.Location = new Point(201, 0);
                view.Height = 28;
                view.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
                actions.Controls.Add(view);
                Button? add = null;
                add = ActionButton("ADD TO CART", (_, _) =>
                {
                    CartService.Instance.AddItem(MenuItem.FromDish(dish, dishWithIndex.index));
                    add.Text = "ADDED ✓";
                    using var feedbackTimer = new System.Windows.Forms.Timer { Interval = 900 };
                    feedbackTimer.Tick += (_, _) =>
                    {
                        add.Text = "ADD TO CART";
                        feedbackTimer.Stop();
                        feedbackTimer.Dispose();
                    };
                    feedbackTimer.Start();
                }, 100);
                add.Location = new Point(86, 0);
                add.Height = 28;
                add.Font = new Font("Segoe UI", 6F, FontStyle.Bold);
                actions.Controls.Add(add);
                card.Controls.Add(actions);
                card.Paint += (_, e) =>
                {
                    using var border = new Pen(Color.FromArgb(229, 213, 181));
                    e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
                };
                grid.Controls.Add(card);
            }
        }

        private static string GetDishIcon(Dish dish) => dish.Category switch
        {
            "Salads" => "🥗",
            "Bowls" => "🍜",
            "Sweets" => "🍰",
            _ when dish.Name.Contains("Chicken", StringComparison.OrdinalIgnoreCase) => "🥐",
            _ => "🍲"
        };

    }
}
