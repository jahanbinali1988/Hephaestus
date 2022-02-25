namespace Hephaestus.Repository.Abstraction.Contract
{
    public interface IObjectPool<T>
    {
        T Acquire();
        void Release(T obj);
    }
}
