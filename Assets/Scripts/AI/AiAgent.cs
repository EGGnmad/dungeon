using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum AiState
{
    Idle,
    Patrol,
    Chase,
    Check,
}

public class AiAgent : MonoBehaviour, IStateMachine<AiState>
{
    private Dictionary<AiState, IState> _stateMap;
    public AiState currentStateKey;

    #region Unity
    private void Awake()
    {
        _stateMap = new();
    }

    private void Start()
    {
        // Init State Map
        var states = GetComponents<StateBehaviorBase<AiState>>();
        foreach (var state in states)
        {
            _stateMap.Add(state.GetStateKey(), state);
        }
        // Init AiSensors
        var sensors = GetComponents<IAiSensor>();
        foreach (var sensor in sensors)
        {
            if (sensor is AiHearingSensor)
            {
                sensor.onSensor += obj =>
                {
                    Check(obj.transform.position);
                };
                continue;
            }
            sensor.onSensor += Chase;
        }

        _stateMap[currentStateKey].StateEnter();
    }

    private void Update()
    {
        _stateMap[currentStateKey].StateUpdate();
    }
    #endregion

    #region FSM

    public AiState GetCurrentStateKey() => currentStateKey;

    public void ChangeState(AiState newStateKey)
    {
        if (GetCurrentStateKey() == newStateKey) return;
        
        _stateMap[currentStateKey].StateExit();

        currentStateKey = newStateKey;
        
        _stateMap[currentStateKey].StateEnter();
    }
    #endregion
    
    private void Chase(GameObject obj)
    {
        if (GetCurrentStateKey() != AiState.Patrol && GetCurrentStateKey() != AiState.Idle && GetCurrentStateKey() != AiState.Check) return;
        
        GetComponent<AiChaseState>().target = obj;
        ChangeState(AiState.Chase);
    }

    private void Check(Vector3 checkPos)
    {
        if (GetCurrentStateKey() != AiState.Patrol && GetCurrentStateKey() != AiState.Idle) return;
        
        GetComponent<AiCheckState>().checkPos = checkPos;
        ChangeState(AiState.Check);
    }
}