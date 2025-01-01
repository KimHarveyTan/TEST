using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    public enum ArrowDirection
    {
        RIGHT = 0,
        DOWN = 1,
        LEFT = 2,
        UP = 3,
    }

    public enum EnemyType
    {
        GREEN,
        RED,
        ROTATING,
    }

    [SerializeField] EnemyType _enemyType;
    [SerializeField] SpriteRenderer _arrowObj;
    [SerializeField] Sprite[] _arrows;
    public bool _isPlayerNear = false;
    int _randArrowNum;
    public ArrowDirection _arrowDirection;
    public GameObject _player;
    public float _moveSpeed;


    void Start()
    {
        if(!_player.GetComponent<Player>()._isOnDash)
        {
            _moveSpeed = 6.6f;
        }

        //randomize type
        _enemyType = (EnemyType)Random.Range(0, 3);

        //generate random arrow direction
        _randArrowNum = Random.Range(0, 4);

        //change arrow color, sprite, and direction depending on EnemyType
        if (_enemyType == EnemyType.GREEN)
        {
            _arrowObj.color = Color.green;
            _arrowObj.sprite = _arrows[_randArrowNum];
            _arrowDirection = (ArrowDirection)_randArrowNum;
        }
        else if (_enemyType == EnemyType.RED)
        {
            _arrowObj.color = Color.red;
            _arrowDirection = (ArrowDirection)_randArrowNum;

            if (_arrowDirection == ArrowDirection.DOWN)
                _arrowObj.sprite = _arrows[(int)ArrowDirection.UP];
            else if (_arrowDirection == ArrowDirection.UP)
                _arrowObj.sprite = _arrows[(int)ArrowDirection.DOWN];
            else if (_arrowDirection == ArrowDirection.LEFT)
                _arrowObj.sprite = _arrows[(int)ArrowDirection.RIGHT];
            else if (_arrowDirection == ArrowDirection.RIGHT)
                _arrowObj.sprite = _arrows[(int)ArrowDirection.LEFT];
        }
        else if (_enemyType == EnemyType.ROTATING)
        {
            _arrowObj.color = Color.green;
            StartCoroutine(CO_ArrowRotation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime /*Time.fixedDeltaTime*/);

        if (_player != null)
        {
            _isPlayerNear = _player.GetComponent<Player>()._isPlayerNear;
        }     
    }

    
    private IEnumerator CO_ArrowRotation()
    {
        if(_enemyType == EnemyType.ROTATING)
        {
            //rotate arrows
            int arrowIndex = 0;

            while (!_isPlayerNear)
            {
                _arrowObj.sprite = _arrows[arrowIndex];
                yield return new WaitForSeconds(.19f);
                arrowIndex++;

                if (arrowIndex >= _arrows.Length)
                {
                    arrowIndex = 0;
                }
            }

            _arrowObj.sprite = _arrows[_randArrowNum];
            _arrowDirection = (ArrowDirection)_randArrowNum;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Player"))
        {
            _arrowObj.GetComponent<Transform>().localScale *= 1.3f;

        }
    }

    public void Reset()
    {
        _moveSpeed = 6f;
    }

}
