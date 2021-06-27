using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehavior : MonoBehaviour
{
	[HideInInspector]
	public NavAgent navAgent;
	public AiState defaultState;

	private List<AiState> aiStates = new List<AiState>();
	private AiState previousState;
	private AiState currentState;

	void Awake()
	{
		if (navAgent == null)
		{
			navAgent = GetComponentInChildren<NavAgent>();
		}
	}

	void OnEnable()
	{
		if (currentState != null && currentState.enabled == false)
		{
			EnableNewState();
		}
	}

	void OnDisable()
	{
		DisableAllStates();
	}

    void Start()
    {
		AiState[] states = GetComponents<AiState>();
        if (states.Length > 0) 
        {
			foreach (AiState state in states)
            {
                aiStates.Add(state);
            }
            if (defaultState != null)
            {
				previousState = currentState = defaultState;
                if (currentState != null)
                {
                    ChangeState(currentState);
                }
                else
                {
                    Debug.LogError("Incorrect default AI state " + defaultState);
                }
            }
            else
            {
                Debug.LogError("AI have no default state");
            }
        } 
        else 
        {
            Debug.LogError("No AI states found");
        }
    }

    public void GoToDefaultState()
    {
        previousState = currentState;
		currentState = defaultState;
        NotifyOnStateExit();
        DisableAllStates();
        EnableNewState();
        NotifyOnStateEnter();
    }

	public void ChangeState(AiState state)
    {
		if (state != null)
        {
			foreach (AiState aiState in aiStates)
            {
                if (state == aiState)
                {
                    previousState = currentState;
                    currentState = aiState;
                    NotifyOnStateExit();
                    DisableAllStates();
                    EnableNewState();
                    NotifyOnStateEnter();
                    return;
                }
            }
            Debug.Log("No such state " + state);
            GoToDefaultState();
            Debug.Log("Go to default state " + aiStates[0]);
        }
    }

    private void DisableAllStates()
    {
		foreach (AiState aiState in aiStates) 
        {
			aiState.enabled = false;
        }
    }

    private void EnableNewState()
    {
		currentState.enabled = true;
    }

    private void NotifyOnStateExit()
    {
		previousState.OnStateExit(previousState, currentState);
    }

    private void NotifyOnStateEnter()
    {
		currentState.OnStateEnter(previousState, currentState);
    }

	public void OnTrigger(AiState.Trigger trigger, Collider2D my, Collider2D other)
    {
		if (currentState == null)
		{
			Debug.Log("Current sate is null");
		}
		currentState.OnTrigger(trigger, my, other);
    }
}
