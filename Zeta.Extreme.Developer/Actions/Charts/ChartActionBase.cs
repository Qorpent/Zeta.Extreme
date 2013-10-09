using Qorpent.Mvc;
using Qorpent.Mvc.Binding;

namespace Zeta.Extreme.Developer.Actions.Charts {
    /// <summary>
    /// 
    /// </summary>
    public abstract class ChartActionBase : ActionBase {
        /// <summary>
        ///     Год
        /// </summary>
        [Bind(Required = true)]
        public int Year { get; set; }
        /// <summary>
        ///     Период
        /// </summary>
        [Bind(Required = true)]
        public int Period { get; set; }
        /// <summary>
        ///     Объект
        /// </summary>
        [Bind(Required = true)]
        public int Object { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object MainProcess() {
            return GenerateChart(Year, Period, Object);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        /// <param name="period"></param>
        /// <param name="obj"></param>
        protected abstract object GenerateChart(int year, int period, int obj);
    }
}
