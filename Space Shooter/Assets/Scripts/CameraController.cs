using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;

	private Vector3 offset; 
	// Use this for initialization
	void Start () {
		if (player != null) {
			offset = transform.position - player.transform.position; 
		}
	}
	
	// LateUpdate is called once per frame
	void LateUpdate () {
		if (player != null) {
			transform.position = player.transform.position + offset; 
		}
	}
}
