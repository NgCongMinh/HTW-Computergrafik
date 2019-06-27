using System.Collections;
using System.Collections.Generic;
using Network;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [Header("Data")]
    [SerializeField]
    private float speed = 50;

    [Header("Class Reference")]
    [SerializeField]
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
           
        transform.position += new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime;
    }
    
}
