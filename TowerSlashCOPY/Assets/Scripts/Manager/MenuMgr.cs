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
	[SerializeField] GameObject[] menus;

	private void Start()
	{
		SwitchMenu((int)MenuType.MainMenu);
	}
	public void SwitchMenu(int index)
	{
		foreach (GameObject menuObj in menus)
		{
			menuObj.SetActive(false);
		}
		menus[index].SetActive(true);
	}
}
