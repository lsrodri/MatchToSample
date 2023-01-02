using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnTouch : MonoBehaviour
{

    public bool hideOnStart;
    Renderer renderer;


    private void Start()
    {
        // Get the renderer of the game object
        renderer = GetComponent<Renderer>();

        // Disable the renderer initially
        if (hideOnStart)
        {
            renderer.enabled = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        renderer.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        renderer.enabled = false;
    }

}
