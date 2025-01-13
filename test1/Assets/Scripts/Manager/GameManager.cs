using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] Player _player;
	public Player Player { get { return _player; } set { _player = value; }  }

	[SerializeField] SceneController sceneController;


    public void Awake()
    {
		/*if(sceneController != null)
			sceneController.Init();*/

		sceneController?.Init();
    }



    public void StartGame()
	{
		SpawnerMgr.Instance.SpawnerController.StartGame();
	}
	public void RestartGame()
	{
		/*Player.Reset();
		SpawnerController.Instance.Reset();*/

		Time.timeScale = 1;
		sceneController.ReloadGame();

		StartCoroutine(StartGameDelay());
	}

	IEnumerator StartGameDelay()
	{
		yield return new WaitForSeconds(5.5f);
        StartGame();
    }


	public void GameOver()
	{
		Time.timeScale = 0;
		MenuMgr.Instance.MenuController.SwitchMenu((int)MenuType.GameOverMenu);
	}
}
