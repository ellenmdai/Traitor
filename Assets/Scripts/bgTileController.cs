using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgTileController : MonoBehaviour
{
    public float scrollSpeed;   // rate at which bg passes; set by GameController
    public Transform destroyPoint;  // to free up memory
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.position -= new Vector3(scrollSpeed, 0, 0);
        }
        if (transform.position.x <= destroyPoint.position.x) {
            Destroy(gameObject);
        }
    }
}
