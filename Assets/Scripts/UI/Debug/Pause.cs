using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class Pause : Debugable
    {
        [SerializeField] private Button _buton;
        private TimeFlow _timeFlow;
        private bool _gamePaused = false;

        protected override void OnAwake()
        {
            _timeFlow = SystemsManager.GetSystemOfType<TimeFlow>();
            _buton.onClick.AddListener(Click);
        }

        private void Click()
        {
            if(_gamePaused)
            {
                _timeFlow.GameIsPaused = false;
                _gamePaused = false;
            }
            else
            {
                _timeFlow.GameIsPaused = true;
                _gamePaused=true;
            }

            _text.text = _gamePaused ? "Resume" : "Pause";
        }
    }
}
