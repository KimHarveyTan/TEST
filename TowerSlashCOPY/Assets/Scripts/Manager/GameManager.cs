using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	public Player _player;
	[SerializeField] Enemy _enemy;

	public Player Player 
	{ 
		get 
		{ 
			return _player; 
		} 
		set 
		{ 
			_player = value; 
		}  
	}

	public Enemy Enemy
	{
		get
		{
			return _enemy;
		}
		set
		{
			_enemy = value;
		}
	}

	public void StartGame()
	{
		SpawnerController.Instance.StartGame();
	}
	public void RestartGame()
	{
		_player.Reset();
		Enemy.Reset();
		SpawnerController.Instance.Reset();
		GameplayMgr.Instance.Reset();
		Time.timeScale = 1;
	}

	public void GameOver()
	{
		Time.timeScale = 0;
		MenuMgr.Instance.SwitchMenu((int)MenuType.GameOverMenu);
	}
}
