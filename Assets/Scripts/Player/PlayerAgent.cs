using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Move,
    Die,
}

public class PlayerAgent : MonoBehaviour, IStateMachine<PlayerState>
{
    public PlayerState currentStateKey;
    private Dictionary<PlayerState, IState> _stateMap;
    [SerializeField] private YouDie dieUi;
    [SerializeField] private Collider capsuleCollider;
    private Rigidbody[] _rigidbodies;
    private bool isRagdoll = false;

    #region Unity

    private void Awake()
    {
        _stateMap = new();
    }

    private void Start()
    {
        var states = GetComponents<StateBehaviorBase<PlayerState>>();
        foreach (var state in states)
        {
            _stateMap.Add(state.GetStateKey(), state);
        }
        
        _stateMap[currentStateKey].StateEnter();

        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        ToggleRagdoll(false);
    }

    private void Update()
    {
        _stateMap[currentStateKey].StateUpdate();
    }
    #endregion
    
    #region FSM
    public PlayerState GetCurrentStateKey() => currentStateKey;

    public void ChangeState(PlayerState newStateKey)
    {
        _stateMap[currentStateKey].StateExit();

        currentStateKey = newStateKey;

        _stateMap[currentStateKey].StateEnter();
    }
    #endregion
    
    public void Die(string reason, Vector3? force = null)
    {
        if (GetCurrentStateKey() == PlayerState.Die) return;
        
        ChangeState(PlayerState.Die);
        
        ToggleRagdoll(true);
        if (force != null)
        {
            foreach (var rigid in _rigidbodies)
            {
                rigid.AddForce(force.Value * 10f, ForceMode.Impulse);
            }
        }
        
        dieUi.Show(reason);
    }

    public void ToggleRagdoll(bool on)
    {
        isRagdoll = on;

        capsuleCollider.enabled = !on;
        foreach (var rigid in _rigidbodies)
        {
            rigid.isKinematic = !on;
        }
    }
}
