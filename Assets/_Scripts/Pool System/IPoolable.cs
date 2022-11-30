


/// <summary>
/// Gives for heir ability to be pooled
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// Tag, which will give a object unique key in pool
    /// </summary>
    public string PoolTag { get; }


    /// <summary>
    /// Object preparations before it be pooled
    /// </summary>
    void PullInPreparations();

    /// <summary>
    /// Object preparations, when it gets unpooled
    /// </summary>
    void PullOutPreparation();
}
