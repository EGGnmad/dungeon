using System;
using UnityEngine;
using UnityEngine.UI;

public class DoorInteractable : InteratableBase
{
    [SerializeField] private Transform door;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Slider slider;
    [SerializeField] private float time = 1f;
    private float _dt = 0f;

    private bool _isPlayed = false;
    [SerializeField] private AudioSource openSound;
    
    public override void Interact()
    {
        if (!canInteract) return;
        if (_dt >= time)
        {
            if (_isPlayed) return;
            
            _isPlayed = true;
            openSound.Play();
            return;
        }

        _dt += Time.deltaTime;
        slider.value = _dt;
        door.transform.position += moveDir * Time.deltaTime;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        if (_dt >= time) return;
        slider.gameObject.SetActive(true);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        slider.gameObject.SetActive(false);
    }
}