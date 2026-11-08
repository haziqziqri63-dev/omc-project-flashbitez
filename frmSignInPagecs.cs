using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmSignInPagecs : WarmForm
    {
        private readonly TextBox name;
        private readonly TextBox email;
        private readonly TextBox password;
        private readonly TextBox confirm;
        private readonly RadioButton buyer = new() { Text = "Buyer", AutoSize = true, Checked = true };
        private readonly RadioButton seller = new() { Text = "Seller", AutoSize = true };
        private readonly Label status = new() { AutoSize = true };

        public frmSignInPagecs()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(1100, 810);
            AddNav();

            var header = new Panel { Dock = DockStyle.Top, Height = 217, BackColor = Forest };
            header.Controls.Add(new Label { Text = "JOIN US", Location = new Point(0, 45), Size = new Size(1100, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "Sign up", Location = new Point(40, 68), Size = new Size(1020, 68), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 40F, FontStyle.Bold | FontStyle.Italic), AutoEllipsis = false });
            header.Controls.Add(new Label { Text = "Create an account to rescue meals and share them with your", Location = new Point(0, 150), Size = new Size(1100, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
            header.Controls.Add(new Label { Text = "community.", Location = new Point(0, 174), Size = new Size(1100, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
            Controls.Add(header);
            header.SendToBack();

            var accountCard = new Panel { Location = new Point(370, 310), Size = new Size(360, 455), BackColor = Cream };
            accountCard.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, accountCard.Width - 1, accountCard.Height - 1);
            };
            accountCard.Controls.Add(new Label { Text = "Create account", Location = new Point(30, 30), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 21F) });
            name = Input("Full name");
            email = Input("Email address");
            password = Input("Password", true);
            confirm = Input("Confirm password", true);
            AddField(accountCard, "NAME", name, 78);
            AddField(accountCard, "EMAIL", email, 136);
            AddField(accountCard, "PASSWORD", password, 194);
            AddField(accountCard, "CONFIRM PASSWORD", confirm, 252);
            accountCard.Controls.Add(new Label { Text = "ACCOUNT TYPE", Location = new Point(30, 310), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            buyer.Location = new Point(30, 329);
            seller.Location = new Point(105, 329);
            accountCard.Controls.Add(buyer);
            accountCard.Controls.Add(seller);
            var signUp = ActionButton("Sign up", SignUp, 120);
            signUp.Location = new Point(30, 365);
            signUp.Height = 34;
            signUp.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            accountCard.Controls.Add(signUp);
            var login = new LinkLabel { Text = "Already have an account? Log in", LinkColor = Rust, AutoSize = true, Location = new Point(30, 420), Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand };
            login.LinkClicked += (_, _) => Go(new frmLogInPage());
            accountCard.Controls.Add(login);
            status.Location = new Point(30, 440);
            status.Font = new Font("Segoe UI", 8F);
            accountCard.Controls.Add(status);
            Controls.Add(accountCard);
            Controls.Add(BuildFooter());
        }

        private static void AddField(Panel panel, string label, TextBox field, int y)
        {
            panel.Controls.Add(new Label { Text = label, Location = new Point(30, y), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            field.Location = new Point(30, y + 16);
            field.Width = 300;
            field.Height = 32;
            panel.Controls.Add(field);
        }

        private static Panel BuildFooter()
        {
            var footer = new Panel { Location = new Point(0, 740), Size = new Size(1100, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(810, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }

        private void SignUp(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text) || !email.Text.Contains('@'))
            {
                ShowError("Please enter your name and a valid email."); return;
            }
            if (password.Text.Length < 6)
            {
                ShowError("Password must contain at least 6 characters."); return;
            }
            if (password.Text != confirm.Text)
            {
                ShowError("Passwords do not match."); return;
            }
            if (AppData.Users.Any(u => u.Email.Equals(email.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                ShowError("That email is already registered."); return;
            }
            var user = new User(name.Text.Trim(), email.Text.Trim(), password.Text, seller.Checked ? "Seller" : "Buyer");
            AppData.Users.Add(user);
            AppData.CurrentUser = user;
            MessageBox.Show("Your account has been created.", "Welcome");
            Go(user.Role.Equals("Seller", StringComparison.OrdinalIgnoreCase)
                ? new frmSellerDashboard()
                : new frmMenuPage());
        }

        private void ShowError(string text)
        {
            status.ForeColor = Rust;
            status.Text = text;
        }
    }
}
