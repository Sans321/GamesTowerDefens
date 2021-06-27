using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UserActionIcon : MonoBehaviour
{
	public float cooldown = 10f;
	public GameObject userActionPrefab;
	public GameObject highlightIcon;
	public GameObject cooldownIcon;
	public Text cooldownText;
	
	
	private enum MyState
	{
		Active,
		Highligted,
		Cooldown
	}
	
	private MyState myState = MyState.Active;

	private GameObject activeUserAction;
	
	private float cooldownCounter;

	
	void OnEnable()
	{
		EventManager.StartListening("UserUiClick", UserUiClick);
		EventManager.StartListening("ActionStart", ActionStart);
		EventManager.StartListening("ActionCancel", ActionCancel);
	}

	
	void OnDisable()
	{
		EventManager.StopListening("UserUiClick", UserUiClick);
		EventManager.StopListening("ActionStart", ActionStart);
		EventManager.StopListening("ActionCancel", ActionCancel);
	}

	
	void Start()
	{
		Debug.Assert(userActionPrefab && highlightIcon && cooldownIcon && cooldownText, "Wrong initial settings");
		StopCooldown();
	}


	void Update()
	{
		if (myState == MyState.Cooldown)
		{
			if (cooldownCounter > 0f)
			{
				cooldownCounter -= Time.deltaTime;
				UpdateCooldownText();
			}
			else if (cooldownCounter <= 0f)
			{
				StopCooldown();
			}
		}
	}

	
	private void UserUiClick(GameObject obj, string param)
	{
		if (obj == gameObject)
		{
			if (myState == MyState.Active)
			{
				highlightIcon.SetActive(true);
				activeUserAction = Instantiate(userActionPrefab);
				Clicked(activeUserAction);
				myState = MyState.Highligted;
			}
		}
		else if (myState == MyState.Highligted) 
		{
			highlightIcon.SetActive(false);
			myState = MyState.Active;
		}
	}

	protected virtual void Clicked(GameObject effect)
	{

	}

	
	private void ActionStart(GameObject obj, string param)
	{
		if (obj == activeUserAction)
		{
			activeUserAction = null;
			highlightIcon.SetActive(false);
			StartCooldown();
		}
	}

	
	private void ActionCancel(GameObject obj, string param)
	{
		if (obj == activeUserAction)
		{
			activeUserAction = null;
			highlightIcon.SetActive(false);
			myState = MyState.Active;
		}
	}

	
	private void StartCooldown()
	{
		myState = MyState.Cooldown;
		cooldownCounter = cooldown;
		cooldownIcon.gameObject.SetActive(true);
		cooldownText.gameObject.SetActive(true);
	}

	private void StopCooldown()
	{
		myState = MyState.Active;
		cooldownCounter = 0f;
		cooldownIcon.gameObject.SetActive(false);
		cooldownText.gameObject.SetActive(false);
	}

	private void UpdateCooldownText()
	{
		cooldownText.text = ((int)Mathf.Ceil(cooldownCounter)).ToString();
	}
}
