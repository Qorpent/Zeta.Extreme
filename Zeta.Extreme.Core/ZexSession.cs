#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Solution: Qorpent.TextExpert
// Original file : ZexSession.cs
// Project: Zeta.Extreme.Core
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zeta.Extreme.Core {
	/// <summary>
	/// 	������� ����� ������ �������� Zeta
	/// </summary>
	/// <remarks>
	/// 	������ �������� �������� ��������
	/// 	����� ��������� �������.
	/// 	����� ������� ������ � �������:
	/// 	create session ==- register queries ==- evaluate  ==- collect result
	/// 	������ �������� � ������������ �������������� async - �����������
	/// </remarks>
	public class ZexSession {
		/// <summary>
		/// 	����������� �� ���������
		/// </summary>
		/// <remarks>
		/// 	���������� �������� ���������
		/// </remarks>
		public ZexSession() {
			MainQueryRegistry = new Dictionary<string, ZexQuery>();
			ActiveSet = new Dictionary<string, ZexQuery>();
			ProcessedSet = new Dictionary<string, ZexQuery>();
		}

		/// <summary>
		/// 	������� ������ ��������
		/// </summary>
		/// <remarks>
		/// 	��� ����������� ������� ������� ������������� ��� ���������� UID
		/// 	�����, � MainQueryRegistry �� ����� �� ������ Value ����� ������� ��������
		/// </remarks>
		public IDictionary<string, ZexQuery> MainQueryRegistry { get; private set; }

		/// <summary>
		/// 	����� ���� ����������, ��� �� ������������ �������� (������)
		/// 	���� - ������
		/// </summary>
		public IDictionary<string, ZexQuery> ActiveSet { get; private set; }

		/// <summary>
		/// 	����� ���� ����������, ���  ������������ ��������
		/// 	���� - ������
		/// </summary>
		public IDictionary<string, ZexQuery> ProcessedSet { get; private set; }

		/// <summary>
		/// 	���������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		/// <exception cref="NotImplementedException"></exception>
		public ZexQuery Register(ZexQuery query, string uid = null) {
			lock (this) {
				var helper = GetRegistryHelper();
				var result = helper.Register(query, uid);
				_currentHelper = helper;
				return result;
			}
		}

		/// <summary>
		/// 	����������� ����������� ������� � ������
		/// </summary>
		/// <param name="query"> �������� ������ </param>
		/// <param name="uid"> ��������� ���� ������� ��������� ��� ��� ����������� ���������������� ��������� �������� </param>
		/// <returns> ������, �� ����������� ������� ������������ ������ �� ������ ����������� � ������ </returns>
		/// <remarks>
		/// 	��� ����������� �������, �� �������� �������������� ����������� � �������� �� ������,
		/// 	������������ ������ �������� ������
		/// </remarks>
		public Task<ZexQuery> RegisterAsync(ZexQuery query, string uid = null) {
			lock (this) {
				var id = rcount++;
				var task = new Task<ZexQuery>(() =>
					{
						try {
							var helper = GetRegistryHelper();
							var result = helper.Register(query, uid);
							_currentHelper = helper;
							return result;
						}
						finally {
							lock (_regq) _regq.Remove(id);
						}
					});
				lock (_regq) _regq[id] = task;
				task.Start();
				return task;
			}
		}

		/// <summary>
		/// 	������� ��������� ���� ��������� ����������� �����������
		/// </summary>
		public void WaitRegistration() {
			while (_regq.Count != 0) {
				Task.WaitAll(_regq.Values.ToArray().Where(x => null != x).ToArray());
			}
		}


		/// <summary>
		/// 	���������� ��������� ������� ��� ����������� � ������ � ��������� �����
		/// </summary>
		/// <returns> </returns>
		protected IZexRegistryHelper GetRegistryHelper() {
			lock (this) {
				if (null != _currentHelper) {
					var result = _currentHelper;
					_currentHelper = null;
					return result;
				}
				if (null != CustomRegistryHelperClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IZexRegistryHelper;
				}
				return new DefaultZexRegistryHelper(this);
			}
		}

		/// <summary>
		/// 	���������� ������ �������������
		/// </summary>
		/// <returns> </returns>
		/// <exception cref="NotImplementedException"></exception>
		public IZexPreloadProcessor GetPreloadProcessor() {
			lock (_preloadprocesspool) {
				if (_preloadprocesspool.Count != 0) {
					return _preloadprocesspool.Pop();
				}
			}
			lock (this) {
				if (null != CustomPreloadProcessorClass) {
					return Activator.CreateInstance(CustomRegistryHelperClass, this) as IZexPreloadProcessor;
				}
				return new DefaultZexPreloadProcessor(this);
			}
		}

		/// <summary>
		/// 	���������� ������������ � ���
		/// </summary>
		/// <param name="processor"> </param>
		public void ReturnPreloadPreprocessor(IZexPreloadProcessor processor) {
			lock (_preloadprocesspool) {
				if (_preloadprocesspool.Count < 99) {
					_preloadprocesspool.Push(processor);
				}
			}
		}

		private readonly Stack<IZexPreloadProcessor> _preloadprocesspool = new Stack<IZexPreloadProcessor>(100);
		private readonly IDictionary<int, Task<ZexQuery>> _regq = new Dictionary<int, Task<ZexQuery>>();

		/// <summary>
		/// 	��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomPreloadProcessorClass;

		/// <summary>
		/// 	��������� �������������� ��� ������� �����������
		/// </summary>
		public Type CustomRegistryHelperClass;

		private IZexRegistryHelper _currentHelper; //one-instance cache
		private int rcount;
	}
}