namespace omc
{
    partial class form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            lblContactUs = new Label();
            lblAboutUs = new Label();
            lblHistory = new Label();
            lblDonate = new Label();
            lblDeals = new Label();
            lblMenu = new Label();
            lblHome = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.BurlyWood;
            panel1.Controls.Add(lblContactUs);
            panel1.Controls.Add(lblAboutUs);
            panel1.Controls.Add(lblHistory);
            panel1.Controls.Add(lblDonate);
            panel1.Controls.Add(lblDeals);
            panel1.Controls.Add(lblMenu);
            panel1.Controls.Add(lblHome);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1929, 63);
            panel1.TabIndex = 0;
            // 
            // lblContactUs
            // 
            lblContactUs.AutoSize = true;
            lblContactUs.Font = new Font("Perpetua", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblContactUs.Location = new Point(1004, 22);
            lblContactUs.Name = "lblContactUs";
            lblContactUs.Size = new Size(91, 23);
            lblContactUs.TabIndex = 7;
            lblContactUs.Text = "Contact Us";
            // 
            // lblAboutUs
            // 
            lblAboutUs.AutoSize = true;
            lblAboutUs.Font = new Font("Perpetua", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAboutUs.Location = new Point(900, 22);
            lblAboutUs.Name = "lblAboutUs";
            lblAboutUs.Size = new Size(79, 23);
            lblAboutUs.TabIndex = 6;
            lblAboutUs.Text = "About Us";
            // 
            // lblHistory
            // 
            lblHistory.AutoSize = true;
            lblHistory.Font = new Font("Perpetua", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblHistory.Location = new Point(786, 22);
            lblHistory.Name = "lblHistory";
            lblHistory.Size = new Size(64, 23);
            lblHistory.TabIndex = 5;
            lblHistory.Text = "History";
            // 
            // lblDonate
            // 
            lblDonate.AutoSize = true;
            lblDonate.Font = new Font("Perpetua", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDonate.Location = new Point(595, 22);
            lblDonate.Name = "lblDonate";
            lblDonate.Size = new Size(62, 23);
            lblDonate.TabIndex = 4;
            lblDonate.Text = "Donate";
            // 
            // lblDeals
            // 
            lblDeals.AutoSize = true;
            lblDeals.Font = new Font("Perpetua", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDeals.Location = new Point(400, 13);
            lblDeals.Name = "lblDeals";
            lblDeals.Size = new Size(85, 35);
            lblDeals.TabIndex = 3;
            lblDeals.Text = "Deals";
            // 
            // lblMenu
            // 
            lblMenu.AutoSize = true;
            lblMenu.Font = new Font("Perpetua", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMenu.Location = new Point(205, 13);
            lblMenu.Name = "lblMenu";
            lblMenu.Size = new Size(90, 35);
            lblMenu.TabIndex = 2;
            lblMenu.Text = "Menu";
            lblMenu.Click += lblMenu_Click;
            // 
            // lblHome
            // 
            lblHome.AutoSize = true;
            lblHome.Font = new Font("Perpetua", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHome.Location = new Point(27, 13);
            lblHome.Name = "lblHome";
            lblHome.Size = new Size(92, 35);
            lblHome.TabIndex = 1;
            lblHome.Text = "Home";
            // 
            // form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.OldLace;
            ClientSize = new Size(1924, 1009);
            Controls.Add(panel1);
            Name = "form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label lblHome;
        private Label lblMenu;
        private Label lblHistory;
        private Label lblDonate;
        private Label lblDeals;
        private Label lblAboutUs;
        private Label lblContactUs;
    }
}
