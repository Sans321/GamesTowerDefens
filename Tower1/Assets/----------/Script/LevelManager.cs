using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Level control script.
/// </summary>
public class LevelManager : MonoBehaviour
{

	public List<GameObject> allowedSpells = new List<GameObject>();

   
    private UiManager uiManager;
    // Nymbers of enemy spawners in this level
    private int spawnNumbers;
	// Current loose counter

	private bool triggered = false;


	void Start()
	{
		uiManager = FindObjectOfType<UiManager>();
		
		Debug.Assert(uiManager, "Wrong initial parameters");
	}
    private void AllEnemiesAreDead(GameObject obj, string param)
    {
        spawnNumbers--;
        // Enemies dead at all spawners
        if (spawnNumbers <= 0)
        {
			// Check if loose condition was not triggered before
			if (triggered == false)
			{
	            // Victory
				EventManager.TriggerEvent("Victory", null, null);
			}
        }
    }
}
