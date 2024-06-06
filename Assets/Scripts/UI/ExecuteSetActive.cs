using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteSetActive : ExecuteBase
{
    [SerializeField] private GameObject obj;
    [SerializeField] private bool value;


    public override void Execute()
    {
        obj.SetActive(value);
    }
}
