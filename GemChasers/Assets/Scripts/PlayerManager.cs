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
        battle.GetComponent<BattleArea>().SetSpawnPoint();
        battle.GetComponent<BattleArea>().AddCreature(hitEnemy,true);
    }
}
