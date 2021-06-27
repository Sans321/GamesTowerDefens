using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AiState : MonoBehaviour
{
	
	public enum Trigger
	{
		TriggerEnter,	
		TriggerStay,	
		TriggerExit,	
		Damage,			
		Cooldown,		
		Alone			
	}

	[Serializable]
	public class AiTransaction
	{
		public Trigger trigger;
		public AiState newState;
	}
	public AiTransaction[] specificTransactions;

	protected Animator anim;
	protected AiBehavior aiBehavior;

	public virtual void Awake()
	{
		aiBehavior = GetComponent<AiBehavior> ();
		anim = GetComponentInParent<Animator>();
		Debug.Assert (aiBehavior, "Wrong initial parameters");
	}

	public virtual void OnStateEnter(AiState previousState, AiState newState)
	{
		
	}

	public virtual void OnStateExit(AiState previousState, AiState newState)
	{

	}

	public virtual bool OnTrigger(Trigger trigger, Collider2D my, Collider2D other)
	{
		bool res = false;
		foreach (AiTransaction transaction in specificTransactions)
		{
			if (trigger == transaction.trigger)
			{
				aiBehavior.ChangeState(transaction.newState);
				res = true;
				break;
			}
		}
		return res;
	}
}
