using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmHistoryPage : WarmForm
    {
        public frmHistoryPage()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 715);
            AddNav();

            var header = new Panel { Dock = DockStyle.Top, Height = 217, BackColor = Forest };
            header.Controls.Add(new Label { Text = "YOUR IMPACT", Location = new Point(0, 45), Size = new Size(header.ClientSize.Width, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "Order history", Location = new Point(0, 68), Size = new Size(header.ClientSize.Width, 68), AutoSize = false, AutoEllipsis = false, Padding = new Padding(12, 0, 12, 0), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 32F, FontStyle.Bold | FontStyle.Italic) });
            header.Controls.Add(new Label { Text = "Every meal you have rescued or donated, all in one place.", Location = new Point(0, 150), Size = new Size(header.ClientSize.Width, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 10F) });
            Controls.Add(header);
            header.SendToBack();

            if (AppData.CurrentUser is null)
            {
                Controls.Add(new Label { Text = "Log in to see your history and the meals you have rescued.", Location = new Point(0, 350), Size = new Size(ClientSize.Width, 35), AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Forest, Font = new Font("Georgia", 16F) });
                var login = ActionButton("Log in", (_, _) => Go(new frmLogInPage()), 120);
                login.Location = new Point((ClientSize.Width - login.Width) / 2, 395);
                Controls.Add(login);
                var footer = BuildFooter();
                Controls.Add(footer);
                ClientSizeChanged += (_, _) => ResizePageBands(header, footer, login);
                ResizePageBands(header, footer, login);
                ResetScrollPosition();
                return;
            }
            var orders = AppData.Orders.Where(o => o.Email == AppData.CurrentUser.Email).ToList();
            if (orders.Count == 0)
            {
                Controls.Add(new Label { Text = "Your rescued meals will appear here.", Location = new Point(215, 350), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 16F) });
            }
            else
            {
                var rows = new FlowLayoutPanel { Location = new Point(235, 335), Size = new Size(630, 290), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true, BackColor = Cream };
                foreach (var order in orders)
                {
                    rows.Controls.Add(BuildOrderRow(order));
                }
                Controls.Add(rows);
            }

            var pageFooter = BuildFooter();
            Controls.Add(pageFooter);
            ClientSizeChanged += (_, _) => ResizePageBands(header, pageFooter, null);
            ResizePageBands(header, pageFooter, null);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            BeginInvoke(ResetScrollPosition);
        }

        private void ResetScrollPosition()
        {
            AutoScrollPosition = Point.Empty;
            if (VerticalScroll.Visible)
                VerticalScroll.Value = VerticalScroll.Minimum;
        }

        private void ResizePageBands(Panel header, Panel footer, Button? login)
        {
            var width = Math.Max(1, ClientSize.Width);
            footer.Width = width;

            foreach (Control control in header.Controls)
                control.Width = width;

            footer.Controls[1].Left = Math.Max(35, (width - footer.Controls[1].Width) / 2);
            footer.Controls[2].Left = Math.Max(35, width - footer.Controls[2].Width - 20);
            if (login is not null)
                login.Left = Math.Max(0, (width - login.Width) / 2);
        }

        private static Panel BuildOrderRow(Order order)
        {
            var row = new Panel { Size = new Size(610, 68), BackColor = Cream, Margin = new Padding(0) };
            row.Controls.Add(new Label { Text = order.Item, Location = new Point(0, 5), Size = new Size(500, 24), AutoEllipsis = true, ForeColor = Ink, Font = new Font("Georgia", 13F) });
            row.Controls.Add(new Label { Text = $"{order.DisplayId} · FlashBitez · {order.Date:dd MMM yyyy}", Location = new Point(0, 30), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 8F) });
            row.Controls.Add(new Label { Text = $"RM {order.Total:0.00}", Location = new Point(535, 5), AutoSize = true, ForeColor = Ink, Font = new Font("Segoe UI", 8F) });
            row.Controls.Add(new Label { Text = order.Status.ToUpperInvariant(), Location = new Point(535, 30), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 8F) });
            row.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawLine(border, 0, row.Height - 1, row.Width, row.Height - 1);
            };
            return row;
        }

        private static Panel BuildFooter()
        {
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(335, 25), Size = new Size(400, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(760, 25), Size = new Size(285, 22), Anchor = AnchorStyles.Top | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }
    }
}
