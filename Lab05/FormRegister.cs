using Lab05.BUS;
using Lab05.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05
{
    public partial class FormRegister : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly MajorService majorService = new MajorService();
        private readonly FacultyService facultyService = new FacultyService();
        public FormRegister()
        {
            InitializeComponent();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            try
            {
                //var listStudents = studentService.GetAllHasNoMajor();
                var listFacultys = facultyService.GetAll();
                FillFacultyCombobox(listFacultys);
                //BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFacultyCombobox(List<Faculty> listFacultys)
        {
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void FillMajorCombobox(List<Major> listMajors)
        {
            this.cmbMajor.DataSource = listMajors;
            this.cmbMajor.DisplayMember = "Name";
            this.cmbMajor.ValueMember = "MajorID";
        }

        private void cmbFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Faculty selectedFaculty = cmbFaculty.SelectedItem as Faculty;
            if (selectedFaculty != null)
            {
                var listMajor = majorService.GetAllByFaculty(selectedFaculty.FacultyID);
                FillMajorCombobox(listMajor);
                var listStudents = studentService.GetAllHasNoMajor(selectedFaculty.FacultyID);
                BindGrid(listStudents);
            }
        }

        private void BindGrid(List<Student> listStudents)
        {
            dgvRegister.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvRegister.Rows.Add();
                dgvRegister.Rows[index].Cells[1].Value = item.StudentID;
                dgvRegister.Rows[index].Cells[2].Value = item.FullName;
                if(item.Faculty != null)
                {
                    dgvRegister.Rows[index].Cells[3].Value = item.Faculty.FacultyName;
                }
                dgvRegister.Rows[index].Cells[4].Value = item.AverageScore + "";
                if(item.MajorID != null)
                {
                    dgvRegister.Rows[index].Cells[5].Value = item.Major.Name + "";
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvRegister.Rows.Count > 0)
                {
                    foreach(DataGridViewRow i in dgvRegister.Rows)
                    {
                        Student s = studentService.FindById(dgvRegister.Rows[i.Index].Cells[1].Value.ToString());
                        s.MajorID = int.Parse(cmbMajor.SelectedValue.ToString());
                        studentService.InsertUpdate(s);
                    }
                }
                MessageBox.Show("Đăng ký chuyên ngành thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
