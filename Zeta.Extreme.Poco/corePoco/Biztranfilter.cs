using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco {
	public partial class biztranfilter : IBizTranFilter {
		public virtual int Id { get; set; }
		public virtual int Action { get; set; }
		public virtual int MainId { get; set; }
		public virtual int ContrId { get; set; }
		public virtual string TranCode { get; set; }
		public virtual string Role { get; set; }
		public virtual string Raw { get; set; }
		public virtual string FirstForm { get; set; }
		public virtual string FirstType { get; set; }
		public virtual string SecondType { get; set; }
		public virtual string SecondForm { get; set; }
	}
}