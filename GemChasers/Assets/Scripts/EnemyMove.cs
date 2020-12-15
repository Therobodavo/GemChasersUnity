using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * EnemyMove Class
 * Programmed by David Knolls
 * 
 * Base code for enemy moves
 * Similar to buffs for player
 */

public class EnemyMove
{
    public float energyCost;
    public IType.MoveType moveType;
    public Enemy user;
    public IType.ElementType moveElement;
    public IType.Stat lingerStat;
    public float damageScale;
    PlayerManager player;
    public EnemyMove(Enemy host, IType.MoveType type,float energy, IType.ElementType element) 
    {
        user = host;
        moveType = type;
        energyCost = energy;
        moveElement = element;
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        damageScale = 1;
    }
    //Trigger usage based off move type
    public void Use() 
    {
        if (moveType == IType.MoveType.InstantAttack)
        {
            OnInstantAttack();
        }
        else if (moveType == IType.MoveType.DelayedAttack)
        {
            OnDelayedAttack();
        }
        else if (moveType == IType.MoveType.InstantPassive)
        {
            OnInstantPassive();
        }
        else if (moveType == IType.MoveType.DelayedPassive) 
        {
            OnDelayedPassive();
        }
    }
    private void BaseMove() 
    {
        user.TakeEnergy(energyCost);
    }

    //Event for Instant Attacks
    public virtual void OnInstantAttack()
    {
        BaseMove();

        float typeMod = 1;
        if (moveElement != IType.ElementType.NoType)
        {
            typeMod = user.statModifiers[(int)moveElement, 0];
        }
        player.TakeDamage(user, (user.baseStats[0] * typeMod * damageScale), false);
    }
    //Event for Instant Passive Attacks
    public virtual void OnInstantPassive()
    {
        BaseMove();
        user.Relax(user.MAX_ENERGY);
    }
    //Event for Delayed Damage Attacks
    public virtual void OnDelayedAttack()
    {
        BaseMove();
        float typeMod = 1;
        if (moveElement != IType.ElementType.NoType)
        {
            typeMod = user.statModifiers[(int)moveElement, 0];
        }
        player.turnDamage.Add(new BattleEffect(user, IType.Stat.Health, (user.baseStats[0] * typeMod * damageScale) / 3, moveElement, 3));
    }
    //Event for Delayed Passive Attacks
    public virtual void OnDelayedPassive()
    {
        BaseMove();
        float typeMod = 1;
        if (moveElement != IType.ElementType.NoType)
        {
            typeMod = user.statModifiers[(int)moveElement, 1];
        }
        player.turnEffects.Add(new BattleEffect(user, IType.Stat.Defense, 1.3f, moveElement, 3));
    }
}
