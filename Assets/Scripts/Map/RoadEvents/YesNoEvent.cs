using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesNoEvent : GraphicRoadEvent<YesNoEventGraphics>
{
    [SerializeField] private ActionsCollection _yesActions;
    [SerializeField] private ActionsCollection _noActions;
    protected override void InitializeGraphics(YesNoEventGraphics graphics)
    {
        graphics.SetCallbacks(_yesActions, _noActions, MarkEventAsPassed);
    }
}
