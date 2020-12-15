using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Enemy Class
 * Programmed by David Knolls
 * 
 * Basic functions for every enemy inheriting from IBattle
 */

public class Enemy : IBattle
{
    public UI playerUI;
    public PlayerManager player;
    public IType.EnemyType speciesType;
    public Path path = null;
    public GameObject currentPathTarget;
    public float spawnTime;
    public float despawnTime = 10f;
    public List<EnemyMove> moves;
    public override void Start()
    {
        base.Start();
        playerUI = GameObject.Find("Canvas").GetComponent<UI>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //Follow path and despawn after X amount of time IF spawned from a spawner (path != null)
        if (path != null && currentPathTarget != null && !inBattle) 
        {
            transform.position += (currentPathTarget.transform.position - transform.position).normalized * 5 * Time.deltaTime;
            if (Time.timeSinceLevelLoad - spawnTime >= despawnTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //When enemy hits another trigger
    private void OnTriggerEnter(Collider other)
    {
        //If trigger is for BattleArea, attempt to join battle
        if (other.tag == "BattleArea" && !inBattle)
        {
            BattleArea area = other.GetComponent<BattleArea>();
            if (area.AreOpenSpots())
            {
                currentBattleArea = area;
                currentBattleArea.AddCreature(gameObject, true);
                inBattle = true;
            }
            else 
            {
                Destroy(this.gameObject);
            }
        }
        //If trigger is part of path, keep following path
        else if (other.tag == "PathBlock") 
        {
            currentPathTarget = GetNextSpot(other.gameObject);
            transform.rotation = currentPathTarget.transform.rotation;
        }
    }
    //Find next spot in path to follow
    private GameObject GetNextSpot(GameObject current) 
    {
        GameObject result = current;
        for (int i = 0; i < path.spots.Length; i++) 
        {
            if(path.spots[i] == current) 
            {
                if (i + 1 >= path.spots.Length)
                {
                    result = path.spots[0];
                }
                else 
                {
                    result = path.spots[i + 1];
                }
                break;
            }
        }
        return result;
    }
    //If inside of trigger
    private void OnTriggerStay(Collider other)
    {
        //Attempt ot join battle if trigger is BattleArea
        if (other.tag == "BattleArea" && !inBattle)
        {
            BattleArea area = other.GetComponent<BattleArea>();
            if (area.AreOpenSpots())
            {
                currentBattleArea = area;
                currentBattleArea.AddCreature(gameObject, true);
                inBattle = true;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
    //Event to use an attack
    public override void UseMove()
    {
        if (currentBattleArea)
        {
            TakeEnergy(30);

            float typeMod = 1;
            if (currentType != IType.ElementType.NoType)
            {
                typeMod = statModifiers[(int)currentType, 0];
            }
            currentBattleArea.player.GetComponent<PlayerManager>().TakeDamage(this,(baseStats[0] * typeMod), false);
        }
    }
    //Event triggered for enemy when the battle starts
    public override void OnBattleStart(int index)
    {
        base.OnBattleStart(index);
        if (playerUI) 
        {
            if (playerUI.enemyUI[index])
            {
                playerUI.enemyUI[index].transform.GetChild(4).GetComponent<Image>().sprite = IconHUD;
                playerUI.enemyTypeIcon[index].GetComponent<Image>().sprite = lm.elementIcons[(int)currentType];
            }
        }
    }
    //Event triggered for enemy when dead
    public override void OnDeath()
    {
        base.OnDeath();

        //Update quests
        if (player.currentQuest != null) 
        {
            for (int i = 0; i < player.currentQuest.enemyTargets.Count;i++) 
            {
                if (player.currentQuest.enemyTargets[i] == speciesType) 
                {
                    player.currentQuest.numKilled[(int)speciesType]++;
                }
            }
        }
    }
}
