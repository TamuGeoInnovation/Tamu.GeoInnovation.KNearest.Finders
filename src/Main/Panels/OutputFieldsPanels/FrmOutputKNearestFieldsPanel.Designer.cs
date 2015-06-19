namespace USC.GISResearchLab.Common.KNearest.Panels.OutputFieldPanels
{
    partial class FrmOutputKNearestFieldsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOutputKNearestFieldsPanel));
            this.label81 = new System.Windows.Forms.Label();
            this.cboCountyFips = new System.Windows.Forms.ComboBox();
            this.label41 = new System.Windows.Forms.Label();
            this.cboTract = new System.Windows.Forms.ComboBox();
            this.label47 = new System.Windows.Forms.Label();
            this.chkInclude = new System.Windows.Forms.CheckBox();
            this.label42 = new System.Windows.Forms.Label();
            this.cboBlock = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.cboStateFips = new System.Windows.Forms.ComboBox();
            this.cboBlockGroup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboCBSAFips = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboCBSAMicro = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboMCDFips = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboMetDivFips = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboPlaceFips = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboMSAFips = new System.Windows.Forms.ComboBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Location = new System.Drawing.Point(45, 225);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(62, 13);
            this.label81.TabIndex = 174;
            this.label81.Text = "County Fips";
            // 
            // cboCountyFips
            // 
            this.cboCountyFips.FormattingEnabled = true;
            this.cboCountyFips.Location = new System.Drawing.Point(113, 222);
            this.cboCountyFips.Name = "cboCountyFips";
            this.cboCountyFips.Size = new System.Drawing.Size(163, 21);
            this.cboCountyFips.TabIndex = 3;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(53, 246);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(54, 13);
            this.label41.TabIndex = 149;
            this.label41.Text = "State Fips";
            // 
            // cboTract
            // 
            this.cboTract.FormattingEnabled = true;
            this.cboTract.Location = new System.Drawing.Point(113, 34);
            this.cboTract.Name = "cboTract";
            this.cboTract.Size = new System.Drawing.Size(163, 21);
            this.cboTract.TabIndex = 4;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(73, 79);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(34, 13);
            this.label47.TabIndex = 157;
            this.label47.Text = "Block";
            // 
            // chkInclude
            // 
            this.chkInclude.AutoSize = true;
            this.chkInclude.Location = new System.Drawing.Point(113, 11);
            this.chkInclude.Name = "chkInclude";
            this.chkInclude.Size = new System.Drawing.Size(61, 17);
            this.chkInclude.TabIndex = 1;
            this.chkInclude.Text = "Include";
            this.chkInclude.UseVisualStyleBackColor = true;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(75, 37);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(32, 13);
            this.label42.TabIndex = 147;
            this.label42.Text = "Tract";
            // 
            // cboBlock
            // 
            this.cboBlock.FormattingEnabled = true;
            this.cboBlock.Location = new System.Drawing.Point(113, 76);
            this.cboBlock.Name = "cboBlock";
            this.cboBlock.Size = new System.Drawing.Size(163, 21);
            this.cboBlock.TabIndex = 6;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(186, 288);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(90, 22);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Auto Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cboStateFips
            // 
            this.cboStateFips.FormattingEnabled = true;
            this.cboStateFips.Location = new System.Drawing.Point(113, 243);
            this.cboStateFips.Name = "cboStateFips";
            this.cboStateFips.Size = new System.Drawing.Size(163, 21);
            this.cboStateFips.TabIndex = 2;
            // 
            // cboBlockGroup
            // 
            this.cboBlockGroup.FormattingEnabled = true;
            this.cboBlockGroup.Location = new System.Drawing.Point(113, 55);
            this.cboBlockGroup.Name = "cboBlockGroup";
            this.cboBlockGroup.Size = new System.Drawing.Size(163, 21);
            this.cboBlockGroup.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 188;
            this.label1.Text = "Block Group";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 190;
            this.label2.Text = "CBSA Fips";
            // 
            // cboCBSAFips
            // 
            this.cboCBSAFips.FormattingEnabled = true;
            this.cboCBSAFips.Location = new System.Drawing.Point(113, 96);
            this.cboCBSAFips.Name = "cboCBSAFips";
            this.cboCBSAFips.Size = new System.Drawing.Size(163, 21);
            this.cboCBSAFips.TabIndex = 189;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 192;
            this.label3.Text = "CBSA Micro";
            // 
            // cboCBSAMicro
            // 
            this.cboCBSAMicro.FormattingEnabled = true;
            this.cboCBSAMicro.Location = new System.Drawing.Point(113, 117);
            this.cboCBSAMicro.Name = "cboCBSAMicro";
            this.cboCBSAMicro.Size = new System.Drawing.Size(163, 21);
            this.cboCBSAMicro.TabIndex = 191;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 194;
            this.label4.Text = "MCD Fips";
            // 
            // cboMCDFips
            // 
            this.cboMCDFips.FormattingEnabled = true;
            this.cboMCDFips.Location = new System.Drawing.Point(113, 138);
            this.cboMCDFips.Name = "cboMCDFips";
            this.cboMCDFips.Size = new System.Drawing.Size(163, 21);
            this.cboMCDFips.TabIndex = 193;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 196;
            this.label5.Text = "MetDiv Fips";
            // 
            // cboMetDivFips
            // 
            this.cboMetDivFips.FormattingEnabled = true;
            this.cboMetDivFips.Location = new System.Drawing.Point(113, 159);
            this.cboMetDivFips.Name = "cboMetDivFips";
            this.cboMetDivFips.Size = new System.Drawing.Size(163, 21);
            this.cboMetDivFips.TabIndex = 195;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(51, 204);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 198;
            this.label6.Text = "Place Fips";
            // 
            // cboPlaceFips
            // 
            this.cboPlaceFips.FormattingEnabled = true;
            this.cboPlaceFips.Location = new System.Drawing.Point(113, 201);
            this.cboPlaceFips.Name = "cboPlaceFips";
            this.cboPlaceFips.Size = new System.Drawing.Size(163, 21);
            this.cboPlaceFips.TabIndex = 197;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(55, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 200;
            this.label7.Text = "MSA Fips";
            // 
            // cboMSAFips
            // 
            this.cboMSAFips.FormattingEnabled = true;
            this.cboMSAFips.Location = new System.Drawing.Point(113, 180);
            this.cboMSAFips.Name = "cboMSAFips";
            this.cboMSAFips.Size = new System.Drawing.Size(163, 21);
            this.cboMSAFips.TabIndex = 199;
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(80, 290);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(100, 20);
            this.txtPrefix.TabIndex = 204;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(41, 293);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 203;
            this.label8.Text = "Prefix";
            // 
            // FrmOutputCensusFieldsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 322);
            this.Controls.Add(this.txtPrefix);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboMSAFips);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboPlaceFips);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboMetDivFips);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboMCDFips);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboCBSAMicro);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboCBSAFips);
            this.Controls.Add(this.cboBlockGroup);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label81);
            this.Controls.Add(this.cboCountyFips);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.cboTract);
            this.Controls.Add(this.label47);
            this.Controls.Add(this.chkInclude);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.cboBlock);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cboStateFips);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmOutputCensusFieldsPanel";
            this.ShowInTaskbar = false;
            this.Text = "Output Fields";
            this.Load += new System.EventHandler(this.FrmOutputAddressFieldsPanel_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmOutputAddressFieldsPanel_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.ComboBox cboCountyFips;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.ComboBox cboTract;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.CheckBox chkInclude;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.ComboBox cboBlock;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ComboBox cboStateFips;
        private System.Windows.Forms.ComboBox cboBlockGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboCBSAFips;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboCBSAMicro;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboMCDFips;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboMetDivFips;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboPlaceFips;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboMSAFips;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.Label label8;
    }
}