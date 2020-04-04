using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f;
    public float viewDistance = 5f;
    public int rayCount = 10;

    public NPC npc;
    private Vector3 origin = Vector3.zero;
    private float startingAngle = 0f;
    private Mesh mesh;
    private MeshFilter meshFilter;


    void Start() {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }

    // Update is called once per frame
    void LateUpdate() {

        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = transform.localPosition;

        origin = transform.position;

        int vertextIndex = 1;
        int triangleIndex = 0;

        for (int i = 0; i <= rayCount; i++) {

            Vector3 vertex = getVectorFromAngle(startingAngle) * viewDistance;
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, getVectorFromAngle(startingAngle), viewDistance);

            if (hits.Length > 0) {
                for (int j = 0; j < hits.Length; j++) {
                    GameObject thingHit = hits[j].collider.gameObject;
                    if (thingHit.layer == LayerMask.NameToLayer("Walls")) {
                        vertex = hits[j].point - new Vector2(transform.position.x, transform.position.y);
                    }
                }
            }

            vertices[vertextIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertextIndex - 1;
                triangles[triangleIndex + 2] = vertextIndex;
                triangleIndex += 3;
            }

            vertextIndex++;
            startingAngle -= angleIncrease;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        updateColliderToMatchMesh();

    }

    void updateColliderToMatchMesh() {

        // Get triangles and vertices from mesh
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        // Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
        Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
        for (int i = 0; i < triangles.Length; i += 3) {
            for (int e = 0; e < 3; e++) {
                int vert1 = triangles[i + e];
                int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                if (edges.ContainsKey(edge)) {
                    edges.Remove(edge);
                }
                else {
                    edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                }
            }
        }

        // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
        Dictionary<int, int> lookup = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> edge in edges.Values) {
            if (lookup.ContainsKey(edge.Key) == false) {
                lookup.Add(edge.Key, edge.Value);
            }
        }

        // Create empty polygon collider
        PolygonCollider2D polygonCollider = gameObject.GetComponent<PolygonCollider2D>();

        polygonCollider.pathCount = 0;
        polygonCollider.isTrigger = true;

        // Loop through edge vertices in order
        int startVert = 0;
        int nextVert = startVert;
        int highestVert = startVert;
        List<Vector2> colliderPath = new List<Vector2>();
        while (true) {
            //print("start in true");
            // Add vertex to collider path
            colliderPath.Add(vertices[nextVert]);

            // Get next vertex
            nextVert = lookup[nextVert];

            // Store highest vertex (to know what shape to move to next)
            if (nextVert > highestVert) {
                highestVert = nextVert;
            }

            // Shape complete
            if (nextVert == startVert) {

                // Add path to polygon collider
                polygonCollider.pathCount++;
                polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderPath.ToArray());
                colliderPath.Clear();

                // Go to next shape if one exists
                if (lookup.ContainsKey(highestVert + 1)) {

                    // Set starting and next vertices
                    startVert = highestVert + 1;
                    nextVert = startVert;

                    // Continue to next loop
                    continue;
                }

                // No more verts
                break;
            }
        }
        //print("out of while");
    }

    public void setOrigin(Vector3 origin) {
        this.origin = origin;
    }

    public void setDirection(Vector3 direction) {

        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        if (angle < 0) {
            angle += 360;
        }

        this.startingAngle = angle + fov / 2f;
    }
    public void setDirection(float dir) {
        this.startingAngle = dir;
    }

    Vector3 getVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<PlayerController>()) {
            npc.NPCSeesPlayer(collision.gameObject);
        }
    }

    

}
