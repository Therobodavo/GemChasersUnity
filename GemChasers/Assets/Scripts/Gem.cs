using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Gem Class
 * Programmed by David Knolls
 * 
 * Determines type of moves
 */

public class Gem
{
    public int gemTypeID = 0;
    public Sprite gemImage;
    protected LevelManager lm;

    public Gem(IType.GemType gemType) 
    {
        gemTypeID = (int)gemType;
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        if (lm) 
        {
            Sprite temp = lm.gemImages[gemTypeID];
            if (temp) 
            {
                gemImage = temp;
            }
        }
    }

}
