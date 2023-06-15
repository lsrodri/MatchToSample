using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//! This MonoBehavior can be applied to any GameObject with a MeshCollider or MeshFilter.
//! It allows for the easy customization of the haptic properties of a "touchable" object.
public class MagneticSphere : MonoBehaviour
{

	private float hlStiffness = 0.7f;    //!< Higher values are less 'rubbery'.
	private float hlDamping = 0.5f;      //!< Higher values are less 'bouncy'.
	private float hlStaticFriction = 0.2f;   //!< Higher values have more 'sticky' surface friction.
	private float hlDynamicFriction = 0.3f;  //!< Higher values have more 'sliding' surface friction.
	private float hlPopThrough = 0.0f;   //!< If non-zero : How much force is required to "pop" through to the inside of the mesh.

	public Material greenMaterial;
	public Material redMaterial;
	private Material currentMaterial;

	public float snapDistance = 20.0f; //!< When in HL_CONTRAINT mode, the maximum distance the stylus will "snap" to the surface.


	//! Used automatically for initialization
	void Start()
	{
		if (GetComponent<MeshCollider>() == null && GetComponent<MeshFilter>() == null)
		{
			Debug.LogError("HapticSurface has been assigned to object without mesh.");
		}

		currentMaterial = gameObject.GetComponent<Renderer>().material;
	}

	public void toggleConstraint(int constraint)
	{
        
		// Commenting it as magnetism might be unnecessary

		//HapticPlugin.shape_settings(gameObject.GetInstanceID(), hlStiffness, hlDamping, hlStaticFriction, hlDynamicFriction, hlPopThrough);

  //      // If constraint = 1, constraint mode is activated, else it's contact mode
  //      HapticPlugin.shape_constraintSettings(gameObject.GetInstanceID(), constraint, snapDistance);
  //      HapticPlugin.shape_flipNormals(gameObject.GetInstanceID(), false);

  //      // Front and back facing
  //      HapticPlugin.shape_facing(gameObject.GetInstanceID(), 3);

		if (constraint == 1)
        {
			transform.position = new Vector3(0.409999996f, 0.800000012f, -0.810000002f);
		}
		else
        {
			transform.position = new Vector3(0f, -10f, 0f);
        }
    }

	// Material needs to be updated using a dummy object for collision detection as 
	public void updateMaterial(bool colliding)
	{
		if (colliding && gameObject.GetComponent<Renderer>().material != greenMaterial)
        {
			gameObject.GetComponent<Renderer>().material = greenMaterial;
		}
		else if (!colliding && gameObject.GetComponent<Renderer>().material != redMaterial)
		{
			gameObject.GetComponent<Renderer>().material = redMaterial;
		}
	}

    void OnDestroy()
	{
		//HapticPlugin.shape_remove(gameObject.GetInstanceID());
	}

}
