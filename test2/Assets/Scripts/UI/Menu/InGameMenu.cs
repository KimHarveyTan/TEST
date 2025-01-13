using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] Image healthBar;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateHealthBar()
    {
        Debug.Log("CurHealth: " + GameManager.Instance.Player);

        healthBar.fillAmount = GameManager.Instance.Player.CurHealth / GameManager.Instance.Player.MaxHealth;
    }

}
