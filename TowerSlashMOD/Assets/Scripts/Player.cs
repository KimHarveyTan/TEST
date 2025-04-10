using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region VARIABLES
    SwipeDetection _swipeDetection;
    Vector3 _startPos;
    bool _isAlive = false;
    bool _gaugeButtonShow = false;
    public bool _isOnDash = false;
    [SerializeField] float curGauge;
    [SerializeField] float maxGauge;
    public Image GaugeBar;
    public Button GaugeButton;
    [SerializeField] GameObject _enemy;
    public bool _isPlayerNear;
    public float _moveDistance;
    
    //health sprites //new health
    public int health;
    public int numOfHearts; //max health

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
    }
    #endregion

    #region UNITY MESSAGES
    private void Awake()
    {
        GameManager.Instance.Player = this;
        _isAlive = true;
    }

    private void Start()
    {
        _swipeDetection = GetComponent<SwipeDetection>();
        //_startPos = this.transform.position;
        _startPos = SpawnerController.Instance.GetComponent<SpawnerController>()._playerIngameSP.transform.position;
        //UpdateHealth();
        UpdateHealthUI();
        curGauge = 0;
        UpdateGauge();
        _gaugeButtonShow = false;
        GaugeButton?.onClick.AddListener(() => Dash());
    }

    private void Update()
    {
        if (_swipeDetection._inputDirection == InputDirection.TAP) //Dash Tap 
        {
            GameplayMgr.Instance.addScore(6);
            /*
            foreach (GameObject enemyObj in SpawnerController.Instance.enemyList)
            {
                Vector3 enemyPos = enemyObj.transform.position;
                enemyPos.y -= 2;
                enemyObj.transform.position = enemyPos;
                _swipeDetection._inputDirection = InputDirection.NULL;
            }

            Vector3 bgPos = GameplayMgr.Instance._background.transform.position;
            bgPos.y -= 2;
            GameplayMgr.Instance._background.transform.position = bgPos;
            */

            StartCoroutine(CO_ShortBGAndEnemySpeed());
            _swipeDetection._inputDirection = InputDirection.NULL;
        }

        if (curGauge < 100) //Gauge Button
        {
            _gaugeButtonShow = false;
        }
        else if (curGauge >= 100)
        {
            _gaugeButtonShow = true;
        }
        UpdateGaugeButtonVisibility();

        if (!_isPlayerNear)
        {
            _swipeDetection._inputDirection = InputDirection.NULL;
        }

        UpdateHealthUI();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        //GetComponent<SwipeDetection>()._inputDirection = SwipeDetection.InputDirection.NULL;

        if (collision.gameObject.name.Contains("Enemy"))
        {
            _isPlayerNear = true;
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (enemy != null)
            {
                if (enemy.gameObject == SpawnerController.Instance.enemyList[0]) // Check if it's the first enemy in list
                {
                    if (_isOnDash == false)
                    {
                        if ((int)enemy._arrowDirection == (int)_swipeDetection._inputDirection) //Swipe Kill 
                        {
                            Debug.Log("The directions are equal.");

                            ///move Enemy pos.y to 1 above the Player pos.y
                            _moveDistance = enemy.transform.position.y - this.transform.position.y; //distance to the player pos
                            _moveDistance -= 1; // distance above the pos.y of player
                                                //move the collided enemy GO
                            Vector3 enemyPosition = enemy.transform.position;
                            enemyPosition.y -= _moveDistance;
                            enemy.transform.position = enemyPosition;
                            enemy._moveSpeed = 0;

                            _isPlayerNear = false;

                            /// Move all other enemies by the same distance 
                            foreach (GameObject enemyObj in SpawnerController.Instance.enemyList)
                            {
                                if (enemyObj != SpawnerController.Instance.enemyList[0] && enemyObj != null) // Check if it's not the first enemy in list
                                {
                                    Vector3 enemyPos = enemyObj.transform.position;
                                    enemyPos.y -= _moveDistance;
                                    enemyObj.transform.position = enemyPos;
                                }
                            }

                            //move bg
                            /*
                            Vector3 bgPos = GameplayMgr.Instance._background.transform.position;
                            bgPos.y -= _moveDistance;
                            GameplayMgr.Instance._background.transform.position = bgPos;
                            */

                            StartCoroutine(CO_ShortBGSpeed());

                            StartCoroutine(RemoveEnemyAfterDelay(collision.gameObject, 0.03f)); // Start a coroutine to remove the enemy after delay
                            AddGauge(100);
                            UpdateGauge();

                            // 3% chance to increase health
                            chanceIncreaseHealth();
                        }
                    }

                    if (_isOnDash == true)
                    {
                        IncreaseEnemyspawnAndSpeed(); //start CO_IncreaseEnemyspawnAndSpeed, coroutine
                    }
                }
            }
        }

        if (collision.gameObject.name.Contains("EnemyTrigger"))
        {

            Debug.Log("Enemy object entered!");

            if (_isOnDash == false)
            {
                SpawnerController.Instance.RemoveEnemy(collision.transform.parent.gameObject);
                _isPlayerNear = false;

                //MINUS HEALTH HERE
                //TakeDamage(10);
                TakeDamage2(1);
            }

            if (_isOnDash == true)
            {
                SpawnerController.Instance.RemoveEnemy(collision.transform.parent.gameObject);
                _isPlayerNear = false;
                GameplayMgr.Instance.addScore(20);
            }
        }
    }
    #endregion

    #region PUBLIC FUNCTIONS
    /*
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
    */
    public void TakeDamage2(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _isAlive = false;
            GameManager.Instance.GameOver();
        }
        UpdateHealthUI();
    }

    //for UI
    /*
    public void UpdateHealth()
    {
        healthBar.fillAmount = (curHealth / maxHealth);

    }
    */

    public void UpdateHealthUI()
    {
        ///hearts 
        //ensure player not have more health than heart containers
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        //display
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void AddGauge(float amount)
    {
        curGauge += amount;

    }

    public void UpdateGauge()
    {
        GaugeBar.fillAmount = (curGauge / maxGauge);
    }

    void UpdateGaugeButtonVisibility()
    {
        if (_gaugeButtonShow == true)
        {
            GaugeButton.gameObject.SetActive(true);
        }
        else if (_gaugeButtonShow == false)
        {
            GaugeButton.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        /*
        curHealth = maxHealth;
        healthBar.fillAmount = curHealth;
        */
        health = numOfHearts;
        UpdateHealthUI();
        curGauge = 0;
        GaugeBar.fillAmount = curGauge;
        _isAlive = true;
        this.transform.position = _startPos;
        _gaugeButtonShow = false;
        _isOnDash = false;
        GaugeButton.gameObject.SetActive(false);
        SpawnerController.Instance.spawnerDelay = 1.3f;
        _enemy.GetComponent<Enemy>()._moveSpeed = 1f;
        _isPlayerNear = false;
        SpawnerController.Instance.SpawnCharacterIngame();
    }

    public void IncreaseEnemyspawnAndSpeed()
    {
        StartCoroutine(CO_IncreaseEnemyspawnAndSpeed());
    }

    public void Dash()
    {
        _isOnDash = true;
        curGauge = 0;
    }

    private IEnumerator RemoveEnemyAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemy != null)
        {
            SpawnerController.Instance.RemoveEnemy(enemy);
            GameplayMgr.Instance.addScore(20);
        }
    }
    #endregion

    #region PRIVATE FUNCTIONS
    private void chanceIncreaseHealth()
    {
        // 3% chance to increase health
        float chance = Random.Range(0f, 100f);
        if (chance <= 3f && health < numOfHearts) // 3% chance and only increase if health is below the max
        {
            health += 1;
            UpdateHealthUI();
            Debug.Log("Health increased by 1!");
        }
        else if (health >= numOfHearts)
        {
            Debug.Log("Health is already at maximum.");
        }
    }
    #endregion

    #region COROUTINES & HELPER FUNCTIONS
    IEnumerator CO_IncreaseEnemyspawnAndSpeed()
    {
        SpawnerController.Instance.spawnerDelay = .6f; //lessen spawn delay
        _enemy.GetComponent<Enemy>()._moveSpeed = 12.6f; //multiply the enemy movespeed
        GameplayMgr.Instance._bgSpeed = 12.6f; //multiply the bg movespeed

        yield return new WaitForSeconds(6.0f);

        //delete all remaining fast enemies
        if (_isOnDash) // Only proceed if dash is still active
        {
            List<GameObject> enemiesToRemove = new List<GameObject>();

            // Collect enemies to remove after the loop
            foreach (GameObject enemyObj in SpawnerController.Instance.enemyList)
            {
                if (enemyObj != null)
                {
                    enemiesToRemove.Add(enemyObj); // Add enemy to the removal list
                }
            }

            // Remove the collected enemies
            foreach (GameObject enemyObj in enemiesToRemove)
            {
                SpawnerController.Instance.RemoveEnemy(enemyObj);
            }
        }

        //return values
        SpawnerController.Instance.spawnerDelay = 1.3f; 
        _enemy.GetComponent<Enemy>()._moveSpeed = 6.6f;
        GameplayMgr.Instance._bgSpeed = 6.6f;
        _isOnDash = false;
        UpdateGauge();
    }

    IEnumerator CO_ShortBGSpeed() // Used after kill
    {
        GameplayMgr.Instance._bgSpeed *= 9.6f;
        yield return new WaitForSeconds(.06f);
        GameplayMgr.Instance._bgSpeed /= 9.6f;
    }

    IEnumerator CO_ShortBGAndEnemySpeed() // Used for dash tap
    {
        foreach (GameObject enemyObj in SpawnerController.Instance.enemyList)
        {
            if (enemyObj != null)
            {
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy._moveSpeed = 40.0f; // Increase speed
                    enemy.GetComponent<SpriteRenderer>().color = Color.yellow;
                    Debug.Log("Increased speed for: " + enemyObj.name);
                }
            }
        }

        yield return new WaitForSeconds(.06f);

        foreach (GameObject enemyObj in SpawnerController.Instance.enemyList)
        {
            if (enemyObj != null)
            {
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy._moveSpeed = 6.6f; // Reset speed
                    enemy.GetComponent<SpriteRenderer>().color = Color.red;
                    Debug.Log("Restored speed for: " + enemyObj.name);
                }
            }
        }
    }
    #endregion
}
