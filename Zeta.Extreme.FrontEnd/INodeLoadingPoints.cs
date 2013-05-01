namespace Zeta.Extreme.FrontEnd {
    /// <summary>
    ///     Интерфейс, описывающий хранилище оценок для элементов рассчёта загрузки сервера
    ///
    /// </summary>
    public interface INodeLoadingPoints {
        /// <summary>
        /// 
        /// </summary>
        int TotalQueriesHandled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int TotalCpuMinute { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int TotalServerReloads { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int TotalSessionsHandled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int CurrentReloadOperations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int CurrentFormLoadingOperations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int CurrentFormRenderingOperations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int CurrentLockFormOperations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int CurrentAttachFileOperations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int CurrentDataSaveOperations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int A { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int P { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int L { get; set; }
    }
}