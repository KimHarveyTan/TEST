using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayMgr : Singleton<GameplayMgr>
{
    public GameObject _player;
    [SerializeField] int _score;
    [SerializeField] TextMeshProUGUI _scoreDisplay;

    public GameObject _background; //bg
    public float _bgSpeed;
    private Vector3 _bgStartPosition;

    void Start()
    {
        if(_player != null)
        {
            if (!_player.GetComponent<Player>()._isOnDash)
            {
                _bgSpeed = 6.6f; //.6f if Time.fixedDeltaTime instead of Time.deltaTime
            }

        }

        _score = 0;
        StartCoroutine(addScoreByTime(2, 1.5f));

        //bg
        _bgStartPosition = _background.transform.position;
    }

    void Update()
    {
        UpdateScore();

        //bg
        _background.transform.Translate(Vector3.down * _bgSpeed * Time.deltaTime /*Time.fixedDeltaTime*/);

        if (_background.transform.position.y < -10.03138f)
        {
            _background.transform.position = _bgStartPosition;
        }
    }

    private IEnumerator addScoreByTime(int value, float delay)
    {
        while (_player.GetComponent<Player>().IsAlive)
        {
            yield return new WaitForSeconds(delay);
            _score += value;
        }
    }

    public void addScore(int value)
    {
        _score += value;
    }

    public void Reset()
    {
        _score = 0;
        UpdateScore();
        _bgSpeed = 6.6f;
    }
    public void UpdateScore()
    {
        _scoreDisplay.text = $"SCORE:{_score}";
    }
}
