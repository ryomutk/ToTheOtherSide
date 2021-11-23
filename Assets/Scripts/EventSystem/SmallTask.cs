using System;

public class SmallTask<T>
where T : class
{
    public bool ready { get { return conditionGetter(); } }
    Func<bool> conditionGetter;
    public T result { get { return resultGetter(); } }

    Func<T> resultGetter;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="conditionGetter">readyになる条件。デフォルトはresult != null</param>
    public SmallTask(Func<bool> conditionGetter, Func<T> resultGetter)
    {
        this.conditionGetter = conditionGetter;
        this.resultGetter = resultGetter;
    }
}

public class SmallTask
{
    static SmallTask _nullTask = new SmallTask();
    public event Action onReady;
    public static SmallTask nullTask
    {
        get
        {
            if (!_nullTask.ready)
            {
                _nullTask.ready = true;
            }
            return _nullTask;
        }
    }
    bool _ready;
    public bool ready
    {
        get { return _ready; }
        set
        {
            if (value&&onReady!=null)
            {
                onReady();
                onReady = null;
            }
            _ready = value;
        }
    }
    public SmallTask()
    {
        ready = false;
    }
}