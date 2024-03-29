﻿using Network;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Data")] private const float Speed = 100;

        [Header("Class Reference")] [SerializeField]
        private NetworkIdentity networkIdentity;

        // Update is called once per frame
        void Update()
        {
            if (networkIdentity.IsControlling())
            {
                CheckMovement();
            }
        }

        private void CheckMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            transform.position += new Vector3(horizontal, vertical, 0) * Speed * Time.deltaTime;

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -15f, 15f),
                Mathf.Clamp(transform.position.y, -11.5f, 11.5f),
                transform.position.z
            );
        }
    }
}