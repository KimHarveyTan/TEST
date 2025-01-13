using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField] Transform  _spawnPoint;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();


    public void Awake()
    {
        SpawnerMgr.Instance.SpawnerController = this;
    }

    public void StartGame()
    {
        Debug.Log("SpawnStart");
        StartCoroutine(SpawnStart());
    }

    IEnumerator SpawnStart()
    {
        Debug.Log("Spawning...");
        while(GameManager.Instance.Player.IsAlive)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(.3f);
        }
    }
    

    void SpawnEnemy()
    {
        GameObject enemyObj = (GameObject)Instantiate(_enemyPrefab);
        enemyObj.transform.SetParent(this.transform);
        enemyObj.transform.position = _spawnPoint.position;
        enemyList.Add(enemyObj);

        enemyObj.GetComponent<Enemy>().OnDeath += GameManager.Instance.Player.Reward;
    }

    public void RemoveEnemy(GameObject obj)
    {
        enemyList.Remove(obj);
        Destroy(obj);
    }

	public void Reset()
	{
        foreach (GameObject enemyObj in enemyList)
        {
            Destroy(enemyObj);
        }
		enemyList.Clear();
	}
}
