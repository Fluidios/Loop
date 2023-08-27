using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class YesNoEventGraphics : RoadEventGraphics
{
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    public void SetCallbacks(ActionsCollection yes, ActionsCollection no, UnityAction onAnySelected)
    {
        _yesButton.onClick.AddListener(yes.Execute);
        _noButton.onClick.AddListener(no.Execute);

        _yesButton.onClick.AddListener(onAnySelected);
        _noButton.onClick.AddListener(onAnySelected);
    }
}
