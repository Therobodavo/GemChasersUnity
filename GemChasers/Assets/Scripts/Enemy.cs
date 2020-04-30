using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : IBattle
{
    protected override void Start()
    {
        base.Start();
        baseStats[0] = 10;
        baseStats[1] = 1;
        baseStats[2] = 3;
        health = 100;
        energy = 100;
        currentType = IType.ElementType.Heat;
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
}
