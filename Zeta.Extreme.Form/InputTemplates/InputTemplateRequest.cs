#region LICENSE

// Copyright 2012-2013 Media Technology LTD 
// Original file : InputTemplateRequest.cs
// Project: Zeta.Extreme.Form
// This code cannot be used without agreement from 
// Media Technology LTD 

#endregion

using System;
using System.Collections.Generic;
using Comdiv.Application;
using Comdiv.Extensions;
using Comdiv.Persistence;
using Comdiv.Security;
using Zeta.Extreme.BizProcess.Themas;
using Zeta.Extreme.Poco.Inerfaces;

namespace Zeta.Extreme.Form.InputTemplates {
	/// <summary>
	/// 	������ �� �����
	/// </summary>
	public class InputTemplateRequest {
		/// <summary>
		/// </summary>
		public InputTemplateRequest() {
			//storage = myapp.storage.Get<IZetaCell>();
		}

		/// <summary>
		/// 	���������, �� �������� �� ����� read-only
		/// </summary>
		public bool IsReadOnly {
			get {
				if (myapp.roles.IsInRole("NOBLOCK", false)) {
					return false;
				}
				return Template.GetState(Object, null) != "0ISOPEN" || !Template.IsPeriodOpen();
			}
		}

		/// <summary>
		/// 	��� �����
		/// </summary>
		public string TemplateCode {
			get { return templateCode; }
			set {
				if (templateCode != value) {
					templateCode = value;
					Template = null;
				}
			}
		}
/*
		/// <summary>
		/// 	ID ������
		/// </summary>
		public object DetailId {
			get { return detailId; }
			set {
				if (detailId != value) {
					detailId = value;
					Detail = null;
				}
			}
		}
*/
		/// <summary>
		/// 	ID �������
		/// </summary>
		public object ObjectId {
			get { return objectId; }
			set {
				if (objectId != value) {
					objectId = value;
					Object = null;
				}
			}
		}

		/// <summary>
		/// 	���
		/// </summary>
		public int Year { get; set; }

		/// <summary>
		/// 	������
		/// </summary>
		public int Period { get; set; }

		/// <summary>
		/// 	������ ����
		/// </summary>
		public DateTime Date { get; set; }
		/*
		/// <summary>
		/// 	������
		/// </summary>
		public IZetaDetailObject Detail {
			get {
				if (null == detail && null != detailId) {
					detail = storage.Load<IZetaDetailObject>(DetailId);
				}
				return detail;
			}
			set { detail = value; }
		}
		 * */

		/// <summary>
		/// 	������
		/// </summary>
		public IZetaMainObject Object {
			get {
				if (null == @object && null != ObjectId) {
					@object = Template.FixedObject ?? MetaCache.Default.Get<IZetaMainObject>(ObjectId); //storage.Load<IZetaMainObject>(ObjectId);
					@objectId = @object.Id;
				}
				return @object;
			}
			set { @object = value; }
		}

		/// <summary>
		/// 	������
		/// </summary>
		public IInputTemplate Template {
			get {
				if (null == template && TemplateCode.hasContent()) {
					ReloadTemplate();
				}
				return template;
			}
			set { template = value; }
		}

		/// <summary>
		/// 	���������
		/// </summary>
		public IDictionary<string, string> Parameters {
			get { return parameters; }
		}

		/// <summary>
		/// 	��������� ���������� ������
		/// </summary>
		public LongTask Task { get; set; }



		/// <summary>
		/// 	�������� ���������� �����
		/// </summary>
		/// <param name="full"> </param>
		/// <exception cref="Exception"></exception>
		public void CheckFormLive(bool full = false) {
			if (Task.HaveToTerminate || (full && (null != Task.Context && !Task.Context.Response.IsClientConnected))) {
				Task.Terminate();
				throw new Exception("���������� ����� ���� �������� (noerrc)");
			}
		}

		/// <summary>
		/// 	������� �����
		/// </summary>
		/// <returns> </returns>
		public InputTemplateRequest Copy() {
			var result = (InputTemplateRequest) MemberwiseClone();
			return result;
		}

		/// <summary>
		/// 	���������� ������, ������� ������������ ������� ������.
		/// </summary>
		/// <returns> ������, �������������� ������� ������. </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString() {
			return string.Format("FORM REQUEST T:{0},O:{1},D:{2}, Y:{3},P:{4},D:{5}", TemplateCode, ObjectId, 0,
			                     Year, Period, Date.ToString("ddMMyyyyHHmmss"));
		}

		/// <summary>
		/// 	����������� ������
		/// </summary>
		public void ReloadTemplate() {
			ReloadTemplate(true);
		}

		// static object sync = new object();
		/// <summary>
		/// </summary>
		/// <param name="keepParameters"> </param>
		/// <exception cref="NotSupportedException"></exception>
		public void ReloadTemplate(bool keepParameters) {
			throw new NotSupportedException("not ported in extreme");
			//lock (typeof (InputTemplateRequest)){
			//	var oldParameters = template != null ? template.Parameters : null;
			//	if(@object == null && 0!= @objectId.toInt()) {
			//		@object = myapp.storage.Get<IZetaMainObject>().Load(@objectId);
			//	}
			//	template = new InputTemplateRepository().GetTemplate(TemplateCode).PrepareForPeriod(Year, Period, Date,
			//																						@object);
			//		//myapp.storage.Get<IInputTemplate>().Load(TemplateCode).PrepareForPeriod(Year, Period, Date);
			//	foreach (var row in template.Rows){
			//		row.Target = RowCache.get(row.Code);
			//	}

			//	foreach (var parameter in Parameters){
			//		template.Parameters[parameter.Key] = parameter.Value;
			//	}

			//	if (keepParameters && null != oldParameters){
			//		foreach (var parameter in oldParameters){
			//			template.Parameters[parameter.Key] = parameter.Value;
			//		}
			//	}
			//}
		}


		/// <summary>
		/// 	���������� ������
		/// </summary>
		/// <param name="object"> </param>
		public void SetObject(IZetaMainObject @object) {
			ObjectId = @object.Code;
			Object = @object;
		}

		/// <summary>
		/// 	���������� ������
		/// </summary>
		/// <param name="template"> </param>
		public void SetTemplate(IInputTemplate template) {
			TemplateCode = template.Code;
			Template = template;
		}

		private readonly IDictionary<string, string> parameters = new Dictionary<string, string>();
		//private readonly StorageWrapper<IZetaCell> storage;
		/*
		private IZetaDetailObject detail;
		private object detailId;
		 */
		private IZetaMainObject @object;
		private object objectId;
		private IInputTemplate template;
		private string templateCode;
	}
}