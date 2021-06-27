using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Eneme : MonoBehaviour
{
    
    [SerializeField]
    Transform exit;
    [SerializeField]
    Transform[] wayPoint;
    [SerializeField]
    float navigate;
    [SerializeField]
    int health;
    [SerializeField]
    int rewardAmount;

    int target = 0;
    Transform enemy;
    Collider2D enemyCollider;
    Animator anim;
    float navigateTime = 0;
    bool isDead = false;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Manager.Instance.RegitEnemy(this);

    }

    
    void Update()
    {
        if (wayPoint != null&& isDead==false)
        {
            navigateTime += Time.deltaTime;
            if (navigateTime > navigate)
            {
                if (target < wayPoint.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoint[target].position, navigateTime);
                }

                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigateTime);
                }
                navigateTime = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MovePoi")
        {
            target += 1;

        }
        else if (collision.tag == "Finish")
        {
            Manager.Instance.RoundEscaped += 1;
            Manager.Instance.TotalEscaped += 1;
            Manager.Instance.UnregEnemy(this);
            Manager.Instance.IsWaveOver();
        }
        else if (collision.tag == "Project")
        {
            Project newP = collision.gameObject.GetComponent<Project>();
            EnemiHit(newP.AttackDamage);
            Destroy(collision.gameObject);
        }
    }
    
    public void EnemiHit(int hitPoints)
    {
        if (health-hitPoints>0)
        {
            health -= hitPoints;
            //damage
            anim.Play("Hit");

        }
        else
        {
            anim.SetTrigger("didDie");
            Die();
            //die
        }
        
    }
    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        Manager.Instance.TotalKilled += 1;
        Manager.Instance.addMoney(rewardAmount);
        Manager.Instance.IsWaveOver();
    }

    

}
