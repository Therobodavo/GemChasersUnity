using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBattle : MonoBehaviour
{
    public bool inBattle = false;
    protected float health = 100;
    protected float energy = 100;

    protected float speedStat = 1;
    protected float defenseStat = 1;
    protected float attackStat = 1;

    IType.ElementType currentType = IType.ElementType.NoType;

    public BattleArea currentBattleArea;
    protected virtual void Start()
    {
        
    }


    protected virtual void Update()
    {
        if (!isAlive() && currentBattleArea)
        {
            OnDeath();
        }
    }
    protected virtual void OnDeath() 
    {
        currentBattleArea.RemoveEnemy(gameObject);
    }
    public virtual float GetHealth()
    {
        return health;
    }
    public virtual float GetEnergy()
    {
        return energy;
    }
    public virtual void TakeDamage(float damage)
    {
        if (damage > 0)
        {
            if (health - damage < 0)
            {
                health = 0;
            }
            else
            {
                health -= damage;
            }
        }
    }
    public virtual void TakeEnergy(float usage)
    {
        if (usage > 0)
        {
            if (energy - usage < 0)
            {
                energy = 0;
            }
            else
            {
                energy -= usage;
            }
        }
    }
    public virtual bool isAlive()
    {
        bool result = false;
        if (health > 0)
        {
            result = true;
        }

        return result;
    }

    public void AttackTarget(IBattle target, float damage) 
    {
        if (target) 
        {
            target.TakeDamage(damage);
        }
    }
    public virtual void UseMove()
    {
        if (!isAlive()) 
        {
            return;
        }
    }
    public virtual bool hasEnoughEnergy(float moveCost)
    {
        bool result = false;
        if (energy >= moveCost)
        {
            result = true;
        }

        return result;
    }
    public virtual float GetSpeed() 
    {
        return speedStat;
    }
}
