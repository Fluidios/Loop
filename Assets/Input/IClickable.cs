using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Input
{
    public interface IClickable
    {
        public void Click();
    }
    public interface IDraggable
    {
        public void BeginDrag(Vector3 mouseWorldPosition);
        public void Drag(Vector3 mouseWorldPosition);
        public void Drop(Vector3 mouseWorldPosition);
    }
}
