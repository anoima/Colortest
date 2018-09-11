namespace WindowsFormsApplication5
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.startbtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.stopbtn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.namelabel = new System.Windows.Forms.Label();
            this.itempic = new System.Windows.Forms.PictureBox();
            this.marketlocbtn = new System.Windows.Forms.Button();
            this.notificationlocbtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.itempic)).BeginInit();
            this.SuspendLayout();
            // 
            // startbtn
            // 
            this.startbtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.startbtn.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startbtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.startbtn.Location = new System.Drawing.Point(150, 230);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(200, 40);
            this.startbtn.TabIndex = 0;
            this.startbtn.Text = "Start";
            this.startbtn.UseVisualStyleBackColor = false;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(175, 276);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 25);
            this.button1.TabIndex = 1;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // stopbtn
            // 
            this.stopbtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.stopbtn.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopbtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.stopbtn.Location = new System.Drawing.Point(150, 230);
            this.stopbtn.Name = "stopbtn";
            this.stopbtn.Size = new System.Drawing.Size(200, 40);
            this.stopbtn.TabIndex = 2;
            this.stopbtn.Text = "Stop";
            this.stopbtn.UseVisualStyleBackColor = false;
            this.stopbtn.Click += new System.EventHandler(this.stopbtn_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Modern No. 20", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(145, 41);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(210, 28);
            this.textBox1.TabIndex = 3;
            // 
            // namelabel
            // 
            this.namelabel.AutoSize = true;
            this.namelabel.Font = new System.Drawing.Font("Segoe UI Emoji", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.namelabel.Location = new System.Drawing.Point(211, 6);
            this.namelabel.Name = "namelabel";
            this.namelabel.Size = new System.Drawing.Size(79, 32);
            this.namelabel.TabIndex = 4;
            this.namelabel.Text = "Name";
            // 
            // itempic
            // 
            this.itempic.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.itempic.Location = new System.Drawing.Point(160, 79);
            this.itempic.Name = "itempic";
            this.itempic.Size = new System.Drawing.Size(180, 110);
            this.itempic.TabIndex = 6;
            this.itempic.TabStop = false;
            this.itempic.Click += new System.EventHandler(this.itempic_Click);
            this.itempic.DoubleClick += new System.EventHandler(this.itempic_DoubleClick);
            // 
            // marketlocbtn
            // 
            this.marketlocbtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.marketlocbtn.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.marketlocbtn.Location = new System.Drawing.Point(23, 86);
            this.marketlocbtn.Name = "marketlocbtn";
            this.marketlocbtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.marketlocbtn.Size = new System.Drawing.Size(97, 29);
            this.marketlocbtn.TabIndex = 7;
            this.marketlocbtn.Text = "Market";
            this.marketlocbtn.UseVisualStyleBackColor = false;
            this.marketlocbtn.Click += new System.EventHandler(this.marketlocbtn_Click);
            // 
            // notificationlocbtn
            // 
            this.notificationlocbtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.notificationlocbtn.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notificationlocbtn.Location = new System.Drawing.Point(23, 138);
            this.notificationlocbtn.Name = "notificationlocbtn";
            this.notificationlocbtn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.notificationlocbtn.Size = new System.Drawing.Size(97, 29);
            this.notificationlocbtn.TabIndex = 8;
            this.notificationlocbtn.Text = "Notification";
            this.notificationlocbtn.UseVisualStyleBackColor = false;
            this.notificationlocbtn.Click += new System.EventHandler(this.notificationlocbtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(480, 307);
            this.ControlBox = false;
            this.Controls.Add(this.notificationlocbtn);
            this.Controls.Add(this.marketlocbtn);
            this.Controls.Add(this.itempic);
            this.Controls.Add(this.namelabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.startbtn);
            this.Controls.Add(this.stopbtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.itempic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button stopbtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label namelabel;
        private System.Windows.Forms.PictureBox itempic;
        private System.Windows.Forms.Button marketlocbtn;
        private System.Windows.Forms.Button notificationlocbtn;
    }
}

