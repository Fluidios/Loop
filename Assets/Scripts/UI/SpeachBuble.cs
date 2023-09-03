using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeachBuble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bubleText;
    public string Text
    {
        get { return _bubleText.text; }
        set { _bubleText.text = value; }
    }
}
