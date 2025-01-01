using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwipeDetection : MonoBehaviour
{
    public enum InputDirection
    {
        RIGHT = 0,
        DOWN = 1,
        LEFT = 2,
        UP = 3,
        TAP = 4,
        NULL = 5,
    }

    //swipe variables
    private Vector2 _startTouchPosition;
    private Vector2 _currentPosition;
    private Vector2 _endTouchPosition;
    private bool _stopTouch = false;
    public float _swipeRange;
    public float _tapRange;

    public InputDirection _inputDirection;

    void Update()
    {
            Touch();
    }

    public void Touch()
    {
        //Swipe
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            _currentPosition = Input.GetTouch(0).position;
            Vector2 Distance = _currentPosition - _startTouchPosition;

            if (!_stopTouch)
            {
                if (Distance.x < -_swipeRange)
                {
                    _stopTouch = true;
                    Debug.Log("Swiped Left");
                    _inputDirection = InputDirection.LEFT;
                }
                else if (Distance.x > _swipeRange)
                {
                    _stopTouch = true;
                    Debug.Log("Swiped Right");
                    _inputDirection = InputDirection.RIGHT;
                }
                else if (Distance.y < -_swipeRange)
                {
                    _stopTouch = true;
                    Debug.Log("Swiped Down");
                    _inputDirection = InputDirection.DOWN;
                }
                else if (Distance.y > _swipeRange)
                {
                    _stopTouch = true;
                    Debug.Log("Swiped Up");
                    _inputDirection = InputDirection.UP;
                }
            }
        }

        //Tap
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _stopTouch = false;
            _endTouchPosition = Input.GetTouch(0).position;
            Vector2 Distance = _endTouchPosition - _startTouchPosition;
            if (Mathf.Abs(Distance.x) < _tapRange && Mathf.Abs(Distance.y) < _tapRange)
            {
                Debug.Log("Tapped");
                _inputDirection = InputDirection.TAP;
            }
        }
    }
}
