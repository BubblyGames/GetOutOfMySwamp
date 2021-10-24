using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Structure),true)]
public class StructureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Structure structureScript = (Structure) target;
        if (GUILayout.Button("Upgrade Structure"))
        {
            structureScript.UpgradeStrucrure();
        }
    }
}
