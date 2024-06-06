using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask layer;
    private Vector3 distance = Vector3.zero;

    private void Start()
    {
        distance = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Ray ray = new Ray(target.position + Vector3.up, distance);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance.magnitude, layer))
        {
            transform.position = hitInfo.point;
        }
        else
        {
            transform.position = target.position + distance;
        }
    }
}
