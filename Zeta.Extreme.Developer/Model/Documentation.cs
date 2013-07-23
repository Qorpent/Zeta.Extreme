using System;
using Qorpent.Serialization;

namespace Zeta.Extreme.Developer.Model {
	/// <summary>
	/// Документация на элементы синтаксиса
	/// </summary>
	[Serialize]
	public class Documentation
	{
		/// <summary>
		/// 
		/// </summary>
		public Documentation() {
			Key = "";
			Name = "";
			Error = "";
			Obsolete = "";
			Comment = "";
			SubComment = "";
			Question = "";
		}

		/// <summary>
		/// Присоединение дополнительного документа в режиме слияния
		/// </summary>
		/// <param name="otherdocument"></param>
		/// <returns></returns>
		public Documentation Merge(Documentation otherdocument) {
			if (null == otherdocument) return this;
			if (this == otherdocument) return this;

			Func<string, string, string> setitem = (my, other) => {
				var result = my;
				if (!string.IsNullOrWhiteSpace(other)) {
					if (!string.IsNullOrWhiteSpace(my)) {
						result += "; ";
					}
					result += other;
				}
				return result;
			};

			Key = setitem(Key, otherdocument.Key);
			Name = setitem(Name, otherdocument.Name);
			Error = setitem(Error, otherdocument.Error);
			Obsolete = setitem(Obsolete, otherdocument.Obsolete);
			Question = setitem(Question, otherdocument.Question);
			Comment = setitem(Comment, otherdocument.Comment);
			SubComment = setitem(SubComment, otherdocument.SubComment);

			IsBiztran = IsBiztran || otherdocument.IsBiztran;
			IsSystem = IsSystem || otherdocument.IsSystem;
			

			return this;
		}
		

		/// <summary>
		/// Ключ документа
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Краткое описание, имя
		/// </summary>
		[SerializeNotNullOnly]
		public string Name { get; set; }

		/// <summary>
		/// Полное описание
		/// </summary>
		[SerializeNotNullOnly]
		public string Comment { get; set; }

		/// <summary>
		/// Признак устаревшего параметра
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsObsolete {
			get { return !string.IsNullOrWhiteSpace(Obsolete); }
		}
		/// <summary>
		/// Информация об устаревании элемента синтаксиса
		/// </summary>
		[SerializeNotNullOnly]
		public string Obsolete { get; set; }


		/// <summary>
		/// Признак устаревшего параметра
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsError
		{
			get { return !string.IsNullOrWhiteSpace(Error); }
		}
		/// <summary>
		/// Информация об устаревании элемента синтаксиса
		/// </summary>
		[SerializeNotNullOnly]
		public string Error { get; set; }


		/// <summary>
		/// Признак устаревшего параметра
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsQuestion
		{
			get { return !string.IsNullOrWhiteSpace(Question); }
		}
		/// <summary>
		/// Информация об устаревании элемента синтаксиса
		/// </summary>
		[SerializeNotNullOnly]
		public string Question { get; set; }

		/// <summary>
		/// Признак устаревшего параметра
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsSubComment
		{
			get { return !string.IsNullOrWhiteSpace(SubComment); }
		}
		/// <summary>
		/// Информация об устаревании элемента синтаксиса
		/// </summary>
		[SerializeNotNullOnly]
		public string SubComment { get; set; }


		/// <summary>
		/// Признак того, что единица кода относится к бизтрану
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsBiztran { get; set; }

		/// <summary>
		/// Признак системного параметра
		/// </summary>
		[SerializeNotNullOnly]
		public bool IsSystem { get; set; }
	}
}