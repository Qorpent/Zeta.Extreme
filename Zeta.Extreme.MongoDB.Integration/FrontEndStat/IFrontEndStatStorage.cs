namespace Zeta.Extreme.MongoDB.Integration.FrontEndStat {
    /// <summary>
    ///     
    /// </summary>
    public interface IFrontEndStatStorage : IMongoDbConnector {
        /// <summary>
        ///     Write statistics to MongoDB as a JSON string
        /// </summary>
        /// <param name="jsonStat">a json-string</param>
        void Write(string jsonStat);
    }
}
