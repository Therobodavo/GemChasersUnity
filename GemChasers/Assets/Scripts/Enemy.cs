using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inBattle = false;
    private float health = 100;
    public BattleArea currentBattleScript;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive() && currentBattleScript) 
        {
            currentBattleScript.RemoveEnemy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BattleArea" && !inBattle) 
        {
            BattleArea area = other.GetComponent<BattleArea>();
            if (area.AreOpenSpots()) 
            {
                currentBattleScript = area;
                currentBattleScript.AddCreature(gameObject, true);
                inBattle = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BattleArea" && !inBattle)
        {
            BattleArea area = other.GetComponent<BattleArea>();
            if (area.AreOpenSpots())
            {
                currentBattleScript = area;
                currentBattleScript.AddCreature(gameObject, true);
                inBattle = true;
            }
        }
    }

    public float GetHealth() 
    {
        return health;
    }
    public void TakeDamage(float damage)
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
    public bool isAlive()
    {
        bool result = false;
        if (health > 0)
        {
            result = true;
        }

        return result;
    }

}
