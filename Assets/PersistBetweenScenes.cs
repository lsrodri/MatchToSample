using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PersistBetweenScenes : MonoBehaviour
{
    [SerializeField] private float waitTime = 2.0f;
    [SerializeField] public bool instantMove = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartElevatorController startElevatorController = GetComponent<StartElevatorController>();

        if (startElevatorController != null)
        {
            StartCoroutine(WaitAndMove(waitTime));
        }
    }

    private IEnumerator WaitAndMove(float delay)
    {
        yield return new WaitForSeconds(delay);

        //if (instantMove && SceneManager.GetActiveScene().buildIndex == 1)
        if (instantMove && SceneManager.GetActiveScene().buildIndex == 1)
        {
            GetComponent<StartElevatorController>().InstantMoveToPosition();
        }
        else
        {
            GetComponent<StartElevatorController>().MoveToNextPosition();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
