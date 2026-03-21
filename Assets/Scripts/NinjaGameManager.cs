using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class NinjaGameManager : MonoBehaviour
{
    public GameObject player;
    public SlashScript playerScript;
    public EnemySpawner enemySpawner;
    public GameObject menuScreen;
    public GameObject shopScreen;
    public GameObject settingsScreen;
    public GameObject HUD;
    public GameObject deathScreen;
    public float moveSpeed;
    public GameObject lava;

    public ShopItemScript[] shopScripts;

    private bool shop;
    private bool settings;

    private bool reseted;

    public PostProcessVolume ppVolume;

    public Slider ppSlider;

    public AudioClip clickClip;




    // Start is called before the first frame update
    void Start()
    {

        menuScreen.SetActive(true);
        deathScreen.SetActive(false);
        HUD.SetActive(false);
        player.SetActive(false);
        playerScript.enabled = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > lava.transform.position.y + 270f)
        {
            lava.transform.position = new Vector3(0f, Mathf.Lerp(lava.transform.position.y, player.transform.position.y - 271f, moveSpeed * Time.deltaTime), 0f);
        }
    }

    public void GameStart()
    {
        lava.transform.position = new Vector3(0f, -255f, 0f);
        player.SetActive(true);
        enemySpawner.Reset();
        
        
        player.transform.position = new Vector3(0f, 1f, 0f);
        menuScreen.SetActive(false);
        deathScreen.SetActive(false);
        HUD.SetActive(true);
        playerScript.enabled = true;
        enemySpawner.Spawn();
    }

    public void GameLose()
    {
        enemySpawner.Reset();
        deathScreen.SetActive(true);
        menuScreen.SetActive(false);
        
        HUD.SetActive(false);
        playerScript.enabled = false;
        player.SetActive(false);
        lava.transform.position = new Vector3(0f, -255f, 0f);
    }

    public void GameMenu()
    {
        enemySpawner.Reset();
        player.SetActive(false);
        player.transform.position = new Vector3(0f, 1f, 0f);
        lava.transform.position = new Vector3(0f, -255f, 0f);
        menuScreen.SetActive(true);
        deathScreen.SetActive(false);
        HUD.SetActive(false);
        playerScript.enabled = false;
    }

    public void Shop()
    {
        shop = !shop;
        shopScreen.SetActive(shop);
        shopScreen.GetComponent<ShopManager>().UpdateUI();
        if (reseted)
        {
            foreach (ShopItemScript shopScript in shopScripts)
            {
                shopScript.ResetPrice();
            }
            reseted = false;
        }
    }

    public void Settings()
    {
        settings = !settings;
        settingsScreen.SetActive(settings);
        ppSlider.value = PlayerPrefs.GetFloat("PPAmount", 0f);
    }


    public void SetPostProcessing(float value)
    {
        PlayerPrefs.SetFloat("PPAmount", value);

        ppVolume.weight = PlayerPrefs.GetFloat("PPAmount", 0f);

        if (value <= 0f)
        {
            ppVolume.enabled = false;
        }
        else
        {
            ppVolume.enabled = true;
        }
    }

    public void Reset()
    {
        PlayerPrefs.SetFloat("BoostTime", 0f);
        PlayerPrefs.SetFloat("RegenerationTime", 0f);
        PlayerPrefs.SetFloat("SliceSpeed", 0f);
        PlayerPrefs.SetInt("highScore", 0);
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetFloat("PPAmount", 0f);
        PlayerPrefs.Save();

        
        foreach (ShopItemScript shopScript in shopScripts)
        {
            shopScript.ResetPrice();
        }

        reseted = true;
    }
    
}
