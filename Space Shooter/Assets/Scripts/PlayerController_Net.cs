using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController_Net : NetworkBehaviour {
	public float speed; 
	public float tilt; 
	public float ascension; 
	public Boundary boundary; 

	public GameObject warp; 
	public GameObject playExplosion; 
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire;

	// Camera Control
	private Transform mainCamera;
	private Vector3 cameraOffset; 

	void Start(){
		if (!isLocalPlayer) {			
			return; 
		}
		cameraOffset = new Vector3(0f, 0.5f, -4f);
		mainCamera = Camera.main.transform;
		MoveCamera (); 
	}

	void Update() {
		if (!isLocalPlayer) {			
			return; 
		}
		if (Input.GetButton ("Fire1") && Time.time > nextFire) {
			CmdSpawnShot (); 
		}
	}

	void FixedUpdate () {
		if (!isLocalPlayer) {			
			return; 
		}
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
		mainCamera.position = transform.position; 
		mainCamera.Translate (cameraOffset); 
	}

	[Command]
	void CmdSpawnShot(){
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			GameObject instance = Instantiate (shot, shotSpawn.position, shotSpawn.rotation); 
			GetComponent<AudioSource> ().Play (); 
			NetworkServer.Spawn (instance); 
		}
	}

	void OnTriggerEnter (Collider other){		
		if (other.tag == "Boundary") {
			Instantiate (warp, transform.position, transform.rotation);
			transform.position = new Vector3 (
				transform.position.x,
				transform.position.y,
				-3.0f 
			); 
			return; 
		}
		Instantiate (playExplosion, transform.position, transform.rotation); 

		if (other.tag == "Player") {
			var health = other.GetComponent<Health>(); 
			if (health != null) {
				health.TakeDamage (10); 
			}	
		}			
	}
}
