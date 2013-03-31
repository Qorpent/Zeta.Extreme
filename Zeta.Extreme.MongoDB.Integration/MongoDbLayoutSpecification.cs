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

        /// <summary>
        ///     The maximum count of items in a single document in statistics collections
        /// </summary>
        public const int MONGODBLOGS_STAT_PARTITION_COUNT = 5;

        /// <summary>
        ///     The dafault name of counter 
        /// </summary>
        public const string MONGODBLOGS_STAT_COUNTER_NAME = "elements";

        /// <summary>
        ///     The default increment value for elements counter
        /// </summary>
        public const int MONGODBLOGS_STAT_COUNTER_INC_VAL = 1;
    }
}