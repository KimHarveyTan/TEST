using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float exp;
    Vector3 _startPos;
    [SerializeField] float curHealth;
    public float CurHealth { get { return curHealth; }}

    [SerializeField] float maxHealth;
    public float MaxHealth { get { return maxHealth; }}

    public UnityEvent OnTakeDamage;

    bool _isAlive = false;
    public bool IsAlive { get { return _isAlive; }}

	private void Awake()
	{
        GameManager.Instance.Player = this;
        _isAlive = true;

        if (OnTakeDamage == null)
            OnTakeDamage = new UnityEvent();
    }

	private void Start()
	{
        _startPos = this.transform.position;
        AddListeners();
	}

    void AddListeners()
    {
        OnTakeDamage.AddListener(MenuMgr.Instance.MenuController.InGameMenu.UpdateHealthBar);
        OnTakeDamage.AddListener(AudioMgr.Instance.PlaySFX);
    }

    void RemoveListeners()
    {
        OnTakeDamage.RemoveListener(MenuMgr.Instance.MenuController.InGameMenu.UpdateHealthBar);
        OnTakeDamage.RemoveListener(AudioMgr.Instance.PlaySFX);
    }

	public void TakeDamage(float damage)
    {
        curHealth -= damage;
        OnTakeDamage?.Invoke();

        if (curHealth <= 0)
        {
            _isAlive = false;
            GameManager.Instance.GameOver();
        }
    }

    public void Reward(float exp)
    {
        this.exp += exp;
    }

    public void Movement(Vector3 direction)
    {
        this.transform.position += direction;
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name.Contains("Enemy"))
        {
            TakeDamage(10);
            SpawnerMgr.Instance.SpawnerController.RemoveEnemy(other.gameObject);
        }
	}

	public void Reset()
	{
        curHealth = maxHealth;
        _isAlive = true;
        this.transform.position = _startPos;

	}

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
