using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Baelle : MonoBehaviour {

	ScoreUpdater scoreupdater;

	private PlayerController playercontroller;

	void OnCollisionEnter(Collision col)
	{
	
		if(col.gameObject.tag == "Wall"){
			if(col.transform.position.z == 100){
				Destroy(gameObject);
				scoreupdater.AddP1();
				scoreupdater.ScoreAusgeben();
			}else{
				Destroy(gameObject);
				scoreupdater.AddP2();
				scoreupdater.ScoreAusgeben();

			}
		}else if(col.gameObject.tag == "Ball"){
			Destroy(gameObject);
		}
		
	}

	void Start(){
		scoreupdater = GameObject.Find("Punktestand").GetComponent<ScoreUpdater>();
	
		gameObject.tag = "Ball";


	}
}
