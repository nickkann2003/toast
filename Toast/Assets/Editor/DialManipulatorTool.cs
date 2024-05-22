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

        Vector3 rotAxis = dial.rotateAround;

        float scale = transform.lossyScale.y * .12f;

        Handles.DrawWireDisc(transform.position, rotAxis, scale);

        // ADD THIS CALC TO THE DIAL CLASS
        Quaternion minRotation = Quaternion.AngleAxis(-dial.maxRotation, rotAxis);
        Vector3 min = minRotation * transform.parent.up;
        Quaternion maxRotation = Quaternion.AngleAxis(dial.maxRotation, rotAxis);
        Vector3 max = maxRotation * transform.parent.up;


        Handles.color = new Color(1.0f, 1.0f, 1.0f, 0.15f);
        Handles.DrawSolidArc(transform.position, rotAxis, min, dial.maxRotation * 2, scale);

        for (int i = 0; i < dial.numSnapPoints; i++)
        {
            // some logic up here
            Handles.color = Color.blue;
            float angle = dial.maxRotation * 2 / (dial.numSnapPoints - 1);
            Quaternion handleQuat = Quaternion.AngleAxis(-dial.maxRotation + angle * i, rotAxis);
            Vector3 handleRot = handleQuat * transform.parent.up;
            Handles.DrawLine(transform.position, transform.position + handleRot * scale);

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
