using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//! This MonoBehavior can be attached to any object with a collider. It will apply a haptic "effect"
//! to any haptic stylus that is within the boundries of the collider.
//! The parameters can be adjusted on the fly.
public class HapticDeviceControl : MonoBehaviour
{


	HapticPlugin[] devices;
	HapticPlugin device;
	bool[] inTheZone;       //Is the stylus in the effect zone?
	Vector3[] devicePoint;  // Current location of stylus
	float[] delta;          // Distance from stylus to zone collider.
	int[] FXID;             // ID of the effect.  (Per device.)

	// These are the user adjustable vectors, converted to world-space. 
	private Vector3 focusPointWorld = Vector3.zero;
	private Vector3 directionWorld = Vector3.up;


	//! Start() is called at the beginning of the simulation.
	//!
	//! It will identify the Haptic devices, initizlize variables internal to this script, 
	//! and request an Effect ID from Open Haptics. (One for each device.)
	//!
	void Start()
	{
		//Initialize the list of haptic devices.
		devices = (HapticPlugin[])Object.FindObjectsOfType(typeof(HapticPlugin));
		inTheZone = new bool[devices.Length];
		devicePoint = new Vector3[devices.Length];
		delta = new float[devices.Length];
		FXID = new int[devices.Length];

		// Generate an OpenHaptics effect ID for each of the devices.
		for (int ii = 0; ii < devices.Length; ii++)
		{
			inTheZone[ii] = false;
			devicePoint[ii] = Vector3.zero;
			delta[ii] = 0.0f;
			FXID[ii] = HapticPlugin.effects_assignEffect(devices[ii].configName);
		}

		HapticPlugin device = devices[0];
		//for (int ii = 0; ii < devices.Length; ii++)
		//{
		//	Debug.Log(devices[ii]);
		//}

		Debug.Log(device.configName);
	}




	//!  Update() is called once per frame.
	//! 
	//! This function 
	//! - Determines if a haptic stylus is inside the collider
	//! - Updates the effect settings.
	//! - Starts and stops the effect when appropriate.
	void Update()
	{
		//// Find the pointer to the collider that defines the "zone". 
		//Collider collider = gameObject.GetComponent<Collider>();
		//if (collider == null)
		//{
		//	Debug.LogError("This Haptic Effect Zone requires a collider");
		//	return;
		//}

		//// Update the World-Space vectors
		//focusPointWorld = transform.TransformPoint(Position);
		//directionWorld = transform.TransformDirection(Direction);

		//// Update the effect seperately for each haptic device.
		//for (int ii = 0; ii < devices.Length; ii++)
		//{
		//	HapticPlugin device = devices[ii];
		//	bool oldInTheZone = inTheZone[ii];
		//	int ID = FXID[ii];

		//	// If a haptic effect has not been assigned through Open Haptics, assign one now.
		//	if (ID == -1)
		//	{
		//		FXID[ii] = HapticPlugin.effects_assignEffect(devices[ii].configName);
		//		ID = FXID[ii];

		//		if (ID == -1) // Still broken?
		//		{
		//			Debug.LogError("Unable to assign Haptic effect.");
		//			continue;
		//		}
		//	}

		//	// Determine if the stylus is in the "zone". 
		//	Vector3 StylusPos = device.stylusPositionWorld; //World Coordinates
		//	Vector3 CP = collider.ClosestPoint(StylusPos);  //World Coordinates
		//	devicePoint[ii] = CP;
		//	delta[ii] = (CP - StylusPos).magnitude;

		//	//If the stylus is within the Zone, The ClosestPoint and the Stylus point will be identical.

		//	Vector3 targetPosition = new Vector3(0f, 0f, 0f);
		//	Vector3 direction = (targetPosition - StylusPos).normalized;

		//	//if (delta[ii] <= Mathf.Epsilon)
		//	//{
		//	inTheZone[ii] = true;

		//	// Convert from the World coordinates to coordinates relative to the haptic device.
		//	Vector3 focalPointDevLocal = device.transform.InverseTransformPoint(focusPointWorld);
		//	Vector3 rotationDevLocal = device.transform.InverseTransformDirection(directionWorld);
		//	double[] pos = { focalPointDevLocal.x, focalPointDevLocal.y, focalPointDevLocal.z };
		//	//double[] dir = { rotationDevLocal.x, rotationDevLocal.y, rotationDevLocal.z };
		//	double[] dir = { direction.x, direction.y, direction.z };

		//	double Mag = Magnitude;

		//	if (device.isInSafetyMode())
		//		Mag = 0;

			HapticPlugin.setForce(device.configName, new double[] { 0, 0.1, 0 }, new double[] { 0, 0, 0 });

		//	// Send the current effect settings to OpenHaptics.
		//	HapticPlugin.effects_settings(
		//		device.configName,
		//		ID,
		//		Gain,
		//		Mag,
		//		Frequency,
		//		pos,
		//		dir);
		//	HapticPlugin.effects_type(
		//		device.configName,
		//		ID,
		//		(int)effectType);

		//	//}
		//	//else
		//	//{
		//	//	inTheZone[ii] = false;

		//	//	// Note : If the device is not in the "Zone", there is no need to update the effect settings.
		//	//}

		//	//// If the on/off state has changed since last frame, send a Start or Stop event to OpenHaptics
		//	//if (oldInTheZone != inTheZone[ii])
		//	//{
		//	//	if (inTheZone[ii])
		//	//	{
		//	if (direction != targetPosition)
		//	{


		//		HapticPlugin.effects_startEffect(device.configName, ID);
		//	}
		//	else
		//	{


		//		//}
		//		//else
		//		//{

		//		HapticPlugin.effects_stopEffect(device.configName, ID);
		//	}
		//	//		}
		//	//	}

		//	//}
		//}
	}


	//! OnDrawGizmos() is called only when the Unity Editor is active.
	//! It draws some hopefully useful wireframes to the editor screen.

	


}

//#if UNITY_EDITOR
//[CustomEditor(typeof(HapticDeviceControl))]
//public class HapticDeviceControlEditor : Editor
//{
//	override public void OnInspectorGUI()
//	{
//		HapticEffect HE = (HapticEffect)target;

//		if (HE.gameObject.gameObject.GetComponent<Collider>() == null)
//		{
//			EditorGUILayout.LabelField("*********************************************************");
//			EditorGUILayout.LabelField("   Haptic Effect must be assigned to an object with a collider.");
//			EditorGUILayout.LabelField("*********************************************************");

//		}
//		else
//		{
//			HE.effectType = (HapticEffect.EFFECT_TYPE)EditorGUILayout.EnumPopup("Effect Type", HE.effectType);


//			switch (HE.effectType)
//			{
//				case HapticEffect.EFFECT_TYPE.CONSTANT:
//					HE.Direction = EditorGUILayout.Vector3Field("Direction", HE.Direction);
//					HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
//					break;
//				case HapticEffect.EFFECT_TYPE.FRICTION:
//					HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
//					HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
//					break;
//				case HapticEffect.EFFECT_TYPE.SPRING:
//					HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
//					HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
//					HE.Position = EditorGUILayout.Vector3Field("Position", HE.Position);
//					break;
//				case HapticEffect.EFFECT_TYPE.VIBRATE:
//					HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
//					HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
//					HE.Frequency = EditorGUILayout.Slider("Frequency", (float)HE.Frequency, 1.0f, 1000.0f);
//					HE.Direction = EditorGUILayout.Vector3Field("Direction", HE.Direction);
//					break;
//				case HapticEffect.EFFECT_TYPE.VISCOUS:
//					HE.Gain = EditorGUILayout.Slider("Gain", (float)HE.Gain, 0.0f, 1.0f);
//					HE.Magnitude = EditorGUILayout.Slider("Magnitude", (float)HE.Magnitude, 0.0f, 1.0f);
//					break;

//			}
//		}

//	}

//}

//#endif






