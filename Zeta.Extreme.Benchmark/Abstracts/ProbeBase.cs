using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// Базовая абстрактная проба
	/// </summary>
	public abstract class ProbeBase : IProbe {
		private IProbeConfig _config;
		private IList<IProbe> _children;

		/// <summary>
		/// Родительская проба
		/// </summary>
		public IProbe Parent { get; set; }

		/// <summary>
		/// Дочерние пробы
		/// </summary>
		public IList<IProbe> Children {
			get { return _children ?? (_children
			                           = new List<IProbe>()); }

		}

		/// <summary>
		///  Инициализация пробы
		/// </summary>
		/// <param name="config"></param>
		public void Initialize(IProbeConfig config = null) {
			this._config = config;
			InternalInitialize();
		}

		/// <summary>
		/// Перекрыть для полноценной конфигурации
		/// </summary>
		protected virtual void InternalInitialize(){}

		/// <summary>
		/// Асинхронное выполнение пробы
		/// </summary>
		/// <returns></returns>
		public async Task<IProbeResult> ExecuteAsync() {
			return await Task.Run(() => InternalExecute(true));
		}

		/// <summary>
		/// Синхронное выполнение пробы
		/// </summary>
		/// <returns></returns>
		public IProbeResult ExecuteSync() {
			return InternalExecute(false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="async"></param>
		/// <returns></returns>
		protected virtual IProbeResult InternalExecute(bool async) {
			var result = new ProbeResult {Probe = this};
			var sw = Stopwatch.StartNew();
			if (null == _children || 0 == _children.Count) {
				ExecuteSelf(result, async);
			}
			else {
				ExecuteSubProbes(result, async);
			}
			sw.Stop();
			result.TotalDuration = sw.Elapsed;
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected abstract void ExecuteSubProbes(IProbeResult result, bool @async);

		/// <summary>
		/// Перекрыть для выполнения собственно терминальной пробы
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected abstract void ExecuteSelf(IProbeResult result, bool async);

		/// <summary>
		/// Акцессор к конфигу
		/// </summary>
		/// <returns></returns>
		public IProbeConfig GetConfig() {
			return _config ?? (_config= new ProbeConfig());
		}
	}
}