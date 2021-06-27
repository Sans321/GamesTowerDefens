using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavAgent : MonoBehaviour
{
   
    public float speed = 1f;
	[HideInInspector]
    public bool move = true;
	[HideInInspector]
    public bool turn = true;
    [HideInInspector]
    public Vector2 destination;
    [HideInInspector]
    public Vector2 velocity;

	private Animator anim;
    private Vector2 prevPosition;

    void OnEnable()
    {
        prevPosition = transform.position;
		anim = GetComponentInParent<Animator>();
    }

    void FixedUpdate()
    {
        if (move == true)
        {
			transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
        }
        
        velocity = (Vector2)transform.position - prevPosition;
		velocity /= Time.fixedDeltaTime;
        if (turn == true)
        {
            SetSpriteDirection(destination - (Vector2)transform.position);
        }
        prevPosition = transform.position;
    }

    private void SetSpriteDirection(Vector2 direction)
    {
        if (direction.x > 0f && transform.localScale.x < 0f) 
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (direction.x < 0f && transform.localScale.x > 0f) 
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

		if (anim != null)
		{
			foreach (AnimatorControllerParameter param in anim.parameters)
			{
				if (param.name == "directionY")
				{
					float directionY = direction.y / (Mathf.Abs(direction.x) + Mathf.Abs(direction.y));
					anim.SetFloat("directionY", directionY);
					break;
				}
			}
		}
    }

 
    public void LookAt(Vector2 direction)
    {
        SetSpriteDirection(direction);
    }

  
    public void LookAt(Transform target)
    {
        SetSpriteDirection(target.position - transform.position);
    }
}
