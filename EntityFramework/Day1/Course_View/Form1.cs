using Course_View.Models;
using Microsoft.EntityFrameworkCore;

namespace Course_View;

public partial class Form1 : Form
{
    CourseContext context;
    int CourseId { get; set; }
    int InstructorId { get; set; }
    public Form1()
    {
        InitializeComponent();
        context = new CourseContext();
    }

    private void UpdateCourseData()
    {
        dgv_course.DataSource = context.Courses.Include(e => e.instructor).ToList().Select(c => new { Id = c.id, Name = c.name, CreditHours = c.credit_hours, InstructorName = c.instructor.name, PreRequest = c.preRequestNavigation?.name ?? "None" }).ToList();

        List<Course> courses = context.Courses.ToList();
        courses.Insert(0, new Course { name = "None", id = -1 });

        cb_preRequest.DataSource = courses;
    }
    private void UpdateInstructorData()
    {
        dgv_instructor.DataSource = context.Instructors.Select(i => new { Id = i.id, Name = i.name, Age = i.age, Mobile = i.mobile, Major = i.major }).ToList();

        List<Instructor> instructors = context.Instructors.ToList();
        instructors.Insert(0, new Instructor { name = "Choose Instructor", id = -1 });

        cb_instructor.DataSource = instructors;
    }
    private void Form1_Load(object sender, EventArgs e)
    {
        this.UpdateCourseData();
        this.UpdateInstructorData();

        cb_instructor.DisplayMember = "name";
        cb_instructor.ValueMember = "id";
        cb_instructor.SelectedIndex = 0;

        cb_preRequest.DisplayMember = "name";
        cb_preRequest.ValueMember = "id";
        cb_preRequest.SelectedValue = -1;
    }

    private void btn_saveInstructor_Click(object sender, EventArgs e)
    {
        bool isNumber = int.TryParse(txt_age.Text, out int age);
        if (!isNumber)
        {
            MessageBox.Show("Age Must be intager", "Error");
            return;
        }
        Instructor instructor = new Instructor()
        {
            name = txt_nameInstructor.Text,
            age = age,
            mobile = txt_mobile.Text,
            major = txt_major.Text
        };

        context.Instructors.Add(instructor);
        context.SaveChanges();

        txt_nameInstructor.Text = txt_age.Text = txt_mobile.Text = txt_major.Text = string.Empty;

        UpdateInstructorData();
    }

    private void btn_updateInstructor_Click(object sender, EventArgs e)
    {
        bool isNumber = int.TryParse(txt_age.Text, out int age);
        if (!isNumber)
        {
            MessageBox.Show("Age Must be intager", "Error");
            return;
        }
        Instructor instructor = context.Instructors.Where(i => i.id == InstructorId).SingleOrDefault() ?? new();
        instructor.name = txt_nameInstructor.Text;
        instructor.age = age;
        instructor.mobile = txt_mobile.Text;
        instructor.major = txt_major.Text;

        context.SaveChanges();

        txt_nameInstructor.Text = txt_age.Text = txt_mobile.Text = txt_major.Text = string.Empty;

        UpdateInstructorData();
    }

    private void btn_deleteInstructor_Click(object sender, EventArgs e)
    {
        Instructor? instructor = context.Instructors.Where(i => i.id == InstructorId).SingleOrDefault();
        if (instructor == null) return;

        context.Instructors.Remove(instructor);

        context.SaveChanges();

        txt_nameInstructor.Text = txt_age.Text = txt_mobile.Text = txt_major.Text = string.Empty;

        UpdateInstructorData();
    }

    private void dgv_instructor_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        InstructorId = (int)dgv_instructor.SelectedRows[0].Cells["Id"].Value;
        Instructor instructor = context.Instructors.Where(i => i.id == InstructorId).SingleOrDefault() ?? new();

        txt_nameInstructor.Text = instructor.name;
        txt_age.Text = instructor.age.ToString();
        txt_mobile.Text = instructor.mobile;
        txt_major.Text = instructor.major;
    }

    private void btn_addCourse_Click(object sender, EventArgs e)
    {
        if ((int)cb_instructor.SelectedValue! == -1)
        {
            MessageBox.Show("You should choose Instructor for the course", "Error");
            return;
        }
        bool isNumber = int.TryParse(txt_creditHours.Text, out int credit);
        if (!isNumber)
        {
            MessageBox.Show("Credit Hours Must be intager", "Error");
            return;
        }
        Course course = new Course()
        {
            name = txt_nameCourse.Text,
            credit_hours = credit,
            instructor_id = (int)cb_instructor.SelectedValue,
            preRequest = (int)cb_preRequest.SelectedValue! == -1 ? null : (int)cb_preRequest.SelectedValue
        };

        context.Courses.Add(course);
        context.SaveChanges();

        txt_nameCourse.Text = txt_creditHours.Text = string.Empty;
        cb_instructor.SelectedValue = -1;
        cb_preRequest.SelectedValue = -1;

        UpdateCourseData();
    }

    private void btn_updateCourse_Click(object sender, EventArgs e)
    {
        if ((int)cb_instructor.SelectedValue! == -1)
        {
            MessageBox.Show("You should choose Instructor for the course", "Error");
            return;
        }
        bool isNumber = int.TryParse(txt_creditHours.Text, out int credit);
        if (!isNumber)
        {
            MessageBox.Show("Credit Hours Must be intager", "Error");
            return;
        }
        Course course = context.Courses.Where(c => c.id == CourseId).SingleOrDefault() ?? new();
        course.name = txt_nameCourse.Text;
        course.credit_hours = credit;
        course.instructor_id = (int)cb_instructor.SelectedValue!;
        course.preRequest = (int)cb_preRequest.SelectedValue! == -1 ? null : (int)cb_preRequest.SelectedValue;

        context.SaveChanges();

        txt_nameCourse.Text = txt_creditHours.Text = string.Empty;
        cb_instructor.SelectedValue = -1;
        cb_preRequest.SelectedValue = -1;

        UpdateCourseData();
    }

    private void btn_deleteCourse_Click(object sender, EventArgs e)
    {
        Course? course = context.Courses.Where(c => c.id == CourseId).SingleOrDefault();
        if (course == null) return;

        context.Courses.Remove(course);

        context.SaveChanges();

        txt_nameCourse.Text = txt_creditHours.Text = string.Empty;
        cb_instructor.SelectedValue = -1;
        cb_preRequest.SelectedValue = -1;

        UpdateCourseData();
    }

    private void dgv_course_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        CourseId = (int)dgv_course.SelectedRows[0].Cells["Id"].Value;
        Course course = context.Courses.Where(c => c.id == CourseId).SingleOrDefault() ?? new Course();

        txt_nameCourse.Text = course.name;
        txt_creditHours.Text = course.credit_hours.ToString();
        cb_instructor.SelectedValue = course.instructor_id;
        cb_preRequest.SelectedValue = course.preRequest ?? -1;
    }
}
