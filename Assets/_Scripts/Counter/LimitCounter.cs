using System;



/// <summary>
/// Increases counter for determined limit
/// </summary>
public class LimitCounter
{
    public event Action OnCounterLimitOverflow;
    public event Action OnCounterLimitRelease;

    private readonly int CountLimit;
    private int _currentCount = 0;

    private bool _isLimitOverflowed = false;



    public LimitCounter(int limit)
    {
        CountLimit = limit;
    }



    public bool IsOverflowed => _isLimitOverflowed;



    public void Increase()
    {
        _currentCount++;

        if (_currentCount >= CountLimit)
        {
            _isLimitOverflowed = true;
            OnCounterLimitOverflow?.Invoke();
        }
    }

    public void Decrease()
    {
        _currentCount--;

        if (_isLimitOverflowed && _currentCount < CountLimit)
        {
            _isLimitOverflowed = false;
            OnCounterLimitRelease?.Invoke();
        }
    }

    public void ResetCounter() => _currentCount = 0;
}
