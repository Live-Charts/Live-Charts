using System;
using LiveCharts.Definitions.Points;
using LiveCharts.Dtos;

namespace LiveCharts.Definitions.Series
{

    /// <summary>
    /// If you would draw the chart points all at one time,
    /// The series algorithm makes view draw through this interface
    /// 
    /// ChartPointをまとめて高速描画する場合、
    /// シリーズアルゴリズムは、本インタフェースを介して、再描画を指示する
    /// </summary>
    public interface ISeriesAccelView
    {
        /// <summary>
        /// Draw the series itself
        /// 
        /// シリーズ自体の描画を行う
        /// </summary>
        //void DrawOrMove();


        /// <summary>
        /// Find a chart point on view
        /// 
        /// ChartPoint毎にFrameworkElementを持たないため、ここでヒットテストを実装する
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        ChartPoint HitTest(CorePoint pt);

        /// <summary>
        /// Called when [hover].
        /// </summary>
        /// <param name="point">The point.</param>
        void OnHover(ChartPoint point);

        /// <summary>
        /// Called when [hover leave].
        /// </summary>
        void OnHoverLeave();

    }
}
