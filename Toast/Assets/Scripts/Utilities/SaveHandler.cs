using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    [Header("Path Variables")]
    [SerializeField]
    private string basePath = "text.txt";
    private string path = "";

    [SerializeField, Button]
    private void TestSave() {
        //SetPath(); 
        //AppendLine("TESTING TESTING!"); 
        StreamWriter wr = new StreamWriter("Assets/Resources/text.txt");
        wr.WriteLine("Editor Test");
        wr.Close();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetPath();

        // Read in file, if its first line is not 'toasty', delete the file and start again
        StreamReader sr = new StreamReader(path);
        string firstLine = sr.ReadLine();
        sr.Close();

        if (firstLine != "toasty")
        {
            StreamWriter fileCreator = new StreamWriter(path, false);
            fileCreator.WriteLine("toasty");
            fileCreator.Close();
        }

        AppendLine("Test Line");
        AppendString(" AND a test string");
    }

    private void AppendLine(string output)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(output);
        writer.Close();
    }

    private void AppendString(string output)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.Write(output);
        writer.Close();
    }

    private void SetPath()
    {
        path = Application.persistentDataPath + "/" + basePath;

#if UNITY_EDITOR
        path = "Assets/Resources/" + basePath;
        Debug.Log("In Editor, changing read/write path to " + path);
#endif
    }

    private void SerializeObjectives()
    {
        // Read in the file and get the current ID of objectives
        string objSerialPath = "Assets/Resources/objs.txt";
        StreamReader sr = new StreamReader(objSerialPath);
        string firstLine = sr.ReadLine();
        int cId = int.Parse(firstLine);
        sr.Close();

        // Write out the new cID of objectives
        StreamWriter wr = new StreamWriter(path);
        wr.Write(cId);
        wr.Close();
    }
}
