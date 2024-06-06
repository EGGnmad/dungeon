using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AxeTrap : MonoBehaviour
{
    private Vector3 origin;
    
    [Header("config")]
    [SerializeField] private float distance = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool isNegative;
    private float _dt = 0f;
    private bool _isUp = true;

    [Header("debug")]
    [SerializeField] private Color debug;

    private void Start()
    {
        origin = transform.position + distance*Vector3.up;

        if (isNegative)
        {
            _dt = 1f;
            _isUp = false;
        }
    }

    public void Update()
    {
        if (_isUp) _dt += Time.deltaTime * speed;
        else _dt -= Time.deltaTime * speed;

        if (_dt >= 1f) _isUp = false;
        else if (_dt <= 0f) _isUp = true;

        float newDt = _dt * 2 - 1;
        float rad = _dt * Mathf.PI;

        Vector3 move = -new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * distance;
        move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * move;
        transform.position = origin + move;

        Vector3 angle = transform.eulerAngles;
        angle.z = rad * Mathf.Rad2Deg - 90f;
        transform.eulerAngles = angle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<PlayerAgent>()?.Die("못 피한 도끼에 발등 찍혔습니다", dir);
        }
    }
}
