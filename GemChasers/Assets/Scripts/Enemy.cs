using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IBattle
{
    protected override void Start()
    {
        base.Start();
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
            TakeEnergy(10);
            AttackTarget(currentBattleArea.player.GetComponent<PlayerManager>(),10);
        }
    }
}
