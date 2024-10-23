namespace Notepad
{
    public partial class Notepad : Form
    {
        private string? currentPath;
        private string startRtf;
        private HashSet<string> recent;
        public Notepad()
        {
            InitializeComponent();
            currentPath = null;
            startRtf = rtb_main.Rtf ?? string.Empty;
            recent = new();
        }
        private void saveState(string? path, string rtf)
        {
            currentPath = path;
            startRtf = rtf;
            if (path is not null && !recent.Contains(path))
            {
                recent.Add(path);
                UpdateRecentList();
            }
        }

        private void UpdateRecentList()
        {
            recentMenu.DropDownItems.Clear();
            if (recent.Count == 0)
                recentMenu.DropDownItems.Add("No Recent Files");
            foreach (string path in recent)
            {
                ToolStripMenuItem recentItem = new ToolStripMenuItem(path);

                recentItem.Click += (sender, e) => OpenRecentFile(path);

                recentMenu.DropDownItems.Add(recentItem);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (startRtf != string.Empty && startRtf != rtb_main.Rtf)
            {
                DialogResult re = MessageBox.Show("Do you want to save your work?", "Warning", MessageBoxButtons.YesNoCancel);
                if (re == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
                else if (re == DialogResult.Cancel)
                    return;
            }

            openFD.DefaultExt = "rtf";
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                rtb_main.LoadFile(openFD.FileName);
                saveState(openFD.FileName, rtb_main.Rtf ?? string.Empty);
            }
        }
        private void OpenRecentFile(string path)
        {
            if (startRtf != string.Empty && startRtf != rtb_main.Rtf)
            {
                DialogResult re = MessageBox.Show("Do you want to save your work?", "Warning", MessageBoxButtons.YesNoCancel);
                if (re == DialogResult.Yes)
                    saveToolStripMenuItem_Click(new(), EventArgs.Empty);
                else if (re == DialogResult.Cancel)
                    return;
            }


            rtb_main.LoadFile(path);
            saveState(path, rtb_main.Rtf ?? string.Empty);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startRtf != string.Empty && startRtf != rtb_main.Rtf)
            {
                DialogResult re = MessageBox.Show("Do you want to save your work?", "Warning", MessageBoxButtons.YesNoCancel);
                if (re == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
                else if (re == DialogResult.Cancel)
                    return;
            }

            rtb_main.Clear();
            saveState(null, rtb_main.Rtf ?? string.Empty);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPath == null)
                saveAsToolStripMenuItem_Click(sender, e);
            else
            {
                rtb_main.SaveFile(currentPath);
                startRtf = rtb_main.Rtf ?? string.Empty;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFD.FileName = "Untitled.rtf";
            saveFD.DefaultExt = "rtf";

            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                rtb_main.SaveFile(saveFD.FileName);
                saveState(saveFD.FileName, rtb_main.Rtf ?? string.Empty);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (startRtf != string.Empty && startRtf != rtb_main.Rtf)
            {
                DialogResult re = MessageBox.Show("Do you want to save your work?", "Warning", MessageBoxButtons.YesNoCancel);
                if (re == DialogResult.Yes)
                    saveToolStripMenuItem_Click(sender, e);
                else if (re == DialogResult.Cancel)
                    return;
            }

            this.Close();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontD.ShowDialog() == DialogResult.OK)
            {
                if (rtb_main.SelectedText != string.Empty)
                    rtb_main.SelectionFont = fontD.Font;
                else
                    rtb_main.Font = fontD.Font;
            }
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorD.ShowDialog() == DialogResult.OK)
            {
                if (rtb_main.SelectedText != string.Empty)
                    rtb_main.SelectionColor = colorD.Color;
                else
                    rtb_main.ForeColor = colorD.Color;
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorD.ShowDialog() == DialogResult.OK)
            {
                if (rtb_main.SelectedText != string.Empty)
                    rtb_main.SelectionBackColor = colorD.Color;
                else
                    rtb_main.BackColor = colorD.Color;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb_main.SelectAll();
        }
    }
}
