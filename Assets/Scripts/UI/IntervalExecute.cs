using System;
using UniRx;
using UnityEngine;

public class IntervalExecute : MonoBehaviour
{
    public float speed;
    public ExecuteBase[] executes;
    private int _cnt = 0;

    private void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(speed)).Subscribe(_ =>
        {
            if (_cnt >= executes.Length) return;
            
            executes[_cnt++].Execute();
        });
    }
}
