using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using BulletUnity;

[CustomEditor(typeof(BSphereShape))]
public class BSphereShapeEditor : Editor
{
    BSphereShape script;
    //SerializedProperty radius;

    void OnEnable()
    {
        script = (BSphereShape)target;
        //GetSerializedProperties();
    }

    /*
    void GetSerializedProperties()
    {
        radius = serializedObject.FindProperty("radius");
    }
    */

    public override void OnInspectorGUI()
    {
        script.drawGizmo = EditorGUILayout.Toggle("Draw Shape", script.drawGizmo);
        script.Radius = EditorGUILayout.FloatField("Radius", script.Radius);
        script.LocalScaling = EditorGUILayout.Vector3Field("Local Scaling", script.LocalScaling);
        script.Margin = EditorGUILayout.FloatField("Margin", script.Margin);
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(script);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Repaint();
        }
    }
}