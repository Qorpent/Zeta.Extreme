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
        public static Int64 CurrentAttachFileOperations;

        /// <summary>
        ///     Количество текущих операций проверки возможности блокирования формы
        /// </summary>
        public static Int64 LockFormCurrentOperations;

        /// <summary>
        ///     Count of current form reloading operations
        /// </summary>
        public static Int64 CurrentFormLoadingOperations;

        /// <summary>
        ///     Count of current form rendering operations
        /// </summary>
        public static Int64 CurrentFormRenderingOperations;

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
            CurrentAttachFileOperations++;
        }

        /// <summary>
        ///     Decrease count of current FileAttach operations
        /// </summary>
        public static void FileAttachOperationsDecrease() {
            CurrentAttachFileOperations--;
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

        /// <summary>
        ///     Увеличивает счётчик количества текущих операций загрузки форм
        /// </summary>
        public static void CurrentFormLoadingOperationsIncrease() {
            CurrentFormLoadingOperations++;
        }

        /// <summary>
        ///     Уменьшает счётчик количества текущих операций загрузки форм
        /// </summary>
        public static void CurrentFormLoadingOperationsDecrease() {
            CurrentFormLoadingOperations--;
        }

        /// <summary>
        ///     Увеличивает счётчик количества текущих операций формирования структуры форм
        /// </summary>
        public static void CurrentFormRenderingOperationsIncrease() {
            CurrentFormRenderingOperations++;
        }

        /// <summary>
        ///     Уменьшает счётчик количества текущих операций формирования структуры форм
        /// </summary>
        public static void CurrentFormRenderingOperationsDecrease() {
            CurrentFormRenderingOperations--;
        }
    }
}
