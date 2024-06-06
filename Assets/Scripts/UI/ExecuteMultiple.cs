using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMultiple : ExecuteBase
{
    [SerializeField] private ExecuteBase[] executes;
    
    public override void Execute()
    {
        foreach (var execute in executes)
        {
            execute.Execute();
        }
    }
}
