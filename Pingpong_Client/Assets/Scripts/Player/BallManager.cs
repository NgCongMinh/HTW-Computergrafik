using Network;
using UnityEngine;

namespace Player
{
    public class BallManager : MonoBehaviour
    {
        [Header("Class Reference")] [SerializeField]
        private NetworkIdentity networkIdentity;

        public Transform ballSpawn;

        public float cooldownTime = 2;

        private float nextFireTime = 0;

        public int numberOfBalls = 0;

        private void Update()
        {
            numberOfBalls = GameObject.FindGameObjectsWithTag("Ball").Length;

            if (networkIdentity.IsControlling())
            {
                if (Time.time > nextFireTime && numberOfBalls < 6
                ) //max 6 bälle ingame und alle 2 sekunden darf ein ball geschossen werden
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        nextFireTime = Time.time + cooldownTime;
                        FireBall();
                    }
                }
            }
        }

        private void FireBall()
        {
            ballSpawn.transform.position = new Vector3(
                ballSpawn.position.x,
                ballSpawn.position.y,
                this.transform.position.z + 1
            );

            float sx = Random.Range(0, 2) == 0 ? -1 : 1;
            float sy = Random.Range(0, 2) == 0 ? -1 : 1;
            float sz = Random.Range(0, 2) == 0 ? -1 : 1;

            Vector3 velocity = new Vector3(30f * sx, 30f * sy, 30f);

            SendBallData(ballSpawn.transform.position, velocity);
        }

        private void SendBallData(Vector3 p, Vector3 v)
        {
            BallPosition ballPosition = new BallPosition();
            ballPosition.id = networkIdentity.GetId();
            ballPosition.position = new Position(
                p.x.ToString(),
                p.y.ToString(),
                p.z.ToString()
            );
            ballPosition.velocity = new Position(
                v.x.ToString(),
                v.y.ToString(),
                v.z.ToString()
            );

            networkIdentity.GetSocket().Emit("spawnBall", new JSONObject(JsonUtility.ToJson(ballPosition)));
        }
    }
}