public interface IEventListener
{
    bool OnNotice();
}

/// <summary>
/// 何かしらの処理をしたらtrueを返す。
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEventListener<T>
{
    bool OnNotice(T arg);
}