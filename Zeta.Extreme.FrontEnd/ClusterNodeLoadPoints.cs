using System;

namespace Zeta.Extreme.FrontEnd {
    /// <summary>
    ///     Node load static points
    /// </summary>
    public static class ClusterNodeLoadPoints {
        /// <summary>
        ///     Процесс перезагрузки сервера
        /// </summary>
        public static Int64 Reloading = 5;

        /// <summary>
        ///     Процесс загрузки данных формы
        /// </summary>
        public static Int64 FormDataLoading = 10;

        /// <summary>
        ///     Процесс формирования структуры формы
        /// </summary>
        public static Int64 FromStructureRendering = 3;

        /// <summary>
        ///     Процесс блокировки формы
        /// </summary>
        public static Int64 FormBlocking = 1;
        
        /// <summary>
        ///     Процесс привязки аттача
        /// </summary>
        public static Int64 FileAttaching = 2;

        /// <summary>
        ///     Операция сохранения данных
        /// </summary>
        public static Int64 DataSave = 7;

        /// <summary>
        ///     Количество рестартов - 20 баллов
        /// </summary>
        public const Int64 POINT_PER_RELOAD = 20;

        /// <summary>
        ///     Общее кол-во обработанных сессий - 10 баллов за каждую
        /// </summary>
        public const Int64 POINT_PEP_HANDLED_SESSION = 10;
    }
}
