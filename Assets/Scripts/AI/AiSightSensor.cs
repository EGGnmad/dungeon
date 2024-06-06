using UnityEngine;

public class AiSightSensor : MonoBehaviour, IAiSensor
{
    public float distance = 10f;
    public float angle = 30f;
    public float height = 1f;
    public LayerMask occlusionLayers;

    [Header("sight config")]
    [SerializeField] private bool debug;
    [SerializeField] private bool gameView;
    [SerializeField] private MeshFilter sightMeshFilter;
        
    public Color meshColor = Color.red;
    private Mesh mesh;

    private void Start()
    {
        if (!gameView)
        {
            sightMeshFilter.mesh = null;
            return;
        }
        sightMeshFilter.mesh = mesh;
        sightMeshFilter.transform.position = transform.position - (height / 2 * Vector3.up);
    }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, distance, occlusionLayers);
        foreach (var col in colliders)
        {
            if (IsInSight(col.gameObject))
            {
                onSensor?.Invoke(col.gameObject);
            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position - (height/2 * Vector3.up);
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;

        float distance = Vector3.Distance(origin, dest);
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (distance > this.distance || deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (!Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    private Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;
        
        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;
        
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        
        // right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            
            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;
            
            // far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
        
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
        
            // top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
        
            // bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            
            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }
    
    private void OnDrawGizmos()
    {
        if (!mesh) return;
        if(!debug) return;

        Gizmos.color = meshColor;
        Gizmos.DrawMesh(mesh, transform.position - (height/2 * Vector3.up), transform.rotation);
    }

    public event IAiSensor.AlertSensor onSensor;
}
