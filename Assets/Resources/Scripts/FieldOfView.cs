using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f;
    public float viewDistance = 5f;
    public int rayCount = 10;
    //public GameObject 
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

            //print(transform.position + " " + transform.localPosition + " " + origin);

            if(hits.Length > 0) {
                for (int j = 0; j < hits.Length; j++) {
                    GameObject thingHit = hits[j].collider.gameObject;
                    if (thingHit.layer == LayerMask.NameToLayer("Walls")) {
                        if (thingHit.layer == LayerMask.NameToLayer("Walls")) {
                            vertex = hits[j].point - new Vector2(transform.position.x, transform.position.y);
                        }
                        break;
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

        //startingAngle = 0f;
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

}
