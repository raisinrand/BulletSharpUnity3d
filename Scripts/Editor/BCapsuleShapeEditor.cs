﻿using BulletUnity;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(BCapsuleShape))]
public class BCapsuleShapeEditor : Editor
{

    BCapsuleShape script;
    SerializedProperty radius;
    SerializedProperty height;
    SerializedProperty upAxis;

    void OnEnable()
    {
        script = (BCapsuleShape)target;
        //GetSerializedProperties();
    }

    /*
	void GetSerializedProperties() {
		radius = serializedObject.FindProperty("radius");
		height = serializedObject.FindProperty("height");
        upAxis = serializedObject.FindProperty("upAxis");
	}
    */

    public override void OnInspectorGUI()
    {
        script.drawGizmo = EditorGUILayout.Toggle("Draw Shape", script.drawGizmo);
        script.Radius = EditorGUILayout.FloatField("Radius", script.Radius);
        script.Height = EditorGUILayout.FloatField("Height", script.Height);
        script.UpAxis = (BCapsuleShape.CapsuleAxis)EditorGUILayout.EnumPopup("Up Axis", script.UpAxis);
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
