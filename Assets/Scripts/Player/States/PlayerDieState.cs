using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDieState : StateBehaviorBase<PlayerState>
{
    public override PlayerState GetStateKey() => PlayerState.Die;

    public override void StateEnter()
    {
        base.StateEnter();

        GetComponent<Animator>().enabled = false;
        Observable.Timer(TimeSpan.FromSeconds(4f)).Subscribe(_ => Die());
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}