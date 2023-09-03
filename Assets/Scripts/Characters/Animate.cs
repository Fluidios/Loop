using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : CharacterActionLogic
{
    [SerializeField] private Character _myCharacter;
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationClip[] clips;
    private Dictionary<string, AnimationClip> _animations;
    public override bool InstantAction => false;

    public string SelectedAnimation { get; set; }

    private void Awake()
    {
        _animations = new Dictionary<string, AnimationClip>();
        foreach (var clip in clips)
        {
            _animations[clip.name] = clip;
        }
        _myCharacter.Stats.Health.Subscribe((val) => StartCoroutine(PlayHitted(val)));
    }

    public override bool ExecuteAction(Character character, Action executionEndsCallback = null)
    {
        if (_animations.ContainsKey(SelectedAnimation))
        {
            StartCoroutine(AnimateSelectedClip(executionEndsCallback));
        }
        else
            executionEndsCallback();
        return true;
    }

    public IEnumerator AnimateSelectedClip(Action executionEndsCallback = null)
    {
        _animator.Play(SelectedAnimation);
        yield return new WaitForSeconds(_animations[SelectedAnimation].length);
        if(executionEndsCallback != null) 
            executionEndsCallback();
    }
    public IEnumerator PlayHitted(int currentHP)
    {
        _animator.Play("Hitted");
        yield return new WaitForSeconds(_animations["Hitted"].length);
        if (currentHP <= 0)
            _animator.Play("Death");
    }
}
