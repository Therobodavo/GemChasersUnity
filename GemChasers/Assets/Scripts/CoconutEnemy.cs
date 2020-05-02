using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoconutEnemy : Enemy
{
    public override void Start()
    {
        base.Start();

        IconHUD = lm.CoconutIconHUD;
        currentType = IType.ElementType.Heat;
        speciesType = IType.EnemyType.Coconut;
    }
    public override void SetStats()
    {
        MAX_HEALTH = 10;
        MAX_ENERGY = 100;
        baseStats[0] = 15;
        baseStats[1] = 6;
        baseStats[2] = 7;
    }
}
