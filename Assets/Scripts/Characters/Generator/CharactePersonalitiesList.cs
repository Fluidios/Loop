using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharactePersonalitiesList
{
    public static Action<object> WasUpdated;
    public List<string> SavedPersonalities;
    public int SelectedIndex = 0;

    public CharactePersonalitiesList()
    {
        SavedPersonalities = new List<string> { "Default Personality" };
        SelectedIndex = 0;
    }

    public static CharactePersonalitiesList Load()
    {
        return ES3.Load("SavedPersonalities", defaultValue: new CharactePersonalitiesList());
    }
    public static void Save(CharactePersonalitiesList list)
    {
        ES3.Save("SavedPersonalities", list);
    }

    public CharactePersonalitiesList(List<string> savedPersonalities)
    {
        SavedPersonalities = savedPersonalities;
    }
    public string this[int index]
    {
        get { return SavedPersonalities[index]; }
    }
    public int Length
    {
        get { return SavedPersonalities.Count; }
    }
    public bool Contains(string name)
    {
        return SavedPersonalities.Contains(name);
    }

    public CharacterPersonality LoadSelectedPersonality()
    {
        return ES3.Load(string.Format("Saved-CP:{0}", SavedPersonalities[SelectedIndex]), new CharacterPersonality("Default Personality")); ;
    }
    public CharacterPersonality LoadPersonality(string name)
    {
        return ES3.Load(string.Format("Saved-CP:{0}", name), new CharacterPersonality(name));
    }
    public void SavePersonality(CharacterPersonality personality)
    {
        ES3.Save(string.Format("Saved-CP:{0}", personality.Name), personality);
        SavedPersonalities[SelectedIndex] = personality.Name;
        Save(this);
    }
    public void AddNewPersonality(CharacterPersonality personality)
    {
        SavedPersonalities.Add(personality.Name);
        ES3.Save(string.Format("Saved-CP:{0}", personality.Name), personality);
        SavedPersonalities[SavedPersonalities.Count-1] = personality.Name;
        Save(this);
    }
    public void DeleteSelectedPersonality()
    {
        ES3.DeleteKey(string.Format("Saved-CP:{0}", SavedPersonalities[SelectedIndex]));
        SavedPersonalities.RemoveAt(SelectedIndex);
        Save(this);
    }
}
