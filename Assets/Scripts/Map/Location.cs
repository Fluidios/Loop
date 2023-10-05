using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Location : MonoBehaviour
{
    [SerializeField] private LocationReference _reference;
    [SerializeField] private Animation _animations;
    [SerializeField] private AnimationClip _spawnClip;
    [SerializeField] private SpriteRenderer _groundGraphics;
    public string Name
    {
        get { return _reference.name; }
    }
    public LocationReference Reference => _reference;
    public SpriteRenderer GroundGraphics
    {
        get { return _groundGraphics; }
    }
    public void ShowSpawnAnimation(float delay = 0)
    {
        if (delay > 0)
        {
            StartCoroutine(DoWithDelay(() => _animations.Play(_spawnClip.name), delay));
        }
        else
        {
            _animations.Play(_spawnClip.name);
        }
        GroundGraphics.sortingOrder = -1;
    }

    IEnumerator DoWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public virtual bool PlaceRool(Node node)
    {
        return node.Location == null;
    }
}
