using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] enemies = new GameObject[3];
    public GameObject[] spots = new GameObject[6];
    public GameObject player;
    private GameObject battleUI;
    public GameObject enemySelector;

    private UI mainUI;
    private int enemiesAdded = 0;
    public enum GameState {BattleStartUp,PlayerStartTurn,PlayerMoveSelection,PlayerMoveSelected,AttackPhase};
    public GameState currentBattleState = GameState.BattleStartUp;

    private List<IBattle> turnOrder;
    private float attackStartTime = 0;
    private bool attackStart = false;
    private int turnIndex = 0;
    void Start()
    {
        turnOrder = new List<IBattle>();
        player = GameObject.Find("Player");
        battleUI = player.GetComponent<PlayerManager>().battleUI;
        mainUI = GameObject.Find("Canvas").GetComponent<UI>();
        player.GetComponent<PlayerManager>().toggleCamera(1);
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
            OnBattleEnd();
        }

        if (currentBattleState == GameState.PlayerStartTurn)
        {
            mainUI.ResetTurn();
            currentBattleState = GameState.PlayerMoveSelection;
            player.GetComponent<PlayerManager>().toggleCamera(1);
        }
        else if (currentBattleState == GameState.PlayerMoveSelected)
        {
            turnOrder = new List<IBattle>();
            turnOrder.Add(player.GetComponent<IBattle>());

            //Calculate turn order
            foreach (GameObject e in enemies) 
            {
                if (e) 
                {
                    for (int i = turnOrder.Count - 1; i >= 0; i--)
                    {
                        //Math to determine move order
                        if (e.GetComponent<IBattle>().GetSpeed() <= turnOrder[i].GetComponent<IBattle>().GetSpeed())
                        {
                            turnOrder.Insert(i + 1,e.GetComponent<IBattle>());
                            break;
                        }
                    }
                }
            }

            //Show order
            foreach (IBattle obj in turnOrder) 
            {
               // Debug.Log(obj.name + " - " + obj.GetSpeed());
            }
            attackStart = false;
            currentBattleState = GameState.AttackPhase;
        }
        else if (currentBattleState == GameState.AttackPhase) 
        {
            player.GetComponent<PlayerManager>().toggleCamera(2);
            if (!turnOrder[turnIndex]) 
            {
                turnOrder.RemoveAt(turnIndex);
                
                if (turnIndex >= turnOrder.Count)
                {
                    currentBattleState = GameState.PlayerStartTurn;
                    attackStart = false;
                }
            }
            if (!attackStart) 
            {
                attackStartTime = Time.timeSinceLevelLoad;
                attackStart = true;
            }

            //If some sort of animation is over
            if (Time.timeSinceLevelLoad - attackStartTime > 1)
            {
                //Run attack
                if (turnOrder[turnIndex]) 
                {
                    turnOrder[turnIndex].UseMove();
                    turnIndex++;
                }

                attackStart = false;
                if (turnIndex >= turnOrder.Count)
                {
                    currentBattleState = GameState.PlayerStartTurn;
                }

            }
        }


    }
    private void OnBattleEnd() 
    {
        player.GetComponent<PlayerManager>().currentBattleArea = null;
        player.GetComponent<PlayerManager>().inBattle = false;
        mainUI.ResetTurn();
        battleUI.SetActive(false);
        player.GetComponent<PlayerManager>().toggleCamera(0);
        Destroy(gameObject);
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
                    obj.GetComponent<IBattle>().inBattle = true;
                    obj.GetComponent<IBattle>().currentBattleArea = this;
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
                if (enemies[i].GetComponent<IBattle>().isAlive()) 
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
    public IBattle[] GetEnemyScripts() 
    {
        IBattle[] enemyScripts = new IBattle[3];
        for (int i = 0; i < 3; i++) 
        {
            if (enemies[i]) 
            {
                enemyScripts[i] = enemies[i].GetComponent<IBattle>();
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
