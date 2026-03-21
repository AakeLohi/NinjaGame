using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    public int money;

    public string playerPrefMoney;

    // Start is called before the first frame update
    void Start()
    {
        
        money = PlayerPrefs.GetInt(playerPrefMoney, 0);
        UpdateUI();
    }


    public void UpdateUI()
    {
        Debug.Log("UpdateUi");
        money = PlayerPrefs.GetInt(playerPrefMoney, 0);
        moneyText.text = money.ToString("F0") + "€";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
