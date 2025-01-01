using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerController : Singleton<SpawnerController>
{
    [SerializeField] Transform  _enemySpawnPoint;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _player;
    public List<GameObject> enemyList = new List<GameObject>();
    public float spawnerDelay;

    [SerializeField] List<GameObject> _charList = new List<GameObject>();
    public List<GameObject> _spawnedCharList = new List<GameObject>();
    [SerializeField] Transform _playerSelectSP;
    public Transform _playerIngameSP;
    [SerializeField] Image GaugeBar;
    [SerializeField] Button GaugeButton;
    public Image[] hearts;

    public void StartGame()
    {
        spawnerDelay = 1.3f;
        StartCoroutine(SpawnStart());
    }

    private void Update()
    {
        if (_spawnedCharList.Count == 0)
        {
            SpawnCharacterPlayerSelect(); // Always spawn the current character
        }
    }

    private void SpawnCharacterPlayerSelect()
    {
        // Clear all currently spawned characters
        foreach (GameObject spawnedChar in _spawnedCharList)
        {
            Destroy(spawnedChar);
        }
        _spawnedCharList.Clear(); // Clear the list to remove references

        // Instantiate the character from _charList at index 0 (first after rotation)
        GameObject playerObj = Instantiate(_charList[0]);

        // Set new transform.position
        Vector3 newPosition = _playerSelectSP.position;
        newPosition.z = 0;
        RectTransform rectTransform = playerObj.GetComponent<RectTransform>();
        rectTransform.position = newPosition;

        // Initialize fields
        Player playerComponent = playerObj.GetComponent<Player>();
        playerComponent.GaugeBar = GaugeBar;
        playerComponent.GaugeButton = GaugeButton;
        playerComponent.hearts = hearts;

        // Add the new player object to the spawned list
        _spawnedCharList.Add(playerObj);
    }

    public void SpawnCharacterIngame()
    {
        // Clear all currently spawned characters
        foreach (GameObject spawnedChar in _spawnedCharList)
        {
            Destroy(spawnedChar);
        }
        _spawnedCharList.Clear(); // Clear the list to remove references

        // Instantiate the character from _charList at index 0 (first after rotation)
        GameObject playerObj = Instantiate(_charList[0]);

        // Set new transform.position
        Vector3 newPosition = _playerIngameSP.position;
        newPosition.z = 0;
        RectTransform rectTransform = playerObj.GetComponent<RectTransform>();
        rectTransform.position = newPosition;

        // Initialize fields
        Player playerComponent = playerObj.GetComponent<Player>();
        playerComponent.GaugeBar = GaugeBar;
        playerComponent.GaugeButton = GaugeButton;
        playerComponent.hearts = hearts;
        GameManager.Instance.GetComponent<GameManager>()._player = playerObj.GetComponent<Player>();
        GameplayMgr.Instance.GetComponent<GameplayMgr>()._player = playerObj;
        _enemyPrefab.GetComponent<Enemy>()._player = playerObj;

        // Add the new player object to the spawned list
        _spawnedCharList.Add(playerObj);
    }

    public void MoveCharacter(int direction)
    {
        Debug.Log("Before rotation: " + string.Join(", ", _charList));

        if (direction == 1) // Right button clicked
        {
            GameObject firstChar = _charList[0];
            _charList.RemoveAt(0);
            _charList.Add(firstChar);
        }
        else if (direction == -1) // Left button clicked
        {
            GameObject lastChar = _charList[_charList.Count - 1];
            _charList.RemoveAt(_charList.Count - 1);
            _charList.Insert(0, lastChar);
        }

        Debug.Log("After rotation: " + string.Join(", ", _charList));

        // Clear all currently spawned characters
        foreach (GameObject spawnedChar in _spawnedCharList)
        {
            Destroy(spawnedChar);  // Despawn the current character
        }
        _spawnedCharList.Clear(); // Clear the list to remove references

        // Spawn the new character (index 0 after the list rotation)
        SpawnCharacterPlayerSelect();
    }

    public void OnLeftButtonClick()
    {
        Debug.Log("Left button clicked");
        MoveCharacter(-1); // Move left
    }

    public void OnRightButtonClick()
    {
        Debug.Log("Right button clicked");
        MoveCharacter(1); // Move right
    }

    IEnumerator SpawnStart()
    {
        Debug.Log("Spawning...");

        while (GameManager.Instance.Player.IsAlive)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnerDelay);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemyObj = (GameObject)Instantiate(_enemyPrefab);
        enemyObj.transform.position = _enemySpawnPoint.position;
        enemyObj.GetComponent<Enemy>()._player = _spawnedCharList[0];
        enemyList.Add(enemyObj);
    }

    public void RemoveEnemy(GameObject obj)
    {
        if (obj != null && enemyList.Contains(obj))
        {
            enemyList.Remove(obj);
            Destroy(obj);
        }
    }

    public void Reset()
	{
        // Create a temporary list to store enemies to remove
        List<GameObject> enemiesToRemove = new List<GameObject>();

        // Collect enemies to remove after the loop
        foreach (GameObject enemyObj in enemyList)
        {
            if (enemyObj != null)
            {
                enemiesToRemove.Add(enemyObj); // Add enemy to the removal list
            }
        }

        // Remove the collected enemies
        foreach (GameObject enemyObj in enemiesToRemove)
        {
            RemoveEnemy(enemyObj); // This also removes it from enemyList
        }

        enemyList.Clear();

        spawnerDelay = 1.3f;
    }
}
