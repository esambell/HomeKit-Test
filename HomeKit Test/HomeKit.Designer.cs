namespace HomeKit_Test
{
    partial class HomeKit
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
            this.TCPListenerTask = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Status1 = new System.Windows.Forms.Label();
            this.CryptoTest = new System.ComponentModel.BackgroundWorker();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.MontyTest = new System.ComponentModel.BackgroundWorker();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.identifyBox = new System.Windows.Forms.CheckBox();
            this.Identify = new System.ComponentModel.BackgroundWorker();
            this.loopCountLabel = new System.Windows.Forms.Label();
            this.loopCountTimer = new System.Windows.Forms.Timer(this.components);
            this.cycleDelayTrackBar = new System.Windows.Forms.TrackBar();
            this.cycleDelayLabel = new System.Windows.Forms.Label();
            this.sessionListBox = new System.Windows.Forms.ListBox();
            this.pairingsListBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.cycleDelayTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // TCPListenerTask
            // 
            this.TCPListenerTask.DoWork += new System.ComponentModel.DoWorkEventHandler(this.TCPListenerTask_DoWork);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 486);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(104, 65);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(893, 387);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(172, 514);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(729, 26);
            this.textBox2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(515, 577);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 39);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Status1
            // 
            this.Status1.AutoSize = true;
            this.Status1.Location = new System.Drawing.Point(36, 529);
            this.Status1.Name = "Status1";
            this.Status1.Size = new System.Drawing.Size(51, 20);
            this.Status1.TabIndex = 4;
            this.Status1.Text = "label2";
            // 
            // CryptoTest
            // 
            this.CryptoTest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CryptoTest_DoWork);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(515, 635);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(196, 42);
            this.button2.TabIndex = 5;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(515, 704);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(196, 44);
            this.button3.TabIndex = 6;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // MontyTest
            // 
            this.MontyTest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MontyTest_DoWork);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(117, 649);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(106, 50);
            this.button4.TabIndex = 7;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "label2";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(346, 623);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(113, 24);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // identifyBox
            // 
            this.identifyBox.AutoSize = true;
            this.identifyBox.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.identifyBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.identifyBox.Location = new System.Drawing.Point(259, 670);
            this.identifyBox.Name = "identifyBox";
            this.identifyBox.Size = new System.Drawing.Size(206, 62);
            this.identifyBox.TabIndex = 10;
            this.identifyBox.Text = "Outlet Status";
            this.identifyBox.UseVisualStyleBackColor = true;
            this.identifyBox.CheckedChanged += new System.EventHandler(this.identifyBox_CheckedChanged);
            // 
            // Identify
            // 
            this.Identify.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Identify_DoWork);
            // 
            // loopCountLabel
            // 
            this.loopCountLabel.AutoSize = true;
            this.loopCountLabel.Location = new System.Drawing.Point(36, 574);
            this.loopCountLabel.Name = "loopCountLabel";
            this.loopCountLabel.Size = new System.Drawing.Size(51, 20);
            this.loopCountLabel.TabIndex = 11;
            this.loopCountLabel.Text = "label3";
            // 
            // loopCountTimer
            // 
            this.loopCountTimer.Enabled = true;
            this.loopCountTimer.Interval = 50;
            this.loopCountTimer.Tick += new System.EventHandler(this.loopCountTimer_Tick);
            // 
            // cycleDelayTrackBar
            // 
            this.cycleDelayTrackBar.Location = new System.Drawing.Point(104, 754);
            this.cycleDelayTrackBar.Maximum = 5000;
            this.cycleDelayTrackBar.Name = "cycleDelayTrackBar";
            this.cycleDelayTrackBar.Size = new System.Drawing.Size(862, 69);
            this.cycleDelayTrackBar.SmallChange = 100;
            this.cycleDelayTrackBar.TabIndex = 12;
            this.cycleDelayTrackBar.TickFrequency = 100;
            this.cycleDelayTrackBar.Value = 200;
            this.cycleDelayTrackBar.Scroll += new System.EventHandler(this.cycleDelayTrackBar_Scroll);
            // 
            // cycleDelayLabel
            // 
            this.cycleDelayLabel.AutoSize = true;
            this.cycleDelayLabel.Location = new System.Drawing.Point(801, 596);
            this.cycleDelayLabel.Name = "cycleDelayLabel";
            this.cycleDelayLabel.Size = new System.Drawing.Size(51, 20);
            this.cycleDelayLabel.TabIndex = 13;
            this.cycleDelayLabel.Text = "label3";
            // 
            // sessionListBox
            // 
            this.sessionListBox.FormattingEnabled = true;
            this.sessionListBox.ItemHeight = 20;
            this.sessionListBox.Location = new System.Drawing.Point(1184, 65);
            this.sessionListBox.Name = "sessionListBox";
            this.sessionListBox.Size = new System.Drawing.Size(796, 284);
            this.sessionListBox.TabIndex = 14;
            // 
            // pairingsListBox
            // 
            this.pairingsListBox.FormattingEnabled = true;
            this.pairingsListBox.ItemHeight = 20;
            this.pairingsListBox.Location = new System.Drawing.Point(1184, 391);
            this.pairingsListBox.Name = "pairingsListBox";
            this.pairingsListBox.Size = new System.Drawing.Size(796, 384);
            this.pairingsListBox.TabIndex = 15;
            // 
            // HomeKit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2059, 831);
            this.Controls.Add(this.pairingsListBox);
            this.Controls.Add(this.sessionListBox);
            this.Controls.Add(this.cycleDelayLabel);
            this.Controls.Add(this.cycleDelayTrackBar);
            this.Controls.Add(this.loopCountLabel);
            this.Controls.Add(this.identifyBox);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Status1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "HomeKit";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cycleDelayTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker TCPListenerTask;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Status1;
        private System.ComponentModel.BackgroundWorker CryptoTest;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.ComponentModel.BackgroundWorker MontyTest;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox identifyBox;
        private System.ComponentModel.BackgroundWorker Identify;
        private System.Windows.Forms.Label loopCountLabel;
        private System.Windows.Forms.Timer loopCountTimer;
        private System.Windows.Forms.TrackBar cycleDelayTrackBar;
        private System.Windows.Forms.Label cycleDelayLabel;
        private System.Windows.Forms.ListBox sessionListBox;
        private System.Windows.Forms.ListBox pairingsListBox;
    }
}

