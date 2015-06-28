namespace GSI
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetTrainingSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTrainingSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectValidationSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recommandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRolloutFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runButton = new System.Windows.Forms.Button();
            this.trainingSetGrid = new System.Windows.Forms.DataGridView();
            this.pagingStartButton = new System.Windows.Forms.Button();
            this.pagingPreviousButton = new System.Windows.Forms.Button();
            this.pagingNextButton = new System.Windows.Forms.Button();
            this.pagingEndButton = new System.Windows.Forms.Button();
            this.pagingTextBox = new System.Windows.Forms.TextBox();
            this.progressLabel = new System.Windows.Forms.Label();
            this.chooseIdColumnLabel = new System.Windows.Forms.Label();
            this.chooseResultColumnLabel = new System.Windows.Forms.Label();
            this.resultColumnComboBox = new System.Windows.Forms.ComboBox();
            this.idColumnComboBox = new System.Windows.Forms.ComboBox();
            this.testSetButton = new System.Windows.Forms.Button();
            this.trainingConfusionMatrixLabel = new System.Windows.Forms.Label();
            this.validationConfisionMatrixLabel = new System.Windows.Forms.Label();
            this.rangeTextBox = new System.Windows.Forms.TextBox();
            this.rangeLabel = new System.Windows.Forms.Label();
            this.rolloutButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trainingSetGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "csv";
            this.openFileDialog1.InitialDirectory = "C:\\Users\\Mr Mush\\Dropbox\\MBA\\Data Mining and Knowledge Discovery\\Final Project\\C\\" +
    "Code\\GSI\\GSI\\ExcelFiles";
            this.openFileDialog1.Title = "Select training file";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.testToolStripMenuItem,
            this.recommandToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(952, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetTrainingSetToolStripMenuItem,
            this.selectTrainingSetToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // resetTrainingSetToolStripMenuItem
            // 
            this.resetTrainingSetToolStripMenuItem.Name = "resetTrainingSetToolStripMenuItem";
            this.resetTrainingSetToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.resetTrainingSetToolStripMenuItem.Text = "Reset Training Set";
            this.resetTrainingSetToolStripMenuItem.Click += new System.EventHandler(this.resetTrainingSetToolStripMenuItem_Click);
            // 
            // selectTrainingSetToolStripMenuItem
            // 
            this.selectTrainingSetToolStripMenuItem.AutoSize = false;
            this.selectTrainingSetToolStripMenuItem.Name = "selectTrainingSetToolStripMenuItem";
            this.selectTrainingSetToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.selectTrainingSetToolStripMenuItem.Text = "Select Training Set";
            this.selectTrainingSetToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectValidationSetToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.testToolStripMenuItem.Text = "Test";
            // 
            // selectValidationSetToolStripMenuItem
            // 
            this.selectValidationSetToolStripMenuItem.Name = "selectValidationSetToolStripMenuItem";
            this.selectValidationSetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectValidationSetToolStripMenuItem.Text = "Select Validation Set";
            this.selectValidationSetToolStripMenuItem.Click += new System.EventHandler(this.selectValidationSetToolStripMenuItem_Click);
            // 
            // recommandToolStripMenuItem
            // 
            this.recommandToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectRolloutFileToolStripMenuItem});
            this.recommandToolStripMenuItem.Name = "recommandToolStripMenuItem";
            this.recommandToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.recommandToolStripMenuItem.Text = "Recommand";
            // 
            // selectRolloutFileToolStripMenuItem
            // 
            this.selectRolloutFileToolStripMenuItem.Name = "selectRolloutFileToolStripMenuItem";
            this.selectRolloutFileToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.selectRolloutFileToolStripMenuItem.Text = "Select RolloutSet";
            this.selectRolloutFileToolStripMenuItem.Click += new System.EventHandler(this.selectRolloutFileToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // runButton
            // 
            this.runButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.runButton.Enabled = false;
            this.runButton.Location = new System.Drawing.Point(865, 633);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(75, 23);
            this.runButton.TabIndex = 1;
            this.runButton.Text = "Run GSI";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // trainingSetGrid
            // 
            this.trainingSetGrid.AllowUserToAddRows = false;
            this.trainingSetGrid.AllowUserToDeleteRows = false;
            this.trainingSetGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trainingSetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.trainingSetGrid.Location = new System.Drawing.Point(12, 27);
            this.trainingSetGrid.Name = "trainingSetGrid";
            this.trainingSetGrid.ReadOnly = true;
            this.trainingSetGrid.Size = new System.Drawing.Size(928, 541);
            this.trainingSetGrid.TabIndex = 2;
            // 
            // pagingStartButton
            // 
            this.pagingStartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pagingStartButton.Location = new System.Drawing.Point(13, 634);
            this.pagingStartButton.Name = "pagingStartButton";
            this.pagingStartButton.Size = new System.Drawing.Size(37, 23);
            this.pagingStartButton.TabIndex = 3;
            this.pagingStartButton.Text = "| <<";
            this.pagingStartButton.UseVisualStyleBackColor = true;
            this.pagingStartButton.Click += new System.EventHandler(this.pagingStartButton_Click);
            // 
            // pagingPreviousButton
            // 
            this.pagingPreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pagingPreviousButton.Location = new System.Drawing.Point(56, 634);
            this.pagingPreviousButton.Name = "pagingPreviousButton";
            this.pagingPreviousButton.Size = new System.Drawing.Size(35, 23);
            this.pagingPreviousButton.TabIndex = 4;
            this.pagingPreviousButton.Text = "<<";
            this.pagingPreviousButton.UseVisualStyleBackColor = true;
            this.pagingPreviousButton.Click += new System.EventHandler(this.pagingPreviousButton_Click);
            // 
            // pagingNextButton
            // 
            this.pagingNextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pagingNextButton.Location = new System.Drawing.Point(158, 635);
            this.pagingNextButton.Name = "pagingNextButton";
            this.pagingNextButton.Size = new System.Drawing.Size(40, 23);
            this.pagingNextButton.TabIndex = 5;
            this.pagingNextButton.Text = ">>";
            this.pagingNextButton.UseVisualStyleBackColor = true;
            this.pagingNextButton.Click += new System.EventHandler(this.pagingNextButton_Click);
            // 
            // pagingEndButton
            // 
            this.pagingEndButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pagingEndButton.Location = new System.Drawing.Point(204, 635);
            this.pagingEndButton.Name = "pagingEndButton";
            this.pagingEndButton.Size = new System.Drawing.Size(38, 23);
            this.pagingEndButton.TabIndex = 6;
            this.pagingEndButton.Text = ">> |";
            this.pagingEndButton.UseVisualStyleBackColor = true;
            this.pagingEndButton.Click += new System.EventHandler(this.pagingEndButton_Click);
            // 
            // pagingTextBox
            // 
            this.pagingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pagingTextBox.Enabled = false;
            this.pagingTextBox.Location = new System.Drawing.Point(98, 635);
            this.pagingTextBox.Name = "pagingTextBox";
            this.pagingTextBox.Size = new System.Drawing.Size(54, 20);
            this.pagingTextBox.TabIndex = 7;
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(248, 640);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(27, 13);
            this.progressLabel.TabIndex = 8;
            this.progressLabel.Text = "N\\A";
            // 
            // chooseIdColumnLabel
            // 
            this.chooseIdColumnLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chooseIdColumnLabel.AutoSize = true;
            this.chooseIdColumnLabel.Location = new System.Drawing.Point(12, 576);
            this.chooseIdColumnLabel.Name = "chooseIdColumnLabel";
            this.chooseIdColumnLabel.Size = new System.Drawing.Size(104, 13);
            this.chooseIdColumnLabel.TabIndex = 9;
            this.chooseIdColumnLabel.Text = "Choose ID Column - ";
            // 
            // chooseResultColumnLabel
            // 
            this.chooseResultColumnLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chooseResultColumnLabel.AutoSize = true;
            this.chooseResultColumnLabel.Location = new System.Drawing.Point(13, 606);
            this.chooseResultColumnLabel.Name = "chooseResultColumnLabel";
            this.chooseResultColumnLabel.Size = new System.Drawing.Size(120, 13);
            this.chooseResultColumnLabel.TabIndex = 10;
            this.chooseResultColumnLabel.Text = "Choose Result Column -";
            // 
            // resultColumnComboBox
            // 
            this.resultColumnComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resultColumnComboBox.FormattingEnabled = true;
            this.resultColumnComboBox.Location = new System.Drawing.Point(139, 603);
            this.resultColumnComboBox.Name = "resultColumnComboBox";
            this.resultColumnComboBox.Size = new System.Drawing.Size(166, 21);
            this.resultColumnComboBox.TabIndex = 11;
            // 
            // idColumnComboBox
            // 
            this.idColumnComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.idColumnComboBox.FormattingEnabled = true;
            this.idColumnComboBox.Location = new System.Drawing.Point(139, 574);
            this.idColumnComboBox.Name = "idColumnComboBox";
            this.idColumnComboBox.Size = new System.Drawing.Size(166, 21);
            this.idColumnComboBox.TabIndex = 12;
            // 
            // testSetButton
            // 
            this.testSetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.testSetButton.Enabled = false;
            this.testSetButton.Location = new System.Drawing.Point(865, 601);
            this.testSetButton.Name = "testSetButton";
            this.testSetButton.Size = new System.Drawing.Size(75, 23);
            this.testSetButton.TabIndex = 13;
            this.testSetButton.Text = "Run Test Set";
            this.testSetButton.UseVisualStyleBackColor = true;
            this.testSetButton.Visible = false;
            this.testSetButton.Click += new System.EventHandler(this.testSetButton_Click);
            // 
            // trainingConfusionMatrixLabel
            // 
            this.trainingConfusionMatrixLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trainingConfusionMatrixLabel.AutoSize = true;
            this.trainingConfusionMatrixLabel.Location = new System.Drawing.Point(322, 582);
            this.trainingConfusionMatrixLabel.Name = "trainingConfusionMatrixLabel";
            this.trainingConfusionMatrixLabel.Size = new System.Drawing.Size(94, 13);
            this.trainingConfusionMatrixLabel.TabIndex = 14;
            this.trainingConfusionMatrixLabel.Text = "Not Evaluated Yet";
            // 
            // validationConfisionMatrixLabel
            // 
            this.validationConfisionMatrixLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.validationConfisionMatrixLabel.AutoSize = true;
            this.validationConfisionMatrixLabel.Location = new System.Drawing.Point(611, 582);
            this.validationConfisionMatrixLabel.Name = "validationConfisionMatrixLabel";
            this.validationConfisionMatrixLabel.Size = new System.Drawing.Size(94, 13);
            this.validationConfisionMatrixLabel.TabIndex = 15;
            this.validationConfisionMatrixLabel.Text = "Not Evaluated Yet";
            // 
            // rangeTextBox
            // 
            this.rangeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rangeTextBox.Location = new System.Drawing.Point(394, 637);
            this.rangeTextBox.Name = "rangeTextBox";
            this.rangeTextBox.Size = new System.Drawing.Size(45, 20);
            this.rangeTextBox.TabIndex = 16;
            this.rangeTextBox.Text = "5";
            // 
            // rangeLabel
            // 
            this.rangeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rangeLabel.AutoSize = true;
            this.rangeLabel.Location = new System.Drawing.Point(322, 640);
            this.rangeLabel.Name = "rangeLabel";
            this.rangeLabel.Size = new System.Drawing.Size(39, 13);
            this.rangeLabel.TabIndex = 17;
            this.rangeLabel.Text = "Range";
            // 
            // rolloutButton
            // 
            this.rolloutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rolloutButton.Enabled = false;
            this.rolloutButton.Location = new System.Drawing.Point(865, 571);
            this.rolloutButton.Name = "rolloutButton";
            this.rolloutButton.Size = new System.Drawing.Size(75, 23);
            this.rolloutButton.TabIndex = 18;
            this.rolloutButton.Text = "Run Rollout";
            this.rolloutButton.UseVisualStyleBackColor = true;
            this.rolloutButton.Visible = false;
            this.rolloutButton.Click += new System.EventHandler(this.rolloutButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 668);
            this.Controls.Add(this.rolloutButton);
            this.Controls.Add(this.rangeLabel);
            this.Controls.Add(this.rangeTextBox);
            this.Controls.Add(this.validationConfisionMatrixLabel);
            this.Controls.Add(this.trainingConfusionMatrixLabel);
            this.Controls.Add(this.testSetButton);
            this.Controls.Add(this.idColumnComboBox);
            this.Controls.Add(this.resultColumnComboBox);
            this.Controls.Add(this.chooseResultColumnLabel);
            this.Controls.Add(this.chooseIdColumnLabel);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.pagingTextBox);
            this.Controls.Add(this.pagingEndButton);
            this.Controls.Add(this.pagingNextButton);
            this.Controls.Add(this.pagingPreviousButton);
            this.Controls.Add(this.pagingStartButton);
            this.Controls.Add(this.trainingSetGrid);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "GSI - Main";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trainingSetGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetTrainingSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTrainingSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.DataGridView trainingSetGrid;
        private System.Windows.Forms.ToolStripMenuItem selectValidationSetToolStripMenuItem;
        private System.Windows.Forms.Button pagingStartButton;
        private System.Windows.Forms.Button pagingPreviousButton;
        private System.Windows.Forms.Button pagingNextButton;
        private System.Windows.Forms.Button pagingEndButton;
        private System.Windows.Forms.TextBox pagingTextBox;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Label chooseIdColumnLabel;
        private System.Windows.Forms.Label chooseResultColumnLabel;
        private System.Windows.Forms.ComboBox resultColumnComboBox;
        private System.Windows.Forms.ComboBox idColumnComboBox;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.Button testSetButton;
        private System.Windows.Forms.Label trainingConfusionMatrixLabel;
        private System.Windows.Forms.Label validationConfisionMatrixLabel;
        private System.Windows.Forms.TextBox rangeTextBox;
        private System.Windows.Forms.Label rangeLabel;
        private System.Windows.Forms.ToolStripMenuItem recommandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectRolloutFileToolStripMenuItem;
        private System.Windows.Forms.Button rolloutButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

