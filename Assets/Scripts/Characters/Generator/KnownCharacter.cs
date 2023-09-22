using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class KnownCharacter
{
    /// <summary>
    /// To get link to character in save
    /// </summary>
    public string ReferenceName;
    /// <summary>
    /// We may not know the name, or know the wrong one like pseudonym
    /// </summary>
    public string KnownName;
    /// <summary>
    /// From -100 to 100
    /// </summary>
    public int ConnectedExperience;
    /// <summary>
    /// Grows by encounters with that character or other events that relate to that character.
    /// Changes from 0 to 100
    /// </summary>
    public int Importance;
    /// <summary>
    /// Last location where we mentioned the character
    /// </summary>
    public string LastKnownLocation;

    private LocationReference _lastKnownLocation;

    public bool OnGUI(ref bool show)
    {
#if UNITY_EDITOR
        show = EditorGUILayout.Foldout(show, ReferenceName);
        if(show)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            KnownName = EditorGUILayout.TextField("Known name: ", KnownName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Connected experience: ", GUILayout.Width(200));
            ConnectedExperience = EditorGUILayout.IntSlider(ConnectedExperience, -100,100);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Importance: ", GUILayout.Width(200));
            Importance = EditorGUILayout.IntSlider(Importance, 0, 100);
            EditorGUILayout.EndHorizontal();

            if(_lastKnownLocation == null)
            {
                _lastKnownLocation = AssetDatabase.LoadAssetAtPath<LocationReference>(string.Format("Assets/Resources/CharactersGenerator/LocationReferences/{0}.asset", LastKnownLocation));
            }
            EditorGUILayout.BeginHorizontal();
            if(_lastKnownLocation != null)
            {
                _lastKnownLocation = (LocationReference)EditorGUILayout.ObjectField("Last known location:", _lastKnownLocation, typeof(LocationReference), true);
            }
            else
            {
                _lastKnownLocation = (LocationReference)EditorGUILayout.ObjectField("Last known location:", _lastKnownLocation, typeof(LocationReference), true, GUILayout.Width(350));
                EditorGUILayout.HelpBox("Location unknown", MessageType.None, true);
            }
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                LastKnownLocation = _lastKnownLocation.name;
                return true;
            }
            EditorGUI.indentLevel--;
        }
#endif
        return false;
    }
}
