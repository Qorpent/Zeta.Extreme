using System;
using Qorpent.Serialization;
using Zeta.Extreme.Form;
using Zeta.Extreme.Form.InputTemplates;

namespace Zeta.Extreme.BizProcess.Forms
{
	/// <summary>
	/// Описатель аттачмента к форме, является и описателем и запросом одновременно
	/// </summary>
	[Serialize]
	public class FormAttachment : Attachment {
		/// <summary>
		/// Год файла 
		/// </summary>
		public int Year {
			get {
				object result;
				return Metadata.TryGetValue("year",out result) ? Convert.ToInt32(result) : 0;
			}
			set { Metadata["year"] = value; }
		}

		/// <summary>
		/// Период файла 
		/// </summary>
		public int Period {
			get {
				object result;
				return Metadata.TryGetValue("period", out result) ? Convert.ToInt32(result) : 0; 
			}
			set { Metadata["period"] = value; }
		}

		/// <summary>
		/// Предприятие файла
		/// </summary>
		public int ObjId {
			get {
				object result;
				return Metadata.TryGetValue("obj", out result) ? Convert.ToInt32(result) : 0; 
			}
			set { Metadata["obj"] = value; }
		}

		/// <summary>
		/// Код шаблона файла
		/// </summary>
		public string TemplateCode {
			get {
				object result;
				return Metadata.TryGetValue("template", out result) ? Convert.ToString(result) : "";
			}
			set { Metadata["template"] = value; }
		}
		/// <summary>
		/// Роль присоединенного файла
		/// </summary>
		public AttachedFileType AttachType { get; set; }

		/// <summary>
		/// Создает пустой объект присоединенного контента
		/// </summary>
		public FormAttachment() {
			Year = 0;
			Period = 0;
			ObjId = 0;
			TemplateCode = "";
		}

		/// <summary>
		/// Создает присоединеный объект формы из существующего объекта в заданном контексте
		/// </summary>
		/// <param name="session">контекст сохраненного файла </param>
		/// <param name="source">исходный объект присоединенногоф файла</param>
		/// <param name="attachType">тип присоединения</param>
		/// <param name="preserveSourceBizData">true - настройки формы берутся из source, false - из session</param>
		public FormAttachment(IFormSession session, Attachment source, AttachedFileType attachType, bool preserveSourceBizData = true) {
			AttachType = attachType;
			if(null!=source) {
				Uid = source.Uid;
				Hash = source.Hash;
				Type = source.Type;
				MimeType = source.MimeType;
				Name = source.Name;
				Revision = source.Revision;
				Size = source.Size;
				Comment = source.Comment;
				foreach (var metadata in source.Metadata) {
					Metadata[metadata.Key] = metadata.Value;
				}
			}
			if(null!=session) {
				if(session.Year!=Year) {
					if(!preserveSourceBizData || 0==Year) Year = session.Year;
				}
				if(session.Period!=Period) {
					if (!preserveSourceBizData || 0 == Period) Period = session.Period;
				}
				if(session.Object.Id!=ObjId) {
					if (!preserveSourceBizData || 0 == ObjId) ObjId = session.Object.Id;
				}
				if(session.Template.Code!=TemplateCode) {
					if(!preserveSourceBizData || ""==TemplateCode) TemplateCode = session.Template.Code;
				}
			}




		}

		

	}
}
