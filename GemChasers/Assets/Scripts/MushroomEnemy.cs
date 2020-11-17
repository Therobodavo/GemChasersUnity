using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : Enemy
{
    public override void Start()
    {
        base.Start();
        IconHUD = lm.MushroomIconHUD;
        speciesType = IType.EnemyType.Mushroom;
    }
    public override void SetStats()
    {
        MAX_HEALTH = 12;
        MAX_ENERGY = 100;
        baseStats[0] = 13;
        baseStats[1] = 8;
        baseStats[2] = 4;
        moves = new List<EnemyMove>();
        EnemyMove move = new EnemyMove(this, IType.MoveType.InstantAttack, 30, currentType);
        move.damageScale = 1.2f;
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.InstantAttack, 50, currentType);
        move.damageScale = 1.7f;
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.DelayedAttack, 40, currentType);
        move.damageScale = 1.5f;
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.InstantPassive, 40, currentType);
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.DelayedPassive, 25, currentType);
        moves.Add(move);
    }
    public override void UseMove()
    {
        if (currentBattleArea && isAlive())
        {
            int pickMove = Random.Range(0, moves.Count);
            Debug.Log(pickMove);
            if (moves[pickMove].energyCost <= energy)
            {
                moves[pickMove].Use();
            }
            else
            {
                isPassing = true;
            }
        }
    }
}
