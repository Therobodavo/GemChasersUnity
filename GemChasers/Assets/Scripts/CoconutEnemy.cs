using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Coconut Enemy Class
 * Programmed by David Knolls
 * 
 * Custom stats/move choices for Coconut Enemy, inherited from Enemy
 */

public class CoconutEnemy : Enemy
{

    public override void Start()
    {
        base.Start();

        IconHUD = lm.CoconutIconHUD;
        speciesType = IType.EnemyType.Coconut;
    }

    //Set custom stats for coconut enemy
    public override void SetStats()
    {
        MAX_HEALTH = 10;
        MAX_ENERGY = 100;
        baseStats[0] = 15;
        baseStats[1] = 6;
        baseStats[2] = 7;

        //Create different enemy moves
        moves = new List<EnemyMove>();
        EnemyMove move = new EnemyMove(this,IType.MoveType.InstantAttack,30,currentType);
        move.damageScale = 1.3f;
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.InstantAttack, 50, currentType);
        move.damageScale = 2f;
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.DelayedAttack, 40, currentType);
        move.damageScale = 1.5f;
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.InstantPassive, 40, currentType);
        moves.Add(move);
        move = new EnemyMove(this, IType.MoveType.DelayedPassive, 25, currentType);
        moves.Add(move);
    }

    //Event to pick which move to use
    public override void UseMove()
    {
        if (currentBattleArea && isAlive()) 
        {
            int pickMove = Random.Range(0,moves.Count);
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
