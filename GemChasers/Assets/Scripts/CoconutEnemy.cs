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
}
