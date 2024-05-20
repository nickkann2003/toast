//using log4net.Util;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Dial))]
[CanEditMultipleObjects]
public class DialManipulatorTool : Editor
{
    public void OnSceneGUI()
    {
        Dial dial = (Dial)target;

        Transform transform = dial.transform;

        Handles.DrawWireDisc(transform.position, transform.forward, transform.lossyScale.y * .12f);

        // ADD THIS CALC TO THE DIAL CLASS
        Quaternion minRotation = Quaternion.AngleAxis(-dial.maxRotation, transform.parent.forward);
        Vector3 min = minRotation * transform.parent.up;
        Quaternion maxRotation = Quaternion.AngleAxis(dial.maxRotation, transform.parent.forward);
        Vector3 max = maxRotation * transform.parent.up;


        Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.15f);
        Handles.DrawSolidArc(transform.position, transform.forward, min, dial.maxRotation * 2, transform.lossyScale.y * .12f);

        for (int i = 0; i < 3; i++)
        {
            // some logic up here

            EditorGUI.BeginChangeCheck();

            // some logic here

            if (EditorGUI.EndChangeCheck())
            {
                // store the obj before the change was made so that you can "undo" the change
                Undo.RecordObject(dial, "Moved dial point");
                //dial.SetPoint(i, point);
            }

        }
    }
}
