namespace Course_View
{
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
            tabControl1 = new TabControl();
            P_Course = new TabPage();
            btn_updateCourse = new Button();
            btn_deleteCourse = new Button();
            btn_addCourse = new Button();
            cb_preRequest = new ComboBox();
            cb_instructor = new ComboBox();
            txt_creditHours = new TextBox();
            txt_nameCourse = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            dgv_course = new DataGridView();
            P_Instructor = new TabPage();
            btn_updateInstructor = new Button();
            btn_deleteInstructor = new Button();
            btn_addInstructor = new Button();
            txt_mobile = new TextBox();
            txt_major = new TextBox();
            txt_age = new TextBox();
            txt_nameInstructor = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            dgv_instructor = new DataGridView();
            tabControl1.SuspendLayout();
            P_Course.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_course).BeginInit();
            P_Instructor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_instructor).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(P_Course);
            tabControl1.Controls.Add(P_Instructor);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 509);
            tabControl1.TabIndex = 0;
            // 
            // P_Course
            // 
            P_Course.BackColor = Color.DarkGray;
            P_Course.Controls.Add(btn_updateCourse);
            P_Course.Controls.Add(btn_deleteCourse);
            P_Course.Controls.Add(btn_addCourse);
            P_Course.Controls.Add(cb_preRequest);
            P_Course.Controls.Add(cb_instructor);
            P_Course.Controls.Add(txt_creditHours);
            P_Course.Controls.Add(txt_nameCourse);
            P_Course.Controls.Add(label4);
            P_Course.Controls.Add(label3);
            P_Course.Controls.Add(label2);
            P_Course.Controls.Add(label1);
            P_Course.Controls.Add(dgv_course);
            P_Course.Location = new Point(4, 24);
            P_Course.Name = "P_Course";
            P_Course.Padding = new Padding(3);
            P_Course.Size = new Size(792, 481);
            P_Course.TabIndex = 0;
            P_Course.Text = "Course";
            // 
            // btn_updateCourse
            // 
            btn_updateCourse.Font = new Font("Nirmala UI", 10F, FontStyle.Bold);
            btn_updateCourse.Location = new Point(145, 180);
            btn_updateCourse.Name = "btn_updateCourse";
            btn_updateCourse.Size = new Size(90, 32);
            btn_updateCourse.TabIndex = 24;
            btn_updateCourse.Text = "Update";
            btn_updateCourse.UseVisualStyleBackColor = true;
            btn_updateCourse.Click += btn_updateCourse_Click;
            // 
            // btn_deleteCourse
            // 
            btn_deleteCourse.Font = new Font("Nirmala UI", 10F, FontStyle.Bold);
            btn_deleteCourse.Location = new Point(260, 180);
            btn_deleteCourse.Name = "btn_deleteCourse";
            btn_deleteCourse.Size = new Size(90, 32);
            btn_deleteCourse.TabIndex = 23;
            btn_deleteCourse.Text = "Delete";
            btn_deleteCourse.UseVisualStyleBackColor = true;
            btn_deleteCourse.Click += btn_deleteCourse_Click;
            // 
            // btn_addCourse
            // 
            btn_addCourse.Font = new Font("Nirmala UI", 10F, FontStyle.Bold);
            btn_addCourse.Location = new Point(30, 180);
            btn_addCourse.Name = "btn_addCourse";
            btn_addCourse.Size = new Size(90, 32);
            btn_addCourse.TabIndex = 22;
            btn_addCourse.Text = "Add";
            btn_addCourse.UseVisualStyleBackColor = true;
            btn_addCourse.Click += btn_addCourse_Click;
            // 
            // cb_preRequest
            // 
            cb_preRequest.FormattingEnabled = true;
            cb_preRequest.Location = new Point(134, 130);
            cb_preRequest.Name = "cb_preRequest";
            cb_preRequest.Size = new Size(216, 23);
            cb_preRequest.TabIndex = 8;
            // 
            // cb_instructor
            // 
            cb_instructor.FormattingEnabled = true;
            cb_instructor.Location = new Point(134, 89);
            cb_instructor.Name = "cb_instructor";
            cb_instructor.Size = new Size(216, 23);
            cb_instructor.TabIndex = 7;
            // 
            // txt_creditHours
            // 
            txt_creditHours.Location = new Point(134, 50);
            txt_creditHours.Name = "txt_creditHours";
            txt_creditHours.Size = new Size(216, 23);
            txt_creditHours.TabIndex = 6;
            // 
            // txt_nameCourse
            // 
            txt_nameCourse.Location = new Point(134, 13);
            txt_nameCourse.Name = "txt_nameCourse";
            txt_nameCourse.Size = new Size(216, 23);
            txt_nameCourse.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(25, 130);
            label4.Name = "label4";
            label4.Size = new Size(95, 21);
            label4.TabIndex = 4;
            label4.Text = "Pre-Request";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(26, 89);
            label3.Name = "label3";
            label3.Size = new Size(77, 21);
            label3.TabIndex = 3;
            label3.Text = "Instructor";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(26, 52);
            label2.Name = "label2";
            label2.Size = new Size(94, 21);
            label2.TabIndex = 2;
            label2.Text = "CreditHours";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(26, 15);
            label1.Name = "label1";
            label1.Size = new Size(52, 21);
            label1.TabIndex = 1;
            label1.Text = "Name";
            // 
            // dgv_course
            // 
            dgv_course.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_course.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv_course.BackgroundColor = SystemColors.ControlDarkDark;
            dgv_course.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_course.Dock = DockStyle.Bottom;
            dgv_course.Location = new Point(3, 264);
            dgv_course.Name = "dgv_course";
            dgv_course.Size = new Size(786, 214);
            dgv_course.TabIndex = 0;
            dgv_course.RowHeaderMouseDoubleClick += dgv_course_RowHeaderMouseDoubleClick;
            // 
            // P_Instructor
            // 
            P_Instructor.BackColor = Color.DarkGray;
            P_Instructor.Controls.Add(btn_updateInstructor);
            P_Instructor.Controls.Add(btn_deleteInstructor);
            P_Instructor.Controls.Add(btn_addInstructor);
            P_Instructor.Controls.Add(txt_mobile);
            P_Instructor.Controls.Add(txt_major);
            P_Instructor.Controls.Add(txt_age);
            P_Instructor.Controls.Add(txt_nameInstructor);
            P_Instructor.Controls.Add(label5);
            P_Instructor.Controls.Add(label6);
            P_Instructor.Controls.Add(label7);
            P_Instructor.Controls.Add(label8);
            P_Instructor.Controls.Add(dgv_instructor);
            P_Instructor.Location = new Point(4, 24);
            P_Instructor.Name = "P_Instructor";
            P_Instructor.Padding = new Padding(3);
            P_Instructor.Size = new Size(792, 481);
            P_Instructor.TabIndex = 1;
            P_Instructor.Text = "Instructor";
            // 
            // btn_updateInstructor
            // 
            btn_updateInstructor.Font = new Font("Nirmala UI", 10F, FontStyle.Bold);
            btn_updateInstructor.Location = new Point(145, 180);
            btn_updateInstructor.Name = "btn_updateInstructor";
            btn_updateInstructor.Size = new Size(90, 32);
            btn_updateInstructor.TabIndex = 22;
            btn_updateInstructor.Text = "Update";
            btn_updateInstructor.UseVisualStyleBackColor = true;
            btn_updateInstructor.Click += btn_updateInstructor_Click;
            // 
            // btn_deleteInstructor
            // 
            btn_deleteInstructor.Font = new Font("Nirmala UI", 10F, FontStyle.Bold);
            btn_deleteInstructor.Location = new Point(260, 180);
            btn_deleteInstructor.Name = "btn_deleteInstructor";
            btn_deleteInstructor.Size = new Size(90, 32);
            btn_deleteInstructor.TabIndex = 21;
            btn_deleteInstructor.Text = "Delete";
            btn_deleteInstructor.UseVisualStyleBackColor = true;
            btn_deleteInstructor.Click += btn_deleteInstructor_Click;
            // 
            // btn_addInstructor
            // 
            btn_addInstructor.Font = new Font("Nirmala UI", 10F, FontStyle.Bold);
            btn_addInstructor.Location = new Point(30, 180);
            btn_addInstructor.Name = "btn_addInstructor";
            btn_addInstructor.Size = new Size(90, 32);
            btn_addInstructor.TabIndex = 20;
            btn_addInstructor.Text = "Add";
            btn_addInstructor.UseVisualStyleBackColor = true;
            btn_addInstructor.Click += btn_saveInstructor_Click;
            // 
            // txt_mobile
            // 
            txt_mobile.Location = new Point(134, 89);
            txt_mobile.Name = "txt_mobile";
            txt_mobile.Size = new Size(216, 23);
            txt_mobile.TabIndex = 19;
            // 
            // txt_major
            // 
            txt_major.Location = new Point(134, 130);
            txt_major.Name = "txt_major";
            txt_major.Size = new Size(216, 23);
            txt_major.TabIndex = 18;
            // 
            // txt_age
            // 
            txt_age.Location = new Point(134, 50);
            txt_age.Name = "txt_age";
            txt_age.Size = new Size(216, 23);
            txt_age.TabIndex = 15;
            // 
            // txt_nameInstructor
            // 
            txt_nameInstructor.Location = new Point(134, 13);
            txt_nameInstructor.Name = "txt_nameInstructor";
            txt_nameInstructor.Size = new Size(216, 23);
            txt_nameInstructor.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(26, 128);
            label5.Name = "label5";
            label5.Size = new Size(51, 21);
            label5.TabIndex = 13;
            label5.Text = "major";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(26, 89);
            label6.Name = "label6";
            label6.Size = new Size(58, 21);
            label6.TabIndex = 12;
            label6.Text = "mobile";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(26, 52);
            label7.Name = "label7";
            label7.Size = new Size(37, 21);
            label7.TabIndex = 11;
            label7.Text = "Age";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F);
            label8.Location = new Point(26, 15);
            label8.Name = "label8";
            label8.Size = new Size(52, 21);
            label8.TabIndex = 10;
            label8.Text = "Name";
            // 
            // dgv_instructor
            // 
            dgv_instructor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_instructor.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgv_instructor.BackgroundColor = SystemColors.ControlDarkDark;
            dgv_instructor.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_instructor.Dock = DockStyle.Bottom;
            dgv_instructor.Location = new Point(3, 264);
            dgv_instructor.Name = "dgv_instructor";
            dgv_instructor.Size = new Size(786, 214);
            dgv_instructor.TabIndex = 9;
            dgv_instructor.RowHeaderMouseDoubleClick += dgv_instructor_RowHeaderMouseDoubleClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 509);
            Controls.Add(tabControl1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            P_Course.ResumeLayout(false);
            P_Course.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_course).EndInit();
            P_Instructor.ResumeLayout(false);
            P_Instructor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv_instructor).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage P_Instructor;
        private TextBox txt_age;
        private TextBox txt_nameInstructor;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private DataGridView dgv_instructor;
        private TextBox txt_mobile;
        private TextBox txt_major;
        private TabPage P_Course;
        private ComboBox cb_preRequest;
        private ComboBox cb_instructor;
        private TextBox txt_creditHours;
        private TextBox txt_nameCourse;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private DataGridView dgv_course;
        private Button btn_deleteCourse;
        private Button btn_addCourse;
        private Button btn_deleteInstructor;
        private Button btn_addInstructor;
        private Button btn_updateInstructor;
        private Button btn_updateCourse;
    }
}
