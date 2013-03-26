namespace Zeta.Extreme.MongoDB.Integration {
    /// <summary>
    ///     Набор заранее определённых данных для работы MongoDB.Integration
    /// </summary>
    public static class MongoLayoutSpecification {
        /// <summary>
        ///     Строка подключения по умолчанию
        /// </summary>
        public const string DEFAULT_CONNECTION_STRING = "localhost";

        /// <summary>
        ///     Коллекция для хранения аттачей по умолчанию
        /// </summary>
        public const string DEFAULT_ATTACHMENT_COLLECTION = "AttachmentsDescription";

        /// <summary>
        ///     БД для храненения аттачей по умолчанию
        /// </summary>
        public const string DEFAULT_ATTACHMENTS_DB = "MongoDbAttachments";
    }
}