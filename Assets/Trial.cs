using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Trial : MonoBehaviour
{

    public int phase = 1;

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

    // Objects that prevent the probe from going under stimuli level
    public GameObject plane;
    public GameObject sampleBarrier;
    public GameObject matchBarrier;

    // Objects that create a mask for stimuli
    private GameObject maskCubeLeft;
    private GameObject maskCubeRight;
    private GameObject maskCubeSample;

    // Timestamps
    private string startTimestamp;
    private string answerTimestamp;

    // Canvas references for requesting answers on countdown end
    public Canvas countdownCanvas;
    public Canvas promptCanvas;
    public Canvas pauseCanvas;

    public float pauseLength;

    public Material mat;

    string sampleNumber;
    string sampleOrder;
    string sampleTime;
    string comparisonTime;
    string condition;

    private GameObject sampleObject;
    private GameObject foilObject;

    private CsvReader csvReader;
    private Dictionary<string, string> rowData;

    // Start is called before the first frame update
    void Start()
    {

        promptCanvas.enabled = false;
        pauseCanvas.enabled = false;

        trialNumber = PlayerPrefs.HasKey("trialNumber") ? PlayerPrefs.GetString("trialNumber") : adhocTrialNumber;
        participantId = PlayerPrefs.HasKey("participantId") ? PlayerPrefs.GetString("participantId") : adhocParticipantId;

        Debug.Log("trialNumber: " + trialNumber);
        Debug.Log("participantId: " + participantId);

        csvReader = FindObjectOfType<CsvReader>();

        maskCubeLeft = GameObject.Find("MaskCubeLeft");
        maskCubeRight = GameObject.Find("MaskCubeRight");
        maskCubeSample = GameObject.Find("MaskCubeSample");

        startTrial();
    }

    // Update is called once per frame
    void Update()
    {
        if (phase == 1)
        {
            currentTime -= Time.deltaTime;
            timerText.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                if (!pauseCanvas.enabled)
                {
                    startPause(2);
                }
                else
                {
                    // If time is up after canvas is shown, this means that the first phase is over and the second phase should be loaded
                    phase = 2;
                    currentTime = float.Parse(comparisonTime) / 1000f;
                    pauseCanvas.enabled = false;
                    startTrial();
                }
            }
        }
        else if (phase == 2)
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
    }

    private void startTrial()
    {

        rowData = csvReader.ReadCsvRow(participantId, trialNumber);

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
        sampleObject.transform.Find("default").GetComponent<Renderer>().material = mat;

        if (phase == 1)
        {
            // to-do: find position for single stimulus, duplicate barriers to display here, hide other barrier set

            // Central position for sample stimulus
            sampleObject.transform.localPosition = new Vector3(-1.76999998f, 0.5500000007f, -0.50999999f);

            // Surface Sample Barrier and hide Match Barrier
            sampleBarrier.transform.localPosition = new Vector3(1.70600009f, -0.271763206f, 0.100000001f);
            matchBarrier.transform.localPosition = new Vector3(1.70600009f, -2.271763206f, 0.100000001f);

            // Surfacing the sample mask and hiding the others
            maskCubeSample.transform.localPosition = new Vector3(0.0399999991f, -0.448236823f, -0.0400003791f);
            maskCubeLeft.transform.localPosition = new Vector3(-1.58000028f, -2.448236823f, -0.0400003791f);
            maskCubeRight.transform.localPosition = new Vector3(1.66999972f, -2.448236823f, -0.0400003791f);
            

            // As far as our understanding goes, conditions only apply to the first phase because the second needs to resemble real-world conditions, in this case, visuohaptic.
            // If this changes, we would only need to remove this conditional statament below
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
                //foilObject.transform.Find("default").tag = "Untagged";
                //GameObject newFoilObject = Instantiate(foilObject, foilObject.transform.position, Quaternion.identity);
                //newFoilObject.transform.SetParent(foilObject.transform.parent);
                //Destroy(foilObject);
            }
            else if (condition == "H")
            {
                sampleObject.transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
                //foilObject.transform.Find("default").GetComponent<MeshRenderer>().enabled = false;
            }

            // For development purposes, I am only reading the csv comparisonTime if I haven't set it on the controller game object
            if (timeLimit == 0)
            {
                // Transforming the csv time in ms to seconds for the countdown
                timeLimit = float.Parse(sampleTime) / 1000f;
            }
            
        }
        else if (phase == 2)
        {
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
        
            foilObject.transform.Find("default").GetComponent<Renderer>().material = mat;

            // Surface Match Barrier and hide Sample Barrier
            matchBarrier.transform.localPosition = new Vector3(1.70600009f, -0.271763206f, 0.100000001f);
            sampleBarrier.transform.localPosition = new Vector3(1.70600009f, -2.271763206f, 0.100000001f);

            // Surfacing the sample mask and hiding the others
            maskCubeSample.transform.localPosition = new Vector3(0.0399999991f, -2.448236823f, -0.0400003791f);
            maskCubeLeft.transform.localPosition = new Vector3(-1.58000028f, -0.448236823f, -0.0400003791f);
            maskCubeRight.transform.localPosition = new Vector3(1.66999972f, -0.448236823f, -0.0400003791f);

            if (timeLimit == 0)
            {
                // Transforming the csv time in ms to seconds for the countdown
                timeLimit = float.Parse(comparisonTime) / 1000f;
            }

        }

        // Initializing the timer
        currentTime = timeLimit;

        startTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
        // to-do: move current trial object's back to original position.
        // Setting the next trial
        int nextTrial = int.Parse(trialNumber) + 1;
        trialNumber = nextTrial.ToString();
        
        // Persisting it in case of scene unloading or crash
        PlayerPrefs.SetString("trialNumber", trialNumber);

        Debug.Log("Next trial: " + nextTrial);

        countdownCanvas.enabled = true;
        promptCanvas.enabled = false;

        sampleObject.transform.localPosition = new Vector3(-8.56f, 0.5500000007f, -0.50999999f);
        foilObject.transform.localPosition = new Vector3(8.05f, 0.5500000007f, -0.50999999f);

        phase = 1;
        currentTime = float.Parse(sampleTime) / 1000f;

        startPause(phase);
    }

    private void startPause(int nextPhase)
    {

        countdownCanvas.enabled = false;
        pauseCanvas.enabled = true;
        currentTime = pauseLength;

        // If this is a pause between sample and match
        if (nextPhase == 2)
        {
            // Moving objects out of the way
            sampleObject.transform.localPosition = new Vector3(-8.56f, 0.5500000007f, -0.50999999f);
        }
        // If this is a pause between this and the next trial
        else if (nextPhase == 1)
        {
            startTrial();
        }
    }

    private void PromptAnswer()
    {

        sampleObject.transform.localPosition = new Vector3(-8.56f, 0.5500000007f, -0.50999999f);
        foilObject.transform.localPosition = new Vector3(8.05f, 0.5500000007f, -0.50999999f);

        countdownCanvas.enabled = false;
        promptCanvas.enabled = true;
    }
}
