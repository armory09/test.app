using System.Windows;
using Me.DataLayer.Util;
using Me.Models.View;

namespace Me.App.Form.Subform
{
    /// <summary>
    /// Interaction logic for FrmSearch.xaml
    /// </summary>
    public partial class FrmSearch : Window
    {
        //private readonly EmployeeRepository _employeeRepo = new EmployeeRepository();


        public FrmSearch()
        {
            InitializeComponent();
            Loaded += FrmSearch_Loaded;
        }

        private async void FrmSearch_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBoxLoad cmbItems = new ComboBoxLoad();
            CmbLeaveType.Items.Clear();
            CmbLeaveType.ItemsSource = await cmbItems.Load();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            if (CmbLeaveType.SelectedIndex >= 0)
            {
                CreateHelper<LeaveSelected> ch = new CreateHelper<LeaveSelected>();

                await ch.DeleteFile("leaveSelected");

                LeaveSelected leaveSelected = new LeaveSelected
                {
                    LeaveName = CmbLeaveType.SelectedValue.ToString(),
                    DateFrom = DpFrom.SelectedDate,
                    DateTo = DpTo.SelectedDate
                };

                ch.WriteJson(leaveSelected, "leaveSelected");

                FrmReport report = new FrmReport();
                report.Show();
            }
            else
            {
                MessageBox.Show("Please select leave type", "Leave Monitoring", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
