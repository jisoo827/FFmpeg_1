namespace FFMPEGTest1
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
            ((System.ComponentModel.ISupportInitialize)(this.Img_video)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wmp_1)).BeginInit();
            this.SuspendLayout();
            // 
            // Img_video
            // 
            this.Img_video.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Img_video.Location = new System.Drawing.Point(100, 73);
            this.Img_video.Name = "Img_video";
            this.Img_video.Size = new System.Drawing.Size(551, 333);
            this.Img_video.TabIndex = 0;
            this.Img_video.TabStop = false;
            this.Img_video.Visible = false;
            // 
            // Btn_Play
            // 
            this.Btn_Play.Location = new System.Drawing.Point(632, 12);
            this.Btn_Play.Name = "Btn_Play";
            this.Btn_Play.Size = new System.Drawing.Size(75, 23);
            this.Btn_Play.TabIndex = 2;
            this.Btn_Play.Text = "재생";
            this.Btn_Play.UseVisualStyleBackColor = true;
            this.Btn_Play.Click += new System.EventHandler(this.Btn_Play_Click);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.Location = new System.Drawing.Point(713, 12);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(75, 23);
            this.Btn_Stop.TabIndex = 3;
            this.Btn_Stop.Text = "중지";
            this.Btn_Stop.UseVisualStyleBackColor = true;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // Txt_URL
            // 
            this.Txt_URL.Location = new System.Drawing.Point(504, 12);
            this.Txt_URL.Name = "Txt_URL";
            this.Txt_URL.Size = new System.Drawing.Size(100, 21);
            this.Txt_URL.TabIndex = 4;
            this.Txt_URL.Text = "C:\\_Works\\쏘쓰\\FFmpeg_1-main\\FFMPEGTest1\\FFMPEGTest1\\bin\\Debug\\\\output_merge.avi";
            // 
            // Cmb_VType
            // 
            this.Cmb_VType.FormattingEnabled = true;
            this.Cmb_VType.Items.AddRange(new object[] {
            "WebCam",
            "MP4"});
            this.Cmb_VType.Location = new System.Drawing.Point(404, 12);
            this.Cmb_VType.Name = "Cmb_VType";
            this.Cmb_VType.Size = new System.Drawing.Size(94, 20);
            this.Cmb_VType.TabIndex = 5;
            this.Cmb_VType.Text = "WebCam";
            // 
            // Btn_Record
            // 
            this.Btn_Record.Location = new System.Drawing.Point(12, 12);
            this.Btn_Record.Name = "Btn_Record";
            this.Btn_Record.Size = new System.Drawing.Size(75, 23);
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
            this.Wmp_1.Location = new System.Drawing.Point(100, 73);
            this.Wmp_1.Name = "Wmp_1";
            this.Wmp_1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Wmp_1.OcxState")));
            this.Wmp_1.Size = new System.Drawing.Size(551, 333);
            this.Wmp_1.TabIndex = 6;
            // 
            // Btn_RecordStop
            // 
            this.Btn_RecordStop.Location = new System.Drawing.Point(93, 12);
            this.Btn_RecordStop.Name = "Btn_RecordStop";
            this.Btn_RecordStop.Size = new System.Drawing.Size(75, 23);
            this.Btn_RecordStop.TabIndex = 1;
            this.Btn_RecordStop.Text = "녹화중지";
            this.Btn_RecordStop.UseVisualStyleBackColor = true;
            this.Btn_RecordStop.Click += new System.EventHandler(this.btn_RecordStop_Click);
            // 
            // Btn_Merge
            // 
            this.Btn_Merge.Location = new System.Drawing.Point(174, 12);
            this.Btn_Merge.Name = "Btn_Merge";
            this.Btn_Merge.Size = new System.Drawing.Size(75, 23);
            this.Btn_Merge.TabIndex = 1;
            this.Btn_Merge.Text = "합치기";
            this.Btn_Merge.UseVisualStyleBackColor = true;
            this.Btn_Merge.Click += new System.EventHandler(this.Btn_Merge_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Wmp_1);
            this.Controls.Add(this.Cmb_VType);
            this.Controls.Add(this.Txt_URL);
            this.Controls.Add(this.Btn_Stop);
            this.Controls.Add(this.Btn_Play);
            this.Controls.Add(this.Btn_Merge);
            this.Controls.Add(this.Btn_RecordStop);
            this.Controls.Add(this.Btn_Record);
            this.Controls.Add(this.Img_video);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.Img_video)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wmp_1)).EndInit();
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
    }
}
