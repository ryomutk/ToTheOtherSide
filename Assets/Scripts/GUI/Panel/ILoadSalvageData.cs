public interface ILoadSalvageData<T>
where T:ISalvageData
{
    ITask Load(T data);
}