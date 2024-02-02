using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif // UNITY_EDITOR

public class Button : MonoBehaviour
{
    [SerializeField] private UnityEvent buttonTrigger;

    // position in world
    public GameObject maxHeight;
    public GameObject minHieght;

    public Vector3 testMaxHeight;

    // amount that the button has moved towards min height
    private float interpolateAmount;

    // timer
    public float timer;
    public float maxTime;

    public enum Trigger
    {
        onDown,
        onUp,
        onHold
    }

    public Trigger trigger;
    private bool pressed;


    private void Start()
    {
        timer = maxTime;
    }

    private void Update()
    {
        if (pressed)
        {
            Interact();
        }
        else
        {
            Depress();
        }
    }

    private void OnMouseDown()
    {
        Interact();
    }

    private void OnMouseUp()
    {
        pressed = false;
        if (trigger == Trigger.onUp)
        {
            Activate();
        }
        timer = maxTime;
    }

    void Interact()
    {
        Press();
        switch (trigger)
        {
            case Trigger.onDown:
                if (!pressed)
                {
                    Activate();
                }
                break;
            case Trigger.onHold:
                Activate();
                break;
        }
        pressed = true;
        AudioManager.instance.PlaySound(AudioManager.instance.physicalButton);
    }

    void Activate()
    {
        if (timer >= maxTime)
        {
            buttonTrigger.Invoke();
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    // pushes button down
    void Press()
    {
        if (interpolateAmount >= 0 && interpolateAmount < 1)
        {
            interpolateAmount += Time.deltaTime * 50;
        }
        else
        {
            interpolateAmount = 1;
        }
  
        transform.position = Vector3.Lerp(maxHeight.transform.position, minHieght.transform.position, interpolateAmount);
    }

    void Depress()
    {
        if (interpolateAmount > 0 && interpolateAmount <= 1)
        {
            interpolateAmount -= Time.deltaTime * 50;
        }
        else
        {
            interpolateAmount = 0;
        }

        transform.position = Vector3.Lerp(maxHeight.transform.position, minHieght.transform.position, interpolateAmount);
    }
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(Button))]
//public class ButtonEditor : Editor
//{
//    public void OnSceneGUI()
//    {
//        Button button = target as Button;

//        Transform parentTransform = button.transform.parent.transform;

//        EditorGUI.BeginChangeCheck();

//        Vector3 newMaxHeight = Handles.PositionHandle(parentTransform.TransformPoint(button.testMaxHeight), Quaternion.identity);
//        Handles.DrawWireCube(parentTransform.TransformPoint(button.testMaxHeight), new Vector3(.1f, .1f, .1f));

//        if (EditorGUI.EndChangeCheck())
//        {
//            Undo.RecordObject(target, "Update Max Height");
//            button.testMaxHeight = parentTransform.InverseTransformPoint(newMaxHeight);
//        }
//    }
//}
//#endif // UNITY_EDITOR