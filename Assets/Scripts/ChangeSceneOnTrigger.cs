using System;

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class ChangeSceneOnTrigger : MonoBehaviour
{
    [SerializeField] private string toScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene(toScene);
        }
    }
}
