using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public int price;
    
    private GameObject buyButtonGO;
    private GameObject shadeGO;
    private GameObject doneGO;
    private GameObject coinsGO;

    void Start()
    {
        coinsGO = GameObject.FindGameObjectWithTag("Coin");
        buyButtonGO = transform.Find("Buy").gameObject;
        shadeGO = transform.Find("Shade").gameObject;
        doneGO = transform.Find("Done").gameObject;

        Button buyButton = buyButtonGO.GetComponent<UnityEngine.UI.Button>();

        //if bought
        if (PlayerPrefs.GetInt(gameObject.tag, 0) == 1 || price == 0)
        {
            buyButtonGO.SetActive(false);
        }
        else
        {
            doneGO.SetActive(false);
            shadeGO.SetActive(false);
            buyButton.onClick.AddListener(BuyPlanet);
        }
        
    }

    void BuyPlanet()
    {
        int coins = PlayerPrefs.GetInt("coins", 0);
        if (coins >= price)
        {
            PlayerPrefs.SetInt(gameObject.tag, 1);
            PlayerPrefs.SetInt("coins", coins - price);
            doneGO.SetActive(true);
            buyButtonGO.SetActive(false);
            shadeGO.SetActive(true);
            if (coinsGO != null)
            {
                coinsGO.GetComponentInChildren<UnityEngine.UI.Text>().text = (coins-price).ToString();
            }
        }
    }
}
