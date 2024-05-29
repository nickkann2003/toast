using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class SaveHandler : MonoBehaviour
{
    // ------------------------------- Variables -------------------------------
    [Header("Singleton Variables")]
    public static SaveHandler instance;

    [Header("Path Variables")]
    [SerializeField]
    private string basePath = "text.txt";
    private string path = "";
    private string objValuePath = "Assets/Resources/obj.txt";

    [Header("Save Variables")]
    [SerializeField]
    public int numSaveFiles = 3;

    private int currentSaveFile = 0;
    private string currentSaveFileName = string.Empty;

    private string saveFileParser = "\n";
    private string fileDataParser = "|";

    private int saveFileSections = 2;
    private int saveFileNameLocation = 0;
    private int objectiveDataLocation = 1;

    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI SaveFile1;
    [SerializeField]
    private TextMeshProUGUI SaveFile2;
    [SerializeField]
    private TextMeshProUGUI SaveFile3;

    // ------------------------------- Buttons -------------------------------
    [SerializeField, Button]
    private void CreateSaveFile() { CreateFormattedSaveFile(); }
    [SerializeField, Button]
    private void SaveAllGameData() { SaveAllData(); }

    // ------------------------------- Functions -------------------------------
    private void Awake()
    {
        instance = this;
        SetPath();
    }

    private void Start()
    {
        string file3Name;
        SetCurrentSaveFileByID(2);
        file3Name = GetCurrentFileInfo().Split(fileDataParser)[saveFileNameLocation];

        string file2Name;
        SetCurrentSaveFileByID(1);
        file2Name = GetCurrentFileInfo().Split(fileDataParser)[saveFileNameLocation];

        string file1Name;
        SetCurrentSaveFileByID(0);
        file1Name = GetCurrentFileInfo().Split(fileDataParser)[saveFileNameLocation];
        
        SaveFile1.text = file1Name == "" ? "NEW SAVE" : file1Name;
        SaveFile2.text = file2Name == "" ? "NEW SAVE" : file2Name;
        SaveFile3.text = file3Name == "" ? "NEW SAVE" : file3Name;
    }

    /// <summary>
    /// Collects and saves all data for the current save file
    /// </summary>
    public void SaveAllData()
    {
        string objectiveData = ObjectiveManager.instance.GetObjectiveStorageString();

        SaveObjectiveData(objectiveData);
    }

    /// <summary>
    /// Creates an empty file with the format for saving information set up
    /// </summary>
    public void CreateFormattedSaveFile()
    {
        string formattedEmptyFileString = "";
        for (int i = 0; i < numSaveFiles; i++)
        {
            for (int j = 0; j < saveFileSections; j++)
            {
                formattedEmptyFileString += fileDataParser;
            }
            formattedEmptyFileString += saveFileParser;
        }
        StreamWriter writer = new StreamWriter(path);
        writer.Write(formattedEmptyFileString);
        writer.Close();
    }

    public void GetCurrentSaveFileStringFromUI(TextMeshProUGUI uiElement)
    {
        SetSaveFileName(uiElement.text);
    }

    /// <summary>
    /// Sets the current save files name to a given string
    /// </summary>
    /// <param name="filename">New file name</param>
    public void SetSaveFileName(string filename)
    {
        string cData = GetCurrentFileInfo();
        string[] parsedDat = cData.Split(fileDataParser);
        parsedDat[saveFileNameLocation] = filename;
        SetCurrentFileInfo(ArrayToFileData(parsedDat));
    }

    /// <summary>
    /// Takes a given string of objective data and saves it to the current save file
    /// </summary>
    /// <param name="objDat">Data to save</param>
    public void SaveObjectiveData(string objDat)
    {
        string allDat = GetCurrentFileInfo();
        string[] parsedDat = allDat.Split(fileDataParser);
        parsedDat[objectiveDataLocation] = objDat;
        SetCurrentFileInfo(ArrayToFileData(parsedDat));
    }

    /// <summary>
    /// Reads in and returns the objective data for the current save file
    /// </summary>
    /// <returns>Objective data string for current save file</returns>
    public string ReadObjectiveData()
    {
        string allDat = GetCurrentFileInfo();
        string[] parsedDat = allDat.Split(fileDataParser);

        return parsedDat[objectiveDataLocation];
    }

    /// <summary>
    /// Sets the save file path based on editor or not
    /// </summary>
    private void SetPath()
    {
        path = Application.persistentDataPath + "/" + basePath;

#if UNITY_EDITOR
        path = "Assets/Resources/" + basePath;
        Debug.Log("In Editor, changing read/write path to " + path);
#endif
    }

    /// <summary>
    /// Sets the current save file by ID
    /// </summary>
    /// <param name="fileNum">ID of file to change to</param>
    public void SetCurrentSaveFileByID(int fileNum)
    {
        currentSaveFile = fileNum;
    }

    /// <summary>
    /// Returns the save string for the current save file
    /// </summary>
    /// <returns></returns>
    private string GetCurrentFileInfo()
    {
        StreamReader sr = new StreamReader(path);
        string allDat = sr.ReadToEnd();
        sr.Close();

        string[] parsedDat = allDat.Split(saveFileParser);

        return parsedDat[currentSaveFile];
    }

    /// <summary>
    /// Sets the current save file data to a given string
    /// </summary>
    /// <param name="newSaveData">Save string to set file data to</param>
    private void SetCurrentFileInfo(string newSaveData)
    {
        StreamReader sr = new StreamReader(path);
        string allDat = sr.ReadToEnd();
        sr.Close();

        string[] parsedDat = allDat.Split(saveFileParser);

        parsedDat[currentSaveFile] = newSaveData;

        allDat = ArrayToSaveData(parsedDat);

        StreamWriter wr = new StreamWriter(path);
        wr.Write(allDat);
        wr.Close();
    }

    /// <summary>
    /// Turns an array of file data into a formatted file string
    /// </summary>
    /// <param name="data">Singular save file data array</param>
    /// <returns>Formatted save string</returns>
    private string ArrayToFileData(string[] data)
    {
        string newDat = string.Empty;
        foreach (string dat in data)
        {
            newDat += dat;
            newDat += fileDataParser;
        }
        newDat = newDat.Substring(0, newDat.Length - 1);
        return newDat;
    }

    /// <summary>
    /// Turns an array of all saved files into a formatted save string
    /// </summary>
    /// <param name="data">All saved files as an array</param>
    /// <returns>Formatted save string</returns>
    private string ArrayToSaveData(string[] data)
    {
        string allDat = string.Empty;
        foreach (string file in data)
        {
            allDat += file;
            allDat += saveFileParser;
        }
        allDat = allDat.Substring(0, allDat.Length - 1);
        return allDat;
    }
}
