using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SceneLoader : MonoBehaviour
{
    
    public TextMeshProUGUI participantIdText;
    public TextMeshProUGUI trialNumberText;

    private string participantId;
    private string trialNumber;


    public void LoadScene(string sceneName)
    {
        // Remove any leading or trailing whitespace
        participantId = participantIdText.text;
        participantId = participantId.Trim();
        participantId = Regex.Replace(participantId, "[^0-9]", "");

        trialNumber = trialNumberText.text;
        trialNumber = trialNumber.Trim();
        trialNumber = Regex.Replace(trialNumber, "[^0-9]", "");

        //Debug.Log(trialNumber);

        PlayerPrefs.SetString("participantId", participantId);
        PlayerPrefs.SetString("trialNumber", trialNumber);
        SceneManager.LoadScene(sceneName);
    }
}