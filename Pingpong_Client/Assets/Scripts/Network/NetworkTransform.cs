using System;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkTransform : MonoBehaviour
    {
        private const int SendStandStillThreshold = 2;

        private const float ChangedPositionThreshold = 0.1f;

        [SerializeField] private Vector3 oldPosition;

        private NetworkIdentity networkIdentity;

        private Player player;

        // if the palyer does not move --> after certain time the position will be updated
        private float stillCounter = 0;

        // Start is called before the first frame update
        void Start()
        {
            networkIdentity = GetComponent<NetworkIdentity>();
            
            oldPosition = transform.position;
            
            player = new Player();
            player.id = networkIdentity.GetId();
            player.position = new Position("0", "0", "0");

            if (!networkIdentity.IsControlling())
            {
                enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (networkIdentity.IsControlling())
            {
                if (Moved())
                {
                    oldPosition = transform.position;
                    stillCounter = 0;
                    SendData();
                }
                else
                {
                    stillCounter += Time.deltaTime;

                    if (stillCounter >= SendStandStillThreshold)
                    {
                        stillCounter = 0;
                        SendData();
                    }
                }
            }
        }

        private bool Moved()
        {
            Vector3 newPosition = transform.position;

            return (oldPosition != newPosition) && (
                       (Math.Abs(newPosition.x - oldPosition.x) >= ChangedPositionThreshold) ||
                       (Math.Abs(newPosition.y - oldPosition.y) >= ChangedPositionThreshold) ||
                       (Math.Abs(newPosition.z - oldPosition.z) >= ChangedPositionThreshold)
                   );
        }

        private void SendData()
        {
            Vector3 position = transform.position;

            player.position.x = position.x.ToString();
            player.position.y = position.y.ToString();
            player.position.z = position.z.ToString();
            
            //Debug.Log("Player Position : " + JsonUtility.ToJson(player));

            networkIdentity.GetSocket().Emit("updatePosition", new JSONObject(JsonUtility.ToJson(player)));
        }
    }
}