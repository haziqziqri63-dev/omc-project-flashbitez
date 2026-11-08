using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmContactUs : WarmForm
    {
        private readonly TextBox name;
        private readonly TextBox email;
        private readonly TextBox message;
        private readonly Label status = new() { AutoSize = true, ForeColor = Forest };

        public frmContactUs()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 1000);
            AddNav();

            var header = new Panel { Dock = DockStyle.Top, Height = 170, BackColor = Forest };
            header.Controls.Add(new Label { Text = "SAY HELLO  👋", Location = new Point(0, 40), Size = new Size(header.ClientSize.Width, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "Let's chat", Location = new Point(0, 65), Size = new Size(header.ClientSize.Width, 55), AutoSize = false, AutoEllipsis = false, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 32F, FontStyle.Bold | FontStyle.Italic) });
            header.Controls.Add(new Label { Text = "Questions, partnerships or just a friendly hello: we reply fast.", Location = new Point(0, 125), Size = new Size(header.ClientSize.Width, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
            Controls.Add(header);
            header.SendToBack();

            Controls.Add(BuildInfoCard("✉", "EMAIL", $"[{AppData.BusinessEmail}]", 270));
            Controls.Add(BuildInfoCard("📞", "PHONE", $"[{AppData.BusinessPhone}]", 335));
            Controls.Add(BuildInfoCard("📍", "ADDRESS", $"[{AppData.BusinessAddress}]", 400));
            Controls.Add(BuildInfoCard("◷", "HOURS", "[OPENING HOURS]", 465));

            var reply = ActionButton("● We usually reply within 24 hours", (_, _) => { }, 365);
            reply.Location = new Point(75, 650);
            reply.Height = 34;
            reply.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            Controls.Add(reply);

            var messageCard = new Panel { Location = new Point(570, 270), Size = new Size(385, 410), BackColor = Cream };
            messageCard.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, messageCard.Width - 1, messageCard.Height - 1);
            };
            messageCard.Controls.Add(new Label { Text = "Send us a message 👋", Location = new Point(30, 28), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 19F) });
            AddTopicButtons(messageCard);

            name = Input("Your name");
            email = Input("Your email");
            message = Input("Your message");
            message.Multiline = true;
            messageCard.Controls.Add(FieldLabel("NAME", 30, 140));
            messageCard.Controls.Add(FieldLabel("EMAIL", 30, 208));
            messageCard.Controls.Add(FieldLabel("MESSAGE", 30, 276));
            name.Location = new Point(30, 156); name.Width = 325; name.Height = 32;
            email.Location = new Point(30, 224); email.Width = 325; email.Height = 32;
            message.Location = new Point(30, 292); message.Width = 325; message.Height = 78;
            messageCard.Controls.Add(name); messageCard.Controls.Add(email); messageCard.Controls.Add(message);
            var send = ActionButton("SEND MESSAGE ↗", Send, 135);
            send.Location = new Point(30, 380);
            send.Height = 30;
            send.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            messageCard.Controls.Add(send);
            status.Location = new Point(30, 415);
            messageCard.Controls.Add(status);
            Controls.Add(messageCard);
            Controls.Add(BuildQuickAnswers());
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

        private static Panel BuildInfoCard(string icon, string label, string value, int y)
        {
            var card = new Panel { Location = new Point(75, y), Size = new Size(425, 55), BackColor = Cream };
            card.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
            };
            card.Controls.Add(new Label { Text = icon, Location = new Point(18, 10), Size = new Size(38, 35), TextAlign = ContentAlignment.MiddleCenter, BackColor = Tan, Font = new Font("Segoe UI Emoji", 16F) });
            card.Controls.Add(new Label { Text = label, Location = new Point(78, 10), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 7F, FontStyle.Bold) });
            card.Controls.Add(new Label { Text = value, Location = new Point(78, 25), Size = new Size(330, 25), AutoSize = false, AutoEllipsis = true, Padding = new Padding(0, 0, 6, 0), ForeColor = Rust, Font = new Font("Georgia", 12F) });
            return card;
        }

        private static Label FieldLabel(string text, int x, int y) => new() { Text = text, Location = new Point(x, y), AutoSize = true, ForeColor = Forest, Font = new Font("Segoe UI", 7F, FontStyle.Bold) };

        private static void AddTopicButtons(Panel panel)
        {
            foreach (var topic in new[] { "💬 Question", "💛 Partnership", "⭐ Feedback", "💚 Say thanks" })
            {
                var button = new Button { Text = topic, AutoSize = false, Width = 100, Height = 28, FlatStyle = FlatStyle.Flat, BackColor = Cream, ForeColor = Forest, Font = new Font("Segoe UI", 7F), Margin = new Padding(0, 0, 8, 5) };
                button.FlatAppearance.BorderColor = Tan;
                panel.Controls.Add(button);
            }
            panel.Controls[1].Location = new Point(30, 68);
            panel.Controls[2].Location = new Point(140, 68);
            panel.Controls[3].Location = new Point(250, 68);
            panel.Controls[4].Location = new Point(30, 101);
        }

        private static Panel BuildQuickAnswers()
        {
            var section = new Panel { Location = new Point(0, 710), Size = new Size(1100, 220), BackColor = Color.FromArgb(246, 232, 190) };
            section.Controls.Add(new Label { Text = "Quick answers", Location = new Point(0, 35), Size = new Size(1100, 45), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Forest, Font = new Font("Georgia", 25F) });
            AddAnswer(section, "🛒", "How do I reserve a meal?", "[Short answer about reserving and pickup.]", 70);
            AddAnswer(section, "🤝", "Can my restaurant join?", "[Short answer about becoming a partner.]", 395);
            AddAnswer(section, "💚", "How do donations work?", "[Short answer about how donated meals are delivered.]", 720);
            return section;
        }

        private static void AddAnswer(Panel section, string icon, string title, string text, int x)
        {
            var card = new Panel { Location = new Point(x, 100), Size = new Size(290, 115), BackColor = Cream };
            card.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
            };
            card.Controls.Add(new Label { Text = icon, Location = new Point(18, 14), AutoSize = true, Font = new Font("Segoe UI Emoji", 13F) });
            card.Controls.Add(new Label { Text = title, Location = new Point(18, 48), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 11F) });
            card.Controls.Add(new Label { Text = text, Location = new Point(18, 73), Size = new Size(250, 35), ForeColor = Ink, Font = new Font("Segoe UI", 8F) });
            section.Controls.Add(card);
        }

        private static Panel BuildFooter()
        {
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(335, 25), Size = new Size(400, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(760, 25), Size = new Size(285, 22), Anchor = AnchorStyles.Top | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }

        private void Send(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text) || !email.Text.Contains('@') || string.IsNullOrWhiteSpace(message.Text))
            {
                status.ForeColor = Rust;
                status.Text = "Please complete your name, email, and message.";
                return;
            }
            status.ForeColor = Forest;
            status.Text = "Thanks! Your message has been sent.";
        }
    }
}
