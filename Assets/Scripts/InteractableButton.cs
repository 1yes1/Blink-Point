using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InteractableButton : MonoBehaviour
{
    [SerializeField] private UnityEvent _onClickEvent;
    private bool _isPressed = false;


    public void OnPointerClick()
    {
        _isPressed = true;
        _onClickEvent.Invoke();
    }

    public void OnPointerExit()
    {

    }

    public void OnPointerEnter()
    {

    }

}
