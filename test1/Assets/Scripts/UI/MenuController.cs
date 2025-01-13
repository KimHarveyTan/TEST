using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{


    [SerializeField] List<GameObject> menus;

    public void Awake()
    {
        MenuMgr.Instance.MenuController = this;

        foreach (Transform child in transform)
        {
            menus.Add(child.gameObject);
        }
        SwitchMenu((int)MenuType.MainMenu);
    }

    public void GameStart()
    {
        GameManager.Instance.StartGame();
        SwitchMenu((int)MenuType.InGameMenu);
    }

    public void GameRestart()
    {
        GameManager.Instance.RestartGame();
        SwitchMenu((int)MenuType.InGameMenu);
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
