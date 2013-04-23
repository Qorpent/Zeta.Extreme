using System;

namespace Zeta.Extreme.FrontEnd {
    /// <summary>
    /// 
    /// </summary>
    public static class FormSessionsState {
        /// <summary>
        ///     Count of total handled sessions
        /// </summary>
        public static Int64 HandledSessions;

        /// <summary>
        ///     Count of total current sessions
        /// </summary>
        public static Int64 CurrentSessions;

        /// <summary>
        ///     DataSave method current invokings
        /// </summary>
        public static Int64 DataSaveOperations;

        /// <summary>
        ///     AttachFile method current invokings
        /// </summary>
        public static Int64 AttachFileOperations;

        /// <summary>
        ///     Количество текущих операций проверки возможности блокирования формы
        /// </summary>
        public static Int64 LockFormCurrentOperations;

        /// <summary>
        ///     Increase count of invokings of the DataSave method
        /// </summary>
        public static void DataSaveOperationsIncrease() {
            DataSaveOperations++;
        }

        /// <summary>
        ///     Decrease count of invokings of the DataSave method
        /// </summary>
        public static void DataSaveOperationsDecrease() {
            DataSaveOperations--;
        }

        /// <summary>
        ///     Increase count of current FileAttach operations
        /// </summary>
        public static void FileAttachOperationsIncrease() {
            AttachFileOperations++;
        }

        /// <summary>
        ///     Decrease count of current FileAttach operations
        /// </summary>
        public static void FileAttachOperationsDecrease() {
            AttachFileOperations--;
        }

        /// <summary>
        ///     Increase count of current sessions
        /// </summary>
        public static void CurrentSessionsIncrease() {
            HandledSessions++;
            CurrentSessions++;
        }

        /// <summary>
        ///     Decrease count of current sessions
        /// </summary>
        public static void CurrentSessionsDecrease() {
            CurrentSessions--;
        }

        /// <summary>
        ///     Increase count of current LockForm operations
        /// </summary>
        public static void LockFormOperationsIncrease() {
            LockFormCurrentOperations++;
        }

        /// <summary>
        ///     Decrease count of current LockForm operations
        /// </summary>
        public static void LockFormOperationsDecrease() {
            LockFormCurrentOperations--;
        }
    }
}
