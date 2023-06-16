using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ResultsSummary : MonoBehaviour
{
    // Assuming you have a TextMeshProUGUI component on the same GameObject
    public TextMeshProUGUI csvOutputText;

    public void LoadCsv()
    {
        // File path
        string filePath = Path.Combine(Application.persistentDataPath, "Results", PlayerPrefs.GetString("participantId")+".csv");

        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV file not found at " + filePath);
            return;
        }

        // Read all lines of the CSV file
        string[] lines = File.ReadAllLines(filePath);

        // Get the headers (Participant ID, Trial Number, Response, Correctness, Start Timestamp, End Timestamp)
        string[] headers = lines[0].Split(',');

        // Create a table to hold CSV data
        List<string> csvTable = new List<string>();

        // Loop through each row in the CSV file
        for (int i = 1; i < lines.Length; i++)
        {
            // Split the row into its values
            string[] values = lines[i].Split(',');

            // Transform the "Correctness" and "Timestamp" values and construct the row string
            string correctness = values[3] == "true" ? "Correct" : "Incorrect";

            DateTime startTimestamp = DateTime.Parse(values[4]);
            DateTime endTimestamp = DateTime.Parse(values[5]);
            TimeSpan reactionTime = endTimestamp - startTimestamp;

            string row = string.Format("{0}, {1}, {2}, {3}",
                values[0], // Participant ID
                values[1], // Trial Number
                correctness,
                reactionTime.TotalSeconds // Reaction Time in seconds
            );

            // Replace commas with tabulations
            row = row.Replace(",", "\t");

            // Add the row string to the table
            csvTable.Add(row);
        }

        // Output the table to the TextMeshProUGUI component, separating rows by newlines
        csvOutputText.text = string.Join("\n", csvTable);
    }

    // Call LoadCsv when the script starts (or call it whenever you want to load the CSV)
    private void Start()
    {
        LoadCsv();
    }
}
