using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrialMatch : MonoBehaviour
{
    // Variables to be read from Player Preferences, set by previous scenes
    private string trialNumber;
    private string participantId;


    // Possibility of setting manual variables for dev and debugging
    public string adhocTrialNumber;
    public string adhocParticipantId;

    // Variables needed for capturing an answer and saving it in a csv
    private bool leftKeyPressed = false;
    private bool rightKeyPressed = false;
    private bool isAnswered = false;

    // Time limit, imported from csv
    public float timeLimit;
    public TextMeshProUGUI timerText;
    private bool timeIsUp = false;
    public bool loadScene;
    private float currentTime;
    public string sceneName;

    public GameObject plane;

    // Timestamps
    private string startTimestamp;
    private string answerTimestamp;

    // Canvas references for requesting answers on countdown end
    public Canvas countdownCanvas;
    public Canvas promptCanvas;

    public Material mat;

    string sampleNumber;
    string sampleOrder;
    string sampleTime;
    string comparisonTime;
    string condition;

    GameObject sampleObject;
    GameObject foilObject;

    // Start is called before the first frame update
    void Start()
    {

        promptCanvas.enabled = false;
        
        trialNumber = PlayerPrefs.HasKey("trialNumber") ? PlayerPrefs.GetString("trialNumber") : adhocTrialNumber;
        participantId = PlayerPrefs.HasKey("participantId") ? PlayerPrefs.GetString("participantId") : adhocParticipantId;

        Debug.Log("trialNumber: " + trialNumber);
        Debug.Log("participantId: " + participantId);

        CsvReader csvReader = FindObjectOfType<CsvReader>();
        Dictionary<string, string> rowData = csvReader.ReadCsvRow(participantId, trialNumber);

        if (rowData != null)
        {
            // Access the data for the columns you're interested in
            sampleNumber = rowData["Sample Number"];
            sampleOrder = rowData["Sample Order"];
            sampleTime = rowData["Sample Time"];
            comparisonTime = rowData["Comparison Time"];
            condition = rowData["Condition"];
        } 
        else
        {
            // Manual data for OSX csv issues
            sampleNumber = "30";
            sampleOrder = "right";
            sampleTime = "10000";
            comparisonTime = "20000";
            condition = "V";

            Debug.LogError($"Could not find row with Participant ID {participantId} and Trial Number {trialNumber}");
        }

        sampleObject = GameObject.Find(sampleNumber + "s");
        foilObject = GameObject.Find(sampleNumber + "f");

        if (sampleOrder == "left")
        {
            sampleObject.transform.localPosition = new Vector3(-3.34f, 0.5500000007f, -0.50999999f);
            foilObject.transform.localPosition = new Vector3(3.21f, 0.5500000007f, -0.50999999f);
        } 
        else if (sampleOrder == "right")
        {
            sampleObject.transform.localPosition = new Vector3(-0.1499996f, 0.5500000007f, -0.50999999f);
            foilObject.transform.localPosition = new Vector3(0.0199995f, 0.5500000007f, -0.50999999f);
        }

        sampleObject.transform.Find("default").GetComponent<Renderer>().material = mat;
        foilObject.transform.Find("default").GetComponent<Renderer>().material = mat;

        if (condition == "V")
        {
            // Moving the background plane to stop device for dropping and to allow proble to glide through stimuli
            plane.transform.SetPositionAndRotation(new Vector3(0.27f, -0.9f, 0.508f), new Quaternion(0f, 0f, 0f, 0f));
            plane.GetComponent<Renderer>().enabled = false;

            // Reinstantiating the sample and foil game objects so that it does not get mapped by OpenHaptics, just removing the tag does not solve it
            // Removing the tag, else the new instance comes tagged and gets mapped
            sampleObject.transform.Find("default").tag = "Untagged";
            GameObject newSampleObject = Instantiate(sampleObject, sampleObject.transform.position, Quaternion.identity);
            // Set the parent of the new instance to the parent of the original object, else it's out of position
            newSampleObject.transform.SetParent(sampleObject.transform.parent);
            // Destroying the original
            Destroy(sampleObject);

            // Replicating for foil
            foilObject.transform.Find("default").tag = "Untagged";
            GameObject newFoilObject = Instantiate(foilObject, foilObject.transform.position, Quaternion.identity);
            newFoilObject.transform.SetParent(foilObject.transform.parent);
            Destroy(foilObject);
        } 
        else if(condition == "H")
        {
            sampleObject.transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
            foilObject.transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
        }

        // For development purposes, I am only reading the csv comparisonTime if I haven't set it on the controller game object
        if (timeLimit == 0)
        {
            // Transforming the csv time in ms to seconds for the countdown
            timeLimit = float.Parse(comparisonTime) / 1000f;
        }

        // Initializing the timer
        currentTime = timeLimit;

        startTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeIsUp)
        {
            // Managing and rendering the countdown
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("0");
        }
        

        if (currentTime <= 0 && promptCanvas.enabled == false)
        {
            timeIsUp = true;
            PromptAnswer();
        }

        // Capture arrow key input
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            leftKeyPressed = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rightKeyPressed = true;
        }

        // Save the answer to the CSV file
        if (leftKeyPressed && !isAnswered)
        {
            SaveAnswerToCsv("left");
            isAnswered = true;
        }
        else if (rightKeyPressed && !isAnswered)
        {
            SaveAnswerToCsv("right");
            isAnswered = true;
        }
    }

    private void SaveAnswerToCsv(string answer)
    {
        // Time since the scene was loaded, saved as participant reaction time
        string elapsedTime = Time.timeSinceLevelLoad.ToString();
        string correctness = answer == sampleOrder ? "true" : "false";

        answerTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");

        // Create a new row for the CSV file
        string[] rowData = new string[] { participantId, trialNumber, answer, correctness, elapsedTime, startTimestamp, answerTimestamp };
        // Check if the file exists
        string filePath = Path.Combine(Application.dataPath, "Results", participantId + ".csv");
        bool fileExists = File.Exists(filePath);

        // Write the row to the CSV file
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            if (!fileExists)
            {
                // Add the header row if the file did not exist previously
                sw.WriteLine("Participant ID,Trial Number,Response,Correctness,Reaction Time, Start Timestamp, End Timestamp");
            }

            sw.WriteLine(string.Join(",", rowData));
        }

        // After writing current trial data, setting the next trial
        nextTrial();
    }

    private void nextTrial()
    {
        // to-do: check count for csv participant id to check if this is the last trial
        // Setting the next trial
        int nextTrial = int.Parse(trialNumber) + 1;
        PlayerPrefs.SetString("trialNumber", nextTrial.ToString());

        Debug.Log("Next trial: " + nextTrial);

        if(loadScene)
        {
            SceneManager.LoadScene(sceneName);
        }
        // 
    }

    private void PromptAnswer()
    {
        countdownCanvas.enabled = false;
        promptCanvas.enabled = true;
        sampleObject.SetActive(false);
        foilObject.SetActive(false);
    }
}
