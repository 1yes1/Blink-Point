using BlinkPoints;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _processInfoText;


    private void OnEnable()
    {
        GameEventReceiver.OnCompletedEvent += OnCompleted;
    }

    private void OnCompleted()
    {
        _processInfoText.color = Color.green;
        _processInfoText.text = "TAMAMLANDI";
    }
}
