using Lab05.BUS;
using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Lab05
{
    public partial class frmStudent : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        public frmStudent()
        {
            InitializeComponent();
        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var listStudents = studentService.GetAll();
                var listFacultys = facultyService.GetAll();
                FillFacultyCombobox(listFacultys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFacultyCombobox(List<Faculty> faculties)
        {
            faculties.Insert(0, new Faculty());
            this.cmbFaculty.DataSource = faculties;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void BindGrid(List<Student> students)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in students)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                {
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                }
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore + "";
                if (item.MajorID != null)
                {
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name + "";
                }
                ShowAvatar(item.Avatar);
            }
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void ShowAvatar(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                pbAvatar.Image = null;
            }
            else
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                string imagePath = Path.Combine(parentDirectory, "Images", imageName);
                pbAvatar.Image = Image.FromFile(imagePath);
                pbAvatar.Refresh();
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG file|*.png|JPEG file|*.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbAvatar.Image = new Bitmap(ofd.FileName);
            }
        }

        private void ckUnregisterMajor_CheckedChanged(object sender, EventArgs e)
        {
            var listStudent = new List<Student>();
            if (this.ckUnregisterMajor.Checked)
            {
                listStudent = studentService.GetAllHasNoMajor();
            }
            else
                listStudent = studentService.GetAll();
            BindGrid(listStudent);
        }

        private void Refresh()
        {
            txtStudentID.Clear();
            txtFullName.Clear();
            txtAverageScore.Clear();
            cmbFaculty.SelectedValue = 1;
        }

        private void LoadList()
        {
            StudentModel context = new StudentModel();
            List<Student> students = context.Students.ToList();
            BindGrid(students);
        }

        private bool checkNull(string a, string b, string c)
        {
            return true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            StudentModel context = new StudentModel();
            Student deleteStudent = context.Students.FirstOrDefault(p => p.StudentID == txtStudentID.Text);
            try
            {
                if (deleteStudent != null)
                {
                    DialogResult rs = MessageBox.Show($"Bạn có chắc muốn xóa sinh viên {deleteStudent.FullName} này không ?",
                        "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (rs == DialogResult.Yes)
                    {
                        dgvStudent.Rows.RemoveAt(dgvStudent.CurrentRow.Index);
                        context.Students.Remove(deleteStudent);
                        context.SaveChanges();
                        LoadList();
                        MessageBox.Show($"Xóa sinh viên {deleteStudent.FullName} thành công !", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Refresh();
                    }
                }
                else
                    throw new Exception("Không tồn tại mã số sinh viên này trong danh sách !");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStudent.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvStudent.Rows)
                {
                    if (row.Selected)
                    {
                        txtStudentID.Text = dgvStudent.Rows[row.Index].Cells[0].Value.ToString();
                        txtFullName.Text = dgvStudent.Rows[row.Index].Cells[1].Value.ToString();
                        cmbFaculty.Text = dgvStudent.Rows[row.Index].Cells[2].Value.ToString();
                        txtAverageScore.Text = dgvStudent.Rows[row.Index].Cells[3].Value.ToString();
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txtStudentID.Text) || string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtAverageScore.Text))
                {
                    throw new Exception("Vui lòng nhập đầy đủ thông tin !");
                }
                if (txtStudentID.Text.Length != 10)
                {
                    throw new Exception("Mã sinh viên bao gồm 10 số !");
                }
                foreach(DataGridViewRow i in dgvStudent.Rows)
                {
                    if(txtStudentID.Text == dgvStudent.Rows[i.Index].Cells[0].Value.ToString())
                    {
                        throw new Exception("Mã số sinh viên đã tồn tại trong danh sách !");
                    }
                }
                if(float.Parse(txtAverageScore.Text) < 0 || float.Parse(txtAverageScore.Text) > 10)
                {
                    throw new Exception("Nhập điểm từ 0 -> 10");
                }
                var item = new Student()
                {
                    StudentID = txtStudentID.Text,
                    FullName = txtFullName.Text,
                    FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString()),
                    AverageScore = float.Parse(txtAverageScore.Text)
                };
                studentService.InsertNew(item);
                LoadList();
                MessageBox.Show("Thêm mới sinh viên thành công !", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            StudentModel context = new StudentModel();
            Student updateStudentList = studentService.FindById(txtStudentID.Text);
            try
            {
                if (updateStudentList != null)
                {
                    updateStudentList.FullName = txtFullName.Text;
                    updateStudentList.FacultyID = int.Parse(cmbFaculty.SelectedValue.ToString());
                    if(float.Parse(txtAverageScore.Text) > 10 || float.Parse(txtAverageScore.Text) < 0)
                    {
                        MessageBox.Show("Nhập điểm từ 0 -> 10", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtAverageScore.Clear();
                        txtAverageScore.Focus();
                    }
                    else
                    {
                        updateStudentList.AverageScore = float.Parse(txtAverageScore.Text);
                        studentService.InsertUpdate(updateStudentList);
                        LoadList();
                        MessageBox.Show($"Cập nhật thông tin sinh viên {updateStudentList.FullName} thành công !",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Refresh();
                    }
                }
                else
                    throw new Exception("Mã số sinh viên chưa có trong danh sách ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtStudentID.Focus();
            }

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            FormRegister f = new FormRegister();
            f.ShowDialog();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn muốn thoát chương trình ?", "Thoát",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if(rs == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}
