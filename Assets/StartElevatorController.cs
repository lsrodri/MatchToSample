using System.Collections;
using UnityEngine;

public class StartElevatorController : MonoBehaviour
{
    [SerializeField] private Vector3[] targetPositions;
    [SerializeField] private float moveSpeed = 1.0f;
    private int currentSceneIndex = 0;

    private void Start()
    {
        if (targetPositions.Length > 0)
        {
            StartCoroutine(MoveToPosition(targetPositions[currentSceneIndex]));
        }
    }

    public void MoveToNextPosition()
    {
        currentSceneIndex++;

        if (currentSceneIndex < targetPositions.Length)
        {
            StartCoroutine(MoveToPosition(targetPositions[currentSceneIndex]));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void InstantMoveToPosition()
    {
        if (currentSceneIndex < targetPositions.Length)
        {
            transform.position = targetPositions[currentSceneIndex];
            currentSceneIndex++;
        }
    }

}
