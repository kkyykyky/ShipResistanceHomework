using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Win32;
using ShipAnalysis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using static DataExportation.DataExportation;
using static ShipAnalysis.ResistanceAnalysis;
namespace InteractiveInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Configuration ShipConfiguration { get; set; } = new Configuration();
        public List<List<double>>? ResistanceData { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ShipConfiguration;
            Chart.AxisX.Add(new LiveCharts.Wpf.Axis()
            {
                FontSize = 20,
                Title = "傅汝德数",
                LabelFormatter = (x) =>
                {
                    return String.Format("{0:F5}", x);
                }

            });
            Chart.AxisY.Add(new LiveCharts.Wpf.Axis()
            {
                FontSize = 20,
                Title = "阻力系数",
                LabelFormatter = (x) =>
                {
                    return String.Format("{0:F5}", x);
                }
            });
        }
        private void UpdateFigure()
        {
            Chart.Series = new SeriesCollection();
            Series series = new LineSeries
            {
                Values = new ChartValues<ScatterPoint>(),
                PointGeometry = Geometry.Empty,
                Title = "兴波阻力系数"
            };

            Chart.Series.Add(series);
            for (int i = 0; i < ResistanceData?[0].Count; i++)
            {
                series.Values.Add(new ScatterPoint(ResistanceData[0][i], ResistanceData[2][i]));
            }
            Chart.Update();
        }

        #region 回调
        private void AnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            AnalysisButton.IsEnabled = false;
            Thread thread = new(
                () =>
                {
                    List<List<double>> result = AnalyzeShipResistance(ShipConfiguration);
                    AnalysisButton.Dispatcher.Invoke(() =>
                    {
                        ResistanceData = result;
                        UpdateFigure();
                        AnalysisButton.IsEnabled = true;
                        ExportDataButton.IsEnabled = true;
                    });
                });
            thread.Start();
        }
        private void ExportDataButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new();
            dialog.Filter = "Excel 工作簿(*.xlsx)|*.xlsx|Excel 97-2003 工作簿(*.xls)|*.xls|CSV UTF-8 (逗号分隔)(*.CSV)|*.CSV";
            if (dialog.ShowDialog() == true && ResistanceData != null)
            {
                switch (dialog.FilterIndex)
                {
                    case 1:
                        ExportAsXlsx(dialog.FileName, ResistanceData);
                        break;
                    case 2:
                        ExportAsXls(dialog.FileName, ResistanceData);
                        break;
                    case 3:
                        ExportAsCSV(dialog.FileName, ResistanceData);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                return;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
        }
        #endregion
    }
}
