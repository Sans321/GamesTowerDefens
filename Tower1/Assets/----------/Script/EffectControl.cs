using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


public class EffectControl : MonoBehaviour
{
	
	private class EffectDescriptor
	{
		public string name; 
		public GameObject fx; 
		public Dictionary<float, float> effects = new Dictionary<float, float>(); 
	}

	private enum EffectTrigger
	{
		Added,
		NewModifier,
		ModifierExpired,
		Removed
	}

	private List<EffectDescriptor> effectList = new List<EffectDescriptor>();

	
	public void AddEffect(string effectName, float modifier, float duration, GameObject fxPrefab)
	{
		
		MethodInfo methodInfo = GetType().GetMethod(effectName, BindingFlags.Instance | BindingFlags.NonPublic);
		if (methodInfo != null)
		{
			EffectDescriptor hit = null;
			foreach (EffectDescriptor sem in effectList)
			{
				if (effectName == sem.name)
				{
					hit = sem;
					break;
				}
			}

			if (hit == null) 
			{
				EffectDescriptor newSem = new EffectDescriptor();
				newSem.name = effectName;
				
				newSem.effects.Add(modifier, Time.time + duration);
				
				effectList.Add(newSem);
				
				if (fxPrefab != null)
				{
					newSem.fx = Instantiate(fxPrefab, transform);
				}
			
				methodInfo.Invoke(this, new object[] {EffectTrigger.Added, modifier});
			}
			else 
			{
				
				hit.effects.Add(modifier, Time.time + duration);
				methodInfo.Invoke(this, new object[] {EffectTrigger.NewModifier, modifier});
			}
		}
		else
		{
			Debug.LogError("Unknown effect - " + effectName);
		}
	}

	public void AddConstantEffect(string effectName, float modifier, GameObject fxPrefab)
	{
		AddEffect(effectName, modifier, float.MaxValue, fxPrefab);
	}

	public bool RemoveConstantEffect(string effectName, float modifier)
	{
		bool res = false;
		foreach (EffectDescriptor desc in effectList)
		{
			if (desc.name == effectName)
			{
				List<float> expiredTimes = new List<float>(desc.effects.Keys);
				for (int i = 0; i < expiredTimes.Count; ++i)
				{
					if (expiredTimes[i] == modifier && desc.effects[expiredTimes[i]] == float.MaxValue)
					{
						res = true;
						desc.effects[expiredTimes[i]] = Time.time;
						break;
					}
				}
			}
		}
		return res;
	}

	public bool RemoveEffects(string effectName)
	{
		bool res = false;
		foreach (EffectDescriptor desc in effectList)
		{
			if (desc.name == effectName)
			{
				res = true;
				List<float> expiredTimes = new List<float>(desc.effects.Keys);
				for (int i = 0; i < expiredTimes.Count; ++i)
				{
					desc.effects[expiredTimes[i]] = Time.time;
				}
				break;
			}
		}
		return res;
	}

	void FixedUpdate()
	{
		float currentTime = Time.time;
		List<EffectDescriptor> emptyEffects = new List<EffectDescriptor>();
		foreach (EffectDescriptor desc in effectList)
		{
			List<float> expiredList = new List<float>();
			foreach (KeyValuePair<float, float> effectData in desc.effects)
			{
				
				if (effectData.Value != float.MaxValue && currentTime > effectData.Value)
				{
					expiredList.Add(effectData.Key);
					MethodInfo methodInfo = GetType().GetMethod(desc.name, BindingFlags.Instance | BindingFlags.NonPublic);
					methodInfo.Invoke(this, new object[] {EffectTrigger.ModifierExpired, effectData.Key});
				}
			}
			foreach (float expired in expiredList)
			{
				desc.effects.Remove(expired);
			}
			if (desc.effects.Count <= 0)
			{
				MethodInfo methodInfo = GetType().GetMethod(desc.name, BindingFlags.Instance | BindingFlags.NonPublic);
				methodInfo.Invoke(this, new object[] {EffectTrigger.Removed, 0f});
				emptyEffects.Add(desc);
			}
		}
		foreach (EffectDescriptor emptyEffect in emptyEffects)
		{
			if (emptyEffect.fx != null)
			{
				Destroy(emptyEffect.fx);
			}
			effectList.Remove(emptyEffect);
		}
	}
	public bool HasActiveEffect(string effectName)
	{
		bool res = false;
		foreach (EffectDescriptor effect in effectList)
		{
			if (effect.name == effectName)
			{
				res = true;
				break;
			}
		}
		return res;
	}
	void OnDestroy()
	{
		StopAllCoroutines();
	}

	private void Stun(EffectTrigger trigger, float modifier)
	{
		AiBehavior aiBehavior = GetComponent<AiBehavior>();
		NavAgent navAgent = GetComponent<NavAgent>();
		switch (trigger)
		{
		case EffectTrigger.Added:
			aiBehavior.enabled = false;
			navAgent.enabled = false;
			break;
		case EffectTrigger.Removed:
			aiBehavior.enabled = true;
			navAgent.enabled = true;
			break;
		}
	}
	private void Speed(EffectTrigger trigger, float modifier)
	{
		NavAgent navAgent = GetComponent<NavAgent>();
		switch (trigger)
		{
		case EffectTrigger.Added:
		case EffectTrigger.NewModifier:
			navAgent.speed *= 1 + modifier;
			break;
		case EffectTrigger.ModifierExpired:
			navAgent.speed /= 1 + modifier;
			break;
		}
	}
}
