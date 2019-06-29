using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;



public class PlayerController : NetworkBehaviour {

	// Use this for initialization
	public GameObject ballPrefab;
	public Transform ballSpawn;

	public float cooldownTime = 2;
	private float nextFireTime = 0;

	public int numberOfBalls = 0;

		//public float speed = 0.00001f;



	// Update is called once per frame
	void Update ()
	{
		numberOfBalls = GameObject.FindGameObjectsWithTag("Ball").Length;

		if(!isLocalPlayer)
		{
			return;
		}
		
		var x = Input.GetAxis("Horizontal");

		if(this.transform.position == new Vector3(this.transform.position.x,this.transform.position.y,30))
		{
 			x = x * Time.deltaTime * -50.0f;
 		}
 		else
 		{
 			x = x * Time.deltaTime * 50.0f;
 		}
 		
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 50.0f;
			

		transform.Translate(x,0,0);
		transform.Translate(0,y,0);	

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -15f, 15f), transform.position.y, transform.position.z);
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -11.5f, 11.5f), transform.position.z);


		if(Time.time > nextFireTime && numberOfBalls < 6)		//max 6 bälle ingame und alle 2 sekunden darf ein ball geschossen werden
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				nextFireTime = Time.time + cooldownTime;
				CmdFire();
			}
		}
	}

	[Command]
	void CmdFire()
	{

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
			ball.GetComponent<Rigidbody>().velocity = new Vector3 (1f, 1f, -1f);
		}
		else
		{
			ball.GetComponent<Rigidbody>().velocity = new Vector3 (1f, 1f, 1f);
		}

		float sx = Random.Range(0,2) == 0 ? -1 : 1;
		float sy = Random.Range(0,2) == 0 ? -1 : 1;
		float sz = Random.Range(0,2) == 0 ? -1 : 1;

		ball.GetComponent<Rigidbody>().velocity = new Vector3 (30f * sx,30f* sy, 30f * sz);

		NetworkServer.Spawn(ball);
	}

	public override void OnStartLocalPlayer()
	{

		if(this.transform.position.z == 30){
			GetComponent<MeshRenderer>().material.color = new Color(0,0,1,0.5f);
		}else{
			GetComponent<MeshRenderer>().material.color = new Color(1,0,0,0.5f);
		}
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
