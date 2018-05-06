using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; 

public class GameController : NetworkBehaviour {
	public GameObject hazard; 
	public Vector3 spawnValues; 
	public int hazardCount;
	public float spawnWait; 
	public float startWait; 
	public float waveWait; 

	void Start (){		
		StartCoroutine (SpawnWaves ()); 
	}


	IEnumerator SpawnWaves(){
		while (true) {
			yield return new WaitForSeconds (startWait); 
			for (int i = 0; i < hazardCount; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range(-spawnValues.x, spawnValues.x),Random.Range(-spawnValues.y, spawnValues.y),Random.Range(0.0f, spawnValues.z));
				if (spawnPosition.x == 0.0f && spawnPosition.y == 0.0f && spawnPosition.z == 0.0f)
					spawnPosition.x = 5.0f;
				Quaternion spawnRotation = Quaternion.identity; 
				Instantiate (hazard, spawnPosition, spawnRotation); 
				yield return new WaitForSeconds (spawnWait); 
			}
			yield return new WaitForSeconds (waveWait); 
		}
	}
		
	[Command]
	public void CmdEndGame(){
		NetworkManager.singleton.StopClient (); 
		NetworkManager.singleton.StopHost (); 
	}
}
