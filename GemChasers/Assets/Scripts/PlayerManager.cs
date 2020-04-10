using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    private float currentRotation = 0;
    public int speed = 10;
    public GameObject battleArea;
    private bool inBattle = false;
    public BattleArea currentBattleArea;
    private float health = 100;
    private float energy = 100;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!inBattle) 
        {
            float sideMovement = Input.GetAxis("Horizontal");
            float vertMovement = Input.GetAxis("Vertical");
            this.gameObject.transform.position += new Vector3(sideMovement, 0, vertMovement) * Time.deltaTime * speed;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && !inBattle)
        {
            //Look for nearest spawn point

            CreateBattleArea(other.gameObject);
            inBattle = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !inBattle)
        {
            //Look for nearest spawn point

            CreateBattleArea(other.gameObject);
            inBattle = true;
        }
    }
    private void CreateBattleArea(GameObject hitEnemy) 
    {
        GameObject battle = Instantiate(battleArea,new Vector3(0,1,0),Quaternion.identity);
        currentBattleArea = battle.GetComponent<BattleArea>();
        currentBattleArea.SetSpawnPoint();
        currentBattleArea.AddCreature(hitEnemy,true);
    }

    public float GetHealth() 
    {
        return health;
    }
    public float GetEnergy() 
    {
        return energy;
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
    public void TakeEnergy(float usage) 
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
    public bool hasEnoughEnergy(float moveCost) 
    {
        bool result = false;
        if (energy >= moveCost) 
        {
            result = true;
        }

        return result;
    }
    public bool isAlive() 
    {
        bool result = false;
        if (health >= 0)
        {
            result = true;
        }

        return result;
    }
}
