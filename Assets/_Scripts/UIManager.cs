using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject gameHUD,
        lostScreen;
    public TMP_Text timeText,
        lostTimeText;

    void Update()
    {
        switch (GameManager.GetGameStates())
        {
            case GameState.MENU:
                lostScreen.SetActive(false);
                gameHUD.SetActive(false);
                break;

            case GameState.IN_GAME:
                lostScreen.SetActive(false);
                gameHUD.SetActive(true);

                timeText.text = StatsManager.Instance.GetSurvivedDuration() + " seconds";
                break;

            case GameState.LOST:
                gameHUD.SetActive(false);
                lostScreen.SetActive(true);

                lostTimeText.text =
                    "Bubble's Lifespan: "
                    + StatsManager.Instance.GetSurvivedDuration()
                    + " seconds";
                break;
        }
    }
}