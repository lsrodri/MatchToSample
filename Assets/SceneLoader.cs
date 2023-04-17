using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public TextMeshProUGUI participantId;
    public TextMeshProUGUI trialNumber;
    
    
    public void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("participantId", participantId.text);
        PlayerPrefs.SetString("trialNumber", trialNumber.text);
        SceneManager.LoadScene(sceneName);
    }
}