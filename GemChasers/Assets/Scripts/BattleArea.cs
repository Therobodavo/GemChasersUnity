using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * BattleArea Class
 * Programmed by David Knolls
 * 
 * Main code for a battle area (combat)
 */

public class BattleArea : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] enemies = new GameObject[3];
    public GameObject[] spots = new GameObject[6];
    public GameObject player;
    private GameObject battleUI;
    public GameObject enemySelector;
    public Material whiteMat;
    public Material yellowMat;
    private UI mainUI;
    private int enemiesAdded = 0;
    public enum GameState {BattleStartUp,PlayerStartTurn,PlayerMoveSelection,PlayerMoveSelected,AttackPhase,BattleEnd};
    public GameState currentBattleState = GameState.BattleStartUp;

    private List<IBattle> turnOrder;
    private float attackStartTime = 0;
    private bool attackStart = false;
    private int turnIndex = 0;
    private PlayerManager manager;
    void Start()
    {
        whiteMat = Resources.Load("Materials/White", typeof(Material)) as Material;
        yellowMat = Resources.Load("Materials/Yellow", typeof(Material)) as Material;
        turnOrder = new List<IBattle>();
        player = GameObject.Find("Player");
        manager = player.GetComponent<PlayerManager>();
        battleUI = manager.battleUI;
        mainUI = GameObject.Find("Canvas").GetComponent<UI>();
        manager.toggleCamera(1);
        manager.toggleModel(1);

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
            //Decrease Effect turns
            LowerEffects(player.GetComponent<IBattle>());

            //Add Energy to Everyone Alive
            manager.Relax(20);
            manager.isPassing = false;


            for (int i = 0; i < enemies.Length; i++) 
            {
                if (enemies[i]) 
                {
                    IBattle enemyComp = enemies[i].GetComponent<IBattle>();
                    if (enemies[i].GetComponent<IBattle>().isAlive()) 
                    {
                        LowerEffects(enemyComp);
                        enemyComp.Relax(20);
                        enemyComp.isPassing = false;
                    }
                }
            }
            mainUI.ResetTurn();
            currentBattleState = GameState.PlayerMoveSelection;
            manager.toggleCamera(1);
            ResetPlatforms();
        }
        else if (currentBattleState == GameState.PlayerMoveSelected)
        {
            turnOrder = new List<IBattle>();
            turnIndex = 0;
            turnOrder.Add(player.GetComponent<IBattle>());

            //Calculate turn order
            foreach (GameObject e in enemies) 
            {
                if (e) 
                {
                    IBattle eComp = e.GetComponent<IBattle>();
                    //Get e speed
                    float enemySpeed = CalcTurnSpeed(eComp.turnEffects, eComp.GetSpeed());
                    

                    bool added = false;
                    for (int i = turnOrder.Count - 1; i >= 0; i--)
                    {
                        //Math to determine move order
                        float otherSpeed = CalcTurnSpeed(turnOrder[i].turnEffects, turnOrder[i].GetComponent<IBattle>().GetSpeed());

                        if (enemySpeed <= otherSpeed)
                        {
                            turnOrder.Insert(i + 1, eComp);
                            added = true;
                            break;
                        }
                    }
                    if (!added) 
                    {
                        turnOrder.Insert(0, eComp);
                    }
                }
            }
            attackStart = false;
            currentBattleState = GameState.AttackPhase;
        }
        else if (currentBattleState == GameState.AttackPhase) 
        {

            manager.toggleCamera(2);
            if (!turnOrder[turnIndex] || !turnOrder[turnIndex].isAlive())
            {
                turnOrder.RemoveAt(turnIndex);

                if (turnIndex >= turnOrder.Count)
                {
                    currentBattleState = GameState.PlayerStartTurn;
                    attackStart = false;
                }
            }
            else 
            {
                if (!attackStart)
                {
                    attackStartTime = Time.timeSinceLevelLoad;
                    attackStart = true;
                    ResetPlatforms();
                    if (turnIndex < turnOrder.Count && turnIndex >= 0)
                    {
                        if (turnOrder[turnIndex].isAlive())
                        {
                            SetCurrentAttacking();
                            TriggerTurnDamage();
                            TriggerPassives();
                        }
                    }
                }

                //If some sort of animation is over
                if (Time.timeSinceLevelLoad - attackStartTime > 1)
                {
                    //Run attack
                    if (turnOrder[turnIndex])
                    {
                        if (!turnOrder[turnIndex].isPassing) 
                        {
                            turnOrder[turnIndex].UseMove();
                        }        
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
    }

    //Trigger Debuffs/DoT for given creature
    private void LowerEffects(IBattle obj) 
    {
        for (int j = obj.turnEffects.Count - 1; j >= 0; j--)
        {
            if (obj.turnEffects[j].numUses - 1 <= 0)
            {
                //remove
                obj.turnEffects.RemoveAt(j);
            }
            else
            {
                obj.turnEffects[j].numUses -= 1;
            }
        }
        for (int j = obj.turnDamage.Count - 1; j >= 0; j--)
        {
            if (obj.turnDamage[j].numUses - 1 <= 0)
            {
                //remove
                obj.turnDamage.RemoveAt(j);
            }
            else
            {
                obj.turnDamage[j].numUses -= 1;
            }
        }
    }

    //Trigger Passive effects for all creatures
    private void TriggerPassives() 
    {
        for (int i = 0; i < turnOrder[turnIndex].turnEffects.Count; i++) 
        {
            if (turnOrder[turnIndex].turnEffects[i].statType == IType.Stat.Health) 
            {
                turnOrder[turnIndex].Heal(turnOrder[turnIndex].turnEffects[i].value);
            }
            if (turnOrder[turnIndex].turnEffects[i].statType == IType.Stat.Energy)
            {
                turnOrder[turnIndex].Relax(turnOrder[turnIndex].turnEffects[i].value);
            }
        }
    }

    //Trigger Damage Over Time effects for all creatures
    private void TriggerTurnDamage() 
    {
        for (int i = 0; i < turnOrder[turnIndex].turnDamage.Count; i++) 
        {
            BattleEffect currentEffect = turnOrder[turnIndex].turnDamage[i];
            turnOrder[turnIndex].TakeDamage(currentEffect.attacker, currentEffect.value, false);
        }
    }

    //Calculate the turn speed for a creature based on effects
    private float CalcTurnSpeed(List<BattleEffect> effects, float currentSpeed) 
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].statType == IType.Stat.Speed)
            {
                currentSpeed += effects[i].value;
            }
        }
        return currentSpeed;
    }

    //Event for when battle is over
    private void OnBattleEnd() 
    {
        currentBattleState = GameState.BattleEnd;
        mainUI.ResetTurn();
        battleUI.SetActive(false);
        manager.OnBattleEnd();
        Destroy(gameObject);
    }

    //Get all spots in battle area
    public GameObject[] GetSpots() 
    {
        return spots;
    }

    //Check if enemy spot is valid and set onto enemy data structure
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

    //Set object to spot in battle
    private void SetObject(GameObject obj, int index) 
    {
        if (index >= 0 && index <= 5) 
        {
            obj.transform.position = spots[index].transform.position;
            obj.transform.rotation = spots[index].transform.rotation;
            obj.transform.parent = spots[index].transform;
        }
    }

    //Add Creature passed in to battle
    public bool AddCreature(GameObject obj, bool isEnemy) 
    {
        bool canAdd = false;
        IBattle objComp = obj.GetComponent<IBattle>();
        if (isEnemy)
        {
            for (int i = 0; i < 3; i++) 
            {
                if (!enemies[i]) 
                {
                    
                    enemiesAdded++;
                    canAdd = true;
                    objComp.OnBattleStart(i);
                    objComp.inBattle = true;
                    objComp.currentBattleArea = this;
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

    //Get bool array for enemy spots that are alive
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

    //Get all enemy GameObjects in battle
    public GameObject[] GetEnemies() 
    {
        return enemies;
    }
    
    //Get IBattle scripts of each enemy
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

    //Remove enemy from specific spot of battle area
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

    //Check if enemy spot is free and a new one can join
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
    
    //Set current attacking spot to yellow
    private void SetCurrentAttacking() 
    {
        turnOrder[turnIndex].transform.parent.GetChild(0).GetComponent<MeshRenderer>().material = yellowMat;
    }

    //Set all spots to white in battle
    private void ResetPlatforms() 
    {
        for (int i = 0; i < spots.Length; i++) 
        {
            spots[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = whiteMat;
        }
    }
}
