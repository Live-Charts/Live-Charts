using System;
using System.Collections.Generic;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Dtos;

namespace LiveCharts
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.AxisCore" />
    public class IrregularAxisCore : AxisCore
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="IrregularAxisCore"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public IrregularAxisCore(IAxisView view) : base(view)
        {
            CleanFactor = 1.5;
        }

        internal override CoreMargin PrepareChart(AxisOrientation source, ChartCore chart)
        {
            //ラベルの表示云々にかかわらず、セパレータは表示するのでShowLabelsのジャッジは外す
            //if (!(Math.Abs(TopLimit - BotLimit) > S * .01) || !ShowLabels)
            if (!(Math.Abs(TopLimit - BotLimit) > S * .01))
            {
                return new CoreMargin();
            }

            //表示区間をセパレートした時の１区間の幅に関する情報の計算
            // Magnitude:１区間幅を桁数ベースでFloorしたもの（細かい目盛り幅となる）
            // S : 区間幅にMagnitudeが何個入るかを知る目安（目盛り幅となる）
            CalculateSeparator(chart, source);

            var f = GetFormatter();

            var currentMargin = new CoreMargin();
            //if (S < 1) S = 1;
            var tolerance = S / 10;

            //GarbageCollectorIndexを１カウントアップするのがメイン
            InitializeGarbageCollector();


            IEnumerable<double> ticks = GetSeparators(BotLimit, TopLimit - (EvaluatesUnitWidth ? 1 : 0), Magnitude, S);

            foreach (var i in ticks)
            {
                SeparatorElementCore asc;

                var key = Math.Round(i / tolerance) * tolerance;
                if (!Cache.TryGetValue(key, out asc))
                {
                    asc = new SeparatorElementCore { IsNew = true };
                    Cache[key] = asc;
                }
                else
                {
                    asc.IsNew = false;
                }

                View.RenderSeparator(asc, Chart);

                asc.Key = key;
                asc.Value = i;
                asc.GarbageCollectorIndex = GarbageCollectorIndex;

                var labelsMargin = asc.View.UpdateLabel(f(i), this, source);

                currentMargin.Width = labelsMargin.TakenWidth > currentMargin.Width
                    ? labelsMargin.TakenWidth
                    : currentMargin.Width;
                currentMargin.Height = labelsMargin.TakenHeight > currentMargin.Height
                    ? labelsMargin.TakenHeight
                    : currentMargin.Height;

                currentMargin.Left = labelsMargin.Left > currentMargin.Left
                    ? labelsMargin.Left
                    : currentMargin.Left;
                currentMargin.Right = labelsMargin.Right > currentMargin.Right
                    ? labelsMargin.Right
                    : currentMargin.Right;

                currentMargin.Top = labelsMargin.Top > currentMargin.Top
                    ? labelsMargin.Top
                    : currentMargin.Top;
                currentMargin.Bottom = labelsMargin.Bottom > currentMargin.Bottom
                    ? labelsMargin.Bottom
                    : currentMargin.Bottom;

                if (LastAxisMax == null)
                {
                    asc.State = SeparationState.InitialAdd;
                    continue;
                }

                asc.State = SeparationState.Keep;
            }


            return currentMargin;
        }



        private IEnumerable<double> GetSeparators(double st, double ed, double Magnitude, double S)
        {
            var separatorFunc = (View as IIrregularAxisView)?.SeparatorProvider;

            if (separatorFunc == null)
            {
                //build default separators
                List<double> list = new List<double>();
                for (var l = Math.Truncate(st / S) * S; l < ed; l += S)
                {
                    list.Add(l);
                }
                return list;
            }

            return separatorFunc(st, ed, Magnitude, S);
        }
    }
}
