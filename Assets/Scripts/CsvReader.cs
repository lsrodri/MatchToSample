using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvReader : MonoBehaviour
{
    public static CsvReader instance; // Static instance variable to make the class a singleton
    public string csvFileName; // The name of the CSV file to read from

    // Called when the script instance is being loaded
    private void Awake()
    {
        // Check if an instance of the class already exists in the scene
        if (instance == null)
        {
            // If not, set this instance as the static instance
            instance = this;
            // Prevent the object from being destroyed when a new scene is loaded
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this object to prevent duplicates
            Destroy(gameObject);
        }
    }

    // Reads the CSV file and returns a dictionary containing the data for a specific row
    public Dictionary<string, string> ReadCsvRow(string participantId, string trialNumber)
    {
        Dictionary<string, string> rowData = null;

        // Load the CSV file
        // string filePath = "../MatchToSample/Assets/Stimuli/" + csvFileName;
        
        string folderName = "Stimuli";
        string fileName = "match_to_sample_data.csv";
        string filePath = Path.Combine(Application.dataPath, folderName, fileName);

        // if (File.Exists(filePath))
        // {
        //     Debug.Log("File exists!");
        // }
        // else
        // {
        //     Debug.Log("File does not exist!");
        // }

        string[] lines = File.ReadAllLines(filePath);
        
        // Get the header row
        string[] headers = lines[0].Split(',');

        // Loop through each row in the CSV file
        for (int i = 1; i < lines.Length; i++)
        {
            // Split the row into its values
            string[] values = lines[i].Split(',');

            // Debug.Log("values[0]'"+values[0]+"'participantId"+"'"+participantId+"'"+"'values[1]"+values[1]+"'trialNumber"+"'"+trialNumber+"'");

            // Check if the row matches the participant ID and trial number
            if (values[0] == participantId && values[1] == trialNumber)
            {
                // Create a new dictionary to store the row data
                rowData = new Dictionary<string, string>();

                // Loop through the columns you're interested in and add them to the dictionary
                for (int j = 2; j < headers.Length; j++)
                {
                    string header = headers[j];
                    string value = values[j];
                    rowData.Add(header, value);
                }

                // Stop looping through the CSV file since we found the row we're interested in
                break;
            }
        }

        return rowData;
    }

}
