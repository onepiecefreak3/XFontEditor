namespace L5XFEditor
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.largeDict = new System.Windows.Forms.DataGridView();
            this.Character = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuTool = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallDict = new System.Windows.Forms.DataGridView();
            this.Character2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImageID2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Width2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Height2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imgSelector = new System.Windows.Forms.ComboBox();
            this.searchTextBox1 = new System.Windows.Forms.TextBox();
            this.coordinatesLabel = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.image0 = new Cyotek.Windows.Forms.ImageBox();
            this.image1 = new Cyotek.Windows.Forms.ImageBox();
            this.image2 = new Cyotek.Windows.Forms.ImageBox();
            this.searchTextBox2 = new System.Windows.Forms.TextBox();
            this.searchCharLabel1 = new System.Windows.Forms.Label();
            this.searchCharLabel2 = new System.Windows.Forms.Label();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.largeDict)).BeginInit();
            this.menuTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smallDict)).BeginInit();
            this.SuspendLayout();
            // 
            // largeDict
            // 
            this.largeDict.AllowUserToAddRows = false;
            this.largeDict.AllowUserToDeleteRows = false;
            this.largeDict.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.largeDict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.largeDict.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Character,
            this.ImageID,
            this.X,
            this.Y,
            this.Width,
            this.Height});
            this.largeDict.Location = new System.Drawing.Point(539, 54);
            this.largeDict.Name = "largeDict";
            this.largeDict.Size = new System.Drawing.Size(520, 240);
            this.largeDict.TabIndex = 0;
            // 
            // Character
            // 
            this.Character.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Character.HeaderText = "Character";
            this.Character.Name = "Character";
            this.Character.ReadOnly = true;
            this.Character.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ImageID
            // 
            this.ImageID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ImageID.HeaderText = "Image";
            this.ImageID.Name = "ImageID";
            this.ImageID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // X
            // 
            this.X.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Y
            // 
            this.Y.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Width
            // 
            this.Width.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Width.HeaderText = "Width";
            this.Width.Name = "Width";
            this.Width.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Height
            // 
            this.Height.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Height.HeaderText = "Height";
            this.Height.Name = "Height";
            this.Height.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // menuTool
            // 
            this.menuTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.imageToolStripMenuItem});
            this.menuTool.Location = new System.Drawing.Point(0, 0);
            this.menuTool.Name = "menuTool";
            this.menuTool.Size = new System.Drawing.Size(1071, 24);
            this.menuTool.TabIndex = 1;
            this.menuTool.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.importToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Enabled = false;
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.extractToolStripMenuItem.Text = "&Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Enabled = false;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "&Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // smallDict
            // 
            this.smallDict.AllowUserToAddRows = false;
            this.smallDict.AllowUserToDeleteRows = false;
            this.smallDict.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.smallDict.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.smallDict.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Character2,
            this.ImageID2,
            this.X2,
            this.Y2,
            this.Width2,
            this.Height2});
            this.smallDict.Location = new System.Drawing.Point(539, 326);
            this.smallDict.Name = "smallDict";
            this.smallDict.Size = new System.Drawing.Size(520, 240);
            this.smallDict.TabIndex = 2;
            // 
            // Character2
            // 
            this.Character2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Character2.HeaderText = "Character";
            this.Character2.Name = "Character2";
            this.Character2.ReadOnly = true;
            this.Character2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ImageID2
            // 
            this.ImageID2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ImageID2.HeaderText = "Image";
            this.ImageID2.Name = "ImageID2";
            this.ImageID2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // X2
            // 
            this.X2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.X2.HeaderText = "X";
            this.X2.Name = "X2";
            this.X2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Y2
            // 
            this.Y2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Y2.HeaderText = "Y";
            this.Y2.Name = "Y2";
            this.Y2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Width2
            // 
            this.Width2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Width2.HeaderText = "Width";
            this.Width2.Name = "Width2";
            this.Width2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Height2
            // 
            this.Height2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Height2.HeaderText = "Height";
            this.Height2.Name = "Height2";
            this.Height2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // imgSelector
            // 
            this.imgSelector.FormattingEnabled = true;
            this.imgSelector.Items.AddRange(new object[] {
            "Image 0",
            "Image 1",
            "Image 2"});
            this.imgSelector.Location = new System.Drawing.Point(15, 27);
            this.imgSelector.Name = "imgSelector";
            this.imgSelector.Size = new System.Drawing.Size(121, 21);
            this.imgSelector.TabIndex = 6;
            this.imgSelector.Text = "Image 0";
            this.imgSelector.SelectedIndexChanged += new System.EventHandler(this.imgSelector_SelectedIndexChanged);
            // 
            // searchTextBox1
            // 
            this.searchTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox1.Location = new System.Drawing.Point(635, 28);
            this.searchTextBox1.Name = "searchTextBox1";
            this.searchTextBox1.Size = new System.Drawing.Size(100, 20);
            this.searchTextBox1.TabIndex = 7;
            this.searchTextBox1.TextChanged += new System.EventHandler(this.searchTextBox1_TextChanged);
            // 
            // coordinatesLabel
            // 
            this.coordinatesLabel.AutoSize = true;
            this.coordinatesLabel.Location = new System.Drawing.Point(142, 30);
            this.coordinatesLabel.Name = "coordinatesLabel";
            this.coordinatesLabel.Size = new System.Drawing.Size(0, 13);
            this.coordinatesLabel.TabIndex = 8;
            // 
            // image0
            // 
            this.image0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.image0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.image0.GridCellSize = 16;
            this.image0.GridColor = System.Drawing.Color.Black;
            this.image0.GridColorAlternate = System.Drawing.Color.Black;
            this.image0.ImageBorderColor = System.Drawing.Color.Black;
            this.image0.ImageBorderStyle = Cyotek.Windows.Forms.ImageBoxBorderStyle.FixedSingleDropShadow;
            this.image0.Location = new System.Drawing.Point(15, 54);
            this.image0.Name = "image0";
            this.image0.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Zoom;
            this.image0.Size = new System.Drawing.Size(512, 512);
            this.image0.TabIndex = 6;
            this.image0.MouseMove += new System.Windows.Forms.MouseEventHandler(this.image0_MouseMove);
            // 
            // image1
            // 
            this.image1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.image1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.image1.GridCellSize = 16;
            this.image1.GridColor = System.Drawing.Color.Black;
            this.image1.GridColorAlternate = System.Drawing.Color.Black;
            this.image1.ImageBorderColor = System.Drawing.Color.Black;
            this.image1.ImageBorderStyle = Cyotek.Windows.Forms.ImageBoxBorderStyle.FixedSingleDropShadow;
            this.image1.Location = new System.Drawing.Point(15, 54);
            this.image1.Name = "image1";
            this.image1.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Zoom;
            this.image1.Size = new System.Drawing.Size(512, 512);
            this.image1.TabIndex = 6;
            this.image1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.image1_MouseMove);
            // 
            // image2
            // 
            this.image2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.image2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.image2.GridCellSize = 16;
            this.image2.GridColor = System.Drawing.Color.Black;
            this.image2.GridColorAlternate = System.Drawing.Color.Black;
            this.image2.ImageBorderColor = System.Drawing.Color.Black;
            this.image2.ImageBorderStyle = Cyotek.Windows.Forms.ImageBoxBorderStyle.FixedSingleDropShadow;
            this.image2.Location = new System.Drawing.Point(15, 54);
            this.image2.Name = "image2";
            this.image2.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Zoom;
            this.image2.Size = new System.Drawing.Size(512, 512);
            this.image2.TabIndex = 6;
            this.image2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.image2_MouseMove);
            // 
            // searchTextBox2
            // 
            this.searchTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox2.Location = new System.Drawing.Point(635, 300);
            this.searchTextBox2.Name = "searchTextBox2";
            this.searchTextBox2.Size = new System.Drawing.Size(100, 20);
            this.searchTextBox2.TabIndex = 9;
            this.searchTextBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // searchCharLabel1
            // 
            this.searchCharLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchCharLabel1.AutoSize = true;
            this.searchCharLabel1.Location = new System.Drawing.Point(536, 31);
            this.searchCharLabel1.Name = "searchCharLabel1";
            this.searchCharLabel1.Size = new System.Drawing.Size(93, 13);
            this.searchCharLabel1.TabIndex = 10;
            this.searchCharLabel1.Text = "Search Character:";
            // 
            // searchCharLabel2
            // 
            this.searchCharLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchCharLabel2.AutoSize = true;
            this.searchCharLabel2.Location = new System.Drawing.Point(536, 303);
            this.searchCharLabel2.Name = "searchCharLabel2";
            this.searchCharLabel2.Size = new System.Drawing.Size(93, 13);
            this.searchCharLabel2.TabIndex = 11;
            this.searchCharLabel2.Text = "Search Character:";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 582);
            this.Controls.Add(this.searchCharLabel2);
            this.Controls.Add(this.searchCharLabel1);
            this.Controls.Add(this.searchTextBox2);
            this.Controls.Add(this.image1);
            this.Controls.Add(this.image0);
            this.Controls.Add(this.coordinatesLabel);
            this.Controls.Add(this.searchTextBox1);
            this.Controls.Add(this.imgSelector);
            this.Controls.Add(this.smallDict);
            this.Controls.Add(this.largeDict);
            this.Controls.Add(this.menuTool);
            this.Controls.Add(this.image2);
            this.MainMenuStrip = this.menuTool;
            this.Name = "Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Main_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Main_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.largeDict)).EndInit();
            this.menuTool.ResumeLayout(false);
            this.menuTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smallDict)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView largeDict;
        private System.Windows.Forms.MenuStrip menuTool;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.DataGridView smallDict;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ComboBox imgSelector;
        private System.Windows.Forms.TextBox searchTextBox1;
        private System.Windows.Forms.Label coordinatesLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Cyotek.Windows.Forms.ImageBox image0;
        private Cyotek.Windows.Forms.ImageBox image1;
        private Cyotek.Windows.Forms.ImageBox image2;
        private System.Windows.Forms.TextBox searchTextBox2;
        private System.Windows.Forms.Label searchCharLabel1;
        private System.Windows.Forms.Label searchCharLabel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Character;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImageID;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewTextBoxColumn Width;
        private System.Windows.Forms.DataGridViewTextBoxColumn Height;
        private System.Windows.Forms.DataGridViewTextBoxColumn Character2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImageID2;
        private System.Windows.Forms.DataGridViewTextBoxColumn X2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Width2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Height2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    }
}

