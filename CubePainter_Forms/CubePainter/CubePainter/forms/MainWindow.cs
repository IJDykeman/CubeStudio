using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CubePainter;
using System.Threading;
using System.Diagnostics;
using CubeAnimator;


namespace CubeStudio
{
    public partial class MainWindow : Form
    {
        TreeNode lastNodeClicked;
        public static string mainFolderPath = "C:/Users/Public/CubeStudio/";
        private TreeNode m_OldSelectNode;
        public static MainWindow singleton;
        string previousVoxPath = "";
        string previousCharacterPath = "";
        static TreeView staticCharacterTreeView;

        public MainWindow()
        {
            if (!Directory.Exists(mainFolderPath))
            {
                Directory.CreateDirectory(mainFolderPath);
            }

            InitializeComponent();
            animatorDisplay1.mainWindow = this;
            painterDisplay1.mainWindow = this;
            
            //setting up click events
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newFileMenuItem_Click);
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openFileMenuMenuItem_Click);
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            this.openCubeStudioFolder.Click += new System.EventHandler(this.openCubeStudioFolder_Click);
            this.displayControlsToolStripMenuItem.Click += new System.EventHandler(this.displayPainterControlHelpWindow);

            this.treeView1.Click += new System.EventHandler(this.treeView1_NodeMouseClick);

            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);

            this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_PreviewKeyDown);

            this.characterTreeView.KeyDown += new  System.Windows.Forms.KeyEventHandler(this.characterTreeView_PreviewKeyDown);
            this.characterTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.characterTreeView_MouseUp);


            this.FileTreeRefreshButton.Click += new  System.EventHandler(this.refreshFileTreeEvent);
           

            this.openMenuOption_nodeContextMenu.Click += new System.EventHandler(this.openSpaceFromNodeTree);
            this.openCharacterOptionNodeMenu.Click += new System.EventHandler(this.openCharacterFromNodeTree);

            this.removePartButton.Click += new System.EventHandler(this.removePartFromNodeContextMenu);
            this.replacePartBosyPartTreeMenuButton.Click += new System.EventHandler(this.replacePartFromNodeContextMenu);
            this.changePartTypeToolStripMenuItem.Click += new System.EventHandler(this.changePartTypeFromNodeContextMenu);

            staticCharacterTreeView = characterTreeView;

            singleton = this;

            updateFileTreeView();
            


        }

        private void refreshFileTreeEvent(object sender, EventArgs e)
        {

            updateFileTreeView();
        }

        private void displayPainterControlHelpWindow(object sender, EventArgs e)
        {

            MessageBox.Show("WASD to move\nShift to select color and slow down movement\nAlt to fill\nSpace to select color\nRight click to place a block\n Left click to destroy a block\n");
        }

        public void updateFileTreeView()
        {
            ListDirectory(treeView1, "C:/Users/Public/CubeStudio");
        }

        public bool hasDialogOpen()
        {
            if (this.Visible && !this.CanFocus)
            {
                return true;
            }
            return false;
        }

        bool isTabSelectedControl(Control test)
        {
            return tabControl1.SelectedTab.Contains(test);
        }

        Control getTabSelectedControl()
        {
            if (isTabSelectedControl(animatorDisplay1))
            {
                return animatorDisplay1;
            }
            else
            {
                return painterDisplay1;
            }
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo,0));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo, int depth)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);

                directoryNode.Expand();


            foreach (var directory in directoryInfo.GetDirectories())
            {
                TreeNode node = CreateDirectoryNode(directory, depth + 1);
                
                directoryNode.Nodes.Add(node);

            }
            foreach (var file in directoryInfo.GetFiles())
            {
                TreeNode node = new TreeNode(file.Name);
                node.Tag = file.FullName;
                directoryNode.Nodes.Add(node);

            }
            return directoryNode;
        }



        private void treeView1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {
                
                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeView1.GetNodeAt(p);
                lastNodeClicked = node;
                if (node != null)
                {
                    //MessageBox.Show("The calculations are complete");
                    // Select the node the user has clicked.
                    // The node appears selected until the menu is displayed on the screen.
                    m_OldSelectNode = treeView1.SelectedNode;
                    treeView1.SelectedNode = node;

                    if (getTabSelectedControl() is AnimatorDisplay)
                    {
                        if (AnimationProgram.state == AnimationProgram.AppState.admiringCharacter)
                        {
                            return;
                        }
                    }
                    // Find the appropriate ContextMenu depending on the selected node.
                    //MessageBox.Show(Convert.ToString(node.Tag));
                    switch (Convert.ToString(node.Tag).Split('.')[Convert.ToString(node.Tag).Split('.').Length - 1])
                    {
                        case "vox":
                            nodeContextMenu1.Show(treeView1, p);
                            break;
                        case "chr":

                            this.nodeContextCharacterMenu.Show(treeView1, p);


                            break;

                    }

                    // Highlight the selected node.
                    treeView1.SelectedNode = node;
                    m_OldSelectNode = null;
                }
            }
        }

        private void newFileMenuItem_Click(object sender, EventArgs e)
        {

            
            ((StudioGraphicsDeviceControl)getTabSelectedControl()).newEvent();
            


        }

        private void openFileMenuMenuItem_Click(object sender, EventArgs e)
        {

            //if (getTabSelectedControl() is PainterDisplay)
           // {
            using (OpenFileDialog openForm = new OpenFileDialog())
                {

                    openForm.Filter = "Voxel files (.vox)|*.vox";
                    if (getTabSelectedControl() is AnimatorDisplay)
                    {
                        openForm.Filter = "Character or voxel files (.chr, .vox)|*.vox;*.chr";
                    }
                    
                    string path = "C:\\Users\\Public\\CubeStudio\\"; // this is the path that you are checking.
                    if (Directory.Exists(path))
                    {

                    }
                    else
                    {
                        //openForm.SelectedPath = @"C:\";
                        //MessageBox.Show("The folder C:\Users\\Public\CubeStudio\ does not exist.");

                    }
                    DialogResult dr = openForm.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        //MessageBox.Show(openForm.SelectedPath);
                        ((StudioGraphicsDeviceControl)getTabSelectedControl()).openEvent(openForm.FileName);

                    }
                    else
                    {
                        // ...
                    }
                }
            //}
            //else
            //{

            //}
        }

        private void openCubeStudioFolder_Click(object sender, EventArgs e)
        {

            Process.Start(mainFolderPath);
        }


        private void openSpaceFromNodeTree(object sender, EventArgs e)
        {


            string tagPath = Convert.ToString(lastNodeClicked.Tag);

            if (getTabSelectedControl() is AnimatorDisplay)
            {

                CubeAnimator.BodyPartType type;
                type = proptUserForBodySelection();
                if (type == CubeAnimator.BodyPartType.unknown)
                {
                    return;
                }
                ((AnimatorDisplay)getTabSelectedControl()).addModelOfBodyPartType_form(tagPath, type);

            }
            else
            {
                ((StudioGraphicsDeviceControl)getTabSelectedControl()).openEvent(tagPath);
            }
        }


        private void removePartFromNodeContextMenu(object sender, EventArgs e)
        {
            if (getTabSelectedControl() is AnimatorDisplay)
            {
                animatorDisplay1.removePart(lastNodeClicked.Tag);
            }
        }

        private void replacePartFromNodeContextMenu(object sender, EventArgs e)
        {
            using (OpenFileDialog saveForm = new OpenFileDialog())
            {


                //openForm.ShowNewFolderButton = true;
                string path = "C:\\Users\\Public\\CubeStudio\\"; // this is the path that you are checking.
                if (Directory.Exists(path))
                {
                    //openForm.SelectedPath = path;
                }
                else
                {
                    // openForm.SelectedPath = @"C:\";
                    //MessageBox.Show("The folder C:\Users\\Public\CubeStudio\ does not exist.");

                }
                DialogResult dr = saveForm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    //MessageBox.Show(openForm.SelectedPath);
                    if (getTabSelectedControl() is AnimatorDisplay)
                    {
                        animatorDisplay1.replacePart(lastNodeClicked.Tag, saveForm.FileName);
                    }


                }
            }


        }

        private void changePartTypeFromNodeContextMenu(object sender, EventArgs e)
        {

            if (getTabSelectedControl() is AnimatorDisplay)
            {
                CubeAnimator.BodyPartType type = proptUserForBodySelection();
                if (type == CubeAnimator.BodyPartType.unknown)
                {
                    return;
                }
                animatorDisplay1.changePartType(lastNodeClicked.Tag, type);
            }



        }
        
        public static CubeAnimator.BodyPartType proptUserForBodySelection()
        {
                SelectBodyPartType newWin = new SelectBodyPartType();
                newWin.ShowDialog();
                if (newWin.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    CubeAnimator.BodyPartType type;
                    if (newWin.selectedButton == null)
                    {
                        return CubeAnimator.BodyPartType.unknown;
                    }
                    type = CubeAnimator.BodyPart.getBodyPartTypeFromFullName(newWin.selectedButton.Name);
                    return type;
                }
                return CubeAnimator.BodyPartType.unknown;
        }

        private void openCharacterFromNodeTree(object sender, EventArgs e)
        {


            string tagPath = Convert.ToString(lastNodeClicked.Tag);

            if (getTabSelectedControl() is AnimatorDisplay)
            {
                    ((AnimatorDisplay)getTabSelectedControl()).openCharacter(tagPath);
                
            }
            else
            {
                MessageBox.Show("Please open character files (.chr) in the Animator tab");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (getTabSelectedControl() is PainterDisplay)
            {
                if (File.Exists(previousVoxPath))
                {
                    ((StudioGraphicsDeviceControl)getTabSelectedControl()).saveEvent(previousVoxPath);
                    return;
                }
            }
            else if (getTabSelectedControl() is AnimatorDisplay)
            {
                if (File.Exists(previousCharacterPath))
                {
                    ((StudioGraphicsDeviceControl)getTabSelectedControl()).saveEvent(previousCharacterPath);
                    return;
                }
            }

            saveAsToolStripMenuItem_Click(sender, e);
            
           
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog saveForm = new SaveFileDialog())
            {


                //openForm.ShowNewFolderButton = true;
                string path = "C:\\Users\\Public\\CubeStudio\\"; // this is the path that you are checking.
                if (Directory.Exists(path))
                {
                    //openForm.SelectedPath = path;
                }
                else
                {
                    // openForm.SelectedPath = @"C:\";
                    //MessageBox.Show("The folder C:\Users\\Public\CubeStudio\ does not exist.");

                }
                DialogResult dr = saveForm.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    //MessageBox.Show(openForm.SelectedPath);
                    ((StudioGraphicsDeviceControl)getTabSelectedControl()).saveEvent(saveForm.FileName);//saveform.FileName gets full path
                    
                    if (getTabSelectedControl() is PainterDisplay)
                    {
                        previousVoxPath = saveForm.FileName;
                    }
                    else if (getTabSelectedControl() is AnimatorDisplay)
                    {
                        previousCharacterPath = saveForm.FileName;
                        
                    }
                }
            }

        }

        private void treeView1_NodeMouseClick(object sender, EventArgs e)
        {
            //((StudioGraphicsDeviceControl)getTabSelectedControl()).saveAsEvent();
            //MouseEventArgs click = (MouseEventArgs)e;
            //if (click.Button == MouseButtons.Right) treeView1.SelectedNode = click.Node;

        }

        private void listView1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            //MessageBox.Show("The calculations are complete");
        }


        private void characterTreeView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void characterTreeView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {


            // Show menu only if the right mouse button is clicked.
            if (e.Button == MouseButtons.Right)
            {

                // Point where the mouse is clicked.
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = characterTreeView.GetNodeAt(p);
                lastNodeClicked = node;
                if (node != null)
                {
                    //animatorDisplay1.removePart(node.Tag);
                    if (getTabSelectedControl() is AnimatorDisplay)
                    {
                        this.BodyPartTreeMenu.Show(characterTreeView, p);
                    }
                    characterTreeView.SelectedNode = node;
                    m_OldSelectNode = null;
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = characterTreeView.GetNodeAt(p);
                lastNodeClicked = node;
                if (node != null)
                {
                    //animatorDisplay1.removePart(node.Tag);
                    if (getTabSelectedControl() is AnimatorDisplay)
                    {
                        //this.BodyPartTreeMenu.Show(characterTreeView, p);
                        selectPartWithCharacterNodeTreeClick(node.Tag);
                    }
                    characterTreeView.SelectedNode = node;
                    m_OldSelectNode = null;
                }
            }
        }

        public static void selectPartInCharacterView(object tagToFind)
        {
            foreach (TreeNode node in staticCharacterTreeView.Nodes)
            {
                findTagInTree(tagToFind, node);

            }
        }

        public static void findTagInTree(object tagToFind, TreeNode toSearch)
        {
            if (toSearch.Tag == tagToFind)
            {
                staticCharacterTreeView.SelectedNode = toSearch;
                return;
            }

            foreach (TreeNode node in toSearch.Nodes)
            {

                    findTagInTree(tagToFind, node);
                
            }
        }

        void selectPartWithCharacterNodeTreeClick(object nodeTag)
        {
            animatorDisplay1.selectPartWithCharacterTreeClick(nodeTag);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The calculations are complete");
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void nodeContextCharacterMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void BodyPartTreeMenu_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
