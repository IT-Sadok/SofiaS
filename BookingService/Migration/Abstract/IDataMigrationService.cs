using BookingService.Domain.Entities;

namespace Migration.Abstract
{
    internal interface IDataMigrationService
    {
        Task<Result> MigrateData(string fileName);
    }
}