using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter
{
    public event Action OnCounterLimitOverflow;
    public event Action OnCounterLimitRelease;

    private readonly int CountLimit;
    private int _currentCount = 0;

    private bool _isLimitOverflowed = false;



    public Counter(int limit)
    {
        CountLimit = limit;
    }



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
}
