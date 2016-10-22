using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Me.App.Form.Subform;
using Me.DataLayer.Repository;
using Me.DataLayer.Util;
using Me.Models.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Me.Models.Tables;

namespace Me.App.Form
{
    /// <summary>
    /// Interaction logic for FrmMain.xaml
    /// </summary>
    public partial class FrmMain : Window
    {
        private readonly EmployeeLeaveRepository _employeeLeaveRepo = new EmployeeLeaveRepository();

        public FrmMain()
        {
            InitializeComponent();
            Loaded += FormMain_Loaded;
        }

        private async void FormMain_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxLoad cmbItems = new ComboBoxLoad();
            cmbLeave.Items.Clear();
            cmbLeave.ItemsSource = await cmbItems.Load();
            LoadListView(await _employeeLeaveRepo.SelectAll());

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = false;
            Application.Current.Shutdown();
        }

        private void CreateLeave_Click(object sender, RoutedEventArgs e)
        {
            var leaveForm = new FrmLeave();
            leaveForm.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateHelper<Details> create = new CreateHelper<Details>();
            await create.DeleteFile("details");
            FrmAssingLeave assign = new FrmAssingLeave();
            assign.Show();
        }

        public void LoadListView(IEnumerable<Employee> modelList)
        {
            var employees = modelList as IList<Employee> ?? modelList.ToList();
            if (employees.IsAny())
            {
                LeaveListView.Items.Clear();

                employees.ToList().ForEach(s =>
                {
                    LeaveListView.Items.Add(new EmployeeLeaveView
                    {
                        FullName = string.Format("{0} {1}", s.FirstName, s.LastName),
                        Section = s.Section.section_name,
                        Department = s.Department.department_name,
                        Leave = s.Leave.LeaveName,
                        DateCreated = s.EmployeeLeave.DateCreated.Value.ToString("dd/MM/yyyy"),
                        DateFiled = s.EmployeeLeave.DateFiled.Value.ToString("dd/MM/yyyy")
                    });
                });
            }
            else
            {
                LeaveListView.Items.Clear();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            //FrmReport report = new FrmReport();
            //report.Show();
            FrmSearch search = new FrmSearch();
            search.Show();

        }

        public async Task<IEnumerable<Employee>> SearchStringAsync(string leaveType, string employeeName)
        {
            IEnumerable<Employee> employeeLst = await _employeeLeaveRepo.SelectAll();

            if (string.IsNullOrEmpty(leaveType) == false && string.IsNullOrEmpty(employeeName))
            {
                employeeLst = employeeLst.AsQueryable()
                    .Where(m => m.Leave.LeaveName.IndexOf(leaveType, StringComparison.OrdinalIgnoreCase) != -1)
                    .Select(s => s);
            }
            else if (string.IsNullOrEmpty(leaveType) && string.IsNullOrEmpty(employeeName) == false)
            {
                employeeLst = employeeLst.AsQueryable()
                    .Where(m => m.FirstName.IndexOf(employeeName, StringComparison.OrdinalIgnoreCase) != -1 ||
                                m.LastName.IndexOf(employeeName, StringComparison.OrdinalIgnoreCase) != -1)
                    .Select(s => s);
            }
            else
            {
                employeeLst = employeeLst.AsQueryable()
                    .Where(m => m.FirstName.IndexOf(employeeName, StringComparison.OrdinalIgnoreCase) != -1 ||
                                m.LastName.IndexOf(employeeName, StringComparison.OrdinalIgnoreCase) != -1)
                    .Select(s => s);
                employeeLst = employeeLst.AsQueryable()
                   .Where(m => m.Leave.LeaveName.IndexOf(leaveType, StringComparison.OrdinalIgnoreCase) != -1)
                   .Select(s => s);
            }

            return employeeLst;
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLeave.SelectedIndex == -1 && string.IsNullOrEmpty(txtEmployeeName.Text))
            {
                MessageBox.Show("Invalid");
            }
            else
            {
                LoadListView(await SearchStringAsync(cmbLeave.Text, txtEmployeeName.Text));
            }
        }

        private async void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var employee = await _employeeLeaveRepo.SelectAll();
            txtEmployeeName.Clear();
            cmbLeave.SelectedIndex = -1;
            LoadListView(employee);
        }
    }
}
