using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MyEvent : UnityEvent<GameObject, string>
{

}

public class EventManager : MonoBehaviour
{
	
	public static EventManager instance;

    
	private Dictionary<string, MyEvent> eventDictionary = new Dictionary<string, MyEvent>();

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
			return;
		}
	}

	
	void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
    public static void StartListening(string eventName, UnityAction<GameObject, string> listener)
    {
		if (instance == null)
		{
			instance = FindObjectOfType(typeof(EventManager)) as EventManager;
			if (instance == null)
			{
				Debug.Log("Have no event manager on scene");
				return;
			}
		}
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MyEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    public static void StopListening(string eventName, UnityAction<GameObject, string> listener)
    {
		if (instance == null)
		{
			return;
		}
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    public static void TriggerEvent(string eventName, GameObject obj, string param)
    {
		if (instance == null)
		{
			return;
		}
        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(obj, param);
        }
    }
}
