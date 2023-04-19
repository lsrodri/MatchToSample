using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrialSample : MonoBehaviour
{
    // This Boolean is used to check whether the trial condition should affect the sample part as it does with matching
    public bool sampleCondition;
    
    private string trialNumber;
    private string participantId;

    public string adhocTrialNumber;
    public string adhocParticipantId;

    public float timeLimit;
    public TextMeshProUGUI timerText;
    public bool loadScene;
    private float currentTime;
    public string sceneName;

    public Material mat;

    string sampleNumber;
    string sampleTime;
    string condition;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        // Getting the trial and participant numbers from previous scene.
        // For dev purposes, I added ad hoc public versions as fallback
        trialNumber = PlayerPrefs.HasKey("trialNumber") ? PlayerPrefs.GetString("trialNumber") : adhocTrialNumber;
        participantId = PlayerPrefs.HasKey("participantId") ? PlayerPrefs.GetString("participantId") : adhocParticipantId;

        // CsvReader object must be added to the same scene
        CsvReader csvReader = FindObjectOfType<CsvReader>();
        Dictionary<string, string> rowData = csvReader.ReadCsvRow(participantId, trialNumber);

        if (rowData != null)
        {
            // Access the data for the columns you're interested in
            sampleNumber = rowData["Sample Number"];
            sampleTime = rowData["Sample Time"];
            condition = rowData["Condition"];
            
        } 
        else
        {
            // Manual data for OSX csv issues
            sampleNumber = "80";
            sampleTime = "100000";
            condition = "VH";

            Debug.LogError($"Could not find row with Participant ID {participantId} and Trial Number {trialNumber}");
        }
        GameObject sampleObject = GameObject.Find(sampleNumber + "s");

        sampleObject.transform.localPosition = new Vector3(-1.78999996f,0.0500000007f,-0.479999989f);

        sampleObject.transform.Find("default").GetComponent<Renderer>().material = mat;

        // Only modifying the rendering of the sample if allowed by this public boolean
        if(sampleCondition)
        {
            if (condition == "V")
            {
                // Reinstantiating the sample and foil game objects so that it does not get mapped by OpenHaptics, just removing the tag does not solve it
                // Removing the tag, else the new instance comes tagged and gets mapped
                sampleObject.transform.Find("default").tag = "Untagged";
                GameObject newSampleObject = Instantiate(sampleObject, sampleObject.transform.position, Quaternion.identity);
                // Set the parent of the new instance to the parent of the original object, else it's out of position
                newSampleObject.transform.SetParent(sampleObject.transform.parent);
                // Destroying the original
                Destroy(sampleObject);
            } 
            else if(condition == "H")
            {
                sampleObject.transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
            }
        }

        // For development purposes, I am only reading the csv comparisonTime if I haven't set it on the controller game object
        if (timeLimit == 0)
        {
            // Transforming the csv time in ms to seconds for the countdown
            timeLimit = float.Parse(sampleTime) / 1000f;
        }

        // Initializing the timer
        currentTime = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        timerText.text = currentTime.ToString("0");

        if (currentTime <= 0 && loadScene)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
