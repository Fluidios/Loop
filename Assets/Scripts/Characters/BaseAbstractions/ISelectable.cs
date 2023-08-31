using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ISelectable : IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool Focused { get; }
    public Action<ISelectable> OnSelected { get; set; }
}
