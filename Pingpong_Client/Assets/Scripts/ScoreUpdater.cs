using System;
using Model;
using Network;
using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private const int MaxScore = 10;

    private const string SelfWinningText = "You win!";

    private const string EnemyWinningText = "You lose!";

    [Header("Score View")] public TextMeshProUGUI ownScore;

    public TextMeshProUGUI enemyScore;

    public TextMeshProUGUI gameResultBanner;

    private int ownPoints;

    private int enemyPoints;

    [Header("Network")] public NetworkIdentity networkIdentity;

    // Start is called before the first frame update
    void Start()
    {
        networkIdentity = GetComponent<NetworkIdentity>();

        ownPoints = 0;
        enemyPoints = 0;
    }

    // Update is called once per frame

    void Update()
    {
        if (ownPoints >= MaxScore)
        {
            gameResultBanner.text = SelfWinningText;
        }
        else if (enemyPoints >= MaxScore)
        {
            gameResultBanner.text = EnemyWinningText;
        }

        gameResultBanner = GameObject.Find("GameResultBanner").GetComponent<TextMeshProUGUI>();
    }

    public void IncrementPlayerScore(PlayerType type)
    {
        Debug.Log(type);
        switch (type)
        {
            case PlayerType.Self:
                ownPoints = ownPoints + 1;
                break;
            case PlayerType.Enemy:
                enemyPoints = enemyPoints + 1;
                break;
            default:
                throw new ArgumentOutOfRangeException("type", type, null);
        }

        UpdateScore();
    }

    private void UpdateScore()
    {
        ownScore.text = ownPoints.ToString();
        enemyScore.text = enemyPoints.ToString();
    }

    public bool IsGameFinished()
    {
        return ownPoints >= MaxScore || enemyPoints >= MaxScore;
    }
}