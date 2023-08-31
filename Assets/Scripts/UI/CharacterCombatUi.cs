using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterCombatUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _characterName;
    [SerializeField] private RectTransform _actionSelectorsAnchor;
    [SerializeField] private ActionSelector _actionSelectorPrefab;


    public void Init(string characterName, CharacterActionLogic[] actionsToCall, Action<CharacterActionLogic> confirmationCallback)
    {
        _characterName.text = characterName;
        foreach (var item in actionsToCall)
        {
            var selector = Instantiate(_actionSelectorPrefab, _actionSelectorsAnchor);
            selector.ActionName.text = item.Name;
            selector.ButtonComponent.onClick.AddListener(() => confirmationCallback(item));
        }
    }
}
