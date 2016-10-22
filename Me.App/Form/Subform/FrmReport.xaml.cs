using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Printing;
using Me.DataLayer.Repository;
using Me.DataLayer.Util;
using Me.Models.Tables;
using Me.Models.View;
using System.Linq;

namespace Me.App.Form.Subform
{
    /// <summary>
    /// Interaction logic for FrmReport.xaml
    /// </summary>
    public partial class FrmReport : Window
    {
        public FrmReport()
        {
            InitializeComponent();
            Loaded += FrmReport_Loaded;
        }

        //private List<ProductData> _data;
        private readonly List<LeaveReport> _data = new List<LeaveReport>();
        private readonly CheckLeaveRepository _checkLeaveRepo = new CheckLeaveRepository();
        private readonly LeaveRepository _leaveRepo = new LeaveRepository();

        private async void FrmReport_Loaded(object sender, RoutedEventArgs e)
        {
            LeaveSelected selected = new LeaveSelected();

            await Task.Run(() =>
            {
                CreateHelper<LeaveSelected> leave = new CreateHelper<LeaveSelected>();

                var readJson = leave.ReadJson("leaveSelected");

                if (readJson != null)
                {
                    selected = readJson;
                }
            });

            if (selected != null)
            {
                if (selected.DateFrom != null && selected.DateTo != null && selected.LeaveName != null)
                {
                    Leave leave = await _leaveRepo.SelectByName(selected.LeaveName);
                    if (leave.LeaveId != null)
                    {
                        IEnumerable<EmployeeLeave> result = await _checkLeaveRepo.LeaveInclusiveDate(leave.LeaveId, selected.DateFrom, selected.DateTo);

                        result.ToList().ForEach(s =>
                        {
                            if (s.DateTo != null)
                                if (s.DateFrom != null)
                                    _data.Add(new LeaveReport
                                    {
                                        FullName = string.Format("{0} {1}", s.Employee.FirstName, s.Employee.LastName),
                                        DateFrom = s.DateFrom.Value.ToString("dd/MM/yyyy"),
                                        DateTo = s.DateTo.Value.ToString("dd/MM/yyyy"),
                                        LeaveName = s.Leave.LeaveName,
                                        Availed = s.LeaveAvail,
                                        Remaining = s.Leave.MaxValue - s.LeaveAvail
                                    });
                        });
                    }
                }
                else
                {
                    Leave leave = await _leaveRepo.SelectByName(selected.LeaveName);
                    if (leave.LeaveId != null)
                    {
                        IEnumerable<EmployeeLeave> result = await _checkLeaveRepo.LeavePerYear(leave.LeaveId);

                        result.ToList().ForEach(s =>
                        {
                            _data.Add(new LeaveReport
                            {
                                FullName = string.Format("{0} {1}", s.Employee.FirstName, s.Employee.LastName),
                                DateFrom = s.DateCreated.Value.ToString("dd/MM/yyyy"),
                                DateTo = s.DateTo.Value.ToString("dd/MM/yyyy"),
                                LeaveName = s.Leave.LeaveName,
                                Availed = s.LeaveAvail,
                                Remaining = s.TotalAvail
                            });
                        });
                    }
                }


                SimpleLink categoryLink = new SimpleLink();
                categoryLink.ReportHeaderTemplate = (DataTemplate)Resources["HeaderTemplate"];
                categoryLink.DetailTemplate = (DataTemplate)Resources["ProductsTemplate"];
                categoryLink.DetailCount = _data.Count;
                Preview.DocumentSource = categoryLink;
                categoryLink.CreateDetail += CategoryLink_CreateDetail;
                categoryLink.CreateDocument(true);
            }
            else
            {
                MessageBox.Show("No records found!", "Leave Monitoring", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CategoryLink_CreateDetail(object sender, CreateAreaEventArgs e)
        {
            e.Data = _data[e.DetailIndex];
        }
    }
}
