using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Me.DataLayer.Repository;
using Me.DataLayer.Util;
using Me.Models.Tables;
using Me.Models.View;

namespace Me.App.Form.Subform
{
    /// <summary>
    /// Interaction logic for FrmSelection.xaml
    /// </summary>
    public partial class FrmSelection : Window
    {
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();
        private int _approvalEmployeeId;
        private int _applicantEmployeeId;

        public FrmSelection()
        {
            InitializeComponent();
            Loaded += FrmSelection_Loaded;
        }

        private async void FrmSelection_Loaded(object sender, RoutedEventArgs e)
        {
            //_employeeList = await _employeeRepo.SelectAll();
            chkApplicant.IsChecked = true;
            LoadListViewAsync(await _employeeRepo.SelectAll());
        }

        private void LoadListViewAsync(IEnumerable<Employee> model)
        {
            EmployeeListView.Items.Clear();
            foreach (Employee item in model)
            {
                EmployeeListView.Items.Add(new EmployeeView
                {
                    FullName = string.Format("{0} {1}", item.FirstName, item.LastName),
                    EmployeeId = item.EmployeeId,
                    EmployeeNumber = item.EmployeeNumber,
                    Department = item.Department.department_name,
                    Section = item.Section.section_name
                });
            }
        }

        private void chkApplicant_Checked(object sender, RoutedEventArgs e)
        {
            chkApproval.IsChecked = false;
            txtSearch.Clear();
        }

        private void chkApproval_Checked(object sender, RoutedEventArgs e)
        {
            chkApplicant.IsChecked = false;
            txtSearch.Clear();
        }

        private async void txtSearch_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var employeeList = await _employeeRepo.SelectAll();
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                LoadListViewAsync(employeeList);
            }
            else
            {
                employeeList =
                    employeeList.AsQueryable()
                        .Where(
                            m =>
                                m.FirstName.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) != -1 ||
                                m.LastName.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) != -1)
                        .Select(s => s);
                LoadListViewAsync(employeeList);
            }
        }

        private void EmployeeListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            var result = EmployeeListView.SelectedItem as EmployeeView;
            if (result != null)
            {
                if (chkApplicant.IsChecked == true)
                {

                    txtApplicantDepartment.Text = result.Department;
                    txtApplicantFullName.Text = result.FullName;
                    txtApplicantSection.Text = result.Section;
                    _applicantEmployeeId = result.EmployeeId;

                }
                else if (chkApproval.IsChecked == true)
                {

                    txtApprovalDepartment.Text = result.Department;
                    txtApprovalFullName.Text = result.FullName;
                    txtApprovalSection.Text = result.Section;
                    _approvalEmployeeId = result.EmployeeId;
                }
            }
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtApplicantFullName.Text) || string.IsNullOrEmpty(txtApprovalFullName.Text))
            {
                MessageBox.Show("Please choose Applicant and Approval name!", "Leave Monitoring");
            }
            else
            {
                //CreateHelper<LeaveSelected> leaveSelected = new CreateHelper<LeaveSelected>();

                //var leave = leaveSelected.ReadJson("leave");
                //await _leaveRepo.SelectByName(leave.ToList().ForEach(s => s))
                try
                {
                    List<Details> details = new List<Details>
                    {
                        new Details {EmployeeId = _approvalEmployeeId, Designation = "Approval"},
                        new Details {EmployeeId = _applicantEmployeeId, Designation = "Applicant"}
                    };

                    CreateHelper<Details> create = new CreateHelper<Details>();

                    //if folder doen't create
                    //if folder exist dont create 
                    create.CreateFolder();
                    create.WriteJson(details, "details");

                    MessageBox.Show("Selection Complete", "Leave Monitoring");
                    FrmAssingLeave main = new FrmAssingLeave();
                    main.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: ", ex.Message);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtApplicantFullName.Text) || !string.IsNullOrEmpty(txtApprovalFullName.Text))
            {
                ClearCheckNull.TextBox(this);
            }
            else
            {
                FrmMain main = new Me.App.Form.FrmMain();
                main.Show();
                this.Hide();
            }
        }
    }
}
