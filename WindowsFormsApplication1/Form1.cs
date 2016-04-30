using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class H : Form
    {
        public H()
        {
            InitializeComponent();
            LoadPanel("1", this.splitContainer1.Panel1);
            LoadPanel("2", this.splitContainer1.Panel2);
           
        }

        public void LoadPanel(string strPath, Panel pPanel)
        {
            bool bWithLiniarLine = true;
            string[] str = File.ReadAllLines(string.Format("a.txt", this.Text));


            int nCharts = -1;
            MyList lst = new MyList();

            foreach (string strLine in str)
            {
                lst.Load(strLine);
            }
            foreach (Share s in lst.Shares.Values)
            {
                while (s.Candles[0].CandleIndex < 0)
                {
                    s.Candles.RemoveAt(0);
                }

                s.Min = s.Candles.Min(C => C.Low);
                s.Max = s.Candles.Max(C => C.High);
                s.DeltaMxMn = s.Max - s.Min;
            }

            string[] filesNames = Directory.GetFiles(strPath);
            foreach (string curr in filesNames)
            {
                string[] strActivity = File.ReadAllLines(curr);

                if (bWithLiniarLine)
                {
                    foreach (string strLine in strActivity)
                    {
                        lst.LoadOperation_1(strLine);
                    }
                }
                else
                {
                    foreach (string strLine in strActivity)
                    {
                        lst.LoadOperation(strLine);
                    }
                }
            }


            foreach (string strCurrSymbol in lst.Shares.Keys)
            {
                Share currShare = lst.Shares[strCurrSymbol];
                int nSeriesIndex = 10;
                nCharts++;
                Chart chart = new Chart();
                Chart chartDec = new Chart();
                chart.Titles.Add(strCurrSymbol);
                
                chart.Name = strCurrSymbol;
                pPanel.Controls.Add(chart);
                pPanel.Controls.Add(chartDec);
                chart.Show();
                chartDec.Show();
                chart.Width = currShare.Candles.Count * 10;
                chartDec.Width = currShare.Candles.Count * 10; 
                chart.Height = 600;
                chartDec.Height = 300;
                chart.Top = 900 * nCharts;
                chartDec.Top = 600 + (900 * nCharts);





                chart.Series.Clear();
                chart.ChartAreas.Add("1");
                chartDec.Series.Clear();
                chartDec.ChartAreas.Add("1");


                chart.Series.Add("6");
                chart.Series["6"].XValueType = ChartValueType.Int32;
                chart.Series["6"].ChartType = SeriesChartType.Area;
                chart.Series["6"].BorderWidth = 1;
                chart.Series["6"].BorderColor = Color.FromName("Green");

                chart.Series.Add("1");
                chart.Series["1"].XValueType = ChartValueType.Int32;
                chart.Series["1"].ChartType = SeriesChartType.Candlestick;
                chart.Series["1"]["OpenCloseStyle"] = "Triangle";
                chart.Series["1"]["ShowOpenClose"] = "Both";
                chart.Series["1"]["PointWidth"] = "0.8";
                chart.Series["1"]["PriceUpColor"] = "Gray"; // up
                chart.Series["1"]["PriceDownColor"] = "Pink"; // down
                chart.ChartAreas["1"].AxisY.Minimum = 300;

                chart.Series.Add("2");
                chart.Series["2"].XValueType = ChartValueType.Int32;
                chart.Series["2"].ChartType = SeriesChartType.Spline;
                chart.Series["2"].BorderWidth = 2;

                chart.Series.Add("3");
                chart.Series["3"].XValueType = ChartValueType.Int32;
                chart.Series["3"].ChartType = SeriesChartType.Spline;
                chart.Series["3"].BorderWidth = 4;
                chart.Series["3"].BorderColor = Color.FromName("Green");

                chart.Series.Add("4");
                chart.Series["4"].XValueType = ChartValueType.Int32;
                chart.Series["4"].ChartType = SeriesChartType.Spline;
                chart.Series["4"].BorderWidth = 4;
                chart.Series["4"].BorderColor = Color.FromName("Green");

                chart.Series.Add("5");
                chart.Series["5"].XValueType = ChartValueType.Int32;
                chart.Series["5"].ChartType = SeriesChartType.Spline;
                chart.Series["5"].BorderWidth = 4;
                chart.Series["5"].BorderColor = Color.FromName("Green");



                /////////////////////////////////////////////
                /////////////////////////////////////////////
                //chartDec.Series.Add("7");
                //chartDec.Series["7"].XValueType = ChartValueType.Int32;
                //chartDec.Series["7"].ChartType = SeriesChartType.Area;
                //chartDec.Series["7"].BorderWidth = 3;
                //chartDec.Series["7"].BorderColor = Color.FromName("Black");

                chartDec.Series.Add("8");
                chartDec.Series["8"].XValueType = ChartValueType.Int32;
                chartDec.Series["8"].ChartType = SeriesChartType.Spline;
                chartDec.Series["8"].BorderWidth = 3;
                chartDec.Series["8"].BorderColor = Color.FromName("Yellow");

                chartDec.Series.Add("9");
                chartDec.Series["9"].XValueType = ChartValueType.Int32;
                chartDec.Series["9"].ChartType = SeriesChartType.Spline;
                chartDec.Series["9"].BorderWidth = 1;
                chartDec.Series["9"].BorderColor = Color.FromName("Orange");

                //chartDec.Series.Add("10");
                //chartDec.Series["10"].XValueType = ChartValueType.Int32;
                //chartDec.Series["10"].ChartType = SeriesChartType.Spline;
                //chartDec.Series["10"].BorderWidth = 2;
                //chartDec.Series["10"].BorderColor = Color.FromName("Blue");

                foreach (Candle currCandle in currShare.Candles)
                {
           
                    // adding date and high
                    chart.Series["1"].Points.AddXY(currCandle.CandleIndex, currCandle.High);
                    // adding low
                    chart.Series["1"].Points[currCandle.CandleIndex].YValues[1] = currCandle.Low;
                    if (chart.ChartAreas["1"].AxisY.Minimum > currCandle.Low)
                        chart.ChartAreas["1"].AxisY.Minimum = currCandle.Low;
                    //adding open
                    chart.Series["1"].Points[currCandle.CandleIndex].YValues[2] = currCandle.Open;
                    // adding close
                    chart.Series["1"].Points[currCandle.CandleIndex].YValues[3] = currCandle.Close;

                    chart.Series["2"].Points.AddXY(currCandle.CandleIndex, currCandle.WMA);
                    chart.Series["3"].Points.AddXY(currCandle.CandleIndex, currCandle.EMA);
                    chart.Series["4"].Points.AddXY(currCandle.CandleIndex, currCandle.Bol_low);
                    chart.Series["5"].Points.AddXY(currCandle.CandleIndex, currCandle.Bol_high);


                    //chartDec.Series["7"].Points.AddXY(currCandle.CandleIndex, currCandle.NN == -100 ? 0 : currCandle.NN);
                    //chartDec.Series["8"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorB);
                    //chartDec.Series["9"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorC);
                    //chartDec.Series["10"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorD);
                }

                if (bWithLiniarLine)
                {
                    string strSeriesIndex = string.Empty;
                    int nStam = 9999;
                    foreach (Operation o in currShare.BuyOperations)
                    {
                        if (o.Sign == 1)
                        {
                            currShare.LastPrice = o.Price;
                            strSeriesIndex = nSeriesIndex.ToString();
                            chart.Series.Add(strSeriesIndex);
                            chart.Series[strSeriesIndex].XValueType = ChartValueType.Int32;
                            chart.Series[strSeriesIndex].ChartType = SeriesChartType.Point;
                            chart.Series[strSeriesIndex].ChartType = SeriesChartType.Spline;
                            chart.Series[strSeriesIndex].BorderWidth = 8;
                            chart.Series[strSeriesIndex].IsValueShownAsLabel = true;
                            chart.Series[strSeriesIndex].BorderColor = Color.FromName("Black");

                            nSeriesIndex++;
                        }
                        else if (o.Sign == 0)
                        {
                            currShare.PipsProfit += o.Price - currShare.LastPrice;
                            currShare.LastPrice = o.Price;
                        }

                        if ((o.Sign == 0) || (o.Sign == 1))
                            chart.Series[strSeriesIndex].Points.AddXY(o.CandleIndex, o.Price);
                    }

                    string strName = nStam++.ToString();
                    chart.Series.Add(strName);
                    chart.Series[strName].XValueType = ChartValueType.Int32;
                    chart.Series[strName].ChartType = SeriesChartType.Line;
                    chart.Series[strName].BorderWidth = 1;
                    chart.Series[strName].IsValueShownAsLabel = false;
                    chart.Series[strName].BorderColor = Color.FromName("Black");
                    string strName1 = strName + strName;
                    chart.Series.Add(strName1);
                    chart.Series[strName1].XValueType = ChartValueType.Int32;
                    chart.Series[strName1].ChartType = SeriesChartType.Line;
                    chart.Series[strName1].BorderWidth = 3;
                    chart.Series[strName1].IsValueShownAsLabel = false;
                    chart.Series[strName1].BorderColor = Color.FromName("Black");
                    string strName2 = strName1 + strName1;
                    chart.Series.Add(strName2);
                    chart.Series[strName2].XValueType = ChartValueType.Int32;
                    chart.Series[strName2].ChartType = SeriesChartType.StackedColumn;
                    chart.Series[strName2].BorderWidth = 1;
                    chart.Series[strName2].IsValueShownAsLabel = false;
                    chart.Series[strName2].BorderColor = Color.FromName("Black");
                    string strName3 = strName2 + strName2;
                    chart.Series.Add(strName3);
                    chart.Series[strName3].XValueType = ChartValueType.Int32;
                    chart.Series[strName3].ChartType = SeriesChartType.StackedColumn;
                    chart.Series[strName3].BorderWidth = 1;
                    chart.Series[strName3].IsValueShownAsLabel = false;
                    chart.Series[strName3].BorderColor = Color.FromName("Black");
                    foreach (Operation o in currShare.SellOperations)
                    {
                        //chart.Series[strName].Points.AddXY(o.CandleIndex, currShare.Min + (o.Price * currShare.DeltaMxMn));
                        if (o.Price != 0)
                        {
                            if ((o.Sign == 5) && (o.CandleIndex > -1))
                            {
                                chart.Series[strName].Points.AddXY(o.CandleIndex + 1, o.Price);
                            }
                            else if ((o.Sign == 12) && (o.CandleIndex > -1))
                            {
                                chart.Series[strName1].Points.AddXY(o.CandleIndex + 1, o.Price);
                            }
                            else if ((o.Sign == 8) && (o.CandleIndex > -1))
                            {
                                //chart.Series[strName1].Points.AddXY(o.CandleIndex + 1, o.Price);
                                chartDec.Series["8"].Points.AddXY(o.CandleIndex, o.Price == -100 ? 0 : o.Price);
                            }
                            else if ((o.Sign == 9) && (o.CandleIndex > -1))
                            {
                                //chart.Series[strName1].Points.AddXY(o.CandleIndex + 1, o.Price);
                                chartDec.Series["9"].Points.AddXY(o.CandleIndex, o.Price == -1 ? 0 : o.Price);
                            }
                            else if (o.Sign == 4)
                            {
                     //           chart.Series[strName2].Points.AddXY(o.CandleIndex, o.Price);
                            }
                            else if (o.Sign == 3)
                            {
                           //     chart.Series[strName3].Points.AddXY(o.CandleIndex, o.Price);
                            }
                            else if (o.Sign == 2)
                            {
                                chart.Series["6"].Points.AddXY(o.CandleIndex, o.Price * (currShare.Max - currShare.Min) + currShare.Min);
                            }
                        }
                    }
                }
                else
                {
                    foreach (Operation o in currShare.BuyOperations)
                    {
                        chart.Series["5"].Points.AddXY(o.CandleIndex, o.Price);
                    }
                    foreach (Operation o in currShare.SellOperations)
                    {
                        chart.Series["6"].Points.AddXY(o.CandleIndex, o.Price);
                    }
                }
            }


            double d = 0;
            foreach (Share currShare in lst.Shares.Values)
            {
                d += currShare.PipsProfit;
            }
        }

        private void splitContainer1_Panel1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                this.splitContainer1.Panel2.AutoScrollPosition = new Point(this.splitContainer1.Panel1.HorizontalScroll.Value, e.NewValue);
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                this.splitContainer1.Panel2.AutoScrollPosition = new Point(e.NewValue, this.splitContainer1.Panel1.VerticalScroll.Value);
        }

        private void splitContainer1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void splitContainer1_Panel1_RightToLeftChanged(object sender, EventArgs e)
        {

        }
    }
}
