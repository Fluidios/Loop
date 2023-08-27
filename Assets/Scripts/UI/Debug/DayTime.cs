using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class DayTime : Debugable
    {
        [SerializeField] private TimeFlow _timeFlow;

        protected override void OnAwake()
        {
            StartCoroutine(UpdateR());
        }

        private void UpdateUI()
        {
            _text.text = string.Format("Day: {0};\n{1}", _timeFlow.CurrentDay, _timeFlow.CurrentDayPart);
        }

        IEnumerator UpdateR()
        {
            while (true)
            {
                UpdateUI();
                yield return new WaitForSeconds(1);
            }
        }
    }
}
