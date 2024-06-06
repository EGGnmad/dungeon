using UnityEngine;

[ExecuteInEditMode]
public class AttachToWorldObject : MonoBehaviour
{
    public new Camera camera;
    public Transform attachTransform;
    public Vector3 worldOffset = Vector2.zero;
    public bool doRaycast = false;
    public LayerMask layers;
    
    private void Update()
    {
        if (!camera || !attachTransform) return;
        
        Vector3 pos = camera.WorldToScreenPoint(attachTransform.transform.position + worldOffset);
        transform.position = pos;
    }

    [ContextMenu("Set To Main Camera")]
    private void SetToMainCamera()
    {
        camera = Camera.main;
    }
}
