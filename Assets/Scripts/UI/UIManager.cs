using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool m_Paused;

    [SerializeField]
    private Image m_PlayerHealthBar;

    private float m_PlayerPreviousHealth;

    [SerializeField]
    private TMP_Text m_PlayerCurrency;

    [SerializeField]
    private GameObject m_UpgradeMenu;

    [SerializeField]
    private Button m_ResumeButton;

    [SerializeField]
    private Button m_QuitButton;

    [SerializeField]
    private GameObject m_GameOverMenu;

    private PlayerObject m_Player;

    private bool m_GameOver;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        m_Paused = false;

        m_PlayerCurrency.text = "Currency: 0";

        m_UpgradeMenu.SetActive(false);

        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerObject>();

        m_PlayerPreviousHealth = m_Player.GetHealth();

        m_PlayerHealthBar.fillAmount = m_Player.GetHealth() / m_Player.GetMaxHealth();

        m_GameOverMenu.SetActive(false);

        m_GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        float playerHealth = m_Player.GetHealth();

        if (m_PlayerPreviousHealth != playerHealth)
        {
            m_PlayerHealthBar.fillAmount = playerHealth / m_Player.GetMaxHealth();

            m_PlayerPreviousHealth = playerHealth;
        }

        if (m_Player.GetHealth() <= 0.0f)
        {
            GameOver();
        }
    }

    void FixedUpdate()
    {
        m_PlayerCurrency.text = "Currency: " + m_Player.GetCurrency().ToString();
    }

    private void GameOver()
    {
        m_GameOver = true;

        Pause();

        m_GameOverMenu.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Start");
    }

    public void OptionsMenu()
    {
        Pause();

        m_QuitButton.gameObject.SetActive(true);
    }

    public void UpdateUI()
    {
        m_PlayerCurrency.text = "Currency: " + m_Player.GetCurrency().ToString();
    }

    public void UpgradeMenu()
    {
        m_UpgradeMenu.SetActive(true);

        Pause();
    }

    public void ResumeAll()
    {
        m_UpgradeMenu.SetActive(false);

        m_QuitButton.gameObject.SetActive(false);

        Resume();
    }

    public void Resume()
    {
        m_Paused = false;

        Time.timeScale = 1.0f;

        if (!m_GameOver)
        {
            m_ResumeButton.gameObject.SetActive(m_Paused);
        }
    }

    public void Pause()
    {
        m_Paused = true;

        Time.timeScale = 0.0f;

        if (!m_GameOver)
        {
            m_ResumeButton.gameObject.SetActive(m_Paused);
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}