using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Me.DataLayer.Repository;
using Me.DataLayer.Util;
using Me.Models.Tables;
using Me.Models.View;
using FrmSelection = Me.App.Form.Subform.FrmSelection;

namespace Me.App.Form
{
    /// <summary>
    /// Interaction logic for FrmAssingLeave.xaml
    /// </summary>
    public partial class FrmAssingLeave : Window
    {
        private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();
        private readonly LeaveRepository _leaveRepo = new LeaveRepository();
        private readonly EmployeeLeaveRepository _employeeLeaveRepo = new EmployeeLeaveRepository();
        private int? _applicantId;
        private int? _approvedId;

        public FrmAssingLeave()
        {
            InitializeComponent();
            Loaded += FrmAssingLeave_Loaded;
        }
        private async void FrmAssingLeave_Loaded(object sender, RoutedEventArgs e)
        {
            ControlClear();
            ControleDisable();
            ComboBoxLoad cmbItems = new ComboBoxLoad();
            cmbLeave.Items.Clear();
            cmbLeave.ItemsSource = await cmbItems.Load();
            await LoadDetailsAsync();
        }
        private void MenuItem_SeachOnClick(object sender, RoutedEventArgs e)
        {
            FrmSelection subForm = new FrmSelection();
            subForm.Show();
            Hide();
        }

        private async Task LoadDetailsAsync()
        {
            CreateHelper<Details> details = new CreateHelper<Details>();

            var result = details.ReadListJson("details");
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (item.Designation == "Approval")
                    {
                        var employee = await _employeeRepo.SelectById(item.EmployeeId);
                        txtApprovalFullName.Text = string.Format("{0} {1}", employee.FirstName, employee.LastName);
                        txtApprovalSection.Text = employee.Section.section_name;
                        txtApprovalDepartment.Text = employee.Department.department_name;
                        _approvedId = item.EmployeeId;
                    }
                    else if (item.Designation == "Applicant")
                    {
                        var employee = await _employeeRepo.SelectById(item.EmployeeId);
                        txtApplicantFullName.Text = string.Format("{0} {1}", employee.FirstName, employee.LastName);
                        txtApplicantSection.Text = employee.Section.section_name;
                        txtApplicantDepartment.Text = employee.Department.department_name;
                        _applicantId = item.EmployeeId;
                    }
                }
            }

        }

        private async void cmbLeave_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ControlClear();
            ControleDisable();

            if (cmbLeave.SelectedIndex > -1)
            {
                if (CheckFullName())
                {
                    cmbLeave.SelectedIndex = -1;
                    MessageBox.Show("Please select applicant first!");
                }
                else
                {
                    //LeaveCheck check = await _leaveRepo.LeaveCheck()
                    
                    if (_applicantId != null)
                    {
                        LeaveCheck check = await _leaveRepo.LeaveCheck(_applicantId, cmbLeave.SelectedValue.ToString());

                        if (check.IsApproved)
                        {
                            ControlEnabled();
                            TxtRemaining.Text = string.Format("{0}", check.MaxValue - check.Balance);
                            TxtTotal.Text = check.MaxValue.ToString();
                        }
                        else
                        {
                            MessageBox.Show("The requirement for this Leave type doesn't meet by the employee", "Leave Monitoring", MessageBoxButton.OK, MessageBoxImage.Information);
                            ControlClear();
                            ControleDisable();
                        }
                    }
                }
            }
        }

        private void ControlClear()
        {
            TxtInput.Clear();
            TxtValue.Clear();
            TxtTotal.Clear();
            TxtRemaining.Clear();
            txtReason.Document.Blocks.Clear();
            dpDateFile.SelectedDate = null;
            dpDateFrom.SelectedDate = null;
            dpDateTo.SelectedDate = null;
        }

        private void ControleDisable()
        {
            TxtInput.IsReadOnly = true;
            dpDateFile.IsEnabled = false;
            dpDateFrom.IsEnabled = false;
            dpDateTo.IsEnabled = false;
        }

        private void ControlEnabled()
        {
            TxtInput.IsReadOnly = false;
            dpDateFile.IsEnabled = true;
            dpDateFrom.IsEnabled = true;
            dpDateTo.IsEnabled = true;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbLeave.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select Leave Type first!", "Leave Monitoring", MessageBoxButton.OK);
                }
                else if (dpDateFile.SelectedDate == null || dpDateFrom.SelectedDate == null || dpDateTo.SelectedDate == null)
                {
                    MessageBox.Show("Please specify all dates!", "Leave Monitoring", MessageBoxButton.OK);
                }
                else if (string.IsNullOrEmpty(TxtInput.Text))
                {
                    MessageBox.Show("Please input value", "Leave Monitoring", MessageBoxButton.OK);
                }
                else
                {
                    ControlEnabled();
                    CreateHelper<AuthenticationView> read = new CreateHelper<AuthenticationView>();
                    AuthenticationView user = read.ReadJson("user");
                    

                    if (user != null)
                    {
                        Leave leave = await _leaveRepo.SelectByName(cmbLeave.SelectedValue.ToString());

                        EmployeeLeave model = new EmployeeLeave
                        {
                            LeaveId = leave.LeaveId,
                            EmployeeId = _applicantId,
                            Reason = new TextRange(txtReason.Document.ContentStart, txtReason.Document.ContentEnd).Text,
                            DateFiled = dpDateFile.SelectedDate,
                            DateFrom = dpDateFrom.SelectedDate,
                            DateTo = dpDateTo.SelectedDate,
                            LeaveAvail = Convert.ToDecimal(TxtValue.Text),
                            UserId = user.UserId,
                            ApprovedId = _approvedId
                        };

                        var employeeLeaveId = await _employeeLeaveRepo.Insert(model);

                        if (employeeLeaveId != null)
                        {
                            MessageBox.Show("Leave assignment has been successfully saved!", "Leave monitoring", MessageBoxButton.OK, MessageBoxImage.Information);
                            ControlClear();
                            ClearCheckNull.TextBox(this);
                            ControleDisable();
                        }
                        else
                        {
                            MessageBox.Show("Something has gone wrong, try again, if problem persist kindly see your system administrator!", "Leave Monitoring", MessageBoxButton.OK, MessageBoxImage.Error);
                            ControlClear();
                            ControleDisable();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, Please report to system administrator!", "Leave Monitoring", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtApplicantFullName.Text) || cmbLeave.SelectedIndex >= 0)
            {
                ClearCheckNull.TextBox(this);
                cmbLeave.SelectedIndex = -1;
            }
            else
            {
                var main = new FrmMain();
                main.Show();
                this.Hide();
            }
        }

        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtInput.Text))
            {
                if (CheckFullName())
                {
                    TxtInput.Clear();
                    TxtValue.Clear();
                    MessageBox.Show("Please select applicant first!", "Leave Monitoring");
                }
                else
                {
                    if (cmbLeave.SelectedIndex >= 0)
                    {
                        int input = Convert.ToInt32(TxtInput.Text);
                        decimal output = (decimal)input / 8;
                        TxtValue.Text = output.ToString("#.####");
                    }
                    else
                    {
                        TxtInput.Clear();
                        MessageBox.Show("Please select leave type first!", "Leave Monitoring");
                    }
                }
            }
            else
            {
                TxtValue.Clear();
            }
        }

        private void TxtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtValue.Text))
            {
                if (Convert.ToInt32(TxtTotal.Text) != 0)
                {
                    if (Convert.ToDecimal(TxtValue.Text) > Convert.ToInt32(TxtRemaining.Text))
                    {
                        MessageBox.Show("Insufficient Balance", "Leave Monitoring");
                        TxtInput.Clear();
                        TxtValue.Clear();
                    }
                }
            }
            //MessageBox.Show(string.Format("{0}", (Convert.ToInt32(TxtCanAvail.Text) - Convert.ToDecimal(txtBalance.Text))), "Sample");
        }

        private void dpDateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDateTo.SelectedDate != null)
            {
                if (CheckFullName())
                {
                    dpDateTo.SelectedDate = null;
                    MessageBox.Show("Please select applicant first!", "Leave Monitoring");
                }
                else
                {
                    if (cmbLeave.SelectedIndex >= 0)
                    {
                        var picker = sender as DatePicker;

                        DateTime? date = picker.SelectedDate;
                        if (date != null || dpDateFrom.SelectedDate != null)
                        {
                            //var from = dpDateFrom.SelectedDate;

                            //MessageBox.Show(string.Format("From: {0} To: {1}", from.Value.ToShortDateString(), date.Value.ToShortDateString()));
                            if (dpDateFrom.SelectedDate > date)
                            {
                                MessageBox.Show("Inclusive date: 'To' MUST be GREATER than or EQUAL to Inclusive date: 'FROM'", "Leave Monitoring");

                                dpDateFrom.SelectedDate = null;
                                dpDateTo.SelectedDate = null;
                            }
                            else if (dpDateFrom.SelectedDate == null)
                            {
                                MessageBox.Show("Please check Inclusive dates", "Leave Monitoring");
                                dpDateTo.SelectedDate = null;
                            }
                        }
                    }
                    else
                    {
                        dpDateTo.SelectedDate = null;
                        MessageBox.Show("Please select leave type first!", "Leave Monitoring");
                    }
                }
            }
        }

        private bool CheckFullName()
        {
            if (string.IsNullOrEmpty(txtApplicantFullName.Text) || string.IsNullOrEmpty(txtApprovalFullName.Text))
            {
                return true;
            }
            return false;
        }

        private void dpDateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDateFrom.SelectedDate != null)
            {
                if (CheckFullName())
                {
                    dpDateFrom.SelectedDate = null;
                    MessageBox.Show("Please select applicant first!", "Leave Monitoring");
                }
                else
                {
                    if (cmbLeave.SelectedIndex < 0)
                    {
                        dpDateFrom.SelectedDate = null;
                        MessageBox.Show("Please select leave type first!", "Leave Monitoring");
                    }
                }
            }
        }

        private void dpDateFile_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDateFile.SelectedDate != null)
            {
                if (CheckFullName())
                {
                    dpDateFile.SelectedDate = null;
                    MessageBox.Show("Pleas select applicant first!", "Leave Monitoring");
                }
                else
                {
                    if (cmbLeave.SelectedIndex < 0)
                    {
                        dpDateFile.SelectedDate = null;
                        MessageBox.Show("Please select leave type first!", "Leave Monitoring");
                    }
                }
            }
        }

        private void TxtInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !ExpressionFilter.IsTextAllowed(e.Text);
        }
    }
}
