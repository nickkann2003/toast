using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyClicker : MonoBehaviour
{
    private Highlights highlight;
    public StickyHelper currentHelper;

    private void OnMouseDown()
    {
        currentHelper.SendToObjectivePaper();
    }
}
