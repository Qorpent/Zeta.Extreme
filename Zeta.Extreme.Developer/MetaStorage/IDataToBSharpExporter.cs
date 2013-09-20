namespace Zeta.Extreme.Developer.MetaStorage {
    /// <summary>
    /// Интерфейс класса экспорта данных из БД в BSharp
    /// </summary>
    public interface IDataToBSharpExporter {
        /// <summary>
        /// Выполняет экспорт объекта данных в B#
        /// </summary>
        /// <returns></returns>
        string Generate();

        /// <summary>
        /// Имя класса
        /// </summary>
        string ClassName { get; set; }

        /// <summary>
        /// Пространство имен
        /// </summary>
        string Namespace { get; set; }
    }
}