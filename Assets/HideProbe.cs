using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideProbe : MonoBehaviour
{
    public Transform targetObject;

    // Haptic device for visibility toggling
    public GameObject sphere;
    public GameObject cylinder;

    // Get the MeshRenderer component of the game object
    private MeshRenderer sphereMeshRenderer;
    private MeshRenderer cylinderMeshRenderer;

    private bool hidden = false;

    private void Start()
    {
        sphereMeshRenderer = sphere.GetComponent<MeshRenderer>();
        cylinderMeshRenderer = cylinder.GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject.gameObject)
        { 
            sphereMeshRenderer.enabled = false;
            cylinderMeshRenderer.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == targetObject.gameObject)
        {
            sphereMeshRenderer.enabled = false;
            cylinderMeshRenderer.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetObject.gameObject)
        {
            sphereMeshRenderer.enabled = true;
            cylinderMeshRenderer.enabled = true;
        }
    }

}
