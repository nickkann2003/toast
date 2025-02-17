using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exp_DisplayController : MonoBehaviour
{
    [SerializeField]
    List<DisplayItem> displayItems = new List<DisplayItem>();

    private void Start()
    {
        DisableAll();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(DisplayItem item in displayItems)
        {
            if (Input.GetKeyDown(item.keyCode))
            {
                DisableAll();
                item.objectRef.SetActive(true);
            }
        }
    }

    private void DisableAll()
    {
        foreach (DisplayItem item in displayItems)
        {
            item.objectRef.SetActive(false);
        }
    }
}

[System.Serializable]
public class DisplayItem
{
    [SerializeField]
    public GameObject objectRef;
    [SerializeField]
    public KeyCode keyCode;

}
