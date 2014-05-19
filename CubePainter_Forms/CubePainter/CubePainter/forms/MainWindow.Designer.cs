using CubePainter;
namespace CubeStudio
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openCubeStudioFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.painterDisplay1 = new CubePainter.PainterDisplay();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.animatorDisplay1 = new CubeStudio.AnimatorDisplay();
            this.nodeRightCLickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nodeContextMenu1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openMenuOption_nodeContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FileTreeRefreshButton = new System.Windows.Forms.Button();
            this.characterTreeView = new System.Windows.Forms.TreeView();
            this.nodeContextCharacterMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openCharacterOptionNodeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.BodyPartTreeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removePartButton = new System.Windows.Forms.ToolStripMenuItem();
            this.replacePartBosyPartTreeMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.changePartTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.nodeContextMenu1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.nodeContextCharacterMenu.SuspendLayout();
            this.BodyPartTreeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.Gray;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.treeView1.HideSelection = false;
            this.treeView1.LineColor = System.Drawing.Color.DarkGray;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(235, 448);
            this.treeView1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(1026, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(178, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.Silver;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.openCubeStudioFolder});
            this.fileToolStripMenuItem.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 24);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.ToolTipText = "Create a new voxel space";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.ToolTipText = "Open an existing voxel space";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.ToolTipText = "Save this voxel space";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.ToolTipText = "Save this voxel space as a new file";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // openCubeStudioFolder
            // 
            this.openCubeStudioFolder.Name = "openCubeStudioFolder";
            this.openCubeStudioFolder.Size = new System.Drawing.Size(202, 22);
            this.openCubeStudioFolder.Text = "Show CubeStudio folder";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.BackColor = System.Drawing.Color.Silver;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayControlsToolStripMenuItem});
            this.helpToolStripMenuItem.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // displayControlsToolStripMenuItem
            // 
            this.displayControlsToolStripMenuItem.Name = "displayControlsToolStripMenuItem";
            this.displayControlsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.displayControlsToolStripMenuItem.Text = "Display controls";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.HotTrack = true;
            this.tabControl1.Location = new System.Drawing.Point(-2, -1);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1030, 684);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.AllowDrop = true;
            this.tabPage1.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage1.Controls.Add(this.painterDisplay1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1022, 658);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Painter";
            // 
            // painterDisplay1
            // 
            this.painterDisplay1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.painterDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.painterDisplay1.Location = new System.Drawing.Point(0, 0);
            this.painterDisplay1.Margin = new System.Windows.Forms.Padding(0);
            this.painterDisplay1.Name = "painterDisplay1";
            this.painterDisplay1.Size = new System.Drawing.Size(1022, 658);
            this.painterDisplay1.TabIndex = 0;
            this.painterDisplay1.Text = "painterDisplay1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage2.Controls.Add(this.animatorDisplay1);
            this.tabPage2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1022, 658);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Animator";
            // 
            // animatorDisplay1
            // 
            this.animatorDisplay1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animatorDisplay1.Location = new System.Drawing.Point(0, 0);
            this.animatorDisplay1.Margin = new System.Windows.Forms.Padding(0);
            this.animatorDisplay1.Name = "animatorDisplay1";
            this.animatorDisplay1.Size = new System.Drawing.Size(1022, 658);
            this.animatorDisplay1.TabIndex = 0;
            this.animatorDisplay1.Text = "animatorDisplay1";
            // 
            // nodeRightCLickMenu
            // 
            this.nodeRightCLickMenu.Name = "nodeRightCLickMenu";
            this.nodeRightCLickMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // nodeContextMenu1
            // 
            this.nodeContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuOption_nodeContextMenu});
            this.nodeContextMenu1.Name = "nodeContextMenu1";
            this.nodeContextMenu1.Size = new System.Drawing.Size(104, 26);
            this.nodeContextMenu1.Tag = "nodeMenu";
            this.nodeContextMenu1.Text = "node_menu";
            // 
            // openMenuOption_nodeContextMenu
            // 
            this.openMenuOption_nodeContextMenu.Name = "openMenuOption_nodeContextMenu";
            this.openMenuOption_nodeContextMenu.Size = new System.Drawing.Size(103, 22);
            this.openMenuOption_nodeContextMenu.Text = "Open";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(1026, 22);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FileTreeRefreshButton);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.DarkGray;
            this.splitContainer1.Panel2.Controls.Add(this.characterTreeView);
            this.splitContainer1.Size = new System.Drawing.Size(237, 660);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.TabIndex = 5;
            // 
            // FileTreeRefreshButton
            // 
            this.FileTreeRefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileTreeRefreshButton.BackColor = System.Drawing.Color.Transparent;
            this.FileTreeRefreshButton.FlatAppearance.BorderSize = 0;
            this.FileTreeRefreshButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileTreeRefreshButton.Location = new System.Drawing.Point(143, 9);
            this.FileTreeRefreshButton.Margin = new System.Windows.Forms.Padding(0);
            this.FileTreeRefreshButton.Name = "FileTreeRefreshButton";
            this.FileTreeRefreshButton.Size = new System.Drawing.Size(66, 23);
            this.FileTreeRefreshButton.TabIndex = 2;
            this.FileTreeRefreshButton.Text = "Refresh";
            this.FileTreeRefreshButton.UseVisualStyleBackColor = false;
            // 
            // characterTreeView
            // 
            this.characterTreeView.BackColor = System.Drawing.Color.Gray;
            this.characterTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.characterTreeView.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.characterTreeView.ForeColor = System.Drawing.Color.LightGray;
            this.characterTreeView.LineColor = System.Drawing.Color.Silver;
            this.characterTreeView.Location = new System.Drawing.Point(0, 0);
            this.characterTreeView.Name = "characterTreeView";
            this.characterTreeView.Size = new System.Drawing.Size(235, 204);
            this.characterTreeView.TabIndex = 0;
            // 
            // nodeContextCharacterMenu
            // 
            this.nodeContextCharacterMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCharacterOptionNodeMenu});
            this.nodeContextCharacterMenu.Name = "nodeContextCharacterMenu";
            this.nodeContextCharacterMenu.Size = new System.Drawing.Size(156, 26);
            this.nodeContextCharacterMenu.Opening += new System.ComponentModel.CancelEventHandler(this.nodeContextCharacterMenu_Opening);
            // 
            // openCharacterOptionNodeMenu
            // 
            this.openCharacterOptionNodeMenu.Name = "openCharacterOptionNodeMenu";
            this.openCharacterOptionNodeMenu.Size = new System.Drawing.Size(155, 22);
            this.openCharacterOptionNodeMenu.Text = "Open character";
            // 
            // BodyPartTreeMenu
            // 
            this.BodyPartTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removePartButton,
            this.replacePartBosyPartTreeMenuButton,
            this.changePartTypeToolStripMenuItem});
            this.BodyPartTreeMenu.Name = "BodyPartTreeMenu";
            this.BodyPartTreeMenu.Size = new System.Drawing.Size(166, 70);
            this.BodyPartTreeMenu.Opening += new System.ComponentModel.CancelEventHandler(this.BodyPartTreeMenu_Opening);
            // 
            // removePartButton
            // 
            this.removePartButton.Name = "removePartButton";
            this.removePartButton.Size = new System.Drawing.Size(165, 22);
            this.removePartButton.Text = "remove part";
            this.removePartButton.ToolTipText = "Removes this part and its children";
            // 
            // replacePartBosyPartTreeMenuButton
            // 
            this.replacePartBosyPartTreeMenuButton.Name = "replacePartBosyPartTreeMenuButton";
            this.replacePartBosyPartTreeMenuButton.Size = new System.Drawing.Size(165, 22);
            this.replacePartBosyPartTreeMenuButton.Text = "replace part";
            // 
            // changePartTypeToolStripMenuItem
            // 
            this.changePartTypeToolStripMenuItem.Name = "changePartTypeToolStripMenuItem";
            this.changePartTypeToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.changePartTypeToolStripMenuItem.Text = "Change part type";
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(100, 100);
            this.MaximumSize = new System.Drawing.Size(1280, 720);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Cube Studio";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.nodeContextMenu1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.nodeContextCharacterMenu.ResumeLayout(false);
            this.BodyPartTreeMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private PainterDisplay painterDisplay1;
        private CubeStudio.AnimatorDisplay animatorDisplay1;
        private System.Windows.Forms.ContextMenuStrip nodeRightCLickMenu;
        private System.Windows.Forms.ContextMenuStrip nodeContextMenu1;
        private System.Windows.Forms.ToolStripMenuItem openMenuOption_nodeContextMenu;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button FileTreeRefreshButton;
        private System.Windows.Forms.ContextMenuStrip nodeContextCharacterMenu;
        private System.Windows.Forms.ToolStripMenuItem openCharacterOptionNodeMenu;
        public System.Windows.Forms.TreeView characterTreeView;
        private System.Windows.Forms.ContextMenuStrip BodyPartTreeMenu;
        private System.Windows.Forms.ToolStripMenuItem removePartButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openCubeStudioFolder;
        private System.Windows.Forms.ToolStripMenuItem replacePartBosyPartTreeMenuButton;
        private System.Windows.Forms.ToolStripMenuItem changePartTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayControlsToolStripMenuItem;
    }
}