using UnityEngine;
using UnityEngine.Pool;

public class SoundObjectCreator : MonoBehaviour
{
    [SerializeField] private int maxSize;
    [SerializeField] private SoundObject _soPrefab;
    private ObjectPool<SoundObject> _soPool;

    private void Start()
    {
        _soPool = new(
            createFunc: () =>
            {
                var so = Instantiate(_soPrefab);
                so.poolToReturn = _soPool;
                return so;
            },
            actionOnGet: so =>
            {
                so.gameObject.SetActive(true);
                so.Reset();
            },
            actionOnRelease: so =>
            {
                so.gameObject.SetActive(false);
            },
            actionOnDestroy: so =>
            {
                Destroy(so.gameObject);
            },
            maxSize: maxSize
        );
    }

    public void Create(Transform transform, SoundObject prefab)
    {
        var so = _soPool.Get();
        
        so.transform.position = transform.position;
        so.transform.rotation = transform.rotation;
        so.soundRangeRadius = prefab.soundRangeRadius;
        so.lifeTime = prefab.lifeTime;
    }
}