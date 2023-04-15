using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialMatch : MonoBehaviour
{
    private string trialNumber;
    private string participantId;

    public string adhocTrialNumber;
    public string adhocParticipantId;

    public Material mat;

    string sampleNumber;
    string sampleOrder;
    string sampleTime;
    string comparisonTime;
    string condition;

    // Start is called before the first frame update
    void Start()
    {

        trialNumber = PlayerPrefs.HasKey("trialNumber") ? PlayerPrefs.GetString("trialNumber") : adhocTrialNumber;
        participantId = PlayerPrefs.HasKey("participantId") ? PlayerPrefs.GetString("participantId") : adhocParticipantId;

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

            // Do something with the data

            GameObject sampleObject = GameObject.Find(sampleNumber + "s");
            GameObject foilObject = GameObject.Find(sampleNumber + "f");

            if (sampleOrder == "left")
            {
                sampleObject.transform.localPosition = new Vector3(-3.39299989f, 0.0500000007f, -0.50999999f);
                foilObject.transform.localPosition = new Vector3(3.21f, 0.0500000007f, -0.50999999f);
            } 
            else if (sampleOrder == "right")
            {
                sampleObject.transform.localPosition = new Vector3(-0.159999996f, 0.0500000007f, -0.50999999f);
                foilObject.transform.localPosition = new Vector3(-0.01f, 0.0500000007f, -0.50999999f);
            }

            sampleObject.transform.Find("default").GetComponent<Renderer>().material = mat;
            foilObject.transform.Find("default").GetComponent<Renderer>().material = mat;

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

            // Unused for now: Sample outside viewport: Vector3(-8.56000042,0.0500000007,-0.50999999), Foil: Vector3(8.05000019,0.0500000007,-0.50999999)
        }
        else
        {
            Debug.LogError($"Could not find row with Participant ID {participantId} and Trial Number {trialNumber}");
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
