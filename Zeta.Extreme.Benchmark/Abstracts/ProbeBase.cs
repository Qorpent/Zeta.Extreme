using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Zeta.Extreme.Benchmark {
	/// <summary>
	/// ������� ����������� �����
	/// </summary>
	public abstract class ProbeBase : IProbe {
		/// <summary>
		/// ���������� ������������� �����
		/// </summary>
		public string Uid { get; set; }

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			return GetType().Name + ":" + Uid;
		}

		private IProbeConfig _config;
		private IList<IProbe> _children;

		/// <summary>
		/// ������������ �����
		/// </summary>
		public IProbe Parent { get; set; }

		/// <summary>
		/// �������� �����
		/// </summary>
		public IList<IProbe> Children {
			get { return _children ?? (_children
			                           = new List<IProbe>()); }

		}

		/// <summary>
		///  ������������� �����
		/// </summary>
		/// <param name="config"></param>
		public void Initialize(IProbeConfig config = null) {
			this._config = config;
			InternalInitialize();
		}

		/// <summary>
		/// ��������� ��� ����������� ������������
		/// </summary>
		protected virtual void InternalInitialize(){}

		/// <summary>
		/// ����������� ���������� �����
		/// </summary>
		/// <returns></returns>
		public async Task<IProbeResult> ExecuteAsync() {
			return await Task.Run(() => InternalExecute(true));
		}

		/// <summary>
		/// ���������� ���������� �����
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
			string ignoreMessage = "";
			if (CheckIgnore(result, out ignoreMessage)) {
				result.ResultType = ProbeResultType.Ignored;
				result.Message = ignoreMessage;
				return result;
			}
			
			try {
				var sw = Stopwatch.StartNew();
				if (null == _children || 0 == _children.Count) {
					ExecuteSelf(result, async);
				}
				else {
					ExecuteSubProbes(result, async);
				}
				sw.Stop();
				result.TotalDuration = sw.Elapsed;
				if (ProbeResultType.Undefined == result.ResultType) {
					result.ResultType = ProbeResultType.Success;
				}
				
			}
			catch (Exception ex) {
				result.Error = ex;
				result.ResultType= ProbeResultType.Error;

			}
			return result;
		}
		/// <summary>
		/// ��������� ��������� ������ �����
		/// </summary>
		/// <param name="result"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		protected virtual bool CheckIgnore(ProbeResult result, out string message) {
			message = "";
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected virtual void ExecuteSubProbes(IProbeResult result, bool @async) {
			
		}

		/// <summary>
		/// ��������� ��� ���������� ���������� ������������ �����
		/// </summary>
		/// <param name="result"></param>
		/// <param name="async"></param>
		protected virtual void ExecuteSelf(IProbeResult result, bool async) {
			
		}

		/// <summary>
		/// �������� � �������
		/// </summary>
		/// <returns></returns>
		public IProbeConfig GetConfig() {
			return _config ?? (_config= new ProbeConfig());
		}
	}
}