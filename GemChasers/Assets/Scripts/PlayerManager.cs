using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : IBattle
{
    // Start is called before the first frame update
    private float currentRotation = 0;
    public int speed = 10;
    public GameObject battleArea;
    public Camera mainCamera;
    public Camera battleAttackingCamera;
    public Camera battleSelectingCamera;
    public GameObject battleUI;
    public UI UIScript;
    protected override void Start()
    {
        base.Start();
        toggleCamera(0);
        UIScript = GameObject.Find("Canvas").GetComponent<UI>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!inBattle) 
        {
            float sideMovement = Input.GetAxis("Horizontal");
            float vertMovement = Input.GetAxis("Vertical");
            this.gameObject.transform.position += new Vector3(sideMovement, 0, vertMovement) * Time.deltaTime * speed;
        }
    }
    // 0 - Main
    // 1 - Battle Select
    // 2 - Battle Attack
    public void toggleCamera(int cam) 
    {
        if (cam == 0)
        {
            mainCamera.enabled = true;
            battleSelectingCamera.enabled = false;
            battleAttackingCamera.enabled = false;
        }
        else if (cam == 1) 
        {
            battleSelectingCamera.enabled = true;
            battleAttackingCamera.enabled = false;
            mainCamera.enabled = false;
        }
        else if (cam == 2)
        {
            battleAttackingCamera.enabled = true;
            battleSelectingCamera.enabled = false;
            mainCamera.enabled = false;
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
        if (hitEnemy) 
        {
            if (!hitEnemy.GetComponent<IBattle>().inBattle) 
            {
                currentBattleArea.AddCreature(hitEnemy, true);
            }
        }
    }
    protected override void OnDeath()
    {
        //What happens when player dies
        SceneManager.LoadScene("MainScene");
    }
    public override void UseMove()
    {
        base.UseMove();
        if (currentBattleArea)
        {
            int index = UIScript.selectedEnemyTarget;
            GameObject[] battleEnemies = currentBattleArea.GetEnemies();
            if (index >= 0 && index < battleEnemies.Length) 
            {
                if (battleEnemies[index]) 
                {
                    IBattle obj = battleEnemies[index].GetComponent<IBattle>();
                    if (obj) 
                    {
                        TakeEnergy(10);
                        AttackTarget(obj, 100);
                    }
                }    
            }

        }
    }
}
