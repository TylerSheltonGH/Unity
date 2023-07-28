using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct UpgradeData
{
    public UpgradeData(string name, float price, float priceIncrease, int maxLevel = 10, int currentLevel = 0)
    {
        Name = name;
        Price = price;
        PriceIncrease = priceIncrease;
        MaxLevel = maxLevel;
        CurrentLevel = currentLevel;
    }

    public string Name { set; get; }
    public float Price { set; get; }

    public float PriceIncrease { set; get; }

    public int MaxLevel { set; get; }

    public int CurrentLevel { set; get; }

    public float GetTotalPrice()
    {
        return Price + (PriceIncrease * CurrentLevel);
    }
}

public class Upgrade : MonoBehaviour
{
    private GameObject m_Player;

    private UpgradeData m_FireRateData;

    [SerializeField]
    private Button m_FireRateButton;

    private UpgradeData m_ExplodingProjectilesData;

    [SerializeField]
    private Button m_ExplodingProjectilesButton;

    [SerializeField]
    private Button m_LifeStealButton;

    private UpgradeData m_LifeStealData;

    [SerializeField]
    private Button m_DamageButton;

    private UpgradeData m_DamageData;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");

        m_FireRateData = new UpgradeData("FireRate", 100, 200);

        UpdateButtonData(ref m_FireRateData, ref m_FireRateButton);

        m_ExplodingProjectilesData = new UpgradeData("ExplodingProjectiles", 1000, 0, 1);

        UpdateButtonData(ref m_ExplodingProjectilesData, ref m_ExplodingProjectilesButton);

        m_LifeStealData = new UpgradeData("LifeSteal", 1000, 0, 1);

        UpdateButtonData(ref m_LifeStealData, ref m_LifeStealButton);

        m_DamageData = new UpgradeData("Damage", 100, 200, 99);

        UpdateButtonData(ref m_DamageData, ref m_DamageButton);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /*private UpdateType m_UpdateType = new UpdateType();*/
    public void PurchaseUpgrade(string upgrade)
    {
        if (upgrade == "FireRate")
        {
            if (AddUpgrade(ref m_FireRateData, ref m_FireRateButton))
            {
                GameObject.FindGameObjectWithTag("PlayerProjectileLauncher").GetComponent<ProjectileLauncherObject>().SubractTimeBetweenLaunches(0.09f);
            }
        } 
        else if (upgrade == "ExplodingProjectiles")
        {
            if (AddUpgrade(ref m_ExplodingProjectilesData, ref m_ExplodingProjectilesButton))
            {
                GameObject.FindGameObjectWithTag("PlayerProjectileLauncher").GetComponent<ProjectileLauncherObject>().SetExplodingProjeciltes(true);
            }
        } else if (upgrade == "LifeSteal")
        {
            if (AddUpgrade(ref m_LifeStealData, ref m_LifeStealButton))
            {
                GameObject.FindGameObjectWithTag("PlayerProjectileLauncher").GetComponent<ProjectileLauncherObject>().AddLifeStealAmount(0.1f);
            }
        } else if (upgrade == "Damage")
        {
            if (AddUpgrade(ref m_DamageData, ref m_DamageButton))
            {
                GameObject.FindGameObjectWithTag("PlayerProjectileLauncher").GetComponent<ProjectileLauncherObject>().AddDamage(5.0f);
            }
        }
    }

    private bool AddUpgrade(ref UpgradeData upgradeData, ref Button upgradeButton)
    {
        bool success = false;

        float playerCurrency = m_Player.GetComponent<PlayerObject>().GetCurrency();

        float totalPrice = 0.0f;

        if (upgradeData.CurrentLevel < upgradeData.MaxLevel)
        {
            if (playerCurrency >= upgradeData.GetTotalPrice())
            {
                totalPrice += upgradeData.GetTotalPrice();

                upgradeData.CurrentLevel += 1;

                UpdateButtonData(ref upgradeData, ref upgradeButton);

                success = true;
            }
        }

        m_Player.GetComponent<PlayerObject>().SubtractCurrency(totalPrice);

        gameObject.GetComponent<UIManager>().UpdateUI();

        return success;
    }

    private void UpdateButtonData(ref UpgradeData upgradeData, ref Button upgradeButton)
    {
        upgradeButton.gameObject.GetComponent<Transform>().Find("ButtonText").GetComponent<TMP_Text>().text = upgradeData.Name + " " + upgradeData.CurrentLevel.ToString();
        upgradeButton.gameObject.GetComponent<Transform>().Find("PriceText").GetComponent<TMP_Text>().text = "Price: " + upgradeData.GetTotalPrice().ToString();
    }
}