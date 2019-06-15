using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {

public float distance = 1.0f;
	public bool useInitalCameraDistance = false;

	private float actualDistance;
	// Use this for initialization
	void Start () {
		if(useInitalCameraDistance){
			Vector3 toObjectVector = transform.position - Camera.main.transform.position;
			Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
			actualDistance = linearDistanceVector.magnitude;
		}
		else
		{
			actualDistance = distance;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

