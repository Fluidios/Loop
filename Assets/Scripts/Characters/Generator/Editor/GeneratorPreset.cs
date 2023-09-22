using Codice.CM.Client.Differences;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneratorPreset : ScriptableObject
{
    public TextAsset Names;
    public List<MixedArchetype> Archetypes = new List<MixedArchetype>();
    public List<LocationReference> Locations = new List<LocationReference>();
    public int GeneratedPersonalityVariationsRange = 5;
    public float ChanceToKnowOtherCharacter;
    public float ChanceToKnowTheName;
    public float ChanceToKnowActualLocation;

    public string GetRandomName()
    {
        var lines = new List<string>(Names.text.Split('\n'));

        int randomIndex = Random.Range(0, lines.Count);
        return lines[randomIndex];
    }
    public string[] GetArchetypesNames()
    {
        var output = new string[Archetypes.Count];
        for (int i = 0; i < Archetypes.Count; i++)
        {
            output[i] = Archetypes[i].name;
        }
        return output;
    }
    public string[] GetLocationsNames()
    {
        if (Locations == null || Locations.Count == 0)
        {
            string[] guids = AssetDatabase.FindAssets("t:LocationReference");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                LocationReference obj = AssetDatabase.LoadAssetAtPath<LocationReference>(path);
                Locations.Add(obj);
            }
            EditorUtility.SetDirty(this);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        var output = new string[Locations.Count];
        for (int i = 0; i < Locations.Count; i++)
        {
            output[i] = Locations[i].name;
        }
        return output;
    }
}
