﻿namespace JJCastDemo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Btn_Play = new System.Windows.Forms.Button();
            this.Btn_Stop = new System.Windows.Forms.Button();
            this.Txt_URL = new System.Windows.Forms.TextBox();
            this.Cmb_Mic = new System.Windows.Forms.ComboBox();
            this.Btn_Record = new System.Windows.Forms.Button();
            this.Btn_RecordStop = new System.Windows.Forms.Button();
            this.Btn_Merge = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Img_video = new System.Windows.Forms.PictureBox();
            this.Wmp_1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.TimeSlider = new JJCastDemo.UIControl.SelectionRangeSlider();
            this.btn_Split = new System.Windows.Forms.Button();
            this.Btn_MergePlay = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.Btn_Chromakey = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Cmb_Monitor = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Cmb_Cam = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Txt_Title = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Rdb_RightIn = new System.Windows.Forms.RadioButton();
            this.Rdb_RightBottomIn = new System.Windows.Forms.RadioButton();
            this.Rdb_RightBottomOut = new System.Windows.Forms.RadioButton();
            this.Rdb_DiagonalOut = new System.Windows.Forms.RadioButton();
            this.Rdb_DiagonalIn = new System.Windows.Forms.RadioButton();
            this.Rdb_RightOut = new System.Windows.Forms.RadioButton();
            this.Img_DeskTop = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Btn_Pause = new System.Windows.Forms.Button();
            this.Lbl_Min = new System.Windows.Forms.Label();
            this.Btn_Cut = new System.Windows.Forms.Button();
            this.Lbl_Max = new System.Windows.Forms.Label();
            this.Btn_CutPlay = new System.Windows.Forms.Button();
            this.Lbl_Current = new System.Windows.Forms.Label();
            this.xpDataView1 = new DevExpress.Xpo.XPDataView(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Img_video)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wmp_1)).BeginInit();
            this.Wmp_1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Img_DeskTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpDataView1)).BeginInit();
            this.SuspendLayout();
            // 
            // Btn_Play
            // 
            this.Btn_Play.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Play.Location = new System.Drawing.Point(295, 12);
            this.Btn_Play.Name = "Btn_Play";
            this.Btn_Play.Size = new System.Drawing.Size(75, 23);
            this.Btn_Play.TabIndex = 2;
            this.Btn_Play.Text = "테스트";
            this.Btn_Play.UseVisualStyleBackColor = true;
            this.Btn_Play.Click += new System.EventHandler(this.Btn_Play_Click);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.Txt_URL.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_URL.Location = new System.Drawing.Point(12, 130);
            this.Txt_URL.Name = "Txt_URL";
            this.Txt_URL.Size = new System.Drawing.Size(613, 22);
            this.Txt_URL.TabIndex = 4;
            // 
            // Cmb_Mic
            // 
            this.Cmb_Mic.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cmb_Mic.FormattingEnabled = true;
            this.Cmb_Mic.Location = new System.Drawing.Point(74, 24);
            this.Cmb_Mic.Name = "Cmb_Mic";
            this.Cmb_Mic.Size = new System.Drawing.Size(185, 22);
            this.Cmb_Mic.TabIndex = 5;
            // 
            // Btn_Record
            // 
            this.Btn_Record.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Record.Location = new System.Drawing.Point(295, 66);
            this.Btn_Record.Name = "Btn_Record";
            this.Btn_Record.Size = new System.Drawing.Size(106, 23);
            this.Btn_Record.TabIndex = 1;
            this.Btn_Record.Text = "녹화";
            this.Btn_Record.UseVisualStyleBackColor = true;
            this.Btn_Record.Click += new System.EventHandler(this.Btn_Record_Click);
            // 
            // Btn_RecordStop
            // 
            this.Btn_RecordStop.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_RecordStop.Location = new System.Drawing.Point(407, 66);
            this.Btn_RecordStop.Name = "Btn_RecordStop";
            this.Btn_RecordStop.Size = new System.Drawing.Size(106, 23);
            this.Btn_RecordStop.TabIndex = 1;
            this.Btn_RecordStop.Text = "녹화중지";
            this.Btn_RecordStop.UseVisualStyleBackColor = true;
            this.Btn_RecordStop.Click += new System.EventHandler(this.Btn_RecordStop_Click);
            // 
            // Btn_Merge
            // 
            this.Btn_Merge.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.Img_video);
            this.groupBox1.Controls.Add(this.Wmp_1);
            this.groupBox1.Location = new System.Drawing.Point(12, 157);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(953, 425);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // Img_video
            // 
            this.Img_video.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Img_video.BackColor = System.Drawing.Color.Transparent;
            this.Img_video.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Img_video.Location = new System.Drawing.Point(650, 116);
            this.Img_video.Name = "Img_video";
            this.Img_video.Size = new System.Drawing.Size(281, 183);
            this.Img_video.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Img_video.TabIndex = 0;
            this.Img_video.TabStop = false;
            this.Img_video.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Img_video_MouseDown);
            this.Img_video.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Img_video_MouseMove);
            this.Img_video.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Img_video_MouseUp);
            // 
            // Wmp_1
            // 
            this.Wmp_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Wmp_1.Controls.Add(this.TimeSlider);
            this.Wmp_1.Enabled = true;
            this.Wmp_1.Location = new System.Drawing.Point(3, 17);
            this.Wmp_1.Name = "Wmp_1";
            this.Wmp_1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Wmp_1.OcxState")));
            this.Wmp_1.Size = new System.Drawing.Size(623, 399);
            this.Wmp_1.TabIndex = 6;
            this.Wmp_1.Visible = false;
            // 
            // TimeSlider
            // 
            this.TimeSlider.BackColor = System.Drawing.Color.Transparent;
            this.TimeSlider.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TimeSlider.Location = new System.Drawing.Point(0, 364);
            this.TimeSlider.Max = 100;
            this.TimeSlider.Min = 0;
            this.TimeSlider.Name = "TimeSlider";
            this.TimeSlider.SelectedMax = 100;
            this.TimeSlider.SelectedMin = 0;
            this.TimeSlider.Size = new System.Drawing.Size(623, 35);
            this.TimeSlider.TabIndex = 16;
            this.TimeSlider.Value = 50;
            this.TimeSlider.SelectionChanged += new System.EventHandler(this.TimeSlider_SelectionChanged);
            this.TimeSlider.ValueChanged += new System.EventHandler(this.TimeSlider_ValueChanged);
            this.TimeSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimeSlider_MouseDown);
            this.TimeSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimeSlider_MouseMove);
            this.TimeSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimeSlider_MouseUp);
            // 
            // btn_Split
            // 
            this.btn_Split.BackColor = System.Drawing.Color.Transparent;
            this.btn_Split.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Split.Location = new System.Drawing.Point(631, 39);
            this.btn_Split.Name = "btn_Split";
            this.btn_Split.Size = new System.Drawing.Size(92, 23);
            this.btn_Split.TabIndex = 19;
            this.btn_Split.Text = "스플릿";
            this.btn_Split.UseVisualStyleBackColor = false;
            this.btn_Split.Click += new System.EventHandler(this.Btn_Split_Click);
            // 
            // Btn_MergePlay
            // 
            this.Btn_MergePlay.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_MergePlay.Location = new System.Drawing.Point(407, 95);
            this.Btn_MergePlay.Name = "Btn_MergePlay";
            this.Btn_MergePlay.Size = new System.Drawing.Size(106, 23);
            this.Btn_MergePlay.TabIndex = 1;
            this.Btn_MergePlay.Text = "합성 영상 재생";
            this.Btn_MergePlay.UseVisualStyleBackColor = true;
            this.Btn_MergePlay.Click += new System.EventHandler(this.Btn_MergePlay_Click);
            // 
            // Btn_Chromakey
            // 
            this.Btn_Chromakey.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Chromakey.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Chromakey.Location = new System.Drawing.Point(519, 66);
            this.Btn_Chromakey.Name = "Btn_Chromakey";
            this.Btn_Chromakey.Size = new System.Drawing.Size(106, 54);
            this.Btn_Chromakey.TabIndex = 8;
            this.Btn_Chromakey.Text = "크로마키 칼라\r\n";
            this.Btn_Chromakey.UseVisualStyleBackColor = false;
            this.Btn_Chromakey.Click += new System.EventHandler(this.Btn_Chromakey_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Cmb_Monitor);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.Cmb_Cam);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.Cmb_Mic);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(277, 112);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "설정";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Monitor";
            // 
            // Cmb_Monitor
            // 
            this.Cmb_Monitor.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cmb_Monitor.FormattingEnabled = true;
            this.Cmb_Monitor.Location = new System.Drawing.Point(74, 76);
            this.Cmb_Monitor.Name = "Cmb_Monitor";
            this.Cmb_Monitor.Size = new System.Drawing.Size(185, 22);
            this.Cmb_Monitor.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Cam";
            // 
            // Cmb_Cam
            // 
            this.Cmb_Cam.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cmb_Cam.FormattingEnabled = true;
            this.Cmb_Cam.Location = new System.Drawing.Point(74, 50);
            this.Cmb_Cam.Name = "Cmb_Cam";
            this.Cmb_Cam.Size = new System.Drawing.Size(185, 22);
            this.Cmb_Cam.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Mic";
            // 
            // Txt_Title
            // 
            this.Txt_Title.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_Title.Location = new System.Drawing.Point(330, 39);
            this.Txt_Title.Name = "Txt_Title";
            this.Txt_Title.Size = new System.Drawing.Size(295, 23);
            this.Txt_Title.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(295, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = "제목";
            // 
            // Rdb_RightIn
            // 
            this.Rdb_RightIn.Appearance = System.Windows.Forms.Appearance.Button;
            this.Rdb_RightIn.AutoSize = true;
            this.Rdb_RightIn.Image = ((System.Drawing.Image)(resources.GetObject("Rdb_RightIn.Image")));
            this.Rdb_RightIn.Location = new System.Drawing.Point(929, 13);
            this.Rdb_RightIn.Name = "Rdb_RightIn";
            this.Rdb_RightIn.Size = new System.Drawing.Size(111, 65);
            this.Rdb_RightIn.TabIndex = 14;
            this.Rdb_RightIn.TabStop = true;
            this.Rdb_RightIn.UseVisualStyleBackColor = true;
            // 
            // Rdb_RightBottomIn
            // 
            this.Rdb_RightBottomIn.Appearance = System.Windows.Forms.Appearance.Button;
            this.Rdb_RightBottomIn.AutoSize = true;
            this.Rdb_RightBottomIn.Image = ((System.Drawing.Image)(resources.GetObject("Rdb_RightBottomIn.Image")));
            this.Rdb_RightBottomIn.Location = new System.Drawing.Point(1047, 13);
            this.Rdb_RightBottomIn.Name = "Rdb_RightBottomIn";
            this.Rdb_RightBottomIn.Size = new System.Drawing.Size(111, 65);
            this.Rdb_RightBottomIn.TabIndex = 15;
            this.Rdb_RightBottomIn.TabStop = true;
            this.Rdb_RightBottomIn.UseVisualStyleBackColor = true;
            // 
            // Rdb_RightBottomOut
            // 
            this.Rdb_RightBottomOut.Appearance = System.Windows.Forms.Appearance.Button;
            this.Rdb_RightBottomOut.AutoSize = true;
            this.Rdb_RightBottomOut.Image = ((System.Drawing.Image)(resources.GetObject("Rdb_RightBottomOut.Image")));
            this.Rdb_RightBottomOut.Location = new System.Drawing.Point(815, 82);
            this.Rdb_RightBottomOut.Name = "Rdb_RightBottomOut";
            this.Rdb_RightBottomOut.Size = new System.Drawing.Size(111, 65);
            this.Rdb_RightBottomOut.TabIndex = 13;
            this.Rdb_RightBottomOut.TabStop = true;
            this.Rdb_RightBottomOut.UseVisualStyleBackColor = true;
            // 
            // Rdb_DiagonalOut
            // 
            this.Rdb_DiagonalOut.Appearance = System.Windows.Forms.Appearance.Button;
            this.Rdb_DiagonalOut.AutoSize = true;
            this.Rdb_DiagonalOut.Image = ((System.Drawing.Image)(resources.GetObject("Rdb_DiagonalOut.Image")));
            this.Rdb_DiagonalOut.Location = new System.Drawing.Point(929, 82);
            this.Rdb_DiagonalOut.Name = "Rdb_DiagonalOut";
            this.Rdb_DiagonalOut.Size = new System.Drawing.Size(111, 65);
            this.Rdb_DiagonalOut.TabIndex = 14;
            this.Rdb_DiagonalOut.TabStop = true;
            this.Rdb_DiagonalOut.UseVisualStyleBackColor = true;
            // 
            // Rdb_DiagonalIn
            // 
            this.Rdb_DiagonalIn.Appearance = System.Windows.Forms.Appearance.Button;
            this.Rdb_DiagonalIn.AutoSize = true;
            this.Rdb_DiagonalIn.Image = ((System.Drawing.Image)(resources.GetObject("Rdb_DiagonalIn.Image")));
            this.Rdb_DiagonalIn.Location = new System.Drawing.Point(1047, 82);
            this.Rdb_DiagonalIn.Name = "Rdb_DiagonalIn";
            this.Rdb_DiagonalIn.Size = new System.Drawing.Size(100, 65);
            this.Rdb_DiagonalIn.TabIndex = 15;
            this.Rdb_DiagonalIn.TabStop = true;
            this.Rdb_DiagonalIn.UseVisualStyleBackColor = true;
            // 
            // Rdb_RightOut
            // 
            this.Rdb_RightOut.Appearance = System.Windows.Forms.Appearance.Button;
            this.Rdb_RightOut.AutoSize = true;
            this.Rdb_RightOut.Image = ((System.Drawing.Image)(resources.GetObject("Rdb_RightOut.Image")));
            this.Rdb_RightOut.Location = new System.Drawing.Point(815, 13);
            this.Rdb_RightOut.Name = "Rdb_RightOut";
            this.Rdb_RightOut.Size = new System.Drawing.Size(111, 65);
            this.Rdb_RightOut.TabIndex = 13;
            this.Rdb_RightOut.TabStop = true;
            this.Rdb_RightOut.UseVisualStyleBackColor = true;
            // 
            // Img_DeskTop
            // 
            this.Img_DeskTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Img_DeskTop.BackColor = System.Drawing.Color.Transparent;
            this.Img_DeskTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Img_DeskTop.Location = new System.Drawing.Point(18, 175);
            this.Img_DeskTop.Name = "Img_DeskTop";
            this.Img_DeskTop.Size = new System.Drawing.Size(607, 357);
            this.Img_DeskTop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Img_DeskTop.TabIndex = 7;
            this.Img_DeskTop.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(487, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 6;
            // 
            // Btn_Pause
            // 
            this.Btn_Pause.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Pause.Location = new System.Drawing.Point(457, 12);
            this.Btn_Pause.Name = "Btn_Pause";
            this.Btn_Pause.Size = new System.Drawing.Size(75, 23);
            this.Btn_Pause.TabIndex = 18;
            this.Btn_Pause.Text = "일시정지";
            this.Btn_Pause.UseVisualStyleBackColor = true;
            this.Btn_Pause.Click += new System.EventHandler(this.Btn_Pause_Click);
            // 
            // Lbl_Min
            // 
            this.Lbl_Min.AutoSize = true;
            this.Lbl_Min.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Min.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Min.ForeColor = System.Drawing.Color.Red;
            this.Lbl_Min.Location = new System.Drawing.Point(631, 87);
            this.Lbl_Min.Name = "Lbl_Min";
            this.Lbl_Min.Size = new System.Drawing.Size(49, 14);
            this.Lbl_Min.TabIndex = 7;
            this.Lbl_Min.Text = "label6";
            // 
            // Btn_Cut
            // 
            this.Btn_Cut.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Cut.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Cut.Location = new System.Drawing.Point(538, 13);
            this.Btn_Cut.Name = "Btn_Cut";
            this.Btn_Cut.Size = new System.Drawing.Size(75, 23);
            this.Btn_Cut.TabIndex = 19;
            this.Btn_Cut.Text = "자르기";
            this.Btn_Cut.UseVisualStyleBackColor = false;
            this.Btn_Cut.Click += new System.EventHandler(this.Btn_Cut_Click);
            // 
            // Lbl_Max
            // 
            this.Lbl_Max.AutoSize = true;
            this.Lbl_Max.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Max.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Max.Location = new System.Drawing.Point(632, 108);
            this.Lbl_Max.Name = "Lbl_Max";
            this.Lbl_Max.Size = new System.Drawing.Size(49, 14);
            this.Lbl_Max.TabIndex = 20;
            this.Lbl_Max.Text = "label7";
            // 
            // Btn_CutPlay
            // 
            this.Btn_CutPlay.BackColor = System.Drawing.Color.Transparent;
            this.Btn_CutPlay.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_CutPlay.Location = new System.Drawing.Point(619, 13);
            this.Btn_CutPlay.Name = "Btn_CutPlay";
            this.Btn_CutPlay.Size = new System.Drawing.Size(104, 23);
            this.Btn_CutPlay.TabIndex = 21;
            this.Btn_CutPlay.Text = "자른영상재생";
            this.Btn_CutPlay.UseVisualStyleBackColor = false;
            this.Btn_CutPlay.Click += new System.EventHandler(this.Btn_CutPlay_Click);
            // 
            // Lbl_Current
            // 
            this.Lbl_Current.AutoSize = true;
            this.Lbl_Current.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Current.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lbl_Current.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Lbl_Current.Location = new System.Drawing.Point(632, 132);
            this.Lbl_Current.Name = "Lbl_Current";
            this.Lbl_Current.Size = new System.Drawing.Size(84, 15);
            this.Lbl_Current.TabIndex = 22;
            this.Lbl_Current.Text = "Lbl_Current";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(987, 607);
            this.Controls.Add(this.btn_Split);
            this.Controls.Add(this.Lbl_Current);
            this.Controls.Add(this.Lbl_Min);
            this.Controls.Add(this.Btn_CutPlay);
            this.Controls.Add(this.Lbl_Max);
            this.Controls.Add(this.Btn_Cut);
            this.Controls.Add(this.Btn_Pause);
            this.Controls.Add(this.Rdb_DiagonalIn);
            this.Controls.Add(this.Rdb_RightBottomIn);
            this.Controls.Add(this.Rdb_DiagonalOut);
            this.Controls.Add(this.Rdb_RightIn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Rdb_RightOut);
            this.Controls.Add(this.Rdb_RightBottomOut);
            this.Controls.Add(this.Img_DeskTop);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Txt_Title);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Btn_Chromakey);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Txt_URL);
            this.Controls.Add(this.Btn_Stop);
            this.Controls.Add(this.Btn_Play);
            this.Controls.Add(this.Btn_MergePlay);
            this.Controls.Add(this.Btn_Merge);
            this.Controls.Add(this.Btn_RecordStop);
            this.Controls.Add(this.Btn_Record);
            this.Name = "MainForm";
            this.Text = "JJCastDemo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Img_video)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Wmp_1)).EndInit();
            this.Wmp_1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Img_DeskTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xpDataView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Img_video;
        private System.Windows.Forms.Button Btn_Play;
        private System.Windows.Forms.Button Btn_Stop;
        private System.Windows.Forms.TextBox Txt_URL;
        private System.Windows.Forms.ComboBox Cmb_Mic;
        private System.Windows.Forms.Button Btn_Record;
        private AxWMPLib.AxWindowsMediaPlayer Wmp_1;
        private System.Windows.Forms.Button Btn_RecordStop;
        private System.Windows.Forms.Button Btn_Merge;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Btn_MergePlay;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button Btn_Chromakey;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Cmb_Monitor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Cmb_Cam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Txt_Title;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox Img_DeskTop;
        private System.Windows.Forms.RadioButton Rdb_RightIn;
        private System.Windows.Forms.RadioButton Rdb_RightBottomIn;
        private System.Windows.Forms.RadioButton Rdb_RightBottomOut;
        private System.Windows.Forms.RadioButton Rdb_DiagonalOut;
        private System.Windows.Forms.RadioButton Rdb_DiagonalIn;
        private System.Windows.Forms.RadioButton Rdb_RightOut;
        private System.Windows.Forms.Label label5;
        private UIControl.SelectionRangeSlider TimeSlider;
        private System.Windows.Forms.Button Btn_Pause;
        private System.Windows.Forms.Label Lbl_Min;
        private System.Windows.Forms.Button Btn_Cut;
        private System.Windows.Forms.Label Lbl_Max;
        private System.Windows.Forms.Button btn_Split;
        private System.Windows.Forms.Button Btn_CutPlay;
        private System.Windows.Forms.Label Lbl_Current;
        private DevExpress.Xpo.XPDataView xpDataView1;
    }
}
