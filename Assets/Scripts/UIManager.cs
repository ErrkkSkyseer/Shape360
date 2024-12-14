using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    GameManager gm;

    [SerializeField] TMP_Text textKill;
    [SerializeField] TMP_Text textHealth;

    [SerializeField] GameObject startButton;
    [SerializeField] GameObject retryButton;

    Health playerHealth;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<Health>();

        gm = GameManager.Instance;

        gm.OnGameStart += OnGameStart;
        gm.OnGameEnd += OnGameEnd;

        startButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        textKill.text = "Kills : " + gm.EnemyKill;

        string healthStr = "";
        for (int i = 0; i < playerHealth.HP; i++)
        {
            healthStr += "# ";
        }   
        healthStr += ": Health";
        textHealth.text = healthStr;
    }

    private void OnDestroy()
    {
        gm.OnGameStart -= OnGameStart;
        gm.OnGameEnd -= OnGameEnd;
    }

    void OnGameStart()
    {
        startButton.SetActive(false);
        retryButton.SetActive(false);
    }
    void OnGameEnd()
    {
        retryButton.SetActive(true);
    }

    public void OnStartBottonClick()
    {
        gm.StartGame();
    }

    public void OnRetryBottonClick()
    {
        gm.StartGame();
    }
}
