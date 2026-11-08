using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace omc
{
    public partial class frmHomePage : WarmForm
    {
        public frmHomePage()
        {
            AutoScroll = true;
            AutoScrollMinSize = new Size(1060, 1700);
            AddNav();
            Controls.Add(BuildPage());
        }

        private Panel BuildPage()
        {
            var page = new Panel { Location = new Point(0, 68), Size = new Size(1060, 1700), BackColor = Cream };
            page.Controls.Add(BuildHero());
            page.Controls.Add(BuildTicker());
            page.Controls.Add(BuildStats());
            page.Controls.Add(BuildHowItWorks());
            page.Controls.Add(BuildDeals());
            page.Controls.Add(BuildTestimonial());
            page.Controls.Add(BuildDonationBanner());
            page.Controls.Add(BuildFooter());
            return page;
        }

        private Panel BuildHero()
        {
            var hero = new Panel { Location = new Point(0, 0), Size = new Size(1060, 390), BackColor = Forest };
            hero.Controls.Add(new Label { Text = "SURPLUS, RESCUED & SHARED", Location = new Point(70, 72), AutoSize = true, ForeColor = Tan, Font = new Font("Segoe UI", 8F, FontStyle.Bold) });
            hero.Controls.Add(new Label { Text = "Good food today,\nless waste\ntomorrow.", Location = new Point(70, 98), Size = new Size(470, 175), ForeColor = Cream, Font = new Font("Georgia", 39F, FontStyle.Bold) });
            hero.Controls.Add(new Label { Text = "Fresh, beautifully prepared meals before the day ends, at a fraction of the price, or shared with neighbours who need them.", Location = new Point(70, 270), Size = new Size(390, 52), ForeColor = Cream, Font = new Font("Segoe UI", 10F) });

            var browse = new Button
            {
                Text = "BROWSE",
                Location = new Point(70, 335),
                Size = new Size(145, 42),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Tan,
                ForeColor = Forest,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            browse.FlatAppearance.BorderSize = 0;
            browse.Click += (_, _) => Go(new frmDealsPage());
            browse.MouseEnter += (_, _) => browse.BackColor = Rust;
            browse.MouseLeave += (_, _) => browse.BackColor = Tan;
            hero.Controls.Add(browse);
            var donate = new Button { Text = "DONATE A MEAL", Location = new Point(230, 335), Size = new Size(145, 42), FlatStyle = FlatStyle.Flat, UseVisualStyleBackColor = false, BackColor = Forest, ForeColor = Cream, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 8F, FontStyle.Bold), Cursor = Cursors.Hand };
            donate.FlatAppearance.BorderColor = Tan;
            donate.FlatAppearance.BorderSize = 1;
            donate.Click += (_, _) => Go(new frmDonatePage());
            donate.MouseEnter += (_, _) => donate.ForeColor = Tan;
            donate.MouseLeave += (_, _) => donate.ForeColor = Cream;
            hero.Controls.Add(donate);
            hero.Controls.Add(BuildHeroMark());
            return hero;
        }

        private Panel BuildHeroMark()
        {
            var mark = new Panel
            {
                Location = new Point(650, 105),
                Size = new Size(270, 270),
                BackColor = Cream,
                Padding = new Padding(12),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            mark.Paint += (_, e) =>
            {
                using var border = new Pen(Tan, 1);
                e.Graphics.DrawRectangle(border, 0, 0, mark.Width - 1, mark.Height - 1);
            };
            var logo = LoadLogo();
            if (logo is not null)
            {
                var image = new PictureBox
                {
                    Image = logo,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill,
                    BackColor = Cream,
                    AccessibleName = "FlashBitez logo"
                };
                mark.Controls.Add(image);
            }
            return mark;
        }

        private Panel BuildTicker()
        {
            var ticker = new Panel { Location = new Point(0, 390), Size = new Size(1060, 35), BackColor = Rust };
            ticker.Controls.Add(new Label { Text = "SAVE  ✦  ENJOY  ✦  RESCUE  ✦  SHARE  ✦  SAVE  ✦  ENJOY  ✦  RESCUE  ✦  SHARE", Location = new Point(0, 7), Size = new Size(1060, 25), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Italic) });
            return ticker;
        }

        private Panel BuildStats()
        {
            var stats = new Panel { Location = new Point(0, 425), Size = new Size(1060, 140), BackColor = Cream };
            AddStat(stats, "[00,000]", "MEALS RESCUED", 120);
            AddStat(stats, "[00]", "PARTNER KITCHENS", 430);
            AddStat(stats, "[0,000]", "KG OF CO₂ AVOIDED", 740);
            return stats;
        }

        private void AddStat(Panel section, string value, string caption, int x)
        {
            section.Controls.Add(new Label { Text = value, Location = new Point(x, 35), Size = new Size(200, 55), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Rust, Font = new Font("Georgia", 30F, FontStyle.Bold) });
            section.Controls.Add(new Label { Text = caption, Location = new Point(x, 92), Size = new Size(200, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Forest, Font = new Font("Segoe UI", 7F, FontStyle.Bold) });
        }

        private Panel BuildHowItWorks()
        {
            var section = new Panel { Location = new Point(0, 565), Size = new Size(1060, 335), BackColor = Color.FromArgb(246, 232, 190) };
            section.Controls.Add(Heading("How it works", 350, 35, 360));
            AddStepCard(section, "Discover", "Browse today's discounted meals from local kitchens.", 70, "⌕");
            AddStepCard(section, "Reserve", "Claim your meal in a few taps and choose a pickup window.", 380, "▦");
            AddStepCard(section, "Enjoy or donate", "Savour it yourself, or pass it to your community.", 690, "♥");
            return section;
        }

        private void AddStepCard(Panel section, string title, string text, int x, string icon)
        {
            var card = new Panel { Location = new Point(x, 115), Size = new Size(270, 215), BackColor = Cream };
            card.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
            };
            card.Controls.Add(new Label { Text = icon, Location = new Point(105, 25), Size = new Size(60, 55), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Rust, BackColor = Tan, Font = new Font("Segoe UI", 24F, FontStyle.Bold) });
            card.Controls.Add(new Label { Text = title, Location = new Point(10, 92), Size = new Size(250, 30), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Forest, Font = new Font("Georgia", 16F, FontStyle.Bold) });
            card.Controls.Add(new Label { Text = text, Location = new Point(18, 135), Size = new Size(235, 52), TextAlign = ContentAlignment.TopCenter, ForeColor = Ink, Font = new Font("Segoe UI", 9F) });
            section.Controls.Add(card);
        }

        private Panel BuildDeals()
        {
            var section = new Panel { Location = new Point(0, 900), Size = new Size(1060, 330), BackColor = Cream };
            section.Controls.Add(Heading("Today's featured deals", 70, 35, 550));
            section.Controls.Add(new Label { Text = "featured", Location = new Point(380, 43), AutoSize = true, ForeColor = Rust, Font = new Font("Georgia", 24F, FontStyle.Italic) });
            var viewAll = new LinkLabel { Text = "VIEW ALL DEALS", Location = new Point(870, 55), AutoSize = true, LinkColor = Rust, Font = new Font("Segoe UI", 8F, FontStyle.Bold) };
            viewAll.LinkClicked += (_, _) => Go(new frmDealsPage());
            section.Controls.Add(viewAll);
            for (var i = 0; i < Math.Min(3, AppData.Dishes.Count); i++)
                section.Controls.Add(BuildDealCard(AppData.Dishes[i], 70 + i * 310));
            return section;
        }

        private Panel BuildDealCard(Dish dish, int x)
        {
            var card = new Panel { Location = new Point(x, 100), Size = new Size(280, 205), BackColor = Cream };
            var photo = new Panel { Dock = DockStyle.Top, Height = 110, BackColor = Tan };
            photo.Controls.Add(new Label { Text = "[MEAL PHOTO]", AutoSize = true, ForeColor = Forest, Font = new Font("Georgia", 9F, FontStyle.Italic), Location = new Point(95, 47) });
            card.Controls.Add(photo);
            card.Controls.Add(new Label { Text = "ENDING SOON", Location = new Point(12, 118), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 7F, FontStyle.Bold) });
            card.Controls.Add(new Label { Text = dish.Restaurant.ToUpperInvariant(), Location = new Point(12, 138), AutoSize = true, ForeColor = Rust, Font = new Font("Segoe UI", 7F, FontStyle.Bold) });
            card.Controls.Add(new Label { Text = dish.Name, Location = new Point(12, 155), Size = new Size(175, 26), ForeColor = Forest, Font = new Font("Georgia", 13F, FontStyle.Bold) });
            card.Controls.Add(new Label { Text = $"RM {dish.DealPrice:0.00} · ends today", Location = new Point(12, 181), AutoSize = true, ForeColor = Ink, Font = new Font("Segoe UI", 8F) });
            var reserve = ActionButton("RESERVE", (_, _) => Go(new frmDealsPage()), 70);
            reserve.Location = new Point(195, 168);
            reserve.Height = 28;
            reserve.Font = new Font("Segoe UI", 7F, FontStyle.Bold);
            card.Controls.Add(reserve);
            card.Paint += (_, e) =>
            {
                using var border = new Pen(Color.FromArgb(229, 213, 181));
                e.Graphics.DrawRectangle(border, 0, 0, card.Width - 1, card.Height - 1);
            };
            return card;
        }

        private Panel BuildTestimonial()
        {
            var section = new Panel { Location = new Point(0, 1230), Size = new Size(1060, 170), BackColor = Tan };
            section.Controls.Add(new Label { Text = "“", Location = new Point(0, 20), Size = new Size(1060, 40), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Rust, Font = new Font("Georgia", 28F, FontStyle.Bold) });
            section.Controls.Add(new Label { Text = "[A short, warm customer testimonial goes here.]", Location = new Point(0, 65), Size = new Size(1060, 40), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Ink, Font = new Font("Georgia", 16F, FontStyle.Italic) });
            section.Controls.Add(new Label { Text = "[CUSTOMER NAME]", Location = new Point(0, 115), Size = new Size(1060, 20), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Rust, Font = new Font("Segoe UI", 7F, FontStyle.Bold) });
            return section;
        }

        private Panel BuildDonationBanner()
        {
            var banner = new Panel { Location = new Point(0, 1400), Size = new Size(1060, 230), BackColor = Color.FromArgb(25, 43, 24) };
            banner.Controls.Add(new Label { Text = "Share a meal. Make it matter.", Location = new Point(250, 45), Size = new Size(560, 50), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Georgia", 27F, FontStyle.Bold) });
            banner.Controls.Add(new Label { Text = "Every donated meal is one less plate in the bin and one more at someone's table.", Location = new Point(300, 105), Size = new Size(460, 40), TextAlign = ContentAlignment.MiddleCenter, ForeColor = Cream, Font = new Font("Segoe UI", 9F) });
            var donate = ActionButton("DONATE NOW", (_, _) => Go(new frmDonatePage()), 125);
            donate.Location = new Point(468, 165);
            donate.BackColor = Tan;
            donate.ForeColor = Forest;
            banner.Controls.Add(donate);
            return banner;
        }

        private Panel BuildFooter()
        {
            var footer = new Panel { Location = new Point(0, 1630), Size = new Size(1060, 70), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackColor = Ink };
            footer.Controls.Add(new Label { Text = "FlashBitez", Location = new Point(35, 25), AutoSize = true, ForeColor = Cream, Font = new Font("Georgia", 11F, FontStyle.Bold) });
            footer.Controls.Add(new Label { Text = "Good Food Today, Less Waste Tomorrow", Location = new Point(430, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            footer.Controls.Add(new Label { Text = "about us  ·  contact us  ·  history", Location = new Point(810, 28), AutoSize = true, ForeColor = Cream, Font = new Font("Segoe UI", 8F) });
            return footer;
        }
    }
}
