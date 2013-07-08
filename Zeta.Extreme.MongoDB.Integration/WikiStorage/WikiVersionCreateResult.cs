namespace Zeta.Extreme.MongoDB.Integration.WikiStorage {
    /// <summary>
    /// 
    /// </summary>
    public class WikiVersionCreateResult {
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string VersionCode { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ResultComment { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess">It was a succesful create operation?</param>
        /// <param name="versionCode">Version code</param>
        /// <param name="resultComment">Comment for create operation</param>
        public WikiVersionCreateResult(bool isSuccess, string versionCode, string resultComment) {
            IsSuccess = isSuccess;
            VersionCode = versionCode;
            ResultComment = resultComment;
        }
    }
}
