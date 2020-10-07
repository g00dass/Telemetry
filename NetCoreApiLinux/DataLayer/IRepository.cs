namespace DataLayer
{
    public interface IRepository<T>
    {
        T[] GetAll();
        T Find(string id);
        void AddOrUpdate(T dbo);
    }
}
