using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(SphereCollider))]
public class SoundObject : MonoBehaviour
{
    public float soundRangeRadius = 1f;
    public float lifeTime = 10f;
    public IObjectPool<SoundObject> poolToReturn;

    private SphereCollider _collider;

    #region Unity
    private void OnValidate()
    {
        Reset();
    }
    
    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            DestorySoundObject();
        }
    }
    #endregion

    #region ObjectPool

    public void Reset()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = soundRangeRadius;
    }

    public void DestorySoundObject()
    {
        poolToReturn.Release(this);
    }

    #endregion
}