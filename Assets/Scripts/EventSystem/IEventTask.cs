public interface IEventTask
{
    ITask OnNotice();
}

public interface IEventTask<T>
{
    ITask OnNotice(T arg);
}

