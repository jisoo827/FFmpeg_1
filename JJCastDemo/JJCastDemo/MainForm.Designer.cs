namespace JJCastDemo
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Img_video = new System.Windows.Forms.PictureBox();
            this.Btn_Play = new System.Windows.Forms.Button();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.Txt_URL = new System.Windows.Forms.TextBox();
            this.Cmb_VType = new System.Windows.Forms.ComboBox();
            this.Btn_Record = new System.Windows.Forms.Button();
            this.Wmp_1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.Btn_RecordStop = new System.Windows.Forms.Button();
            this.Btn_Merge = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Img_video)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wmp_1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Img_video
            // 
            this.Img_video.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Img_video.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Img_video.Location = new System.Drawing.Point(464, 65);
            this.Img_video.Name = "Img_video";
            this.Img_video.Size = new System.Drawing.Size(243, 165);
            this.Img_video.TabIndex = 0;
            this.Img_video.TabStop = false;
            this.Img_video.Visible = false;
            // 
            // Btn_Play
            // 
            this.Btn_Play.Location = new System.Drawing.Point(295, 12);
            this.Btn_Play.Name = "Btn_Play";
            this.Btn_Play.Size = new System.Drawing.Size(75, 23);
            this.Btn_Play.TabIndex = 2;
            this.Btn_Play.Text = "재생";
            this.Btn_Play.UseVisualStyleBackColor = true;
            this.Btn_Play.Click += new System.EventHandler(this.Btn_Play_Click);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.Location = new System.Drawing.Point(376, 12);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(75, 23);
            this.Btn_Stop.TabIndex = 3;
            this.Btn_Stop.Text = "중지";
            this.Btn_Stop.UseVisualStyleBackColor = true;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // Txt_URL
            // 
            this.Txt_URL.Location = new System.Drawing.Point(12, 130);
            this.Txt_URL.Name = "Txt_URL";
            this.Txt_URL.Size = new System.Drawing.Size(613, 21);
            this.Txt_URL.TabIndex = 4;
            this.Txt_URL.Text = "C:\\_Works\\쏘쓰\\FFmpeg_1-main\\FFMPEGTest1\\FFMPEGTest1\\bin\\Debug\\\\output_merge.avi";
            // 
            // Cmb_VType
            // 
            this.Cmb_VType.FormattingEnabled = true;
            this.Cmb_VType.Items.AddRange(new object[] {
            "WebCam",
            "Video"});
            this.Cmb_VType.Location = new System.Drawing.Point(74, 24);
            this.Cmb_VType.Name = "Cmb_VType";
            this.Cmb_VType.Size = new System.Drawing.Size(185, 20);
            this.Cmb_VType.TabIndex = 5;
            this.Cmb_VType.Text = "WebCam";
            // 
            // Btn_Record
            // 
            this.Btn_Record.Location = new System.Drawing.Point(295, 66);
            this.Btn_Record.Name = "Btn_Record";
            this.Btn_Record.Size = new System.Drawing.Size(106, 23);
            this.Btn_Record.TabIndex = 1;
            this.Btn_Record.Text = "녹화";
            this.Btn_Record.UseVisualStyleBackColor = true;
            this.Btn_Record.Click += new System.EventHandler(this.Btn_Record_Click);
            // 
            // Wmp_1
            // 
            this.Wmp_1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Wmp_1.Enabled = true;
            this.Wmp_1.Location = new System.Drawing.Point(3, 17);
            this.Wmp_1.Name = "Wmp_1";
            this.Wmp_1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Wmp_1.OcxState")));
            this.Wmp_1.Size = new System.Drawing.Size(452, 262);
            this.Wmp_1.TabIndex = 6;
            // 
            // Btn_RecordStop
            // 
            this.Btn_RecordStop.Location = new System.Drawing.Point(407, 66);
            this.Btn_RecordStop.Name = "Btn_RecordStop";
            this.Btn_RecordStop.Size = new System.Drawing.Size(106, 23);
            this.Btn_RecordStop.TabIndex = 1;
            this.Btn_RecordStop.Text = "녹화중지";
            this.Btn_RecordStop.UseVisualStyleBackColor = true;
            this.Btn_RecordStop.Click += new System.EventHandler(this.btn_RecordStop_Click);
            // 
            // Btn_Merge
            // 
            this.Btn_Merge.Location = new System.Drawing.Point(295, 95);
            this.Btn_Merge.Name = "Btn_Merge";
            this.Btn_Merge.Size = new System.Drawing.Size(106, 23);
            this.Btn_Merge.TabIndex = 1;
            this.Btn_Merge.Text = "합성";
            this.Btn_Merge.UseVisualStyleBackColor = true;
            this.Btn_Merge.Click += new System.EventHandler(this.Btn_Merge_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.Wmp_1);
            this.groupBox1.Controls.Add(this.Img_video);
            this.groupBox1.Location = new System.Drawing.Point(12, 157);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 284);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(407, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "합성 영상 재생";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Btn_Merge_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(519, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 46);
            this.button2.TabIndex = 8;
            this.button2.Text = "크로마키 칼라\r\nRGB (0, 0, 0)";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.Cmb_VType);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(277, 112);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "설정";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Mic";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Cam";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "WebCam",
            "Video"});
            this.comboBox1.Location = new System.Drawing.Point(74, 50);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(185, 20);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "WebCam";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Monitor";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "WebCam",
            "Video"});
            this.comboBox2.Location = new System.Drawing.Point(74, 76);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(185, 20);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.Text = "WebCam";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(330, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(295, 21);
            this.textBox1.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "제목";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 444);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Txt_URL);
            this.Controls.Add(this.Btn_Stop);
            this.Controls.Add(this.Btn_Play);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Btn_Merge);
            this.Controls.Add(this.Btn_RecordStop);
            this.Controls.Add(this.Btn_Record);
            this.Name = "MainForm";
            this.Text = "JJCastDemo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Img_video)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wmp_1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Img_video;
        private System.Windows.Forms.Button Btn_Play;
        private System.Windows.Forms.Button Btn_Stop;
        private System.Windows.Forms.TextBox Txt_URL;
        private System.Windows.Forms.ComboBox Cmb_VType;
        private System.Windows.Forms.Button Btn_Record;
        private AxWMPLib.AxWindowsMediaPlayer Wmp_1;
        private System.Windows.Forms.Button Btn_RecordStop;
        private System.Windows.Forms.Button Btn_Merge;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
    }
}
