using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DXApplication1
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private List<SeriesPoint> tempPoints;
        DateTime _lastPlotTime = DateTime.Now;
        int _val;
        Random r = new Random();
        DateTime AxisStartTime = DateTime.Now - new TimeSpan(0,0,1,0);

        public Form1()
        {
            InitializeComponent();
        }







        private void InitializeChart()
        {
            Series series = new Series();

            // Adjust series
            series.ArgumentScaleType = ScaleType.DateTime;
            series.ValueScaleType = ScaleType.Numerical;
            series.Label.Border.Visible = false;
            series.Label.BackColor = Color.Transparent;
            series.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.False;
            series.ChangeView(ViewType.Area);
            ((AreaSeriesView)series.View).Color = Color.Aqua;
            ((AreaSeriesView)series.View).MarkerOptions.Kind = MarkerKind.Diamond;
            ((AreaSeriesView)series.View).MarkerOptions.Size = 7;

            series.Points.Add(new SeriesPoint(DateTime.Now, new double[] { 0 }));

            chartControl1.Series.Add(series);

            // Adjust time axis
            ((XYDiagram)chartControl1.Diagram).AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;
            ((XYDiagram)chartControl1.Diagram).AxisX.Label.DateTimeOptions.Format = DateTimeFormat.ShortTime;
            ((XYDiagram)chartControl1.Diagram).AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Second;
            ((XYDiagram)chartControl1.Diagram).AxisX.Range.SideMarginsEnabled = false;
            ((XYDiagram)chartControl1.Diagram).AxisX.MinorCount = 10;
            ((XYDiagram)chartControl1.Diagram).Margins.Right = 25;
            //((XYDiagram)chartControl1.Diagram).AxisX.GridSpacingAuto = false;

            // Adjust label
            ChartTitle title = new ChartTitle();

            title.Text = "Title";
            chartControl1.Titles.Add(title);

            tempPoints = new List<SeriesPoint>(60);
            timer1.Enabled = true;
        }

        private void AdjustChartRange()
        {
            if (AxisStartTime < DateTime.Now - new TimeSpan(0, 0, 1, 0)) {
                AxisStartTime = DateTime.Now - new TimeSpan(0, 0, 1, 0);
            }

            ((XYDiagram)chartControl1.Diagram).AxisX.Range.MinValue = AxisStartTime;
            ((XYDiagram)chartControl1.Diagram).AxisX.Range.MaxValue = DateTime.Now ;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeChart();
            //            Form1_Resize(null, null);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            _lastPlotTime = DateTime.Now;

            _val = r.Next(100, 200);

            SeriesPoint nextPoint = new SeriesPoint(DateTime.Now, _val);

            chartControl1.Titles[0].Text = $"Current value: {_val}";

            foreach (SeriesPoint point in chartControl1.Series[0].Points)
                if (Convert.ToDateTime(point.Argument) < DateTime.Now - new TimeSpan(0, 0, 0, 59))
                    tempPoints.Add(point);

            if (tempPoints.Count > 0)
                tempPoints.RemoveAt(0);

            foreach (SeriesPoint point in tempPoints)
                chartControl1.Series[0].Points.Remove(point);

            tempPoints.Clear();

            chartControl1.Series[0].Points.Add(nextPoint);

            AdjustChartRange();
        }
    }
}
