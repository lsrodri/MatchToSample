using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public float startTime = 60.0f;
    public TextMeshProUGUI textObject;
    public bool loadScene;
    private float currentTime;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        textObject.text = currentTime.ToString("0");

        if (currentTime <= 0 && loadScene)
        {
            SceneManager.LoadScene(sceneName);
        }

    }
}
