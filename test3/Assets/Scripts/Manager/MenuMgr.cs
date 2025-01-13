using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MenuType
{
	MainMenu		= 0,
	InGameMenu		= 1,
	GameOverMenu	= 2,
}


public class MenuMgr : Singleton<MenuMgr>
{
	[SerializeField] MenuController controller;
	public MenuController MenuController { get { return controller;  } set { controller = value; } }



	private void Start()
	{

		
	}
	
}
