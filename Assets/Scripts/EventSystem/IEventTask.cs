public interface IEventTask
{
    SmallTask OnNotice();
}

public interface IEventTask<T>
{
    SmallTask OnNotice(T arg);
}

