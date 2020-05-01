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
    }
}
