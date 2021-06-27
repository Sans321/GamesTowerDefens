using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpellsManager : MonoBehaviour
{

	public Transform spellsFolder;

	
	void Start()
	{
		LevelManager levelManager = FindObjectOfType<LevelManager>();
		Debug.Assert(spellsFolder && levelManager, "Wrong initial settings");
		foreach (UserActionIcon spell in spellsFolder.GetComponentsInChildren<UserActionIcon>())
		{
			Destroy(spell.gameObject);
		}
		foreach (GameObject spell in levelManager.allowedSpells)
		{
			Instantiate(spell, spellsFolder);
		}
	}
}
