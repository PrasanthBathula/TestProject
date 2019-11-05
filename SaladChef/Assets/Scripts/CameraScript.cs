using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    private Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    IEnumerator ZoomIn()
    {

        yield return null;
    }

    IEnumerator ZoomOut()
    {
        yield return null;
    }
}
