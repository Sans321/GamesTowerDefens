using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControler : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttack;
    [SerializeField]
    float attackRadius;
    [SerializeField]
    Project project;
    Eneme targetEneme = null;
    float attackCouter;
    bool isAttack = false;

    
    void Start()
    {
        
    }

   
    void Update()
    {
        attackCouter -= Time.deltaTime;
        if (targetEneme == null||targetEneme.IsDead)
        {
            Eneme nearestEnemy = GetNearesEnemy();
            if (nearestEnemy != null&& Vector2.Distance(transform.position, nearestEnemy.transform.position) <=attackRadius)
            {
                targetEneme = nearestEnemy;
            }
        }
        else
        {
            if (attackCouter <= 0)
            {
                isAttack = true;

                attackCouter = timeBetweenAttack;

            }
            else
            {
                isAttack = false;
            }
            if (Vector2.Distance(transform.position, targetEneme.transform.position) > attackRadius)
            {
                targetEneme = null;
            }
        }

       
    }
    public void FixedUpdate()
    {
        if (isAttack == true)
        {
            Attack();
        }
    }


    public void Attack()
    {

        isAttack = false;
        Project newProject = Instantiate(project) as Project;
        newProject.transform.localPosition = transform.localPosition;

        if (targetEneme == null)
        {
            Destroy(newProject);
        }
        else
        {
            //move project to enemy
            
            StartCoroutine(MoveProject(newProject));
        }
    }

    


    IEnumerator MoveProject(Project project)
    {
        while(GetTargetDistane(targetEneme)>0.20f && project != null && targetEneme != null)
        {
            var dir = targetEneme.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            project.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            project.transform.localPosition = Vector2.MoveTowards(project.transform.localPosition, targetEneme.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if(project!=null|| targetEneme == null)
        {
            Destroy(project);
        }
         
    }

    private float GetTargetDistane(Eneme thisEneme)
    {
        if (thisEneme == null)
        {
            thisEneme = GetNearesEnemy();
            if (thisEneme == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEneme.transform.localPosition));
    }
    private List<Eneme> GetEnemes()
    {
        List<Eneme> enemiInRange = new List<Eneme>();

        foreach(Eneme eneme in Manager.Instance.EnemeList)
        {
            if(Vector2.Distance(transform.localPosition, eneme.transform.localPosition) <= attackRadius)
            {
                enemiInRange.Add(eneme);
            }

        }
        return enemiInRange;
    }

    private Eneme GetNearesEnemy()
    {
        Eneme nerestEneme = null;
        float smallesDistance = float.PositiveInfinity;

        foreach(Eneme eneme in GetEnemes())
        {
            if (Vector2.Distance(transform.localPosition, eneme.transform.localPosition) < smallesDistance)
            {
                smallesDistance = Vector2.Distance(transform.localPosition, eneme.transform.localPosition);
                nerestEneme = eneme;
            }
        }
        return nerestEneme;
    }
}
