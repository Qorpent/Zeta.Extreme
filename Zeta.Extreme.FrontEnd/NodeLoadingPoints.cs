namespace Zeta.Extreme.FrontEnd {
    /// <summary>
    ///     Класс, хранящий значения вес каждого из элементов рассчёта
    /// </summary>
    public class NodeLoadingPoints : INodeLoadingPoints {
        private int _totalQueriesHandled = 1;
        private int _totalCpuMinute = 1;
        private int _totalServerReloads = 20;
        private int _totalSessionsHandled = 10;

        private int _currentReloadOperations = 5;
        private int _currentFormLoadingOperations = 10;
        private int _currentFormRenderingOperations = 3;
        private int _currentLockFormOperations = 1;
        private int _currentAttachFileOperations = 2;
        private int _currentDataSaveOperations = 7;

        private int _a = 1;
        private int _p = 2;
        private int _l = 3;


        /// <summary>
        /// 
        /// </summary>
        public int TotalQueriesHandled {
            get { return _totalQueriesHandled; }
            set { _totalQueriesHandled = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int TotalCpuMinute {
            get { return _totalCpuMinute; }
            set { _totalCpuMinute = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalServerReloads {
            get { return _totalServerReloads; }
            set { _totalServerReloads = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalSessionsHandled {
            get { return _totalSessionsHandled; }
            set { _totalSessionsHandled = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int CurrentReloadOperations {
            get { return _currentReloadOperations; }
            set { _currentReloadOperations = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentFormLoadingOperations {
            get { return _currentFormLoadingOperations; }
            set { _currentFormLoadingOperations = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int CurrentFormRenderingOperations {
            get { return _currentFormRenderingOperations; }
            set { _currentFormRenderingOperations = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentLockFormOperations {
            get { return _currentLockFormOperations; }
            set { _currentLockFormOperations = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentAttachFileOperations {
            get { return _currentAttachFileOperations; }
            set { _currentAttachFileOperations = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentDataSaveOperations {
            get { return _currentDataSaveOperations; }
            set { _currentDataSaveOperations = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public int A {
            get { return _a; }
            set { _a = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int P {
            get { return _p; }
            set { _p = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int L {
            get { return _l; }
            set { _l = value; }
        }
    }
}
