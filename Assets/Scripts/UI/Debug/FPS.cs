using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class FPS : Debugable
    {

        public void Update()
        {
            if (!Show) return;
            _text.text = string.Format("FPS: {0}", (int)(1f / Time.unscaledDeltaTime));
        }
    }
}
