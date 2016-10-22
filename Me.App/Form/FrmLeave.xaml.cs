using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Me.DataLayer.Repository;
using Me.DataLayer.Util;
using Me.Models.Tables;
using Me.Models.View;

namespace Me.App.Form
{
    /// <summary>
    /// Interaction logic for FrmLeave.xaml
    /// </summary>
    public partial class FrmLeave : Window
    {
        private readonly LeaveRepository _leaveRepo = new LeaveRepository();
        private readonly LeaveSpecificationRepository _leaveSpecsRepo = new LeaveSpecificationRepository();

        public FrmLeave()
        {
            InitializeComponent();
            Loaded += FormLeave_Loaded;
            txtMaxValue.Text = "0";
            txtYearsOfservice.IsReadOnly = true;
        }

        private void txtYearsOfservice_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (chkYearsOfService.IsChecked == false)
            {
                txtYearsOfservice.IsReadOnly = true;
                MessageBox.Show("Please check Service Lenght first!");
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLeaveName.Text) || string.IsNullOrEmpty(txtMaxValue.Text))
            {
                MessageBox.Show("Please fill Name", "Leave Monitoring");
            }
            else
            {
                if (btnSave.Content.ToString() == "Save")
                {
                    Leave leave = new Leave
                    {
                        LeaveName = txtLeaveName.Text,
                        MaxValue = Convert.ToInt32(txtMaxValue.Text)
                    };

                    int leaveId = await _leaveRepo.Insert(leave);

                    LeaveSpecification leaveSpecs = new LeaveSpecification
                    {
                        LeaveId = leaveId,
                        IsAnnual = chkAnnual.IsChecked != null && chkAnnual.IsChecked.Value,
                        IsYearOfService = chkYearsOfService.IsChecked != null && chkYearsOfService.IsChecked.Value,
                        ServiceLength = string.IsNullOrEmpty(txtYearsOfservice.Text) ? 0 : Convert.ToInt32(txtYearsOfservice.Text)
                    };

                    await _leaveSpecsRepo.Insert(leaveSpecs);
                    MessageBox.Show("Save Successfull", "Leave Monitoring");

                }
                else if (btnSave.Content.ToString() == "Update")
                {

                }
                ClearCheckNull.TextBox(this);
                ResetCheckBoxes();
                await LoadListView();
            }
        }

        private async void FormLeave_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadListView();
        }

        private async Task LoadListView()
        {
            LeaveListView.Items.Clear();
            var result = await _leaveRepo.SelectAll();

            result.ToList().ForEach(s =>
            {
                LeaveListView.Items.Add(new LeaveView
                {
                    LeaveId = s.LeaveId,
                    Name = s.LeaveName,
                    MaxValue = s.MaxValue,
                    Annual = s.LeaveSpecification.IsAnnual,
                    Regular = s.LeaveSpecification.IsRegular,
                    Service = s.LeaveSpecification.IsYearOfService,
                    ServiceLength = s.LeaveSpecification.ServiceLength
                });
            });
        }

        private void txtMaxValue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !ExpressionFilter.IsTextAllowed(e.Text);
        }

        private void ResetCheckBoxes()
        {
            chkAnnual.IsChecked = false;
            chkYearsOfService.IsChecked = false;
            txtMaxValue.Text = "0";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLeaveName.Text) || !string.IsNullOrEmpty(txtMaxValue.Text))
            {
                ClearCheckNull.TextBox(this);
            }
            else
            {
                var main = new FrmMain();
                main.Show();
                this.Hide();
            }
        }

        private void chkYearsOfService_Click(object sender, RoutedEventArgs e)
        {
            if (chkYearsOfService.IsChecked == true)
            {
                txtYearsOfservice.IsReadOnly = false;
            }
            else
            {
                txtYearsOfservice.IsReadOnly = true;
                txtYearsOfservice.Clear();
            }
        }
    }
}
