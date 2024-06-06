using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ChangeText : ExecuteBase
{
    public string newString;
    [FormerlySerializedAs("_text")] public TextMeshProUGUI text;

    public override void Execute()
    {
        text.text = newString;
    }
}