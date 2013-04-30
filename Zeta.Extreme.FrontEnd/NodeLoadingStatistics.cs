using System;
using Qorpent;

namespace Zeta.Extreme.FrontEnd {
    class NodeLoadingStatistics {
        INodeLoadingPoints LoadingPoints { get; set; }

        public NodeLoadingStatistics(INodeLoadingPoints loadingPoints) {
            if (loadingPoints != null) {
                LoadingPoints = loadingPoints;
            } else {
                LoadingPoints = new NodeLoadingPoints();  
            }
        }

        public double Age(int queriesDivider = 1) {
            return (ServiceState.TotalQueriesHandled / queriesDivider) * LoadingPoints.TotalQueriesHandled +
                   Convert.ToInt32(ServiceState.CpuMinutes) * LoadingPoints.TotalCpuMinute +
                   FormServersState.TotalReloadsCount * LoadingPoints.TotalServerReloads +
                   FormServersState.TotalSessionsHandled * LoadingPoints.TotalSessionsHandled;
        }

        public double Load() {
            return FormServersState.CurrentReloadOperations * LoadingPoints.CurrentReloadOperations +
                   FormSessionsState.CurrentFormLoadingOperations * LoadingPoints.CurrentFormLoadingOperations +
                   FormSessionsState.CurrentFormRenderingOperations * LoadingPoints.CurrentFormRenderingOperations +
                   FormSessionsState.CurrentLockFormOperations * LoadingPoints.CurrentLockFormOperations +
                   FormSessionsState.CurrentAttachFileOperations * LoadingPoints.CurrentAttachFileOperations +
                   FormSessionsState.CurrentDataSaveOperations * LoadingPoints.CurrentDataSaveOperations;
        }

        public double Power() {
            var estimatedCpuMinutes = ServiceState.CpuMinutes > 0 ? ServiceState.CpuMinutes : 1;
            var estimatedTimeToLoadData = FormServersState.TotalTimeToLoadData.TotalSeconds > 0 ? FormServersState.TotalTimeToLoadData.TotalSeconds : 1;
            return 1.0 / (
                    Convert.ToDouble(ServiceState.TotalQueriesHandled / estimatedCpuMinutes) +
                    Convert.ToDouble(FormServersState.TotalSessionsHandled / estimatedTimeToLoadData)
                );
        }

        public double Availability() {
            return 1 / (LoadingPoints.A * Age() + LoadingPoints.P * Power() + LoadingPoints.L * Load());
        }
    }
}
