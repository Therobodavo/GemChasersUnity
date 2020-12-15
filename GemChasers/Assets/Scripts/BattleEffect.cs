using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * BattleEffect Class
 * Programmed by David Knolls
 * 
 * Base Effect that can impact creature in battle
 */

public class BattleEffect
{
    public IType.Stat statType;
    public float value;
    public IType.ElementType elementType;
    public int numUses = 3;
    public IBattle attacker;
    public BattleEffect(IBattle attacker,IType.Stat affectedStat, float val, IType.ElementType element, int turnsActive = 3) 
    {
        statType = affectedStat;
        value = val;
        elementType = element;
        numUses = turnsActive;
        this.attacker = attacker;
        if (numUses < 0) 
        {
            numUses = 0;
        }
    }
}
