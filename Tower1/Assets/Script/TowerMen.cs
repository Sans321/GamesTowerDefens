using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerMen : Loader<TowerMen>
{

    public TowerButton towerNtnPress{ get; set; }


    SpriteRenderer spriterand;
    private List<TowerControler> TowerLst = new List<TowerControler>();
    private List<Collider2D> Buildlist = new List<Collider2D>();
    private Collider2D buildTile;
    
    void Start()
    {
        spriterand = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();
        spriterand.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoin=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoin, Vector2.zero);

            if (hit.collider.tag=="Towerside")
            {
                buildTile = hit.collider;
                buildTile.tag = "TowersideFull";
                RegisterBuildSite(buildTile);
                PlaceTower(hit);

            }
        }
        if (spriterand.enabled)
        {
            FollowMouse();
        }
    }


    public void RegisterBuildSite(Collider2D buildTag)
    {
        Buildlist.Add(buildTag);
    }

    public void RegisterTower(TowerControler tower)
    {
        TowerLst.Add(tower);
    }

    public void RenameTagBuild()
    {
        foreach(Collider2D buildTag in Buildlist)
        {
            buildTag.tag = "Towerside";
        }
        Buildlist.Clear();
    }

    public void DestroyAllTower()
    {
        foreach(TowerControler tower in TowerLst)
        {
            Destroy(tower.gameObject);
        }
        TowerLst.Clear();
    }


    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerNtnPress!=null)
        {
            TowerControler netTower = Instantiate(towerNtnPress.TowerObject);
            netTower.transform.position = hit.transform.position;
            BuyTower(towerNtnPress.TowerPrice);
            RegisterTower(netTower);
            DisabelDrag();
        }
        
    }

    public void BuyTower(int price)
    {
        Manager.Instance.sutractMoney(price);
    }

    public void SelectTower(TowerButton towerSelect)
    {
        if (towerSelect.TowerPrice<=Manager.Instance.TotatlMoney)
        { 
            towerNtnPress = towerSelect;
             EnabelDrag(towerNtnPress.DragSprite);

        }
       
    }

    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void EnabelDrag(Sprite sprite)
    {
        spriterand.enabled =true;
        spriterand.sprite = sprite;
    }
    public void DisabelDrag()
    {
        spriterand.enabled = false;
       
    }
}
