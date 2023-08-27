using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;

/// <summary>
/// Actions list to execute, provided by DecisionMaker component of Character
/// </summary>
[System.Serializable]
public class CharacterPlan : IEquatable<CharacterPlan>
{
    private readonly List<CharacterActionLogic> actions = new();
    /// <summary>
    /// Actions list
    /// </summary>
    public List<CharacterActionLogic> Actions => actions;

    public CharacterPlan(ICollection<CharacterActionLogic> actions)
    {
        this.actions.AddRange(actions);
    }
    public CharacterPlan(CharacterActionLogic a)
    {
        if(a != null)
        this.actions.Add(a);
    }
    public bool Contains(CharacterActionLogic a)
    {
        return actions.Contains(a);
    }
    public bool Equals(CharacterPlan other)
    {
        if (other == null) return false;

        if (actions.Count != other.actions.Count) return false;

        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i] != other.Actions[i]) return false;
        }

        return true;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (CharacterActionLogic a in actions)
        {
            sb.Append($" --> {a.Name}");
        }

        return sb.ToString();
    }
}
