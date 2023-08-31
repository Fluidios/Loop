using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsVisualizer : GameSystem
{
    [SerializeField] private RectTransform _defaultAnchor;
    [SerializeField] private RectTransform _leftBottomUiAnchor;
    public override bool AsyncInitialization => false;
    public RoadEventGraphics ActiveEventGraphics { get; private set; }
   
    public T ShowEventVisual<T>(T eventVisualPrefab) where T : RoadEventGraphics
    {
        var visual = Instantiate(eventVisualPrefab, transform);
        StartCoroutine(MoveEventVisual(true, visual.RectTransform));
        ActiveEventGraphics = visual;
        return visual;
    }
    public void HideEventVisual<T>(T spawnedEventVisual, Action callback) where T : RoadEventGraphics
    {
        StartCoroutine(MoveEventVisual(false, spawnedEventVisual.RectTransform, () => {
            callback();
            Destroy(spawnedEventVisual.gameObject);
        }));
        ActiveEventGraphics = null;
    }
    public CharacterCombatUi AddUI(CharacterCombatUi prefab)
    {
        return Instantiate(prefab, _leftBottomUiAnchor);
    }

    IEnumerator MoveEventVisual(bool show, RectTransform visual, Action callback = null)
    {
        Vector2 from = show ? Vector2.Scale(_defaultAnchor.anchoredPosition, new Vector2(1, -1)) : _defaultAnchor.anchoredPosition;
        Vector2 to = show ? _defaultAnchor.anchoredPosition : Vector2.Scale(_defaultAnchor.anchoredPosition, new Vector2(1, -1));
        for (float t = 0; t <= 1;)
        {
            t += Time.deltaTime;
            visual.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
        callback?.Invoke();
    }
}
