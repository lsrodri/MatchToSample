using UnityEngine;
using Valve.VR;

public class ControllerInput : MonoBehaviour
{
    //public SteamVR_Action_Vector2 thumbstickAction;
    public SteamVR_Action_Boolean snapLeftAction = SteamVR_Input.GetBooleanAction("SnapTurnLeft");
    public SteamVR_Action_Boolean snapRightAction = SteamVR_Input.GetBooleanAction("SnapTurnRight");
    public SteamVR_Input_Sources handType;
    public float inputThreshold = 0.5f;

    void Update()
    {
        ProcessThumbstickInput();
    }

    private void ProcessThumbstickInput()
    {
        //Vector2 thumbstickInput = thumbstickAction.GetAxis(handType);

        //if (thumbstickInput.x < -inputThreshold)
        //{
        //    MoveLeft();
        //}
        //else if (thumbstickInput.x > inputThreshold)
        //{
        //    MoveRight();
        //}

        if(snapLeftAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            MoveLeft();
        }
    }

    private void MoveLeft()
    {
        Debug.Log("Thumbstick moved left");
        // Your custom logic here
    }

    private void MoveRight()
    {
        Debug.Log("Thumbstick moved right");
        // Your custom logic here
    }
}
