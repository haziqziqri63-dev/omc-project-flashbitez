using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmDealsPage : WarmForm
    {
        private readonly FlowLayoutPanel grid = new()
        {
            Location = new Point(55, 275),
            Size = new Size(950, 410),
            AutoScroll = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 0, 0, 10)
        };

        public frmDealsPage()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 830);
            AddNav();

            var header = new Panel { Location = new Point(0, 68), Size = new Size(1100, 170), BackColor = Forest, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            header.Controls.Add(new Label { Text = "RESCUE A MEAL", Location = new Point(0, 35), Size = new Size(1100, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "Today's", Location = new Point(260, 60), Size = new Size(285, 55), TextAlign = ContentAlignment.MiddleRight, ForeColor = Cream, Font = new Font("Georgia", 32F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = " deals", Location = new Point(545, 60), Size = new Size(255, 55), TextAlign = ContentAlignment.MiddleLeft, ForeColor = Tan, Font = new Font("Georgia", 32F, FontStyle.Italic) });
            header.Controls.Add(new Label { Text = "Discounted meals that must be collected before the day ends.", Location = new Point(0, 125), Size = new Size(1100, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
            Controls.Add(header);

            var filters = new FlowLayoutPanel { Location = new Point(55, 255), Size = new Size(950, 44), BackColor = Cream };
            foreach (var filter in new[] { "All", "Ending soon", "Nearest", "Vegetarian" })
            {
                var button = new Button
                {
                    Text = filter,
                    Width = filter == "Ending soon" ? 90 : 75,
                    Height = 34,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = filter == "All" ? Forest : Cream,
                    ForeColor = filter == "All" ? Color.White : Ink,
                    Font = new Font("Segoe UI", 8F),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 0, 10, 0)
                };
                button.FlatAppearance.BorderColor = Tan;
                button.FlatAppearance.BorderSize = 1;
                filters.Controls.Add(button);
            }
            Controls.Add(filters);
            Controls.Add(grid);
            Controls.Add(BuildFooter());
            ShowDeals();
        }

        private void ShowDeals()
        {
            grid.Controls.Clear();
            foreach (var dish in AppData.Dishes)
            {
                var card = new Panel { Size = new Size(300, 190), Margin = new Padding(0, 0, 18, 18), BackColor = Cream };
                var photo = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(235, 210, 161) };
                photo.Controls.Add(new Label { Text = "ENDING SOON", AutoSize = true, ForeColor = Cream, BackColor = Color.FromArgb(194, 116, 77), Font = new Font("Segoe UI", 7F, FontStyle.Bold), Location = new Point(10, 10), Padding = new Padding(4, 2, 4, 2) });
                photo.Controls.Add(new Label { Text = GetDishIcon(dish), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI Emoji", 30F), Location = new Point(127, 25) });
                photo.Controls.Add(new Label { Text = "♡", AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 14F), Location = new Point(270, 5) });
                card.Controls.Add(photo);
                card.Controls.Add(new Label { Text = dish.Restaurant.ToUpperInvariant(), ForeColor = Rust, Font = new Font("Segoe UI", 7F, FontStyle.Bold), Location = new Point(14, 110), AutoSize = true });
                card.Controls.Add(new Label { Text = dish.Name, ForeColor = Forest, Font = new Font("Georgia", 14F, FontStyle.Bold), Location = new Point(14, 128), Size = new Size(190, 28) });
                card.Controls.Add(new Label { Text = $"RM {dish.DealPrice:0.00}", ForeColor = Ink, Font = new Font("Segoe UI", 8F), Location = new Point(14, 163), AutoSize = true });
                card.Controls.Add(new Label { Text = $"RM {dish.Price:0.00}", ForeColor = Color.Gray, Font = new Font("Segoe UI", 7F, FontStyle.Strikeout), Location = new Point(74, 163), AutoSize = true });
                card.Controls.Add(new Label { Text = "ends 17:00", ForeColor = Ink, Font = new Font("Segoe UI", 7F), Location = new Point(14, 177), AutoSize = true });
                var reserve = ActionButton("RESERVE", (_, _) => Reserve(dish), 70);
                reserve.Location = new Point(215, 151);
                reserve.Height = 28;
                reserve.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
                card.Controls.Add(reserve);
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

        private Panel BuildFooter()
        {
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(850, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }

        private void Reserve(Dish dish)
        {
            CartService.Instance.AddItem(MenuItem.FromDish(dish, AppData.Dishes.IndexOf(dish)));
            Go(new frmCheckoutPage());
        }
    }
}
