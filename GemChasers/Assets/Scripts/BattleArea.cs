using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] enemies = new GameObject[3];
    public GameObject[] spots = new GameObject[6];
    private GameObject player;
    private GameObject battleUI;
    public GameObject enemySelector;

    private UI mainUI;
    private int enemiesAdded = 0;
    public enum GameState {BattleStartUp,PlayerMoveSelection,PlayerAttacking,Enemy1Attacking,Enemy2Attacking,Enemy3Attacking};
    private GameState currentBattleState = GameState.BattleStartUp;
    void Start()
    {
        player = GameObject.Find("Player");
        battleUI = player.GetComponent<PlayerManager>().battleUI;
        mainUI = GameObject.Find("Canvas").GetComponent<UI>();

        //Enable Battle UI
        if (!battleUI.activeSelf) 
        {
            battleUI.SetActive(true);
        }

        //Sets player
        SetObject(player, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemies[0] && !enemies[1] && !enemies[2]) 
        {
            player.GetComponent<PlayerManager>().currentBattleArea = null;
            player.GetComponent<PlayerManager>().inBattle = false;
            mainUI.ResetTurn();
            battleUI.SetActive(false);
            Destroy(gameObject);
        }
    }
    public GameObject[] GetSpots() 
    {
        return spots;
    }
    private void SetEnemy(GameObject e, int index) 
    {
        if (index >= 0 && index <= 2) 
        {
            if (!enemies[index]) 
            {
                enemies[index] = e;
                SetObject(e, index);
            }
        }
    }
    private void SetObject(GameObject obj, int index) 
    {
        if (index >= 0 && index <= 5) 
        {
            obj.transform.position = spots[index].transform.position;
            obj.transform.rotation = spots[index].transform.rotation;
        }
    }
    public void SetSpawnPoint() 
    {

    }
    public bool AddCreature(GameObject obj, bool isEnemy) 
    {
        bool canAdd = false;
        if (isEnemy)
        {
            for (int i = 0; i < 3; i++) 
            {
                if (!enemies[i]) 
                {
                    enemiesAdded++;
                    canAdd = true;
                    obj.GetComponent<Enemy>().inBattle = true;
                    obj.GetComponent<Enemy>().currentBattleScript = this;
                    SetEnemy(obj, i);
                    break;
                }
            }
        }
        else 
        {
            //Save for summons
        }
        return canAdd;
    }
    public bool[] GetEnemiesAlive() 
    {
        bool[] enemiesAlive = { false, false, false };

        for (int i = 0; i < 3; i++) 
        {
            if (enemies[i]) 
            {
                if (enemies[i].GetComponent<Enemy>().isAlive()) 
                {
                    enemiesAlive[i] = true;
                }
            }
        }
        return enemiesAlive;
    }
    public GameObject[] GetEnemies() 
    {
        return enemies;
    }
    public Enemy[] GetEnemyScripts() 
    {
        Enemy[] enemyScripts = new Enemy[3];
        for (int i = 0; i < 3; i++) 
        {
            if (enemies[i]) 
            {
                enemyScripts[i] = enemies[i].GetComponent<Enemy>();
            }
        }
        return enemyScripts;
    }
    public void RemoveEnemy(GameObject e) 
    {
        for (int i = 0; i < 3; i++) 
        {
            if (enemies[i]) 
            {
                if (enemies[i] == e) 
                {
                    enemies[i] = null;
                    Destroy(e);
                    break;
                }
            }
        }
    }
    public bool AreOpenSpots() 
    {
        bool result = false;
        if (enemiesAdded < 3) 
        {
            for (int i = 0; i < 3; i++)
            {
                if (!enemies[i])
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }
}
