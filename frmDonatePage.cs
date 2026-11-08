using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmDonatePage : WarmForm
    {
        private readonly NumericUpDown meals = new() { Minimum = 1, Maximum = 100, Value = 1, Width = 260, Height = 32 };
        private readonly TextBox email;
        private readonly Label result = new() { ForeColor = Forest, AutoSize = true, Location = new(60, 470) };

        public frmDonatePage()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(1100, 750);
            AddNav();

            var header = new Panel { Location = new Point(0, 68), Size = new Size(1100, 240), BackColor = Forest };
            header.Controls.Add(new Label { Text = "SHARE A MEAL", Location = new Point(0, 45), Size = new Size(1100, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "Donate", Location = new Point(0, 72), Size = new Size(1100, 60), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Georgia", 38F, FontStyle.Italic) });
            header.Controls.Add(new Label { Text = "Every donated meal is one less plate in the bin and one more at", Location = new Point(0, 150), Size = new Size(1100, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 10F) });
            header.Controls.Add(new Label { Text = "someone's table.", Location = new Point(0, 174), Size = new Size(1100, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 10F) });
            Controls.Add(header);

            Controls.Add(Step("I", "Choose", "Pick how many meals you would like to give.", 355));
            Controls.Add(Step("II", "We match", "Meals are matched with a partner community kitchen.", 455));
            Controls.Add(Step("III", "Delivered", "Fresh food reaches neighbors the same day.", 555));

            var donationCard = new Panel { Location = new Point(570, 355), Size = new Size(385, 300), BackColor = Cream };
            donationCard.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, donationCard.Width - 1, donationCard.Height - 1);
            };
            donationCard.Controls.Add(new Label { Text = "Make a donation", Location = new Point(30, 30), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 22F, FontStyle.Bold) });
            donationCard.Controls.Add(new Label { Text = "NUMBER OF MEALS", Location = new Point(30, 78), Size = new Size(325, 18), ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft });
            meals.Location = new Point(30, 104);
            meals.Width = 325;
            meals.Height = 38;
            meals.Font = BodyFont;
            meals.Padding = new Padding(8, 0, 8, 0);
            meals.BackColor = Color.FromArgb(255, 250, 237);
            donationCard.Controls.Add(meals);
            donationCard.Controls.Add(new Label { Text = "YOUR EMAIL", Location = new Point(30, 156), Size = new Size(325, 18), ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft });
            email = Input("Your email address");
            email.Location = new Point(30, 182);
            email.Width = 325;
            email.Height = 38;
            email.Padding = new Padding(8, 0, 8, 0);
            email.BackColor = Color.FromArgb(255, 250, 237);
            donationCard.Controls.Add(email);
            var donate = ActionButton("Donate now", Donate, 140);
            donate.Location = new Point(30, 230);
            donate.Height = 34;
            donate.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            donationCard.Controls.Add(donate);
            donationCard.Controls.Add(result);
            result.Location = new Point(30, 270);
            Controls.Add(donationCard);
            Controls.Add(BuildFooter());
        }

        private static Panel Step(string number, string title, string description, int y)
        {
            var step = new Panel { Location = new Point(75, y), Size = new Size(470, 92), BackColor = Cream, Padding = new Padding(0) };
            step.Controls.Add(new Label { Text = number, Location = new Point(0, 0), Size = new Size(450, 28), AutoSize = false, ForeColor = Rust, Font = new Font("Georgia", 20F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft });
            step.Controls.Add(new Label { Text = title, Location = new Point(0, 27), Size = new Size(450, 32), AutoSize = false, ForeColor = Forest, Font = new Font("Georgia", 18F), TextAlign = ContentAlignment.MiddleLeft });
            step.Controls.Add(new Label { Text = description, Location = new Point(0, 60), Size = new Size(450, 30), AutoSize = false, AutoEllipsis = false, ForeColor = Ink, Font = new Font("Segoe UI", 9F), TextAlign = ContentAlignment.TopLeft });
            return step;
        }

        private static Panel BuildFooter()
        {
            var footer = new Panel { Location = new Point(0, 680), Size = new Size(1100, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(810, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }

        private void Donate(object? sender, EventArgs e)
        {
            if (!email.Text.Contains('@'))
            {
                result.ForeColor = Rust;
                result.Text = "Please enter a valid email address.";
                return;
            }
            result.ForeColor = Forest;
            result.Text = $"Thank you! You donated {meals.Value} meal(s).";
        }
    }
}
