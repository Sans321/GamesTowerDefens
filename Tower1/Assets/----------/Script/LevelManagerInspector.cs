using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
/// <summary>
/// Level manager inspector.
/// </summary>
public class LevelManagerInspector : MonoBehaviour
{
	
	public List<GameObject> enemiesList = new List<GameObject>();

	public List<GameObject> spellsList = new List<GameObject>();

	[HideInInspector]
	// Spells list for this level
	public List<GameObject> spells
	{
		get
		{
			return levelManager.allowedSpells;
		}
		set
		{
			levelManager.allowedSpells = value;
		}
	}

	private LevelManager levelManager;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		levelManager = GetComponent<LevelManager>();
		Debug.Assert(levelManager, "Wrong stuff settings");
	}
}
#endif
