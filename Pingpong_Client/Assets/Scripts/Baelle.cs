﻿using System.Collections;
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
				scoreupdater.AddP1();
				scoreupdater.ScoreAusgeben();
			}else{
				scoreupdater.AddP2();
				scoreupdater.ScoreAusgeben();

			}
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
