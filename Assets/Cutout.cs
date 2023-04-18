using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutout : MonoBehaviour
{
    public GameObject sphere;
    public RenderTexture maskTexture;
    public Camera maskCamera;

    // Start is called before the first frame update
    void Start()
    {
        maskTexture = new RenderTexture(Screen.width, Screen.height, 24);

        // Set the mask texture as the target texture for the mask camera
        maskCamera.targetTexture = maskTexture;

        // Set the window plane's material to use the mask texture as its texture
        GetComponent<Renderer>().material.mainTexture = maskTexture;
    }

    // Update is called once per frame
    void Update()
    {
        // Set the position of the sphere to the mask texture position
        sphere.transform.position = transform.position;

        // Render the mask texture to the RenderTexture object
        Graphics.Blit(null, maskTexture);

        // Render the scene to the mask texture using the mask camera
        maskCamera.Render();
    }
}
