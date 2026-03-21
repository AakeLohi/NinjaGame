using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackBarScript : MonoBehaviour
{
    public int attacksLeft;
    public Color fillColor;
    public Color emptyColor;
    public Image[] attackBars;

    public float fillTimer = 0f; // A float set by another script every frame. Goes from 2 to 0.
    public float fillTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateAttackBars();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAttackBars();
    }

    // Update the attack bars based on the number of attacks left
    void UpdateAttackBars()
    {
        for (int i = 0; i < attackBars.Length; i++)
        {
            if (i < attacksLeft)
            {
                attackBars[i].fillAmount = 1f;
                attackBars[i].color = fillColor;
            }
            else if (i == attacksLeft && i < attackBars.Length)
            {
                attackBars[i].fillAmount = 1f - (fillTimer / fillTime);
                attackBars[i].color = emptyColor;
            }
            else
            {
                attackBars[i].color = emptyColor;
                attackBars[i].fillAmount = 0f;
            }
        }
    }
}
