using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutout : MonoBehaviour
{
    public Transform targetObject;
    public float textureSpeed = 0.105f;

    public Texture holeTexture;
    public Texture plainTexture;

    // Haptic device for visibility toggling
    public GameObject sphere;
    public GameObject cylinder;

    // Get the MeshRenderer component of the game object
    private MeshRenderer sphereMeshRenderer;
    private MeshRenderer cylinderMeshRenderer;


    private Renderer textureRenderer;

    void Start()
    {
        // Get the renderer component of the texture
        textureRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calculate the position of the target object in local texture space
        Vector3 localPosition = transform.InverseTransformPoint(targetObject.position);

        // Calculate the UV offset based on the target object position
        Vector2 uvOffset = new Vector2(localPosition.x, localPosition.z) * textureSpeed;

        // Clamp the UV offset values to the range of -0.355 to 0.355
        float clampedU = Mathf.Clamp(uvOffset.x, -0.355f, 0.355f);
        float clampedV = Mathf.Clamp(uvOffset.y, -0.355f, 0.355f);
        uvOffset = new Vector2(clampedU, clampedV);

        // Apply the UV offset to the texture renderer
        textureRenderer.material.mainTextureOffset = uvOffset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject.gameObject)
        {
            textureRenderer.material.mainTexture = holeTexture;

            sphereMeshRenderer = sphere.GetComponent<MeshRenderer>();
            sphereMeshRenderer.enabled = false;
            cylinderMeshRenderer = cylinder.GetComponent<MeshRenderer>();
            cylinderMeshRenderer.enabled = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == targetObject.gameObject)
        {
            textureRenderer.material.mainTexture = holeTexture;

            sphereMeshRenderer = sphere.GetComponent<MeshRenderer>();
            sphereMeshRenderer.enabled = false;
            cylinderMeshRenderer = cylinder.GetComponent<MeshRenderer>();
            cylinderMeshRenderer.enabled = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetObject.gameObject)
        {
            textureRenderer.material.mainTexture = plainTexture;

            sphereMeshRenderer = sphere.GetComponent<MeshRenderer>();
            sphereMeshRenderer.enabled = true;
            cylinderMeshRenderer = cylinder.GetComponent<MeshRenderer>();
            cylinderMeshRenderer.enabled = true;
        }
    }

}
