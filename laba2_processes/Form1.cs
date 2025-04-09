using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace laba2_processes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            chart1.MouseWheel += chart1_MouseWheel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double mk = double.Parse(textBox1.Text);
                double sigma = double.Parse(textBox2.Text);
                double time = double.Parse(textBox3.Text);

                if (sigma > mk)
                    MessageBox.Show("Математическое ожидание не должно быть меньше отклонения");
                else if (time < 0)
                    MessageBox.Show("Время должно быть положительным");
                else
                {
                    Simulation simulator = new Simulation(mk, sigma, time);
                    var times = simulator.Times;
                    var timesJust = simulator.TimesPuassons;

                    InitializeDataTable(times, timesJust);
                    PaintStream(times, 1);
                    PaintStream(timesJust, 2);

                    label4.Text = $"Порядок потока Эрланга: {simulator.K}";
                    label5.Text = $"Общее количество: {times.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void InitializeDataTable(List<double> times, List<double> timesJust)
        {
            dataGridView1.Rows.Clear();

            int maxCount = Math.Max(times.Count, timesJust.Count);

            for (int i = 0; i < maxCount; i++)
            {
                string value1 = i < times.Count ? times[i].ToString("0.000") : "";
                string value2 = i < timesJust.Count ? timesJust[i].ToString("0.000") : "";

                dataGridView1.Rows.Add(value1, value2);
            }
        }

        public void PaintStream(List<double> times, int point)
        {
            chart1.Series[point-1].Points.Clear();
            foreach (var t in times)
            {
                chart1.Series[point-1].Points.AddXY(t, point); 
            }
        }
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                var chartArea = chart1.ChartAreas[0];
                double xMin = chartArea.AxisX.ScaleView.ViewMinimum;
                double xMax = chartArea.AxisX.ScaleView.ViewMaximum;

                double posXStart = chartArea.AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                double posXFinish = chartArea.AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;

                if (e.Delta < 0) // колесо вниз — отдаление
                {
                    chartArea.AxisX.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // колесо вверх — приближение
                {
                    chartArea.AxisX.ScaleView.Zoom(posXStart, posXFinish);
                }
            }
            catch {  }
        }

    }
}
