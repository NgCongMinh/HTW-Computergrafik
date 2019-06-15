using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	// Use this for initialization
	public GameObject ballPrefab;
	public Transform ballSpawn;


	// Update is called once per frame
	void Update ()
	{
		
		if(!isLocalPlayer)
		{
			return;
		}
		
		var x = Input.GetAxis("Horizontal");

		if(this.transform.position == new Vector3(this.transform.position.x,this.transform.position.y,30))
		{
 			x = x * Time.deltaTime * -150.0f;
 		}
 		else
 		{
 			x = x * Time.deltaTime * 150.0f;
 		}
 		
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;

		transform.Translate(x,0,0);
		transform.Translate(0,z,0);	

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -18.25f, 18.25f), transform.position.y, transform.position.z);
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -13.25f, 13.25f), transform.position.z);


		if(Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}
	}

	[Command]
	void CmdFire()
	{
		/*
		if(this.transform.position.z == 30)
		{
			ballSpawn.transform.Translate(this.transform.position.x, this.transform.position.y, this.transform.position.z+1);
			Debug.Log("yo");
		}
		else
		{
			ballSpawn.transform.Translate(this.transform.position.x, this.transform.position.y, this.transform.position.z+3);
			Debug.Log("yi");
		}
		*/
		if(this.transform.position.z == 30)
		{
			ballSpawn.transform.position = new Vector3(ballSpawn.position.x, ballSpawn.position.y, this.transform.position.z-1);
		}
		else
		{
			ballSpawn.transform.position = new Vector3(ballSpawn.position.x, ballSpawn.position.y, this.transform.position.z+1);

		}
		var ball = (GameObject)Instantiate(ballPrefab, ballSpawn.position, ballSpawn.rotation);

		if(this.transform.position.z == 30)
		{
			ball.GetComponent<Rigidbody>().velocity = -(ball.transform.forward) * 30;
		}
		else
		{
			ball.GetComponent<Rigidbody>().velocity = ball.transform.forward * 30;
		}
		//Destroy(ball, 5.0f);mm
		NetworkServer.Spawn(ball);

	}

	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}

	void Start()
	{	
		gameObject.tag = "Player";

		if(!isLocalPlayer)
		{
			return;
		}else
		{
			if(this.transform.position == new Vector3(this.transform.position.x,this.transform.position.y,30))
			{
				Camera.main.transform.position = this.transform.position + this.transform.forward*20 + this.transform.up *3;
				Camera.main.transform.rotation = Quaternion.Euler(0,180,0);
			}
			else
			{
				Camera.main.transform.position = this.transform.position - this.transform.forward*20 + this.transform.up *3;
			}
		}
		
	}

}
