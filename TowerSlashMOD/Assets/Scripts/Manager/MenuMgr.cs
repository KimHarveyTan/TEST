using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType
{
	MainMenu		 = 0,
	InGameMenu		 = 1,
	GameOverMenu	 = 2,
	PlayerSelectMenu = 3,
}

public class MenuMgr : Singleton<MenuMgr>
{
	#region VARIABLES
	[SerializeField] GameObject[] menus;
	#endregion

	#region UNITY MESSAGES
	#endregion

	#region PUBLIC FUNCTIONS
	public void SwitchMenu(int index)
	{
		foreach (GameObject menuObj in menus)
		{
			menuObj.SetActive(false);
		}
		menus[index].SetActive(true);
	}
	#endregion

	#region PRIVATE FUNCTIONS
	private void Start()
	{
		SwitchMenu((int)MenuType.MainMenu);
	}
	#endregion

	#region COROUTINES & HELPER FUNCTIONS
	#endregion
}
