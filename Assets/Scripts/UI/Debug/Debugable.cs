using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class Debugable : MonoBehaviour
    {
        public bool Show;
        [SerializeField] private GameObject _ui;
        [SerializeField] protected TextMeshProUGUI _text;

        public void Awake()
        {
            _ui.SetActive(Show);
            if (Show )
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}
