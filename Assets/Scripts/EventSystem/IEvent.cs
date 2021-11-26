public interface IEvent
{
    void Register(IEventListener listener);
    bool DisRegister(IEventListener listener);
    ITask Notice();
}

public interface IEvent<T>
{
    void Register(IEventListener<T> listener);
    bool DisRegister(IEventListener<T> listener);
    ITask Notice(T arg);
}