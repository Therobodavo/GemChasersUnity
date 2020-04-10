using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] enemies = new GameObject[3];
    public GameObject[] spots = new GameObject[6];
    private GameObject player;
    public enum GameState {BattleStartUp,PlayerMoveSelection,PlayerAttacking,Enemy1Attacking,Enemy2Attacking,Enemy3Attacking};
    private GameState currentBattleState = GameState.BattleStartUp;
    void Start()
    {
        player = GameObject.Find("Player");

        //Sets player
        SetObject(player, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
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
                    canAdd = true;
                    obj.GetComponent<Enemy>().inBattle = true;
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
    public bool[] enemiesAlive() 
    {
        bool[] enemiesAlive = { false, false, false };

        for (int i = 0; i < 3; i++) 
        {
            if (enemies[i]) 
            {
                enemiesAlive[i] = true;
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
}
