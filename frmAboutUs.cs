using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmAboutUs : WarmForm
    {
        public frmAboutUs()
        {
            InitializeComponent();
            AutoScroll = true;
            AutoScrollMinSize = new Size(0, 770);
            AddNav();
            var header = new Panel { Location = new Point(0, 68), Size = new Size(ClientSize.Width, 230), BackColor = Forest, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            header.Controls.Add(new Label { Text = "OUR STORY", Location = new Point(0, 45), Size = new Size(header.ClientSize.Width, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            header.Controls.Add(new Label { Text = "About us", Location = new Point(0, 72), Size = new Size(header.ClientSize.Width, 60), AutoSize = false, AutoEllipsis = false, Padding = new Padding(12, 0, 12, 0), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 38F, FontStyle.Bold | FontStyle.Italic) });
            header.Controls.Add(new Label { Text = "Good food today, less waste tomorrow.", Location = new Point(0, 150), Size = new Size(header.ClientSize.Width, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 10F) });
            Controls.Add(header);

            var photo = new Panel { BackColor = Cream, Location = new Point(105, 345), Size = new Size(420, 260) };
            var logo = LoadLogo();
            if (logo is not null)
            {
                photo.Controls.Add(new PictureBox
                {
                    Image = logo,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(10, 10),
                    Size = new Size(400, 240),
                    BackColor = Cream
                });
            }
            else
            {
                photo.Controls.Add(new Label { Text = "Add logo.png", ForeColor = Forest, Font = new Font("Georgia", 15F, FontStyle.Italic), AutoSize = true, Location = new Point(145, 112) });
            }
            Controls.Add(photo);

            Controls.Add(new Label { Text = "Why we started", Location = new Point(560, 350), AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 25F, FontStyle.Bold) });
            Controls.Add(new Label { Text = "[YOUR FOUNDING STORY: who you are, the problem you saw, and what\nFlashBitez does about it.]", Location = new Point(560, 410), Size = new Size(410, 65), ForeColor = Ink, Font = new Font("Segoe UI", 10F) });

            AddValue("Rescue", "[Describe how you rescue surplus food.]", 110);
            AddValue("Share", "[Describe your donation partners.]", 430);
            AddValue("Sustain", "[Describe your sustainability goals.]", 750);
            var footer = BuildFooter();
            Controls.Add(footer);
            ClientSizeChanged += (_, _) => ResizePageBands(header, footer);
            ResizePageBands(header, footer);
        }

        private void ResizePageBands(Panel header, Panel footer)
        {
            var width = Math.Max(1, ClientSize.Width);
            header.Width = width;
            footer.Width = width;

            foreach (Control control in header.Controls)
                control.Width = width;

            footer.Controls[1].Left = Math.Max(35, (width - footer.Controls[1].Width) / 2);
            footer.Controls[2].Left = Math.Max(35, width - footer.Controls[2].Width - 20);
        }

        private void AddValue(string title, string text, int x)
        {
            var panel = new Panel { Location = new Point(x, 610), Size = new Size(250, 80), BackColor = Cream };
            panel.Controls.Add(new Label { Text = title, Font = new Font("Georgia", 17F), ForeColor = Rust, AutoSize = true });
            panel.Controls.Add(new Label { Text = text, Font = new Font("Segoe UI", 9F), ForeColor = Ink, Location = new Point(0, 35), Size = new Size(250, 40) });
            Controls.Add(panel);
        }

        private static Panel BuildFooter()
        {
            var footer = new Panel { Location = new Point(0, 700), Size = new Size(1100, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(335, 25), Size = new Size(400, 22), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(760, 25), Size = new Size(285, 22), Anchor = AnchorStyles.Top | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleRight, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }
    }
}
