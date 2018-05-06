using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Boundary {
	public float xMin, xMax, yMin, yMax, zMin, zMax; 
}

public class PlayerController : MonoBehaviour {
	public float speed; 
	public float tilt; 
	public float ascension; 
	public Boundary boundary; 

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire;

	// Camera Control
	private Transform mainCamera;
	private Vector3 cameraOffset; 

	void Start(){
		cameraOffset = Camera.main.transform.position - transform.position;
		mainCamera = Camera.main.transform;
		MoveCamera (); 
	}

	void Update() {
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation); 
			GetComponent<AudioSource> ().Play (); 
		}
	}

	void FixedUpdate () {
		float moveX = Input.GetAxis ("Horizontal"); 
		float moveZ = Input.GetAxis ("Vertical"); 
		float moveY = 0.0f; 		 
		if (Input.GetMouseButton (0)) {									
			// Get mouse depth
			moveY = (((Input.mousePosition.y / Screen.height) * 2 ) - 1) * ascension; 

		}

		Vector3 movement = new Vector3 (moveX, moveY, moveZ);

		Rigidbody rb = GetComponent<Rigidbody>();
		rb.velocity = movement * speed; 
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
			//0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax) 
		); 
		rb.rotation = Quaternion.Euler (rb.velocity.y * -tilt, 0.0f, rb.velocity.x * -tilt);
		MoveCamera (); 
	}

	void MoveCamera(){
		mainCamera.position = transform.position + cameraOffset; 
	}
}
