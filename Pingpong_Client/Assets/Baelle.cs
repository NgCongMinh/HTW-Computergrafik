using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baelle : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		
		if(col.gameObject.tag == "Player"){
			if(col.transform.position.z == 30){
				this.GetComponent<Rigidbody>().velocity = -(this.transform.forward) * 30;
			}else{
				this.GetComponent<Rigidbody>().velocity = this.transform.forward * 30;
			}
		}else if(col.gameObject.tag == "Wall" || col.gameObject.tag == "Untagged"){
			this.GetComponent<Rigidbody>().velocity = this.transform.forward * 30;
		}else if(col.gameObject.tag == "Tor"){
			Destroy(gameObject);
		}else if(col.gameObject.tag == "Ball"){
			Destroy(gameObject);
		}
		
	}

	void Start(){
		gameObject.tag = "Ball";
	}
}
