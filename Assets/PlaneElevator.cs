using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneElevator : MonoBehaviour
{

    public GameObject elevator;
    public Vector3 targetPosition;
    public Vector3 originalPosition;
    public float duration;

    public IEnumerator MoveElevator(string direction)
    {
        //Debug.Log(direction);
        if (direction == "up")
        {
            // Moving the plane to its target upwards position
            yield return StartCoroutine(MoveElevatorCoroutine(elevator, targetPosition, duration));
        }
        else
        {
            // Smoothly moving the elevator down makes the probe miss the stimuli, so I'm moving it back abruptly
            yield return StartCoroutine(MoveElevatorCoroutine(elevator, originalPosition, duration));
            //elevator.transform.position = originalPosition;
        }
        
    }

    private IEnumerator MoveElevatorCoroutine(GameObject elevator, Vector3 targetPosition, float duration)
    {

        float elapsedTime = 0f;
        Vector3 startPosition = elevator.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Use a smooth interpolation function like SmoothStep or Sine to create smooth movement
            float smoothStepT = Mathf.SmoothStep(0f, 1f, t);

            // Calculate the new position based on the interpolation value
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, smoothStepT);

            // Update the elevator's position
            elevator.transform.position = newPosition;

            yield return null;
        }

        // Ensure the elevator reaches the exact target position
        elevator.transform.position = targetPosition;

    }
}
