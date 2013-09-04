namespace Zeta.Extreme.Developer.MetaStorage.Tree.DependencyAnalyzer {
	/// <summary>
	/// Ребро графа зависимостией
	/// </summary>
	public class DependencyEdge {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		protected bool Equals(DependencyEdge other) {
			return string.Equals(From, other.From) && string.Equals(To, other.To) && Type == other.Type;
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode() {
			unchecked {
				int hashCode = (From != null ? From.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ (To != null ? To.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ (int) Type;
				return hashCode;
			}
		}

		/// <summary>
		/// Исходящий код узла
		/// </summary>
		public string From { get; set; }
		/// <summary>
		/// Целевой код узла
		/// </summary>
		public string To { get; set; }
		/// <summary>
		/// Тип связи
		/// </summary>
		public DependencyEdgeType Type { get; set; }
		/// <summary>
		/// Надпись
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified object  is equal to the current object; otherwise, false.
		/// </returns>
		/// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((DependencyEdge) obj);
		}
	}
}