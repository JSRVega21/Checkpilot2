using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckPilot.Models
{
    public static class RecordLoggerExtensions
    {
        public static void Initialize(this IRecordLogger entity)
        {
            entity.RecordLog.RecordKey = Guid.NewGuid().ToString();
            entity.RecordLog.ObjectKey = entity.GetType().Name;
            entity.RecordLog.IsSystem = false;
            entity.RecordLog.IsActive = true;
            entity.RecordLog.SyncStatus = SyncStatus.New;
            entity.RecordLog.SyncDate = DateTime.Now;
            entity.RecordLog.CreatedDate = DateTime.Now;
            entity.RecordLog.CreatedBy = UserContext.UserNameContext;
            entity.RecordLog.UpdatedDate = entity.RecordLog.CreatedDate;
            entity.RecordLog.UpdatedBy = UserContext.UserNameContext;
        }

        public static void Updated(this IRecordLogger entity)
        {
            entity.RecordLog.UpdatedDate = DateTime.Now;
            entity.RecordLog.UpdatedBy = UserContext.UserNameContext;
            entity.RecordLog.SyncStatus = SyncStatus.Updated;
        }

        public static void Deleted(this IRecordLogger entity)
        {
            entity.RecordLog.SyncStatus = SyncStatus.Deleted;
        }

        public static void Synced(this IRecordLogger entity)
        {
            entity.RecordLog.SyncDate = DateTime.Now;
            entity.RecordLog.SyncStatus = SyncStatus.Synced;
        }

    }
}
