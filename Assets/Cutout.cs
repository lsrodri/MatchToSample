using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutout : MonoBehaviour
{
    public Transform targetObject;
    public float textureSpeed = 0.105f;

    public Texture holeTexture;
    public Texture plainTexture;

    public bool hideProbe = true;

    // Haptic device for visibility toggling
    public GameObject sphere;
    public GameObject cylinder;
    public GameObject collisionCylinder;

    // Get the MeshRenderer component of the game object
    private MeshRenderer sphereMeshRenderer;
    private MeshRenderer cylinderMeshRenderer;
    private MeshRenderer collisionCylinderMeshRenderer;

    // Needed to check whether the application is running as the probe should not be hidden at this point
    public Canvas promptCanvas;
    public Canvas pauseCanvas;

    private Renderer textureRenderer;
    //private Color sphereColor;
    //private Color cylinderColor;

    void Start()
    {
        // Get the renderer component of the texture
        textureRenderer = GetComponent<Renderer>();
        sphereMeshRenderer = sphere.GetComponent<MeshRenderer>();
        cylinderMeshRenderer = cylinder.GetComponent<MeshRenderer>();
        collisionCylinderMeshRenderer = collisionCylinder.GetComponent<MeshRenderer>();

        // Getting material color for opacity dimming
        //sphereColor = sphereMeshRenderer.material.color;
        //cylinderColor = cylinderMeshRenderer.material.color;
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

    private void ProbeVisibility(bool enable, Collision collision)
    {
        // Performing the collision check away from collision events to prevent repeated code
        if (collision.gameObject == targetObject.gameObject)
        {

            if (enable == true)
            {
                textureRenderer.material.mainTexture = plainTexture;
            }
            else
            {
                textureRenderer.material.mainTexture = holeTexture;
            }

            // Probe is visible if either pause or prompt canvas are enabled, even if collision is happening
            if (promptCanvas.enabled || pauseCanvas.enabled)
            {
                sphereMeshRenderer.enabled = true;
                cylinderMeshRenderer.enabled = true;
            }
            else
            {
                // Probe sphere is hidden or reactivated in any case
                sphereMeshRenderer.enabled = enable;
                cylinderMeshRenderer.enabled = enable;
            }
            
            // If the probe should not disappear, a "shorter" cylinder is shown to prevent occlusion
            if (!hideProbe)
            {
                // Showing the
                collisionCylinderMeshRenderer.enabled = !enable;
            }
            
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProbeVisibility(false, collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        ProbeVisibility(false, collision);
    }

    private void OnCollisionExit(Collision collision)
    {   
        ProbeVisibility(true, collision);
    }

}
