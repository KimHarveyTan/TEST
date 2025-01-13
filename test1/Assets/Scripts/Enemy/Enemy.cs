using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowType
{
    RIGHT   = 0, 
    DOWN    = 1,
    LEFT    = 2, 
    UP      = 3,
}

public enum EnemyType
{
    GREEN,
    RED,
    ROTATING,
}



public class Enemy : MonoBehaviour
{
    [SerializeField] ArrowType      type;
    [SerializeField] EnemyType      enemyType;
    [SerializeField] SpriteRenderer arrowObj; 
    [SerializeField] Sprite[]       arrow;

    [SerializeField] bool isSwipable = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RotateArrow());
    }

    IEnumerator RotateArrow()
    {
        int arrowIndex = 0;
        while (!isSwipable)
        {
            arrowObj.sprite = arrow[arrowIndex];
            yield return new WaitForSeconds(0.5f);
            arrowIndex++;

            if(arrowIndex >= arrow.Length)
            {
                arrowIndex = 0; 
            }
        }

		arrowObj.sprite = arrow[(int)type];

	}

    void ApplyArrowType()
    {

    }
}
