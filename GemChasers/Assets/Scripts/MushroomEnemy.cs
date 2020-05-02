using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : Enemy
{
    public override void Start()
    {
        base.Start();
        IconHUD = lm.MushroomIconHUD;
        currentType = IType.ElementType.Forest;
        speciesType = IType.EnemyType.Mushroom;
    }
    public override void SetStats()
    {
        MAX_HEALTH = 12;
        MAX_ENERGY = 100;
        baseStats[0] = 13;
        baseStats[1] = 8;
        baseStats[2] = 4;
    }
}
