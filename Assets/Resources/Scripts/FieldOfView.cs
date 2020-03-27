using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float fov = 90f;
    public float viewDistance = 5f;
    public int rayCount = 10;

    private Vector3 origin = Vector3.zero;
    private float startingAngle = 0f;
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    void Start() {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter.mesh = mesh;
        //meshRenderer.material.color.a = 0.5;
    }

    // Update is called once per frame
    void LateUpdate() {

        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertextIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex = origin + getVectorFromAngle(startingAngle) * viewDistance;
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, getVectorFromAngle(startingAngle), viewDistance);
            //Debug.DrawRay(transform.position, getVectorFromAngle(angle) * viewDistance);
            if(hits.Length > 0) {
                for (int j = 0; j < hits.Length; j++) {
                    GameObject thingHit = hits[j].collider.gameObject;
                    if (thingHit.layer == LayerMask.NameToLayer("Walls")) {
                        if (thingHit.layer == LayerMask.NameToLayer("Walls")) {
                            vertex = hits[j].point;
                        }
                        break;
                    }
                }
            }
            
            //if (raycastHit2D.collider != null) {
            //    GameObject thingHit = raycastHit2D.collider.gameObject;
            //    print(thingHit.name);
            //    if (thingHit.layer == LayerMask.NameToLayer("Walls")) {
            //        vertex = raycastHit2D.point;
            //    } else {
            //        vertex = origin + getVectorFromAngle(angle) * viewDistance;
            //    }
            //}
            //else {
            //    print("Nothing");
            //    vertex = origin + getVectorFromAngle(angle) * viewDistance;
            //}
            //vertex = origin + getVectorFromAngle(angle) * viewDistance;
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
        this.startingAngle = angle - fov / 2f;
    }
    public void setDirection(float dir) {
        this.startingAngle = dir;
    }

    Vector3 getVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

}
