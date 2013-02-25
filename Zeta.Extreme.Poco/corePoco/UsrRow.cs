using Comdiv.Zeta.Model;namespace Zeta.Extreme.Poco {
    public class UsrRow : IUsrRow{
        #region IUsrRow Members

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Active { get; set; }
        public virtual IZetaUnderwriter Usr { get; set; }
        public virtual IZetaRow Row { get; set; }
        public virtual string Code { get; set; }

        #endregion
    }
}