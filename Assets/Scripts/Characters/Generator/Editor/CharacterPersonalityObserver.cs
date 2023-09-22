using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

public class CharacterPersonalityObserver : EditorWindow
{
    private bool _initialized;

    CharactePersonalitiesList _personalitiesList;
    private CharacterPersonality _selectedPersonality;
    private PersonalityValue[] _personalityValues;
    private LocationReference _currentLocation;

    [MenuItem("Tools/Character personality observer")]
    public static void ShowWindow()
    {
        GetWindow<CharacterPersonalityObserver>("Character personality observer").Init();

    }

    private void OnGUI()
    {
        if (!_initialized) Init();

        //Select personality
        EditorGUI.BeginChangeCheck();
        int index = EditorGUILayout.Popup(_personalitiesList.SelectedIndex, _personalitiesList.SavedPersonalities.ToArray());
        if (EditorGUI.EndChangeCheck())
            SelectPersonality(index);

        DisplayPersonality();
        if(_personalitiesList.Length > 1)
        {
            if (GUILayout.Button("Delete"))
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to delete this personality data?", "Yes", "No"))
                {
                    DeletePersonalitySave();
                }
            }
        }
    }
    private void DisplayPersonality()
    {
        DisplayName();
        DisplayArchetypeDependencies();
        DisplayPersonalityValues();
        DisplayCurrentLocation();
        DisplayKnownCharaters();
    }
    private void DisplayName()
    {
        EditorGUI.BeginChangeCheck();
        _selectedPersonality.Name = EditorGUILayout.TextField("Name", _selectedPersonality.Name);
        if (EditorGUI.EndChangeCheck())
            _personalitiesList.SavePersonality(_selectedPersonality);
    }
    bool displayArchetypeDependencies;
    private void DisplayArchetypeDependencies()
    {
        if (_selectedPersonality.ArchetypeDependenciesNames == null) return;
        displayArchetypeDependencies = EditorGUILayout.Foldout(displayArchetypeDependencies, "Personality archetype dependencies", EditorStyles.foldoutHeader);
        if (displayArchetypeDependencies)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < _selectedPersonality.ArchetypeDependenciesNames.Length; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(_selectedPersonality.ArchetypeDependenciesNames[i], GUILayout.Width(70));
                _selectedPersonality.ArchetypeDependenciesValues[i] = EditorGUILayout.IntSlider(_selectedPersonality.ArchetypeDependenciesValues[i], 0, 100);
                GUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck())
            {
                _personalitiesList.SavePersonality(_selectedPersonality);
            }
            EditorGUI.indentLevel--;
        }
    }
    bool displayValues;
    private void DisplayPersonalityValues()
    {
        displayValues = EditorGUILayout.Foldout(displayValues,"Personality values", EditorStyles.foldoutHeader);
        if(displayValues)
        {
            EditorGUI.indentLevel++;
            int validAmount = 0;
            for (int i = 0; i < _personalityValues.Length; i++)
            {
                if (_personalityValues[i] != null)
                {
                    GUILayout.BeginHorizontal();
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField(_personalityValues[i].name, _personalityValues[i], typeof(PersonalityValue), true);
                    GUI.enabled = true;
                    EditorGUI.BeginChangeCheck();
                    _selectedPersonality.PersonalityValuesCurrentValues[i] = EditorGUILayout.IntSlider(_selectedPersonality.PersonalityValuesCurrentValues[i], 0, 100);
                    if (EditorGUI.EndChangeCheck())
                    {
                        _personalitiesList.SavePersonality(_selectedPersonality);
                    }
                    GUILayout.EndHorizontal();
                    validAmount++;
                }
                else
                {
                    EditorGUILayout.HelpBox(string.Format("MISSING:{0}; Add Personality Value with such name to the project.", _selectedPersonality.PersonalityValuesNames[i]), MessageType.Error, true);
                }
                EditorGUI.indentLevel--;
            }
            if (validAmount == 0) EditorGUILayout.HelpBox("No values or missing references", MessageType.Warning);
        }
    }
    private void DisplayCurrentLocation()
    {
        GUILayout.Space(10);
        GUI.enabled = false;
        if (_currentLocation != null)
        {
            EditorGUILayout.ObjectField("Current location", _currentLocation, typeof(PersonalityValue), true);
        }
        else
        {
            EditorGUILayout.HelpBox(string.Format("MISSING:{0}; Add Location Reference with such name to the project.", _selectedPersonality.CurrentLocation), MessageType.Error, true);
        }
        GUI.enabled = true;
    }
    bool displayKnownCharacters;
    bool[] displaySpecificKnownCharacter;
    private void DisplayKnownCharaters()
    {
        if(_selectedPersonality.KnownCharacters != null)
        {
            displayKnownCharacters = EditorGUILayout.Foldout(displayKnownCharacters, "Known characters", EditorStyles.foldoutHeader);
            if(displayKnownCharacters)
            {
                EditorGUI.indentLevel++;
                if(_selectedPersonality.KnownCharacters.Length > 0)
                {
                    for (int i = 0; i < _selectedPersonality.KnownCharacters.Length; i++)
                    {
                        if (_selectedPersonality.KnownCharacters[i].OnGUI(ref displaySpecificKnownCharacter[i]))
                            _personalitiesList.SavePersonality(_selectedPersonality);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("No known characters", MessageType.Info);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
    private void Init()
    {
        _initialized = true;

        _personalitiesList = CharactePersonalitiesList.Load();
        SelectPersonality(0);
        CharactePersonalitiesList.WasUpdated += PersonalitiesListUpdateListener;
    }
    private void OnDestroy()
    {
        CharactePersonalitiesList.WasUpdated -= PersonalitiesListUpdateListener;
    }

    private void PersonalitiesListUpdateListener(object sender)
    {
        if(!ReferenceEquals(sender, this))
        {
            int selectedPersonality = _personalitiesList.SelectedIndex;
            _personalitiesList = CharactePersonalitiesList.Load();
            SelectPersonality(selectedPersonality);
        }
    }

    private void SelectPersonality(int index)
    {
        _personalitiesList.SelectedIndex = index;
        _selectedPersonality = _personalitiesList.LoadSelectedPersonality();
        if(_selectedPersonality.PersonalityValuesNames != null)
        {
            _personalityValues = new PersonalityValue[_selectedPersonality.PersonalityValuesNames.Length];
            string path;
            for (int i = 0; i < _personalityValues.Length; i++)
            {
                path = string.Format("Assets/Resources/CharactersGenerator/{0}.asset", _selectedPersonality.PersonalityValuesNames[i]);
                if(!AssetDatabase.AssetPathExists(path))
                    Debug.LogWarning(string.Format("Asset at path: {0} is missing!", path));
                else
                {
                    _personalityValues[i] = AssetDatabase.LoadAssetAtPath<PersonalityValue>(path);
                }
            }
        }
        else
        {
            _personalityValues = new PersonalityValue[0];
        }
        _currentLocation = AssetDatabase.LoadAssetAtPath<LocationReference>(string.Format("Assets/Resources/CharactersGenerator/LocationReferences/{0}.asset", _selectedPersonality.CurrentLocation));
        displaySpecificKnownCharacter = new bool[_selectedPersonality.KnownCharacters.Length];
    }
    private void DeletePersonalitySave()
    {
        _personalitiesList.DeleteSelectedPersonality();
        if (_personalitiesList.Length > 0)
            SelectPersonality(0);
    }
}
