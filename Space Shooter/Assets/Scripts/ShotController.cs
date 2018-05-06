using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {

	void OnTriggerEnter (Collider other){	
		var myHealth = other.GetComponent<Health>(); 
		if (myHealth != null) {			
			myHealth.TakeDamage (10); 
		}	
		Destroy (gameObject); 
	}
		
}
