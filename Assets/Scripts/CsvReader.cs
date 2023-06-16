using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class CsvReader : MonoBehaviour
{
    public static CsvReader instance;
    public string csvFileName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static string RemoveNonDigits(string input)
    {
        string result = "";
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                result += c;
            }
        }
        return result;
    }

    public Dictionary<string, string> ReadCsvRow(string participantId, string trialNumber)
    {
        Dictionary<string, string> rowData = null;

        TextAsset asset = Resources.Load<TextAsset>(csvFileName);
        if (asset == null)
        {
            Debug.LogError("CSV File not found in Resources folder");
            return null;
        }

        string[] lines = asset.text.Split('\n');

        string[] headers = lines[0].Split(',');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (RemoveNonDigits(values[0]) == RemoveNonDigits(participantId) && RemoveNonDigits(values[1]) == RemoveNonDigits(trialNumber))
            {
                rowData = new Dictionary<string, string>();

                for (int j = 2; j < headers.Length; j++)
                {
                    string header = headers[j];
                    string value = values[j];
                    rowData.Add(header, value);
                }

                break;
            }
        }

        return rowData;
    }

    public int GetRowCount(string participantId)
    {
        TextAsset asset = Resources.Load<TextAsset>(csvFileName);
        if (asset == null)
        {
            Debug.LogError("CSV File not found in Resources folder");
            return 0;
        }

        string[] lines = asset.text.Split('\n');

        int rowCount = 0;

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            // Check if the row's Participant ID matches the input Participant ID
            if (RemoveNonDigits(values[0]) == RemoveNonDigits(participantId))
            {
                rowCount++;
            }
        }

        return rowCount;
    }


}
