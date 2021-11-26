public interface IEvent
{
    void Register(IEventListener listener);
    bool DisRegister(IEventListener listener);
    SmallTask Notice();
}

public interface IEvent<T>
{
    void Register(IEventListener<T> listener);
    bool DisRegister(IEventListener<T> listener);
    SmallTask Notice(T arg);
}