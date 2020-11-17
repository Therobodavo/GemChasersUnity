using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IBattle : MonoBehaviour
{
    public bool inBattle = false;
    protected float health;
    protected float energy;
    public float MAX_HEALTH = 100;
    public float MAX_ENERGY = 100;
    //Attack,Defense,Speed
    public float[] baseStats = {12,1,1};
    public IType.ElementType currentType;

    public int comboCount = 0;
    public BattleArea currentBattleArea;
    public bool isPassing = false;
    public List<BattleEffect> turnEffects;
    public List<BattleEffect> turnDamage;
    public float[,] statModifiers =
{
        {1.3f,0.8f,1},  //Heat
        {1.1f,0.8f,2},  //Breeze
        {0.8f,1.3f,-1}, //Forest
        {1.1f,0.6f,3},  //Music
        {1,1.2f,-1},    //Space
        {1,1,0}         //Water 
    };

    public Sprite IconHUD;
    public LevelManager lm;
    public virtual void Start()
    {
        turnEffects = new List<BattleEffect>();
        turnDamage = new List<BattleEffect>();
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        SetStats();
        health = MAX_HEALTH;
        energy = MAX_ENERGY;
    }

    protected virtual void Update()
    {
        if (!isAlive() && currentBattleArea)
        {
            OnDeath();
        }
    }
    public virtual void SetStats() 
    {
        MAX_HEALTH = 100;
        MAX_ENERGY = 100;
    }
    public virtual void OnDeath() 
    {
        currentBattleArea.RemoveEnemy(gameObject);
    }
    public virtual float GetHealth()
    {
        return health;
    }
    public void Heal(float incomingHealth) 
    {
        if (incomingHealth > 0) 
        {
            if (health + incomingHealth > MAX_HEALTH)
            {
                health = MAX_HEALTH;
            }
            else 
            {
                health += incomingHealth;
            }
        }
    }
    public void Relax(float incomingEnergy) 
    {
        if (incomingEnergy > 0)
        {
            if (energy + incomingEnergy > MAX_ENERGY)
            {
                energy = MAX_ENERGY;
            }
            else
            {
                energy += incomingEnergy;
            }
        }
    }
    public virtual float GetEnergy()
    {
        return energy;
    }
    public virtual void TakeDamage(IBattle attacker,float damage, bool isCombo)
    {
        if (isCombo)
        {
            comboCount++;
            damage *= Mathf.Pow(1.25f,comboCount);
        }
        else
        {
            comboCount = 0;
        }
        damage = CalcResistDamage(attacker, damage);
        Debug.Log("Damage: " + damage);
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
    protected virtual float CalcResistDamage(IBattle attacker, float damage) 
    {
        float typeMod = 1;
        if (currentType != IType.ElementType.NoType)
        {
            typeMod = statModifiers[(int)currentType, 1];
        }
        float defense = baseStats[1] * typeMod;
        if (attacker.currentType == this.currentType)
        {
            //Reduce damage
            defense *= 1.25f;
        }
        for (int i = 0; i < turnEffects.Count; i++) 
        {
            if (turnEffects[i].statType == IType.Stat.Defense) 
            {
                defense *= turnEffects[i].value;
                if (attacker.currentType == turnEffects[i].elementType) 
                {
                    defense *= 1.25f;
                } 
            }
        }
        damage /= defense;

        return damage;
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
        return baseStats[2];
    }
    public virtual void OnBattleStart(int index) 
    {
        turnEffects = new List<BattleEffect>();
        turnDamage = new List<BattleEffect>();

    }
}
