using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MixedArchetype))]
public class MixedArchetypeEditor : Editor
{
    MixedArchetype _target;
    SerializedProperty _personalityValues;
    SerializedProperty _habitats;

    private void OnEnable()
    {
        _target = (MixedArchetype)target;
        _personalityValues = serializedObject.FindProperty("PersonalityValues");
        _habitats = serializedObject.FindProperty("Habitats");
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Dependencies:", EditorStyles.boldLabel);
        int i = 0;
        foreach (var item in MixedArchetype.BasicArchetypes)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(item, GUILayout.Width(170));
            _target.Dependency[i] = EditorGUILayout.IntSlider(_target.Dependency[i], 0, 100);
            GUILayout.EndHorizontal();
            i++;
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_personalityValues);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_habitats);
    }
}
