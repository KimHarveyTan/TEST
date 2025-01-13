using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Vector3 _startPos;
    [SerializeField] float curHealth;
    [SerializeField] float maxHealth;

    [SerializeField] Image healthBar;

    bool _isAlive = false;
    public bool IsAlive { get { return _isAlive; }}

	private void Awake()
	{
        GameManager.Instance.Player = this;
        _isAlive = true;
	}
	private void Start()
	{
		UpdateHealth();
        _startPos = this.transform.position;
	}

	public void TakeDamage(float damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            _isAlive = false;
            GameManager.Instance.GameOver();
        }
		UpdateHealth();
    }

    public void Movement(Vector3 direction)
    {
        this.transform.position += direction;
    }

    //for UI
    public void UpdateHealth()
    {
		//healthBar.fillAmount = (curHealth/maxHealth);

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
}
