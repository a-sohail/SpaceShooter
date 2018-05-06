using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking; 

public class Health : NetworkBehaviour {	
	public const int maxHealth = 100;
	public GameObject playExplosion; 

	[SyncVar(hook = "OnChangeHealth")]
	public int currentHealth = maxHealth;
	public RectTransform healthBar; 
	private GameController gameController; 
	public void TakeDamage(int amount){
		if (!isServer)
			return; 
		currentHealth -= amount;
		if (currentHealth <= 0) {
			currentHealth = 0;
			Instantiate (playExplosion, gameObject.transform.position, gameObject.transform.rotation); 

			GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
			if (gameControllerObject != null) {
				gameController = gameControllerObject.GetComponent<GameController> (); 
			}
			if (gameController != null) {
				gameController.CmdEndGame (); 
			}
			Destroy (gameObject); 
		}
	}
	void OnChangeHealth(int currentHealth){
		healthBar.sizeDelta = new Vector2 (currentHealth, healthBar.sizeDelta.y); 
	}
}
