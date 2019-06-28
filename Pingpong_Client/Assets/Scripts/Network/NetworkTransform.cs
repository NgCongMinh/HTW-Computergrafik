﻿using System;
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
            oldPosition = transform.localPosition;
            player = new Player();
            player.id = networkIdentity.GetId();
            player.position = new Position();
            player.position.x = "0";
            player.position.y = "0";
            player.position.z = "0";

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
                    oldPosition = transform.localPosition;
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
            Vector3 newPosition = transform.localPosition;

            return (oldPosition != newPosition) && (
                       (Math.Abs(newPosition.x - oldPosition.x) >= ChangedPositionThreshold) ||
                       (Math.Abs(newPosition.y - oldPosition.y) >= ChangedPositionThreshold) ||
                       (Math.Abs(newPosition.z - oldPosition.z) >= ChangedPositionThreshold)
                   );
        }

        private void SendData()
        {
            Vector3 position = transform.localPosition;

            player.position.x = position.x.ToString();
            player.position.y = position.y.ToString();
            player.position.z = position.z.ToString();

            //Debug.Log(JsonUtility.ToJson(player));
            networkIdentity.GetSocket().Emit("updatePosition", new JSONObject(JsonUtility.ToJson(player)));
        }
    }
}