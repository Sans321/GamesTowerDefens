using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public enum gameStatus
{
    next, play, gameover,win
}
public class Manager : Loader<Manager>
{
    [SerializeField]
    int totalWaves;
    [SerializeField]
    Text totalMoneyLabel;
    [SerializeField]
    Text currentWave;
    [SerializeField]
    Text playBtnLabel;
    [SerializeField]
    Text totalEscapedLabel;
    [SerializeField]
    Button playBtn;
    [SerializeField]
    GameObject spawnPoi;
    [SerializeField]
    Eneme[] enemies;
    [SerializeField]
    int totalEnemi=1;
    [SerializeField]
    int enemiesPerSpawn;

    int waveNumber = 0;
    int totalMoney = 10;
    int totalEscaped = 0;
    int roundEscaped=0;
    int totalKilled = 0;
    int whichEnemiesToSpawn = 0;
    int enemiesToSpawn = 0;
    gameStatus currentState = gameStatus.play;

    public List<GameObject> allowedSpells = new List<GameObject>();


    public List<Eneme> EnemeList = new List<Eneme>();

   
    const float spawnDelai = 0.5f;

    public int TotalEscaped
    {
        get
        {
           return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;

        }
        set
        {
            totalKilled = value;
        }
    }

    public int TotatlMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = TotatlMoney.ToString();
        }
    }


    
    void Start()
    {
        
        playBtn.gameObject.SetActive(false);
        ShowMenu();

    }

    private void Update()
    {
        HamdEscape();
    }
    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemeList.Count < totalEnemi)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemeList.Count < totalEnemi)
                {
                    Eneme newEnemi = Instantiate(enemies[Random.Range(0,enemiesToSpawn)]) as Eneme;
                    newEnemi.transform.position = spawnPoi.transform.position;
                }
            }
            yield return new WaitForSeconds(spawnDelai);
            StartCoroutine(Spawn());


        }
    }

    public void RegitEnemy(Eneme enemy)
    {
        EnemeList.Add(enemy);
    }

    public void UnregEnemy(Eneme enemy)
    {
        EnemeList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DestroiEnemy()
    {
        foreach(Eneme eneme in EnemeList)
        {
            Destroy(eneme.gameObject);
        }
        EnemeList.Clear();
    }

    public void addMoney(int amout)
    {
        TotatlMoney += amout;
    }

    public void sutractMoney(int amout)
    {
        TotatlMoney -= amout;
    }

    public void IsWaveOver()
    {
        totalEscapedLabel.text = "Прошли " + TotalEscaped + "/10";

        if ((RoundEscaped+TotalKilled)==totalEnemi)
        {
            if (waveNumber<=enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            SetCurretGameState();
           
            ShowMenu();
        }
    }

    public void SetCurretGameState()
    {
        if (totalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if(waveNumber==0&& (RoundEscaped + TotalKilled) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void PlayButtonPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemi += waveNumber;
                break;
            default:
                
                TotalEscaped = 0;
                TotatlMoney = 10;
                enemiesToSpawn = 0;
                TowerMen.Instance.DestroyAllTower();
                TowerMen.Instance.RenameTagBuild();
                totalMoneyLabel.text = TotatlMoney.ToString();
                totalEscapedLabel.text = "Прошли" + TotalEscaped + "/ 10";
                break;
            case gameStatus.gameover:
                SceneManager.LoadScene("SampleScene");
                break;
            case gameStatus.win:
                SceneManager.LoadScene("SampleScene");
                break;

        }
        DestroiEnemy();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWave.text = "Волна " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
       
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Конец игры";

                break;

            case gameStatus.next:
                playBtnLabel.text = "Некст волна";

                break;
            case gameStatus.play:
                playBtnLabel.text = "Начать играть";
               
                break;

            case gameStatus.win:
                playBtnLabel.text = "Победа";

                break;

        }
        playBtn.gameObject.SetActive(true);
    }

    private void HamdEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerMen.Instance.DisabelDrag();
            TowerMen.Instance.towerNtnPress=null;
        }
    }

  
}
