namespace Zeta.Extreme.MongoDB.Integration
{
    /// <summary>
    ///     Набор заранее определённых данных для работы MongoDB.Integration
    /// </summary>
    public static class MongoDbLayoutSpecification
    {
        /// <summary>
        ///     Строка подключения по умолчанию
        /// </summary>
        public const string DEFAULT_CONNECTION_STRING = "mongodb://localhost";

        /// <summary>
        ///     Коллекция для хранения аттачей по умолчанию
        /// </summary>
        public const string DEFAULT_ATTACHMENT_COLLECTION = "AttachmentsDescription";

        /// <summary>
        ///     БД для храненения аттачей по умолчанию
        /// </summary>
        public const string DEFAULT_ATTACHMENTS_DB = "MongoDbAttachments";

        /// <summary>
        ///     The default logs database name
        /// </summary>
        public const string DEFAULT_LOGS_DB = "logs";

        /// <summary>
        ///     The default logs collection name
        /// </summary>
        public const string DEFAULT_LOGS_COLLECTION = "unknownLogs";
    }
}