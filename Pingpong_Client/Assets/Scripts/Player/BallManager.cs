using Network;
using UnityEngine;

namespace Player
{
    public class BallManager: MonoBehaviour
    {

        [Header("Class Reference")]
        [SerializeField]
        private NetworkIdentity networkIdentity;

        public GameObject ballPrefab;

        private void Update()
        {
            if (networkIdentity.IsControlling())
            {
                FireBall();
            }
        }

        private void FireBall()
        {
            
        }
        
    }
}