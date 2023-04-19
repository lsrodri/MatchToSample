using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskRender : MonoBehaviour
{
    public GameObject sphere;

    void OnWillRenderObject()
    {
        // Get the camera that is rendering the plane
        Camera camera = Camera.current;

        // Convert the position of the sphere from world space to viewport space
        Vector3 spherePos = camera.WorldToViewportPoint(sphere.transform.position);

        // Set a global vector variable in the shader to the position of the sphere in viewport space
        Shader.SetGlobalVector("_MaskPosition", spherePos);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
