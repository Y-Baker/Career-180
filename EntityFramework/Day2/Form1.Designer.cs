namespace Day2;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        dgv_news = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgv_news).BeginInit();
        SuspendLayout();
        // 
        // dgv_news
        // 
        dgv_news.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv_news.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        dgv_news.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgv_news.Dock = DockStyle.Fill;
        dgv_news.Location = new Point(0, 0);
        dgv_news.Name = "dgv_news";
        dgv_news.Size = new Size(770, 467);
        dgv_news.TabIndex = 0;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(770, 467);
        Controls.Add(dgv_news);
        Name = "Form1";
        Text = "Form1";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)dgv_news).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv_news;
}
