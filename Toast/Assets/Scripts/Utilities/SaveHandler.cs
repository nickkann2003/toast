using NaughtyAttributes;
using System;
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

    private int saveFileSections = 6;
    private int saveFileNameLocation = 0;
    private int objectiveDataLocation = 1;
    private int achievementDataLocation = 2;

    private string saveFileBaseFormat = "";

    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI SaveFile1;
    [SerializeField]
    private TextMeshProUGUI SaveFile2;
    [SerializeField]
    private TextMeshProUGUI SaveFile3;

    [SerializeField]
    private TextMeshProUGUI DEBUGOUTPUT;

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
        currentSaveFile = -1;
        
        if (!File.Exists(path))
        {
            DEBUGOUTPUT.text += "Save file not found, creating save file";  //DEBUG
            File.CreateText(path);
        }
        DEBUGOUTPUT.text += "File found at path, checking file info";  //DEBUG


        StreamReader sr = new StreamReader(path);
        string allDat = sr.ReadToEnd();
        sr.Close();
        DEBUGOUTPUT.text += allDat + "\n";  //DEBUG
        if (allDat == "")
        {
            DEBUGOUTPUT.text += "Save file found empty, creating formatted save file";  //DEBUG
            CreateFormattedSaveFile();
        }
        DEBUGOUTPUT.text += "Formatted save file located, verifying format";  //DEBUG

        string[] datSplit = allDat.Split(saveFileParser);
        if (!(datSplit.Length > 1))
        {
            DEBUGOUTPUT.text += "Save files not formatted correctly, rewriting data";  //DEBUG
            CreateFormattedSaveFile();
        }
        else
        {
            if (!(datSplit[0].Split(fileDataParser).Length > 1))
            {
                DEBUGOUTPUT.text += "Save file data not formatted correctly, rewriting data";  //DEBUG
                CreateFormattedSaveFile();
            }
        }
        DEBUGOUTPUT.text += "Save file format verified, loading files"; //DEBUG

        currentSaveFile = -1;
        SetFileDisplayNames();

        saveFileBaseFormat = "";
        for (int i = 0; i < numSaveFiles; i++)
        {
            for (int j = 0; j < saveFileSections; j++)
            {
                saveFileBaseFormat += fileDataParser;
            }
            saveFileBaseFormat += saveFileParser;
        }
    }

    /// <summary>
    /// Collects and saves all data for the current save file
    /// </summary>
    public void SaveAllData()
    {
        if(currentSaveFile != -1)
        {
            string objectiveData = ObjectiveManager.instance.GetObjectiveStorageString();
            string achievementData = AchievementManager.instance.GetAchievementSaveString();

            SaveObjectiveData(objectiveData);
            SaveAchievementData(achievementData);
        }
    }

    /// <summary>
    /// Performs all functions for loading the currently selected save file
    /// </summary>
    public void LoadSaveFile()
    {
        DEBUGOUTPUT.text += "Loading save file";  //DEBUG
        if (currentSaveFileName.Equals("")){
            DEBUGOUTPUT.text += "Name empty, opening file naming menu";  //DEBUG
            UIManager.instance.CloseFileSelectMenu();
            UIManager.instance.OpenFileNamingMenu();
        }
        else
        {
            DEBUGOUTPUT.text += "Name not empty,";  //DEBUG
            UIManager.instance.CloseFileNamingMenu();
            UIManager.instance.CloseFileSelectMenu();
            GameManager.Instance.MainMenuToTutorial();

            ObjectiveManager.instance.LoadObjectives(ReadObjectiveData());
            AchievementManager.instance.LoadAchievementSaveString(ReadAchievementData());
        }
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
        currentSaveFileName = filename;
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
    /// Takes a given string of achievement data and saves it to the current file
    /// </summary>
    /// <param name="achievementData"></param>
    public void SaveAchievementData(string achievementData)
    {
        string allDat = GetCurrentFileInfo();
        string[] parsedDat = allDat.Split(fileDataParser);
        parsedDat[achievementDataLocation] = achievementData;
        SetCurrentFileInfo(ArrayToFileData(parsedDat));
    }

    /// <summary>
    /// Reads in and returns the achievement data for the current save file
    /// </summary>
    /// <returns>Objective data string for current save file</returns>
    public string ReadAchievementData()
    {
        string allDat = GetCurrentFileInfo();
        string[] parsedDat = allDat.Split(fileDataParser);

        return parsedDat[achievementDataLocation];
    }

    /// <summary>
    /// Sets the save file path based on editor or not
    /// </summary>
    private void SetPath()
    {
        path = Application.persistentDataPath;
        DEBUGOUTPUT.text += "PATH: " + path + "\n";  //DEBUG
        DEBUGOUTPUT.text += "Checking directory";  //DEBUG

        if (!Directory.Exists(path))
        {
            DEBUGOUTPUT.text += "Directory not found, creating directory";  //DEBUG
            Directory.CreateDirectory(path);
        }
        DEBUGOUTPUT.text += "Directory found, checking path";  //DEBUG
        path += "/" + basePath;

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
        currentSaveFileName = GetCurrentFileInfo().Split(fileDataParser)[0];
    }

    /// <summary>
    /// Returns the save string for the current save file
    /// </summary>
    /// <returns></returns>
    private string GetCurrentFileInfo()
    {
        DEBUGOUTPUT.text += "Creating Stream Reader";  //DEBUG
        StreamReader sr = new StreamReader(path);
        DEBUGOUTPUT.text += "Stream Reader CREATED";  //DEBUG
        string allDat = sr.ReadToEnd();
        DEBUGOUTPUT.text += "STREAM READER READ DATA";  //DEBUG
        sr.Close();
        DEBUGOUTPUT.text += "STREAM READER CLOSED";  //DEBUG

        string[] parsedDat = allDat.Split(saveFileParser);
        DEBUGOUTPUT.text += "STREAM READER parsed data";  //DEBUG
        if (parsedDat.Length > 1)
        {
            DEBUGOUTPUT.text += "Parsed data found, returning info\n";  //DEBUG
            return parsedDat[currentSaveFile];
        }
        else
        {
            DEBUGOUTPUT.text += "Parsed data no length, returning empty\n";  //DEBUG
            return "";
        }
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

    private void SetFileDisplayNames()
    {
        int cFileId = currentSaveFile;

        string file3Name;
        SetCurrentSaveFileByID(2);
        DEBUGOUTPUT.text += "PARSING SAVE FILE NAME";  //DEBUG
        file3Name = GetCurrentFileInfo().Split(fileDataParser)[saveFileNameLocation];
        DEBUGOUTPUT.text += "SAVE FILE NAME SUCCESSFULLY PARSED\n";  //DEBUG

        string file2Name;
        SetCurrentSaveFileByID(1);
        file2Name = GetCurrentFileInfo().Split(fileDataParser)[saveFileNameLocation];

        string file1Name;
        SetCurrentSaveFileByID(0);
        file1Name = GetCurrentFileInfo().Split(fileDataParser)[saveFileNameLocation];

        SaveFile1.text = file1Name.Equals("") ? "NEW SAVE" : file1Name;
        SaveFile2.text = file2Name.Equals("") ? "NEW SAVE" : file2Name;
        SaveFile3.text = file3Name.Equals("") ? "NEW SAVE" : file3Name;

        currentSaveFile = cFileId;
    }

    /// <summary>
    /// Deletes all information for a given save file
    /// </summary>
    /// <param name="fileId">File ID to be replaced</param>
    public void DeleteAllFileData(int fileId)
    {
        SetCurrentSaveFileByID(fileId);
        SetCurrentFileInfo(saveFileBaseFormat);
        SetFileDisplayNames();

        currentSaveFile = -1;
    }
}
