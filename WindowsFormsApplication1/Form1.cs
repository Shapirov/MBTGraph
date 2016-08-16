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
        public static bool bWithSecondery = false;
        public static int priorChartHighet = 800; // 1200;
        public static int secondChartHighet = 200; // 500;
        public static int width = 2; // 10;

        public H()
        {
            InitializeComponent();
            double d1 = LoadPanel("1", this.splitContainer1.Panel1);
            double d2 = LoadPanel("2", this.splitContainer1.Panel2);

        }

        public double LoadPanel(string strPath, Panel pPanel)
        {
            double ret = 0;
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
                chart.Show();

                if (H.bWithSecondery)
                {
                    pPanel.Controls.Add(chartDec);
                    chartDec.Show();
                }

                chart.Width = currShare.Candles.Count * H.width;
                chartDec.Width = currShare.Candles.Count * H.width;
                chart.Height = H.priorChartHighet;
                chartDec.Height = (H.bWithSecondery ? H.secondChartHighet : 0);
                chart.Top = ((H.bWithSecondery ? H.secondChartHighet : 0) + H.priorChartHighet) * nCharts;
                chartDec.Top = H.priorChartHighet + (((H.bWithSecondery ? H.secondChartHighet : 0) + H.priorChartHighet) * nCharts);





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
                chart.Series["3"].BorderWidth = 2;
                chart.Series["3"].BorderColor = Color.FromName("Green");

                chart.Series.Add("4");
                chart.Series["4"].XValueType = ChartValueType.Int32;
                chart.Series["4"].ChartType = SeriesChartType.Spline;
                chart.Series["4"].BorderWidth = 2;
                chart.Series["4"].BorderColor = Color.FromName("Green");

                chart.Series.Add("5");
                chart.Series["5"].XValueType = ChartValueType.Int32;
                chart.Series["5"].ChartType = SeriesChartType.Spline;
                chart.Series["5"].BorderWidth = 2;
                chart.Series["5"].BorderColor = Color.FromName("Green");



                /////////////////////////////////////////////
                /////////////////////////////////////////////


                chartDec.Series.Add("8");
                chartDec.Series["8"].XValueType = ChartValueType.Int32;
                chartDec.Series["8"].ChartType = SeriesChartType.SplineArea;
                chartDec.Series["8"].BorderWidth = 0;
                chartDec.Series["8"].BorderColor = Color.FromName("Yellow");
                chartDec.ChartAreas["1"].AxisY.Minimum = -0.005;
                chartDec.ChartAreas["1"].AxisY.Maximum = 0.005;

                chartDec.Series.Add("9");
                chartDec.Series["9"].XValueType = ChartValueType.Int32;
                chartDec.Series["9"].ChartType = SeriesChartType.SplineArea;
                chartDec.Series["9"].BorderWidth = 1;
                chartDec.Series["9"].BorderColor = Color.FromName("Orange");
                chartDec.ChartAreas["1"].AxisY.Minimum = -0.005;
                chartDec.ChartAreas["1"].AxisY.Maximum = 0.005;



                chartDec.Series.Add("101");
                chartDec.Series["101"].XValueType = ChartValueType.Int32;
                chartDec.Series["101"].ChartType = SeriesChartType.Spline;
                chartDec.Series["101"].BorderWidth = 3;
                chartDec.Series["101"].BorderColor = Color.FromName("Yellow");

                chartDec.Series.Add("102");
                chartDec.Series["102"].XValueType = ChartValueType.Int32;
                chartDec.Series["102"].ChartType = SeriesChartType.Spline;
                chartDec.Series["102"].BorderWidth = 3;
                chartDec.Series["102"].BorderColor = Color.FromName("Orange");

                chartDec.Series.Add("103");
                chartDec.Series["103"].XValueType = ChartValueType.Int32;
                chartDec.Series["103"].ChartType = SeriesChartType.Spline;
                chartDec.Series["103"].BorderWidth = 3;
                chartDec.Series["103"].BorderColor = Color.FromName("Black");

                chartDec.Series.Add("104");
                chartDec.Series["104"].XValueType = ChartValueType.Int32;
                chartDec.Series["104"].ChartType = SeriesChartType.Spline;
                chartDec.Series["104"].BorderWidth = 2;
                chartDec.Series["104"].BorderColor = Color.FromName("Blue");


                int n = 0;
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
               //     chart.Series["3"].Points.AddXY(currCandle.CandleIndex, currCandle.EMA);
                    //chart.Series["4"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorC);
                    //chart.Series["5"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorD);
                    //                   chart.Series["3"].Points.AddXY(currCandle.CandleIndex, currCandle.WMA);
                    //                   chart.Series["4"].Points.AddXY(currCandle.CandleIndex, currCandle.Bol_low);
                    //                   chart.Series["5"].Points.AddXY(currCandle.CandleIndex, currCandle.Bol_high);


                    double dx = 0;
                    if (n > 0)
                        dx = currCandle.MyIndicatorB / (100 * currShare.PipUnit);//GetTR(currCandle, currShare.Candles[n - 1]) / currCandle.MyIndicatorB;
                    //chartDec.Series["7"].Points.AddXY(currCandle.CandleIndex, currCandle.NN == -100 ? 0 : currCandle.NN);
                    //chartDec.Series["104"].Points.AddXY(currCandle.CandleIndex, dx > 1 ? 1 : dx);
                    //chartDec.Series["102"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorB);
                    //chartDec.Series["103"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorC);
                    //chartDec.Series["104"].Points.AddXY(currCandle.CandleIndex, currCandle.MyIndicatorD);
                    //chartDec.ChartAreas["1"].AxisY.Minimum = 0;
                    //chartDec.ChartAreas["1"].AxisY.Maximum = 1;
                    n++;
                }

                if (bWithLiniarLine)
                {
                    string strSeriesIndex = string.Empty;
                    int nPrev = 0;
                    double buy = 0;
                    foreach (Operation o in currShare.BuyOperations)
                    {


                        if ((o.Sign == 1) && (nPrev != 1))
                        {
                            buy = o.Price;
                            currShare.LastPrice = o.Price;
                            strSeriesIndex = nSeriesIndex.ToString();
                            chart.Series.Add(strSeriesIndex);
                            chart.Series[strSeriesIndex].XValueType = ChartValueType.Int32;
                            chart.Series[strSeriesIndex].ChartType = SeriesChartType.Point;
                            chart.Series[strSeriesIndex].ChartType = SeriesChartType.Spline;
                            chart.Series[strSeriesIndex].BorderWidth = 5;
                            chart.Series[strSeriesIndex].IsValueShownAsLabel = false;

                            chart.Series[strSeriesIndex].BorderColor = Color.FromName("Black");
                            nPrev = 1;
                            nSeriesIndex++;
                        }
                        else if (o.Sign == 0)
                        {
                            ret += o.Direction == 1 ? (o.Direction * (o.Price - buy)) / currShare.PipUnit : 0;
                            chart.Series[strSeriesIndex].Color = (o.Direction * (o.Price - buy) > 0) ? Color.FromName("Green") : Color.FromName("Red");
                            nPrev = 0;
                            currShare.PipsProfit += o.Price - currShare.LastPrice;
                            currShare.LastPrice = o.Price;
                        }

                        if ((o.Sign == 0) || (o.Sign == 1))
                            chart.Series[strSeriesIndex].Points.AddXY(o.CandleIndex, o.Price);

                    }
                    double dMaxPred = 0;
                    ///////////string strName = nStam++.ToString();
                    ///////////chart.Series.Add(strName);
                    ///////////chart.Series[strName].XValueType = ChartValueType.Int32;
                    ///////////chart.Series[strName].ChartType = SeriesChartType.Line;
                    ///////////chart.Series[strName].BorderWidth = 1;
                    ///////////chart.Series[strName].IsValueShownAsLabel = false;
                    ///////////chart.Series[strName].BorderColor = Color.FromName("Black");
                    ///////////string strName1 = strName + strName;
                    ///////////chart.Series.Add(strName1);
                    ///////////chart.Series[strName1].XValueType = ChartValueType.Int32;
                    ///////////chart.Series[strName1].ChartType = SeriesChartType.Line;
                    ///////////chart.Series[strName1].BorderWidth = 3;
                    ///////////chart.Series[strName1].IsValueShownAsLabel = false;
                    ///////////chart.Series[strName1].BorderColor = Color.FromName("Black");
                    ///////////string strName2 = strName1 + strName1;
                    ///////////chart.Series.Add(strName2);
                    ///////////chart.Series[strName2].XValueType = ChartValueType.Int32;
                    ///////////chart.Series[strName2].ChartType = SeriesChartType.StackedColumn;
                    ///////////chart.Series[strName2].BorderWidth = 1;
                    ///////////chart.Series[strName2].IsValueShownAsLabel = false;
                    ///////////chart.Series[strName2].BorderColor = Color.FromName("Black");
                    ///////////string strName3 = strName2 + strName2;
                    ///////////chart.Series.Add(strName3);
                    ///////////chart.Series[strName3].XValueType = ChartValueType.Int32;
                    ///////////chart.Series[strName3].ChartType = SeriesChartType.StackedColumn;
                    ///////////chart.Series[strName3].BorderWidth = 1;
                    ///////////chart.Series[strName3].IsValueShownAsLabel = false;
                    ///////////chart.Series[strName3].BorderColor = Color.FromName("Black");
                    foreach (Operation o in currShare.SellOperations)
                    {

                        //chart.Series[strName].Points.AddXY(o.CandleIndex, currShare.Min + (o.Price * currShare.DeltaMxMn));
                        if (o.Price != 0)
                        {
                            if ((o.Sign == 5) && (o.CandleIndex > -1))
                            {
                                //                              chart.Series[strName].Points.AddXY(o.CandleIndex + 1, o.Price);
                            }
                            else if ((o.Sign == 12) && (o.CandleIndex > -1))
                            {
                                //                               chart.Series[strName1].Points.AddXY(o.CandleIndex + 1, o.Price);
                            }
                            else if ((o.Sign == 8) && (o.CandleIndex > -1))
                            {
                                int nDelta = o.CandleIndex - chartDec.Series["8"].Points.Count;
                                int nOffset = chartDec.Series["8"].Points.Count;
                                for (int i = 0; i < nDelta; i++)
                                {
                                    chartDec.Series["8"].Points.AddXY(i + nOffset, 0);
                                }
                                //chart.Series[strName1].Points.AddXY(o.CandleIndex + 1, o.Price);
/////////                                chartDec.Series["8"].Points.AddXY(o.CandleIndex, o.Price == -100 ? 0 : o.Price);
                            }
                            else if ((o.Sign == 9) && (o.CandleIndex > -1))
                            {
                                int nDelta = o.CandleIndex - chartDec.Series["9"].Points.Count;
                                int nOffset = chartDec.Series["9"].Points.Count;
                                for (int i = 0; i < nDelta; i++)
                                {
                                    chartDec.Series["9"].Points.AddXY(i + nOffset, 0);
                                }
                                if (o.Price > dMaxPred)
                                    dMaxPred = o.Price;
                                //chart.Series[strName1].Points.AddXY(o.CandleIndex + 1, o.Price);
                                chartDec.Series["9"].Points.AddXY(o.CandleIndex, o.Price == -1 ? 0 : o.Price);
                                chartDec.ChartAreas["1"].AxisY.Minimum = -dMaxPred;
                                chartDec.ChartAreas["1"].AxisY.Maximum = dMaxPred;
                            }
                            else if (o.Sign == 4)
                            {
                                //chart.Series[strName2].Points.AddXY(o.CandleIndex, o.Price);
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
            return (ret);
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
        public double GetTR(Candle cCurrCandle, Candle cPrevCandle)
        {
            return (Math.Max(Math.Max(
                cCurrCandle.High - cPrevCandle.Close,
                cPrevCandle.Close - cCurrCandle.Low),
                cCurrCandle.High - cCurrCandle.Low));
        }
    }
}