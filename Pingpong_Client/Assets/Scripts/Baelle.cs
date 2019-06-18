using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Baelle : MonoBehaviour {

	ScoreUpdater scoreupdater;
	public float speed = 100f;

	void OnCollisionEnter(Collision col)
	{

		if(col.gameObject.tag == "Player"){
			if(col.transform.position.z == 30){
				float sx = Random.Range(0,2) == 0 ? -1 : 1;
				float sy = Random.Range(0,2) == 0 ? -1 : 1;
				float sz = Random.Range(0,2) == 0 ? -1 : 1;
				this.GetComponent<Rigidbody>().velocity = new Vector3 (speed * sx, speed * sy, speed * sz);
				//this.GetComponent<Rigidbody>().velocity = new Vector3 (speed * sx, speed * sy, speed * sz, 0f);
				//this.GetComponent<Rigidbody>().velocity = -(this.transform.forward) * 30;
			}else{
				float sx = Random.Range(0,2) == 0 ? -1 : 1;
				float sy = Random.Range(0,2) == 0 ? -1 : 1;
				float sz = Random.Range(0,2) == 0 ? -1 : 1;
				this.GetComponent<Rigidbody>().velocity = new Vector3 (speed * sx, speed * sy, speed * sz);
				//this.GetComponent<Rigidbody>().velocity = new Vector3 (speed * sx, speed * sy, speed * sz, 0f);
			//	this.GetComponent<Rigidbody>().velocity = this.transform.forward * 30;
			}
		}else if(col.gameObject.tag == "Sidewall"){
			float sx = Random.Range(0,2) == 0 ? -1 : 1;
				float sy = Random.Range(0,2) == 0 ? -1 : 1;
				float sz = Random.Range(0,2) == 0 ? -1 : 1;
				this.GetComponent<Rigidbody>().velocity = new Vector3 (speed * sx, speed * sy, speed * sz);	
		}else if(col.gameObject.tag == "Wall"){
			if(col.transform.position.z == 100){
				scoreupdater.AddP1();
				scoreupdater.ScoreAusgeben();
			}else{
				scoreupdater.AddP2();
				scoreupdater.ScoreAusgeben();

			}
			Destroy(gameObject);
		}else if(col.gameObject.tag == "Tor"){
			Destroy(gameObject);
		}else if(col.gameObject.tag == "Ball"){
			Destroy(gameObject);
		}
		
	}

	void Start(){
		scoreupdater = GameObject.Find("Punktestand").GetComponent<ScoreUpdater>();
	
		gameObject.tag = "Ball";
	}
}
