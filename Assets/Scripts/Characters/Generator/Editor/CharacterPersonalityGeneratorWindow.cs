using Codice.CM.Client.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterPersonalityGeneratorWindow : EditorWindow
{
    private GeneratorPreset _generatorPreset;
    private SerializedObject _generatorPresetSO;
    private SerializedObject _selectedArchetypeSO;
    private SerializedProperty _selectedArchetypeValues;
    private SerializedProperty _selectedArchetypeHabitats;
    private bool _specifyCurrentLocation;
    private int _selectedLocation;

    private int _selectedArchetype;

    [MenuItem("Tools/Character personality generator")]
    public static void ShowWindow()
    {
        GetWindow<CharacterPersonalityGeneratorWindow>("Character personality generator").TryLoadParams();
        
    }

    private void OnGUI()
    {
        if(_generatorPreset != null)
        {
            DrawParams();
            DrawButtons();
        }
        else
        {
            EditorGUILayout.LabelField("Generator is broken. Reload it please...");
            if (GUILayout.Button("Reload"))
            {
                TryLoadParams();
            }
        }
    }
    private void GenerateNew()
    {
        var personalitiesList = CharactePersonalitiesList.Load();
        string specificLocation = null;
        if (_specifyCurrentLocation && CurrentLocationIsValid())
        {
            specificLocation = _generatorPreset.Archetypes[_selectedArchetype].Habitats[_selectedLocation].name;
        }
        var personality = PersonalityGenerator.GenerateNew(_generatorPreset, _generatorPreset.Archetypes[_selectedArchetype], personalitiesList, specificLocation);

        personalitiesList.AddNewPersonality(personality);
        Debug.Log(string.Format("New personality {0} generated.", name));
        if(CharactePersonalitiesList.WasUpdated != null)
            CharactePersonalitiesList.WasUpdated(this);
    }
    private void TryLoadParams()
    {
        string[] guids = AssetDatabase.FindAssets("t:GeneratorPreset");
        if (guids.Length > 0)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            _generatorPreset = AssetDatabase.LoadAssetAtPath<GeneratorPreset>(assetPath);
            _generatorPresetSO = new SerializedObject(_generatorPreset);
        }
        else
        {
            _generatorPreset = ScriptableObject.CreateInstance<GeneratorPreset>();
            _generatorPresetSO = new SerializedObject(_generatorPreset);
            if (!Directory.Exists("Assets/Resources/GeneratorPresets"))
            {
                Directory.CreateDirectory("Assets/Resources/GeneratorPresets/");
                AssetDatabase.Refresh();
            }
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/GeneratorPresets/CharacterPersonalityGeneratorPreset.asset");
            AssetDatabase.CreateAsset(_generatorPreset, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    private void DrawButtons()
    {
        EditorGUILayout.Space(15);
        if (GUILayout.Button("Generate"))
        {
            GenerateNew();
        }
        if (GUILayout.Button("Add new archetype"))
        {
            if (EditorUtility.DisplayDialog("Are you sure?", "Do you want to create new archetype data?", "Yes", "No"))
            {
                AddNewArchetype();
            }
        }
    }
    private void DrawParams()
    {
        _generatorPreset.Names = (TextAsset)EditorGUILayout.ObjectField("Names", _generatorPreset.Names, typeof(TextAsset), false);
        DrawArchetype();
        DrawVariationsLevel();
        DrawCurrentLocationSpecificator();
        DrawKnownCharactersSettings();
    }

    private bool showArchetypeSettings;
    private void DrawArchetype()
    {
        if (_generatorPreset.Archetypes.Count > 0)
        {
            GUILayout.BeginHorizontal();

            showArchetypeSettings = EditorGUILayout.Foldout(showArchetypeSettings, "Selected archetype:", EditorStyles.foldoutHeader);

            EditorGUI.BeginChangeCheck();
            _selectedArchetype = EditorGUILayout.Popup(_selectedArchetype, _generatorPreset.GetArchetypesNames(), GUILayout.Width(170));
            if (EditorGUI.EndChangeCheck()) OnArchetypeSelected();

            if (GUILayout.Button("Show", GUILayout.Width(50)))
            {
                EditorGUIUtility.PingObject(_generatorPreset.Archetypes[_selectedArchetype]);
            }

            GUILayout.EndHorizontal();


            if (showArchetypeSettings)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginChangeCheck();
                for (int i = 0; i < _generatorPreset.Archetypes[_selectedArchetype].Dependency.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(MixedArchetype.BasicArchetypes[i], GUILayout.Width(70));
                    _generatorPreset.Archetypes[_selectedArchetype].Dependency[i] = EditorGUILayout.IntSlider(_generatorPreset.Archetypes[_selectedArchetype].Dependency[i], 0, 100);
                    GUILayout.EndHorizontal();
                }

                if (_selectedArchetypeSO == null) OnArchetypeSelected();
                EditorGUILayout.PropertyField(_selectedArchetypeValues, new GUIContent("Archetype Values"), true);
                EditorGUILayout.PropertyField(_selectedArchetypeHabitats, new GUIContent("Archetype Habitats"), true);

                if (EditorGUI.EndChangeCheck())
                {
                    _selectedArchetypeSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_generatorPreset.Archetypes[_selectedArchetype]);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                EditorGUI.indentLevel--;
            }
        }
    }
    private void DrawVariationsLevel()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Generated personalities variation", GUILayout.Width(240));
        _generatorPreset.GeneratedPersonalityVariationsRange = EditorGUILayout.IntSlider(_generatorPreset.GeneratedPersonalityVariationsRange, 0, 100);
        GUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            _generatorPresetSO.ApplyModifiedProperties();
        }
    }
    private void DrawCurrentLocationSpecificator()
    {
        GUILayout.BeginHorizontal();
        _specifyCurrentLocation = EditorGUILayout.ToggleLeft("Specify current location", _specifyCurrentLocation, GUILayout.Width(160));
        if (_specifyCurrentLocation)
        {
            if (_generatorPreset.Archetypes[_selectedArchetype].Habitats.Length > 0)
            {
                _selectedLocation = EditorGUILayout.Popup(_selectedLocation, _generatorPreset.Archetypes[_selectedArchetype].GetHabitatNames(), GUILayout.Width(170));
                if (GUILayout.Button("Show", GUILayout.Width(50)))
                {
                    EditorGUIUtility.PingObject(_generatorPreset.Archetypes[_selectedArchetype].Habitats[_selectedLocation]);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No habitats in selected archetype!", MessageType.Warning, true);
            }
        }
        GUILayout.EndHorizontal();
    }
    private bool showKnownCharacterSettings;
    private void DrawKnownCharactersSettings()
    {
        showKnownCharacterSettings = EditorGUILayout.Foldout(showKnownCharacterSettings, "Known characters settings", EditorStyles.foldoutHeader);
        if (showKnownCharacterSettings)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Chances:", EditorStyles.boldLabel);
            _generatorPreset.ChanceToKnowOtherCharacter = EditorGUILayout.Slider("Know other character", _generatorPreset.ChanceToKnowOtherCharacter, 0, 1);
            _generatorPreset.ChanceToKnowTheName = EditorGUILayout.Slider("Know the name", _generatorPreset.ChanceToKnowTheName, 0, 1);
            _generatorPreset.ChanceToKnowActualLocation = EditorGUILayout.Slider("Know actual location", _generatorPreset.ChanceToKnowActualLocation, 0, 1);
            EditorGUI.indentLevel--;
            if(EditorGUI.EndChangeCheck())
            {
                _generatorPresetSO.ApplyModifiedProperties();
            }
        }
    }
    private void OnArchetypeSelected()
    {
        _selectedArchetypeSO = new SerializedObject(_generatorPreset.Archetypes[_selectedArchetype]);
        _selectedArchetypeValues = _selectedArchetypeSO.FindProperty("PersonalityValues");
        _selectedArchetypeHabitats = _selectedArchetypeSO.FindProperty("Habitats");
        _specifyCurrentLocation = false;
        _selectedLocation = 0;
    }
    private bool CurrentLocationIsValid()
    {
        return _generatorPreset.Archetypes[_selectedArchetype].GetHabitatNames()[_selectedLocation] != "NULL";
    }
    private void AddNewArchetype()
    {
        int archetypesCount = AssetDatabase.FindAssets("t:MixedArchetype").Length;
        var so = ScriptableObject.CreateInstance<MixedArchetype>();
        if (!Directory.Exists("Assets/Resources/CharactersGenerator"))
        {
            Directory.CreateDirectory("Assets/Resources/CharactersGenerator/");
            AssetDatabase.Refresh();
        }
        string path = AssetDatabase.GenerateUniqueAssetPath(string.Format("Assets/Resources/CharactersGenerator/MixedArchetype{0}.asset", archetypesCount));
        AssetDatabase.CreateAsset(so, path);

        _generatorPreset.Archetypes.Add(so);
        EditorUtility.SetDirty(_generatorPreset);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    class ObservedSO
    {
        public ScriptableObject Reference { get; private set; }
        public SerializedObject SerializedReference { get; private set; }
        private Editor _cashedEditor;
        public Editor CashedEditorOfSO
        {
            get
            {
                return _cashedEditor;
            }
        }
        public ObservedSO(ScriptableObject soRef)
        {
            Reference = soRef;
            SerializedReference = new SerializedObject(Reference);

            _cashedEditor = Editor.CreateEditor(Reference);
        }
    }
}
