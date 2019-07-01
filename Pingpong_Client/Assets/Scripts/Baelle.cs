using Model;
using UnityEngine;

public class Baelle : MonoBehaviour
{
    private ScoreUpdater scoreUpdater;

    private PlayerController playercontroller;

    void Start()
    {
        scoreUpdater = (ScoreUpdater) FindObjectOfType(typeof(ScoreUpdater));

        gameObject.tag = "Ball";
    }

    private void OnCollisionEnter(Collision col)
    {
        if (scoreUpdater.IsGameFinished())
        {
            return;
        }
        
        if (col.gameObject.tag == "Wall")
        {
            if (col.transform.position.z == 100)
            {
                Destroy(gameObject);
                scoreUpdater.IncrementPlayerScore(PlayerType.Self);
            }
            else
            {
                Destroy(gameObject);
                scoreUpdater.IncrementPlayerScore(PlayerType.Enemy);
            }
        }
        else if (col.gameObject.tag == "Ball")
        {
            Destroy(gameObject);
        }
    }
}