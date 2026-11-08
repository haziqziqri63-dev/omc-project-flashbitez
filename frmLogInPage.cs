using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmLogInPage : WarmForm
    {
        private readonly TextBox email;
        private readonly TextBox password;
        private readonly RadioButton buyer = new() { Text = "Buyer", AutoSize = true, Checked = true };
        private readonly RadioButton seller = new() { Text = "Seller", AutoSize = true };
        private readonly Label status = new() { AutoSize = true };

        public frmLogInPage()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 750);
            AddNav();

            var header = new Panel { Dock = DockStyle.Top, Height = 217, BackColor = Forest };
            header.Controls.Add(new Label { Text = "WELCOME BACK", Location = new Point(0, 45), Size = new Size(header.ClientSize.Width, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "Log in", Location = new Point(0, 72), Size = new Size(header.ClientSize.Width, 60), AutoSize = false, AutoEllipsis = false, Padding = new Padding(12, 0, 12, 0), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 38F, FontStyle.Italic) });
            header.Controls.Add(new Label { Text = "Log in to reserve meals and track your impact.", Location = new Point(0, 150), Size = new Size(header.ClientSize.Width, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 10F) });
            Controls.Add(header);
            header.SendToBack();

            var loginCard = new Panel { Location = new Point(370, 330), Size = new Size(360, 350), BackColor = Cream };
            loginCard.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, loginCard.Width - 1, loginCard.Height - 1);
            };
            loginCard.Controls.Add(new Label { Text = "Log in", Location = new Point(30, 30), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 22F) });
            loginCard.Controls.Add(new Label { Text = "EMAIL", Location = new Point(30, 78), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            email = Input("Email address");
            password = Input("Password", true);
            email.Location = new Point(30, 94); email.Width = 300; email.Height = 38;
            loginCard.Controls.Add(email);
            loginCard.Controls.Add(new Label { Text = "PASSWORD", Location = new Point(30, 148), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            password.Location = new Point(30, 164); password.Width = 300; password.Height = 38;
            loginCard.Controls.Add(password);
            loginCard.Controls.Add(new Label { Text = "ACCOUNT TYPE", Location = new Point(30, 207), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            buyer.Location = new Point(30, 226);
            seller.Location = new Point(105, 226);
            loginCard.Controls.Add(buyer);
            loginCard.Controls.Add(seller);
            var login = ActionButton("Log in", Login, 120);
            login.Location = new Point(30, 258);
            login.Height = 34;
            login.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            loginCard.Controls.Add(login);
            var signUp = new LinkLabel { Text = "New to FlashBitez? Sign up", LinkColor = Rust, AutoSize = true, Location = new Point(30, 265), Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand };
            signUp.LinkClicked += (_, _) => Go(new frmSignInPagecs());
            loginCard.Controls.Add(signUp);
            signUp.Location = new Point(30, 310);
            status.Location = new Point(30, 330);
            status.Font = new Font("Segoe UI", 8F);
            loginCard.Controls.Add(status);
            Controls.Add(loginCard);
            var footer = BuildFooter();
            Controls.Add(footer);
            ClientSizeChanged += (_, _) => ResizePageBands(header, footer);
            ResizePageBands(header, footer);
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

        private static Panel BuildFooter()
        {
            var footer = new Panel { Location = new Point(0, 680), Size = new Size(1075, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(335, 25), Size = new Size(400, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(760, 25), Size = new Size(285, 22), Anchor = AnchorStyles.Top | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }

        private void Login(object? sender, EventArgs e)
        {
            var user = AppData.Users.FirstOrDefault(u => u.Email.Equals(email.Text.Trim(), StringComparison.OrdinalIgnoreCase) && u.Password == password.Text);
            if (user is null)
            {
                status.ForeColor = Rust;
                status.Text = "Email or password is incorrect.";
                return;
            }
            var selectedRole = seller.Checked ? "Seller" : "Buyer";
            if (!user.Role.Equals(selectedRole, StringComparison.OrdinalIgnoreCase))
            {
                status.ForeColor = Rust;
                status.Text = $"This account is registered as a {user.Role}.";
                return;
            }
            AppData.CurrentUser = user;
            MessageBox.Show($"Welcome, {user.Name}!", "Logged in");
            Go(user.Role.Equals("Seller", StringComparison.OrdinalIgnoreCase)
                ? new frmSellerDashboard()
                : new frmMenuPage());
        }
    }
}
