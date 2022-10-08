using System;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.Script.Optimization;
using DayOfWeek = TSLab.Script.Handlers.DayOfWeek;

namespace Ricom.Scripts
{
    public class Si_universal_10h : IExternalScript
    {
        private IPosition _mnd1Position;
        private IPosition _mnd2Position;
        private IPosition _tue1Position;
        private IPosition _tue2Position;
        private IPosition _wed1Position;
        private IPosition _wed2Position;
        private IPosition _thu1Position;
        private IPosition _thu2Position;
        private IPosition _fri1Position;
        private IPosition _fri2Position;
        private Time _time = new Time();
        private DayOfWeek _day = new DayOfWeek();
        private DaysInPosition _daysInPosition = new DaysInPosition();
        private IPane _mondayPane;
        private IPane _tuesdayPane;
        private IPane _wednesdayPane;
        private IPane _thursdayPane;
        private IPane _fridayPane;

        private IGraphList _mndGraphList;
        private IGraphList _tueGraphList;
        private IGraphList _wedGraphList;
        private IGraphList _thuGraphList;
        private IGraphList _friGraphList;

        public OptimProperty _1_entertime = new OptimProperty(180000, 1, 7, 1);
        public OptimProperty _2_stoploss = new OptimProperty(296, 50, 1000, 5);
        public OptimProperty _3_takeprofit = new OptimProperty(2000, 50, 1000, 5);

        public OptimProperty _41_MndDirection = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _42_TueDirection = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _43_WedDirection = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _44_ThuDirection = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _45_FriDirection = new OptimProperty(0, 0, 1000, 1);

        /*public OptimProperty _51_MndDirection2 = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _52_TueDirection2 = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _53_WedDirection2 = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _54_ThuDirection2 = new OptimProperty(0, 0, 1000, 1);
        public OptimProperty _55_FriDirection2 = new OptimProperty(0, 0, 1000, 1);*/

        //public OptimProperty _5_dayOfWeek = new OptimProperty(1, 1, 5, 1);


        public void Execute(IContext ctx, ISecurity symbol)
        {
            int enterTime = _1_entertime;
            int exitTime = enterTime - 4200;
            int stopLoss = _2_stoploss;
            int takeprofit = _3_takeprofit;
            int mndDirectionValue = _41_MndDirection;
            int tueDirectionValue = _42_TueDirection;
            int wedDirectionValue = _43_WedDirection;
            int thuDirectionValue = _44_ThuDirection;
            int friDirectionValue = _45_FriDirection;

            /*int mndDirectionValue2 = _51_MndDirection2;
            int tueDirectionValue2 = _52_TueDirection2;
            int wedDirectionValue2 = _53_WedDirection2;
            int thuDirectionValue2 = _54_ThuDirection2;
            int friDirectionValue2 = _55_FriDirection2;*/

            int dayOfWeek = 0;
            double mndDaysInPos1 = 0;
            double mndDaysInPos2 = 0;
            double tueDaysInPos1 = 0;
            double tueDaysInPos2 = 0;
            double wedDaysInPos1 = 0;
            double wedDaysInPos2 = 0;
            double thuDaysInPos1 = 0;
            double thuDaysInPos2 = 0;
            double friDaysInPos1 = 0;
            double friDaysInPos2 = 0;
            int barsCount = symbol.Bars.Count;

            var time = ctx.GetData
                (
                    "Time", new[] { "symbol" },
                    () => _time.Execute(symbol)
                );

            var weekDay = ctx.GetData
                (
                    "WeekDay", new[] { "symbol" },
                    () => _day.Execute(symbol)
                );

            #region Grapths
            #region Monday
            _mondayPane = ctx.CreatePane("Monday", 100D, false, true);
            _mndGraphList = _mondayPane.AddList(symbol.Symbol, symbol, CandleStyles.BAR_CANDLE, true, true, true, true, 255,
                PaneSides.RIGHT);
            var mndStopLoss_1 = new double[barsCount];
            var mndTakeProfit_1 = new double[barsCount];
            var mndStopLoss_2 = new double[barsCount];
            var mndTakeProfit_2 = new double[barsCount];
            #endregion

            #region Tuesday
            _tuesdayPane = ctx.CreatePane("Tuesday", 100D, false, false);
            _tueGraphList = _tuesdayPane.AddList(symbol.Symbol, symbol, CandleStyles.BAR_CANDLE, true, true, true, true, 255,
                PaneSides.RIGHT);
            var tueStopLoss_1 = new double[barsCount];
            var tueTakeProfit_1 = new double[barsCount];
            var tueStopLoss_2 = new double[barsCount];
            var tueTakeProfit_2 = new double[barsCount];
            #endregion

            #region Wednesday
            _wednesdayPane = ctx.CreatePane("Wednesday", 100D, false, false);
            _wedGraphList = _wednesdayPane.AddList(symbol.Symbol, symbol, CandleStyles.BAR_CANDLE, true, true, true, true, 255,
                PaneSides.RIGHT);
            var wedStopLoss_1 = new double[barsCount];
            var wedTakeProfit_1 = new double[barsCount];
            var wedStopLoss_2 = new double[barsCount];
            var wedTakeProfit_2 = new double[barsCount];
            #endregion

            #region Thursday
            _thursdayPane = ctx.CreatePane("Thursday", 100D, false, false);
            _thuGraphList = _thursdayPane.AddList(symbol.Symbol, symbol, CandleStyles.BAR_CANDLE, true, true, true, true, 255,
                PaneSides.RIGHT);
            var thuStopLoss_1 = new double[barsCount];
            var thuTakeProfit_1 = new double[barsCount];
            var thuStopLoss_2 = new double[barsCount];
            var thuTakeProfit_2 = new double[barsCount];
            #endregion

            #region Friday
            _fridayPane = ctx.CreatePane("Friday", 100D, false, false);
            _friGraphList = _fridayPane.AddList(symbol.Symbol, symbol, CandleStyles.BAR_CANDLE, true, true, true, true, 255,
                PaneSides.RIGHT);
            var friStopLoss_1 = new double[barsCount];
            var friTakeProfit_1 = new double[barsCount];
            var friStopLoss_2 = new double[barsCount];
            var friTakeProfit_2 = new double[barsCount];
            #endregion
            //var test = new double[barsCount];

            #endregion

            #region Trade Cycle
            for (var bar = 0; bar < barsCount; bar++)
            {
                //_lastActivePosition = symbol.Positions.GetLastPositionActive(bar);
                _mnd1Position = symbol.Positions.GetLastActiveForSignal("Mnd_1", bar);
                _mnd2Position = symbol.Positions.GetLastActiveForSignal("Mnd_2", bar);
                _tue1Position = symbol.Positions.GetLastActiveForSignal("Tue_1", bar);
                _tue2Position = symbol.Positions.GetLastActiveForSignal("Tue_2", bar);
                _wed1Position = symbol.Positions.GetLastActiveForSignal("Wed_1", bar);
                _wed2Position = symbol.Positions.GetLastActiveForSignal("Wed_2", bar);
                _thu1Position = symbol.Positions.GetLastActiveForSignal("Thu_1", bar);
                _thu2Position = symbol.Positions.GetLastActiveForSignal("Thu_2", bar);
                _fri1Position = symbol.Positions.GetLastActiveForSignal("Fri_1", bar);
                _fri2Position = symbol.Positions.GetLastActiveForSignal("Fri_2", bar);

                #region Monday
                dayOfWeek = 1;

                #region Enter
               
                if (_mnd1Position == null)
                {
                    if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                    {
                        //test[bar] = 0;

                        if (mndDirectionValue > 0)
                        {
                            symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(mndDirectionValue), "Mnd_1");
                        }
                        else if (mndDirectionValue < 0)
                        {
                            symbol.Positions.SellAtMarket(bar + 1, Math.Abs(mndDirectionValue), "Mnd_1");
                        }
                    }
                    if (_mnd2Position != null)
                    {
                        mndDaysInPos2 = _daysInPosition.Execute(_mnd2Position, bar);

                        if (_mnd2Position.IsLong)
                        {
                            var mndTake_2 = _mnd2Position.EntryPrice + takeprofit;
                            var mndStop_2 = _mnd2Position.EntryPrice - stopLoss;
                            mndStopLoss_2[bar] = mndStop_2;
                            mndTakeProfit_2[bar] = mndTake_2;

                            if (symbol.HighPrices[bar] >= mndTake_2 || mndDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= mndStop_2)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_SL");
                            }
                        }
                        if (_mnd2Position.IsShort)
                        {
                            var mndTake_2 = _mnd2Position.EntryPrice - takeprofit;
                            var mndStop_2 = _mnd2Position.EntryPrice + stopLoss;
                            mndStopLoss_2[bar] = mndStop_2;
                            mndTakeProfit_2[bar] = mndTake_2;

                            if (symbol.LowPrices[bar] <= mndTake_2 || mndDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= mndStop_2)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_SL");
                            }
                        }
                    }
                }

                #endregion

                #region Exit
                else if (_mnd1Position != null)
                {
                    mndDaysInPos1 = _daysInPosition.Execute(_mnd1Position, bar);
                    //test[bar] = 1.0;

                    if (_mnd1Position.IsLong)
                    {
                        var mndTake_1 = _mnd1Position.EntryPrice + takeprofit;
                        var mndStop_1 = _mnd1Position.EntryPrice - stopLoss;
                        mndStopLoss_1[bar] = mndStop_1;
                        mndTakeProfit_1[bar] = mndTake_1;

                        if (symbol.HighPrices[bar] >= mndTake_1 || mndDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _mnd1Position.CloseAtMarket(bar + 1, "Mnd_1_TP");
                        }
                        else if (symbol.LowPrices[bar] <= mndStop_1)
                        {
                            _mnd1Position.CloseAtMarket(bar + 1, "Mnd_1_SL");
                        }
                    }
                    if (_mnd1Position.IsShort)
                    {
                        var mndTake_1 = _mnd1Position.EntryPrice - takeprofit;
                        var mndStop_1 = _mnd1Position.EntryPrice + stopLoss;
                        mndStopLoss_1[bar] = mndStop_1;
                        mndTakeProfit_1[bar] = mndTake_1;

                        if (symbol.LowPrices[bar] <= mndTake_1 || mndDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _mnd1Position.CloseAtMarket(bar + 1, "Mnd_1_TP");
                        }
                        else if (symbol.HighPrices[bar] >= mndStop_1)
                        {
                            _mnd1Position.CloseAtMarket(bar + 1, "Mnd_1_SL");
                        }
                    }

                    if (_mnd2Position == null)
                    {
                        if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                        {
                            //test[bar] = 0;

                            if (mndDirectionValue > 0)
                            {
                                symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(mndDirectionValue), "Mnd_2");
                            }
                            else if (mndDirectionValue < 0)
                            {
                                symbol.Positions.SellAtMarket(bar + 1, Math.Abs(mndDirectionValue), "Mnd_2");
                            }
                        }
                    }

                    if (_mnd2Position != null)
                    {
                        mndDaysInPos2 = _daysInPosition.Execute(_mnd2Position, bar);

                        if (_mnd2Position.IsLong)
                        {
                            var mndTake_2 = _mnd2Position.EntryPrice + takeprofit;
                            var mndStop_2 = _mnd2Position.EntryPrice - stopLoss;
                            mndStopLoss_2[bar] = mndStop_2;
                            mndTakeProfit_2[bar] = mndTake_2;

                            if (symbol.HighPrices[bar] >= mndTake_2 || mndDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= mndStop_2)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_SL");
                            }
                        }
                        if (_mnd2Position.IsShort)
                        {
                            var mndTake_2 = _mnd2Position.EntryPrice - takeprofit;
                            var mndStop_2 = _mnd2Position.EntryPrice + stopLoss;
                            mndStopLoss_2[bar] = mndStop_2;
                            mndTakeProfit_2[bar] = mndTake_2;

                            if (symbol.LowPrices[bar] <= mndTake_2 || mndDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= mndStop_2)
                            {
                                _mnd2Position.CloseAtMarket(bar + 1, "Mnd_2_SL");
                            }
                        }
                    }
                }

                #endregion
                #endregion

                #region Tuesday
                dayOfWeek = 2;

                #region Enter

                if (_tue1Position == null)
                {
                    if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                    {
                        //test[bar] = 0;

                        if (tueDirectionValue > 0)
                        {
                            symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(tueDirectionValue), "Tue_1");
                        }
                        else if (tueDirectionValue < 0)
                        {
                            symbol.Positions.SellAtMarket(bar + 1, Math.Abs(tueDirectionValue), "Tue_1");
                        }
                    }
                    if (_tue2Position != null)
                    {
                        tueDaysInPos2 = _daysInPosition.Execute(_tue2Position, bar);

                        if (_tue2Position.IsLong)
                        {
                            var tueTake_2 = _tue2Position.EntryPrice + takeprofit;
                            var tueStop_2 = _tue2Position.EntryPrice - stopLoss;
                            tueStopLoss_2[bar] = tueStop_2;
                            tueTakeProfit_2[bar] = tueTake_2;

                            if (symbol.HighPrices[bar] >= tueTake_2 || tueDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= tueStop_2)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_SL");
                            }
                        }
                        if (_tue2Position.IsShort)
                        {
                            var tueTake_2 = _tue2Position.EntryPrice - takeprofit;
                            var tueStop_2 = _tue2Position.EntryPrice + stopLoss;
                            tueStopLoss_2[bar] = tueStop_2;
                            tueTakeProfit_2[bar] = tueTake_2;

                            if (symbol.LowPrices[bar] <= tueTake_2 || tueDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= tueStop_2)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_SL");
                            }
                        }
                    }
                }

                #endregion

                #region Exit
                else if (_tue1Position != null)
                {
                    tueDaysInPos1 = _daysInPosition.Execute(_tue1Position, bar);
                    //test[bar] = 1.0;

                    if (_tue1Position.IsLong)
                    {
                        var tueTake_1 = _tue1Position.EntryPrice + takeprofit;
                        var tueStop_1 = _tue1Position.EntryPrice - stopLoss;
                        tueStopLoss_1[bar] = tueStop_1;
                        tueTakeProfit_1[bar] = tueTake_1;

                        if (symbol.HighPrices[bar] >= tueTake_1 || tueDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _tue1Position.CloseAtMarket(bar + 1, "Tue_1_TP");
                        }
                        else if (symbol.LowPrices[bar] <= tueStop_1)
                        {
                            _tue1Position.CloseAtMarket(bar + 1, "Tue_1_SL");
                        }
                    }
                    if (_tue1Position.IsShort)
                    {
                        var tueTake_1 = _tue1Position.EntryPrice - takeprofit;
                        var tueStop_1 = _tue1Position.EntryPrice + stopLoss;
                        tueStopLoss_1[bar] = tueStop_1;
                        tueTakeProfit_1[bar] = tueTake_1;

                        if (symbol.LowPrices[bar] <= tueTake_1 || tueDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _tue1Position.CloseAtMarket(bar + 1, "Tue_1_TP");
                        }
                        else if (symbol.HighPrices[bar] >= tueStop_1)
                        {
                            _tue1Position.CloseAtMarket(bar + 1, "Tue_1_SL");
                        }
                    }

                    if (_tue2Position == null)
                    {
                        if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                        {
                            //test[bar] = 0;

                            if (tueDirectionValue > 0)
                            {
                                symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(tueDirectionValue), "Tue_2");
                            }
                            else if (tueDirectionValue < 0)
                            {
                                symbol.Positions.SellAtMarket(bar + 1, Math.Abs(tueDirectionValue), "Tue_2");
                            }
                        }
                    }

                    if (_tue2Position != null)
                    {
                        tueDaysInPos2 = _daysInPosition.Execute(_tue2Position, bar);

                        if (_tue2Position.IsLong)
                        {
                            var tueTake_2 = _tue2Position.EntryPrice + takeprofit;
                            var tueStop_2 = _tue2Position.EntryPrice - stopLoss;
                            tueStopLoss_2[bar] = tueStop_2;
                            tueTakeProfit_2[bar] = tueTake_2;

                            if (symbol.HighPrices[bar] >= tueTake_2 || tueDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= tueStop_2)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_SL");
                            }
                        }
                        if (_tue2Position.IsShort)
                        {
                            var tueTake_2 = _tue2Position.EntryPrice - takeprofit;
                            var tueStop_2 = _tue2Position.EntryPrice + stopLoss;
                            tueStopLoss_2[bar] = tueStop_2;
                            tueTakeProfit_2[bar] = tueTake_2;

                            if (symbol.LowPrices[bar] <= tueTake_2 || tueDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= tueStop_2)
                            {
                                _tue2Position.CloseAtMarket(bar + 1, "Tue_2_SL");
                            }
                        }
                    }
                }

                #endregion
                #endregion

                #region Wednesday
                dayOfWeek = 3;

                #region Enter

                if (_wed1Position == null)
                {
                    if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                    {
                        //test[bar] = 0;

                        if (wedDirectionValue > 0)
                        {
                            symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(wedDirectionValue), "Wed_1");
                        }
                        else if (wedDirectionValue < 0)
                        {
                            symbol.Positions.SellAtMarket(bar + 1, Math.Abs(wedDirectionValue), "Wed_1");
                        }
                    }
                    if (_wed2Position != null)
                    {
                        wedDaysInPos2 = _daysInPosition.Execute(_wed2Position, bar);

                        if (_wed2Position.IsLong)
                        {
                            var wedTake_2 = _wed2Position.EntryPrice + takeprofit;
                            var wedStop_2 = _wed2Position.EntryPrice - stopLoss;
                            wedStopLoss_2[bar] = wedStop_2;
                            wedTakeProfit_2[bar] = wedTake_2;

                            if (symbol.HighPrices[bar] >= wedTake_2 || wedDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= wedStop_2)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_SL");
                            }
                        }
                        if (_wed2Position.IsShort)
                        {
                            var wedTake_2 = _wed2Position.EntryPrice - takeprofit;
                            var wedStop_2 = _wed2Position.EntryPrice + stopLoss;
                            wedStopLoss_2[bar] = wedStop_2;
                            wedTakeProfit_2[bar] = wedTake_2;

                            if (symbol.LowPrices[bar] <= wedTake_2 || wedDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= wedStop_2)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_SL");
                            }
                        }
                    }
                }

                #endregion

                #region Exit
                else if (_wed1Position != null)
                {
                    wedDaysInPos1 = _daysInPosition.Execute(_wed1Position, bar);
                    //test[bar] = 1.0;

                    if (_wed1Position.IsLong)
                    {
                        var wedTake_1 = _wed1Position.EntryPrice + takeprofit;
                        var wedStop_1 = _wed1Position.EntryPrice - stopLoss;
                        wedStopLoss_1[bar] = wedStop_1;
                        wedTakeProfit_1[bar] = wedTake_1;

                        if (symbol.HighPrices[bar] >= wedTake_1 || wedDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _wed1Position.CloseAtMarket(bar + 1, "Wed_1_TP");
                        }
                        else if (symbol.LowPrices[bar] <= wedStop_1)
                        {
                            _wed1Position.CloseAtMarket(bar + 1, "Wed_1_SL");
                        }
                    }
                    if (_wed1Position.IsShort)
                    {
                        var wedTake_1 = _wed1Position.EntryPrice - takeprofit;
                        var wedStop_1 = _wed1Position.EntryPrice + stopLoss;
                        wedStopLoss_1[bar] = wedStop_1;
                        wedTakeProfit_1[bar] = wedTake_1;

                        if (symbol.LowPrices[bar] <= wedTake_1 || wedDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _wed1Position.CloseAtMarket(bar + 1, "Wed_1_TP");
                        }
                        else if (symbol.HighPrices[bar] >= wedStop_1)
                        {
                            _wed1Position.CloseAtMarket(bar + 1, "Wed_1_SL");
                        }
                    }

                    if (_wed2Position == null)
                    {
                        if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                        {
                            //test[bar] = 0;

                            if (wedDirectionValue > 0)
                            {
                                symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(wedDirectionValue), "Wed_2");
                            }
                            else if (wedDirectionValue < 0)
                            {
                                symbol.Positions.SellAtMarket(bar + 1, Math.Abs(wedDirectionValue), "Wed_2");
                            }
                        }
                    }

                    if (_wed2Position != null)
                    {
                        wedDaysInPos2 = _daysInPosition.Execute(_wed2Position, bar);

                        if (_wed2Position.IsLong)
                        {
                            var wedTake_2 = _wed2Position.EntryPrice + takeprofit;
                            var wedStop_2 = _wed2Position.EntryPrice - stopLoss;
                            wedStopLoss_2[bar] = wedStop_2;
                            wedTakeProfit_2[bar] = wedTake_2;

                            if (symbol.HighPrices[bar] >= wedTake_2 || wedDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= wedStop_2)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_SL");
                            }
                        }
                        if (_wed2Position.IsShort)
                        {
                            var wedTake_2 = _wed2Position.EntryPrice - takeprofit;
                            var wedStop_2 = _wed2Position.EntryPrice + stopLoss;
                            wedStopLoss_2[bar] = wedStop_2;
                            wedTakeProfit_2[bar] = wedTake_2;

                            if (symbol.LowPrices[bar] <= wedTake_2 || wedDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= wedStop_2)
                            {
                                _wed2Position.CloseAtMarket(bar + 1, "Wed_2_SL");
                            }
                        }
                    }
                }

                #endregion
                #endregion

                #region Thursday
                dayOfWeek = 4;

                #region Enter

                if (_thu1Position == null)
                {
                    if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                    {
                        //test[bar] = 0;

                        if (thuDirectionValue > 0)
                        {
                            symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(thuDirectionValue), "Thu_1");
                        }
                        else if (thuDirectionValue < 0)
                        {
                            symbol.Positions.SellAtMarket(bar + 1, Math.Abs(thuDirectionValue), "Thu_1");
                        }
                    }
                    if (_thu2Position != null)
                    {
                        thuDaysInPos2 = _daysInPosition.Execute(_thu2Position, bar);

                        if (_thu2Position.IsLong)
                        {
                            var thuTake_2 = _thu2Position.EntryPrice + takeprofit;
                            var thuStop_2 = _thu2Position.EntryPrice - stopLoss;
                            thuStopLoss_2[bar] = thuStop_2;
                            thuTakeProfit_2[bar] = thuTake_2;

                            if (symbol.HighPrices[bar] >= thuTake_2 || thuDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= thuStop_2)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_SL");
                            }
                        }
                        if (_thu2Position.IsShort)
                        {
                            var thuTake_2 = _thu2Position.EntryPrice - takeprofit;
                            var thuStop_2 = _thu2Position.EntryPrice + stopLoss;
                            thuStopLoss_2[bar] = thuStop_2;
                            thuTakeProfit_2[bar] = thuTake_2;

                            if (symbol.LowPrices[bar] <= thuTake_2 || thuDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= thuStop_2)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_SL");
                            }
                        }
                    }
                }

                #endregion

                #region Exit
                else if (_thu1Position != null)
                {
                    thuDaysInPos1 = _daysInPosition.Execute(_thu1Position, bar);
                    //test[bar] = 1.0;

                    if (_thu1Position.IsLong)
                    {
                        var thuTake_1 = _thu1Position.EntryPrice + takeprofit;
                        var thuStop_1 = _thu1Position.EntryPrice - stopLoss;
                        thuStopLoss_1[bar] = thuStop_1;
                        thuTakeProfit_1[bar] = thuTake_1;

                        if (symbol.HighPrices[bar] >= thuTake_1 || thuDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _thu1Position.CloseAtMarket(bar + 1, "Thu_1_TP");
                        }
                        else if (symbol.LowPrices[bar] <= thuStop_1)
                        {
                            _thu1Position.CloseAtMarket(bar + 1, "Thu_1_SL");
                        }
                    }
                    if (_thu1Position.IsShort)
                    {
                        var thuTake_1 = _thu1Position.EntryPrice - takeprofit;
                        var thuStop_1 = _thu1Position.EntryPrice + stopLoss;
                        thuStopLoss_1[bar] = thuStop_1;
                        thuTakeProfit_1[bar] = thuTake_1;

                        if (symbol.LowPrices[bar] <= thuTake_1 || thuDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _thu1Position.CloseAtMarket(bar + 1, "Thu_1_TP");
                        }
                        else if (symbol.HighPrices[bar] >= thuStop_1)
                        {
                            _thu1Position.CloseAtMarket(bar + 1, "Thu_1_SL");
                        }
                    }

                    if (_thu2Position == null)
                    {
                        if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                        {
                            //test[bar] = 0;

                            if (thuDirectionValue > 0)
                            {
                                symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(thuDirectionValue), "Thu_2");
                            }
                            else if (thuDirectionValue < 0)
                            {
                                symbol.Positions.SellAtMarket(bar + 1, Math.Abs(thuDirectionValue), "Thu_2");
                            }
                        }
                    }

                    if (_thu2Position != null)
                    {
                        thuDaysInPos2 = _daysInPosition.Execute(_thu2Position, bar);

                        if (_thu2Position.IsLong)
                        {
                            var thuTake_2 = _thu2Position.EntryPrice + takeprofit;
                            var thuStop_2 = _thu2Position.EntryPrice - stopLoss;
                            thuStopLoss_2[bar] = thuStop_2;
                            thuTakeProfit_2[bar] = thuTake_2;

                            if (symbol.HighPrices[bar] >= thuTake_2 || thuDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= thuStop_2)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_SL");
                            }
                        }
                        if (_thu2Position.IsShort)
                        {
                            var thuTake_2 = _thu2Position.EntryPrice - takeprofit;
                            var thuStop_2 = _thu2Position.EntryPrice + stopLoss;
                            thuStopLoss_2[bar] = thuStop_2;
                            thuTakeProfit_2[bar] = thuTake_2;

                            if (symbol.LowPrices[bar] <= thuTake_2 || thuDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= thuStop_2)
                            {
                                _thu2Position.CloseAtMarket(bar + 1, "Thu_2_SL");
                            }
                        }
                    }
                }

                #endregion
                #endregion

                #region Friday
                dayOfWeek = 5;

                #region Enter

                if (_fri1Position == null)
                {
                    if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                    {
                        //test[bar] = 0;

                        if (friDirectionValue > 0)
                        {
                            symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(friDirectionValue), "Fri_1");
                        }
                        else if (friDirectionValue < 0)
                        {
                            symbol.Positions.SellAtMarket(bar + 1, Math.Abs(friDirectionValue), "Fri_1");
                        }
                    }
                    if (_fri2Position != null)
                    {
                        friDaysInPos2 = _daysInPosition.Execute(_fri2Position, bar);

                        if (_fri2Position.IsLong)
                        {
                            var friTake_2 = _fri2Position.EntryPrice + takeprofit;
                            var friStop_2 = _fri2Position.EntryPrice - stopLoss;
                            friStopLoss_2[bar] = friStop_2;
                            friTakeProfit_2[bar] = friTake_2;

                            if (symbol.HighPrices[bar] >= friTake_2 || friDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= friStop_2)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_SL");
                            }
                        }
                        if (_fri2Position.IsShort)
                        {
                            var friTake_2 = _fri2Position.EntryPrice - takeprofit;
                            var friStop_2 = _fri2Position.EntryPrice + stopLoss;
                            friStopLoss_2[bar] = friStop_2;
                            friTakeProfit_2[bar] = friTake_2;

                            if (symbol.LowPrices[bar] <= friTake_2 || friDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= friStop_2)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_SL");
                            }
                        }
                    }
                }

                #endregion

                #region Exit
                else if (_fri1Position != null)
                {
                    friDaysInPos1 = _daysInPosition.Execute(_fri1Position, bar);
                    //test[bar] = 1.0;

                    if (_fri1Position.IsLong)
                    {
                        var friTake_1 = _fri1Position.EntryPrice + takeprofit;
                        var friStop_1 = _fri1Position.EntryPrice - stopLoss;
                        friStopLoss_1[bar] = friStop_1;
                        friTakeProfit_1[bar] = friTake_1;

                        if (symbol.HighPrices[bar] >= friTake_1 || friDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _fri1Position.CloseAtMarket(bar + 1, "Fri_1_TP");
                        }
                        else if (symbol.LowPrices[bar] <= friStop_1)
                        {
                            _fri1Position.CloseAtMarket(bar + 1, "Fri_1_SL");
                        }
                    }
                    if (_fri1Position.IsShort)
                    {
                        var friTake_1 = _fri1Position.EntryPrice - takeprofit;
                        var friStop_1 = _fri1Position.EntryPrice + stopLoss;
                        friStopLoss_1[bar] = friStop_1;
                        friTakeProfit_1[bar] = friTake_1;

                        if (symbol.LowPrices[bar] <= friTake_1 || friDaysInPos1 > 13.5 && time[bar] >= exitTime)
                        {
                            _fri1Position.CloseAtMarket(bar + 1, "Fri_1_TP");
                        }
                        else if (symbol.HighPrices[bar] >= friStop_1)
                        {
                            _fri1Position.CloseAtMarket(bar + 1, "Fri_1_SL");
                        }
                    }

                    if (_fri2Position == null)
                    {
                        if (weekDay[bar] == dayOfWeek && time[bar] == enterTime - 4100)
                        {
                            //test[bar] = 0;

                            if (friDirectionValue > 0)
                            {
                                symbol.Positions.BuyAtMarket(bar + 1, Math.Abs(friDirectionValue), "Fri_2");
                            }
                            else if (friDirectionValue < 0)
                            {
                                symbol.Positions.SellAtMarket(bar + 1, Math.Abs(friDirectionValue), "Fri_2");
                            }
                        }
                    }

                    if (_fri2Position != null)
                    {
                        friDaysInPos2 = _daysInPosition.Execute(_fri2Position, bar);

                        if (_fri2Position.IsLong)
                        {
                            var friTake_2 = _fri2Position.EntryPrice + takeprofit;
                            var friStop_2 = _fri2Position.EntryPrice - stopLoss;
                            friStopLoss_2[bar] = friStop_2;
                            friTakeProfit_2[bar] = friTake_2;

                            if (symbol.HighPrices[bar] >= friTake_2 || friDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_TP");
                            }
                            else if (symbol.LowPrices[bar] <= friStop_2)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_SL");
                            }
                        }
                        if (_fri2Position.IsShort)
                        {
                            var friTake_2 = _fri2Position.EntryPrice - takeprofit;
                            var friStop_2 = _fri2Position.EntryPrice + stopLoss;
                            friStopLoss_2[bar] = friStop_2;
                            friTakeProfit_2[bar] = friTake_2;

                            if (symbol.LowPrices[bar] <= friTake_2 || friDaysInPos2 > 13.5 && time[bar] >= exitTime)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_TP");
                            }
                            else if (symbol.HighPrices[bar] >= friStop_2)
                            {
                                _fri2Position.CloseAtMarket(bar + 1, "Fri_2_SL");
                            }
                        }
                    }
                }

                #endregion
                #endregion
            }
            #endregion

            #region Monday
            var color = new Color(System.Drawing.Color.Red.ToArgb());
            _mondayPane.AddList("mndSL_1", mndStopLoss_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            _mondayPane.AddList("mndTP_1", mndTakeProfit_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);

            color = new Color(System.Drawing.Color.Black.ToArgb());
            _mondayPane.AddList("mndSL_2", mndStopLoss_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Blue.ToArgb());
            _mondayPane.AddList("mndTP_2", mndTakeProfit_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            #endregion

            #region Tuesday
            color = new Color(System.Drawing.Color.Red.ToArgb());
            _tuesdayPane.AddList("tueSL_1", tueStopLoss_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            _tuesdayPane.AddList("tueTP_1", tueTakeProfit_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);

            color = new Color(System.Drawing.Color.Black.ToArgb());
            _tuesdayPane.AddList("tueSL_2", tueStopLoss_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Blue.ToArgb());
            _tuesdayPane.AddList("tueTP_2", tueTakeProfit_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            #endregion

            #region Wednesday
            color = new Color(System.Drawing.Color.Red.ToArgb());
            _wednesdayPane.AddList("wedSL_1", wedStopLoss_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            _wednesdayPane.AddList("wedTP_1", wedTakeProfit_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);

            color = new Color(System.Drawing.Color.Black.ToArgb());
            _wednesdayPane.AddList("wedSL_2", wedStopLoss_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Blue.ToArgb());
            _wednesdayPane.AddList("wedTP_2", wedTakeProfit_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            #endregion

            #region Thursday
            color = new Color(System.Drawing.Color.Red.ToArgb());
            _thursdayPane.AddList("thuSL_1", thuStopLoss_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            _thursdayPane.AddList("thuTP_1", thuTakeProfit_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);

            color = new Color(System.Drawing.Color.Black.ToArgb());
            _thursdayPane.AddList("thuSL_2", thuStopLoss_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Blue.ToArgb());
            _thursdayPane.AddList("thuTP_2", thuTakeProfit_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            #endregion

            #region Friday
            color = new Color(System.Drawing.Color.Red.ToArgb());
            _fridayPane.AddList("friSL_1", friStopLoss_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Green.ToArgb());
            _fridayPane.AddList("friTP_1", friTakeProfit_1, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);

            color = new Color(System.Drawing.Color.Black.ToArgb());
            _fridayPane.AddList("friSL_2", friStopLoss_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            color = new Color(System.Drawing.Color.Blue.ToArgb());
            _fridayPane.AddList("friTP_2", friTakeProfit_2, ListStyles.LINE_WO_ZERO, color, LineStyles.DOT, PaneSides.LEFT);
            #endregion
            /*color = new Color(System.Drawing.Color.Black.ToArgb());
            pane.AddList("Test", test, ListStyles.HISTOHRAM, color, LineStyles.SOLID, PaneSides.LEFT);*/
        }
    }



}