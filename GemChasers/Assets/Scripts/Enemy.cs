using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : IBattle
{
    public UI playerUI;
    public PlayerManager player;
    public IType.EnemyType speciesType;
    public Path path = null;
    public GameObject currentPathTarget;
    public float spawnTime;
    public float despawnTime = 10f;
    public override void Start()
    {
        base.Start();
        currentType = IType.ElementType.NoType;
        playerUI = GameObject.Find("Canvas").GetComponent<UI>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (path != null && currentPathTarget != null && !inBattle) 
        {
            transform.position += (currentPathTarget.transform.position - transform.position).normalized * 5 * Time.deltaTime;
            if (Time.timeSinceLevelLoad - spawnTime >= despawnTime)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
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
        else if (other.tag == "PathBlock") 
        {
            currentPathTarget = GetNextSpot(other.gameObject);
        }
    }
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
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BattleArea" && !inBattle)
        {
            BattleArea area = other.GetComponent<BattleArea>();
            if (area.AreOpenSpots())
            {
                currentBattleArea = area;
                currentBattleArea.AddCreature(gameObject, true);
                inBattle = true;
            }
        }
    }
    public override void UseMove()
    {
        base.UseMove();
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
    public override void OnDeath()
    {
        base.OnDeath();
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
