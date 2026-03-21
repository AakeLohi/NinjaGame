using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItemScript : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    public TextMeshProUGUI priceText;

    public string playerPrefMoney;
    public string playerPref;

    public int itemPrice;
    public float increaseRate;
    public float increaseAmount;

    public int timesUpgraded = 0;

    public int maxUpgrade; // New variable to set the maximum number of upgrades

    // Start is called before the first frame update
    void Start()
    {
        int price = PlayerPrefs.GetInt(playerPref + "Price", itemPrice);
        if (timesUpgraded >= maxUpgrade)
        {
            priceText.text = "MAXED OUT";
        }
        else
        {
            priceText.text = price.ToString("F0") + "€";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Buy()
    {
        int price = PlayerPrefs.GetInt(playerPref + "Price", itemPrice);
        int money = PlayerPrefs.GetInt(playerPrefMoney, 0);
        float value = PlayerPrefs.GetFloat(playerPref, 0);

        if (money >= price && timesUpgraded < maxUpgrade) // Check if the value is less than maxUpgrade
        {
            money -= price;
            value += increaseAmount;
            timesUpgraded += 1;
            PlayerPrefs.SetInt(playerPrefMoney, money);
            PlayerPrefs.SetFloat(playerPref, value);

            price += (int)(price * increaseRate); // You can use the increaseRate for a percentage increase instead of fixed amount
            PlayerPrefs.SetInt(playerPref + "Price", price);
        }
        else if (timesUpgraded >= maxUpgrade) // If the item is already at the maximum upgrade level
        {
            Debug.Log("Item already maxed out");
        }
        else
        {
            Debug.Log("Not enough money");
        }
        PlayerPrefs.Save();
        shopManager.UpdateUI();
    }

    public void ResetPrice()
    {
        PlayerPrefs.SetInt(playerPref + "Price", itemPrice);
        timesUpgraded = 0;

        priceText.text = itemPrice.ToString("F0") + "€";
    }
}
