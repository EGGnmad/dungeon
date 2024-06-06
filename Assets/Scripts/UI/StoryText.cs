using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StoryText : MonoBehaviour
{
    public float speed = 1f;
    public string[] strings;
    public UnityEvent onEnd;
    
    private TextMeshProUGUI text;
    private int _cnt = 0;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        Observable.Interval(TimeSpan.FromSeconds(speed)).Subscribe(_ =>
        {
            if (_cnt >= strings.Length) return;
            Show(strings[_cnt++]);
        });
    }

    public void Show(string story)
    {
        text.text = story;

        if (_cnt == strings.Length - 1)
        {
            onEnd.Invoke();
        }
    }
}
