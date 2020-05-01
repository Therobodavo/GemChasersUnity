using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : IBattle
{
    public UI playerUI;
    public PlayerManager player;
    public IType.EnemyType speciesType;
    public override void Start()
    {
        base.Start();
        baseStats[0] = 10;
        baseStats[1] = 1;
        baseStats[2] = 3;
        health = 100;
        energy = 100;
        currentType = IType.ElementType.NoType;
        playerUI = GameObject.Find("Canvas").GetComponent<UI>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
        }
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
