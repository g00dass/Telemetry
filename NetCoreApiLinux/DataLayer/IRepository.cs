using DataLayer.Dbo.AppInfo;

namespace DataLayer
{
    public interface IRepository<T, TId> where TId : IMongoBsonId
    {
        T[] GetAll();
        T Find(TId appId);
        void AddOrUpdateAppInfo(T dbo);
    }
}
