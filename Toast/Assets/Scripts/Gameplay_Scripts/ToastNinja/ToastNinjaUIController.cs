using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastNinjaUIController : MonoBehaviour
{
    [SerializeField]
    private ToastNinjaScore toastNinjaScore;

    [SerializeField]
    private GameObject[] livesDisplay;

    [SerializeField]
    private int bombsHit = 0;

    // Update is called once per frame
    void Update()
    {
        if (bombsHit != toastNinjaScore.BombsHit)
        {
            if (bombsHit > toastNinjaScore.BombsHit)
            {
                for (int i = bombsHit; i >= toastNinjaScore.BombsHit; i--)
                {
                    livesDisplay[i].gameObject.SetActive(false);
                }
            }
            else if (bombsHit < toastNinjaScore.BombsHit)
            {
                for (int i = bombsHit; i < toastNinjaScore.BombsHit; i++)
                {
                    livesDisplay[i].gameObject.SetActive(true);
                }
            }

            bombsHit = toastNinjaScore.BombsHit;
        }
    }
}
