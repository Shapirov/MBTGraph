﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace WindowsFormsApplication1
{
    public class MyList
    {
        public Dictionary<string, Share> Shares;

        public MyList()
        {
            this.Shares = new Dictionary<string, Share>();
            this.Shares.Add("AUD/JPY", new Share(0.01));
            this.Shares.Add("AUD/USD", new Share(0.0001));
            this.Shares.Add("EUR/AUD", new Share(0.0001));
            this.Shares.Add("EUR/CHF", new Share(0.0001));
            this.Shares.Add("EUR/GBP", new Share(0.0001));
            this.Shares.Add("EUR/JPY", new Share(0.01));
            this.Shares.Add("EUR/USD", new Share(0.0001));
            this.Shares.Add("GBP/USD", new Share(0.0001));
            this.Shares.Add("GBP/CHF", new Share(0.0001));
            this.Shares.Add("GBP/JPY", new Share(0.01));
            this.Shares.Add("NZD/JPY", new Share(0.01));
            this.Shares.Add("NZD/USD", new Share(0.0001));
            this.Shares.Add("USD/CAD", new Share(0.0001));
            this.Shares.Add("USD/CHF", new Share(0.0001));
            this.Shares.Add("USD/JPY", new Share(0.01));

        }
        public void Load(string strLine)
        {
            string[] arr = strLine.Split(';');
            
            this.Shares[arr[0]].Load(arr);
        }
        public void LoadOperation(string strLine)
        {
            string[] arr = strLine.Split(';');
            if (int.Parse(arr[0]) == 1)
            {
                this.Shares[arr[1]].BuyOperations.Add(new Operation(1, (arr[2]), 1, int.Parse(arr[3])));
            }
            else if (int.Parse(arr[0]) == 0)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(0, (arr[2]), int.Parse(arr[3]), int.Parse(arr[4])));
            }
        }
        public void LoadOperation_1(string strLine)
        {
            string[] arr = strLine.Split(';');

            // ZigZag - 5
            if (int.Parse(arr[0]) == 5)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(5, arr[2], 1, int.Parse(arr[3])));
            }
            // ZigZag - 12
            else if (int.Parse(arr[0]) == 12)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(12, arr[2], 1, int.Parse(arr[3])));
            }
            // NN Real - 8
            else if (int.Parse(arr[0]) == 8)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(8, arr[2], 1, int.Parse(arr[3])));
            }
            // NN predicted - 9
            else if (int.Parse(arr[0]) == 9)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(9, arr[2], 1, int.Parse(arr[3])));
            }
            // Stoploss raize 
            else if (int.Parse(arr[0]) == 4)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(4, arr[2], 1, int.Parse(arr[3])));
            }
            // ??
            else if (int.Parse(arr[0]) == 3)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(3, arr[2], 1, int.Parse(arr[3])));
            }
            // NN
            else if (int.Parse(arr[0]) == 2)
            {
                this.Shares[arr[1]].SellOperations.Add(new Operation(2, (arr[2]), 1, int.Parse(arr[3])));
            }
            else
            {
                this.Shares[arr[1]].BuyOperations.Add(new Operation(int.Parse(arr[0]), (arr[2]), arr.Length > 4 ? int.Parse(arr[3]) : 1, arr.Length > 4 ? int.Parse(arr[4]) : int.Parse(arr[3])));
            }
        }
    }
    public class Share
    {
        public double PipUnit = 0.0001;
        public double Min = double.MaxValue;
        public double Max = double.MinValue;
        public double DeltaMxMn;
        public double LastPrice = 0;
        public double PipsProfit = 0;
        public List<Candle> Candles;
        public List<Operation> SellOperations;
        public List<Operation> BuyOperations;
        public Share(double d)
        {
            PipUnit = d;
            this.Candles = new List<Candle>();
            this.SellOperations = new List<Operation>();
            this.BuyOperations = new List<Operation>();
        }

        public void Load(string[] arr)
        {
            this.Candles.Add(new Candle(arr));
        }
    }
    public class Candle
    {
        public double Open;
        public double Close;
        public double High;
        public double Low;
        public double WMA;
        public double EMA;
        public double Bol_low;
        public double Bol_high;
        public double NN_Real;
        public double NN_Predicted;
        public double MyIndicatorA;
        public double MyIndicatorB;
        public double MyIndicatorC;
        public double MyIndicatorD;
        public int CandleIndex;

        public Candle(string[] arr)
        {
            double temp;
            this.Open = double.Parse(arr[1]);
            this.Close = double.Parse(arr[2]);
            this.High = double.Parse(arr[3]);
            this.Low = double.Parse(arr[4]);
            this.WMA = double.Parse(arr[5]);
            this.EMA = double.Parse(arr[6]);
            this.Bol_low = double.TryParse(arr[12], out temp) ? temp : this.EMA;
            this.Bol_high = double.TryParse(arr[13], out temp) ? temp : this.EMA;
            this.MyIndicatorA = double.TryParse(arr[8], out temp) ? temp : 0;
            this.MyIndicatorB = double.TryParse(arr[16], out temp) ? temp : 0;
            this.MyIndicatorC = double.TryParse(arr[15], out temp) ? temp : 0;
            this.MyIndicatorD = double.TryParse(arr[14], out temp) ? temp : 0;
            this.CandleIndex = int.Parse(arr[arr.Length - 1]);
        }
    }
    public class Operation
    {
        public double Price;
        public int CandleIndex;
        public int Sign;
        public int Direction;
        public string Lable;
        public Operation(int nSign, string dPrice, int nDirection, int nCandleIndex)
        {
            double d;
            this.Direction = nDirection;

            if (Double.TryParse(dPrice, out d))
            {
                this.Price = d;
            }
            else
            {
                this.Lable = dPrice;
            }
            this.CandleIndex = nCandleIndex;// -1;
            this.Sign = nSign;
        }
    }
}
