﻿using System;
using Qorpent;

namespace Zeta.Extreme.FrontEnd {
    class NodeLoadingStatistics {
        internal class Points {
            public const int TotalQueriesHandled = 1;
            public const int TotalCpuMinute = 1;
            public const int TotalServerReloads = 20;
            public const int TotalSessionsHandled = 10;

            public const int CurrentReloadOperations = 5;
            public const int CurrentFormLoadingOperations = 10;
            public const int CurrentFormRenderingOperations = 3;
            public const int CurrentLockFormOperations = 1;
            public const int CurrentAttachFileOperations = 2;
            public const int CurrentDataSaveOperations = 7;

            public const int A = 1;
            public const int P = 2;
            public const int L = 3;
        }

        public static int Age() {
            return ServiceState.TotalQueriesHandled * Points.TotalQueriesHandled +
                   Convert.ToInt32(ServiceState.CpuTime.TotalMinutes) * Points.TotalCpuMinute +
                   FormServersState.TotalReloadsCount * Points.TotalServerReloads +
                   FormServersState.TotalSessionsHandled * Points.TotalSessionsHandled;
        }

        public static int Load() {
            return FormServersState.CurrentReloadOperations * Points.CurrentReloadOperations +
                   FormSessionsState.CurrentFormLoadingOperations * Points.CurrentFormLoadingOperations +
                   FormSessionsState.CurrentFormRenderingOperations * Points.CurrentFormRenderingOperations +
                   FormSessionsState.CurrentLockFormOperations * Points.CurrentLockFormOperations +
                   FormSessionsState.CurrentAttachFileOperations * Points.CurrentAttachFileOperations +
                   FormSessionsState.CurrentDataSaveOperations * Points.CurrentDataSaveOperations;
        }
    
        public static int Power() {
            return (ServiceState.TotalQueriesHandled / (Convert.ToInt32(ServiceState.CpuTime.TotalMinutes) != 0 ? Convert.ToInt32(ServiceState.CpuTime.TotalMinutes) : 1)) +
                   (FormServersState.TotalSessionsHandled / Convert.ToInt32(FormServersState.TotalTimeToLoadData));
        }

        public static int Availability() {
            return Points.A * Age() + Points.P * Power() + Points.L * Load();
        }
    }
}
