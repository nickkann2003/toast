using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

[EditorTool("Dial Manipulator Tool", typeof(Dial))]
public class DialManipulatorTool : EditorTool
{
    // checks which window we are currently in
    public override void OnToolGUI(EditorWindow window)
    {
        // only continue if we are in scene view
        if (!(window is SceneView))
            return;

        foreach(var obj in targets)
        {
            // check to see if the obj is a dial
            if (!(obj is Dial dial))
                continue;

            Transform transform = dial.transform;
            
            Handles.DrawWireDisc(transform.position, transform.forward, transform.lossyScale.y  * .5f);

            for (int i = 0; i < 3; i ++)
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
}
