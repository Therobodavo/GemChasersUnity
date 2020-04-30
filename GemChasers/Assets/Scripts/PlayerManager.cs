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
    public GameObject modelWandering;
    public GameObject modelBattle;

    public UI UIScript;

    public Buff[] wheelBuffs;
    public Gem[] wheelGems;
    public PlayerAttack[] moves;
    public int selectedMoveIndex = -1;
    protected override void Start()
    {
        base.Start();
        toggleCamera(0);
        baseStats[0] = 20;
        baseStats[1] = 10;
        baseStats[2] = 6;
        UIScript = GameObject.Find("Canvas").GetComponent<UI>();
        wheelBuffs = new Buff[12];
        wheelGems = new Gem[6];
        moves = new PlayerAttack[3];

        //Default Buffs
        wheelBuffs[0] = new StrengthBuff();
        wheelBuffs[1] = new SpeedBuff();
        wheelBuffs[2] = new StrengthBuff();
        wheelBuffs[3] = new SplitBuff();
        wheelBuffs[4] = new HealBuff();
        wheelBuffs[5] = new LingerBuff();
        wheelBuffs[6] = new StrengthBuff();
        wheelBuffs[7] = new LingerBuff();
        wheelBuffs[8] = new RelaxBuff();
        wheelBuffs[9] = new LingerBuff();
        wheelBuffs[10] = new HealBuff();
        wheelBuffs[11] = new SpeedBuff();

        wheelGems[0] = new Gem(IType.GemType.Breeze);
        wheelGems[1] = new Gem(IType.GemType.Forest);
        wheelGems[2] = new Gem(IType.GemType.Heat);
        wheelGems[3] = new Gem(IType.GemType.Music);
        wheelGems[4] = new Gem(IType.GemType.Space);
        wheelGems[5] = new Gem(IType.GemType.Water);
        UIScript.SetUpWheel();
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
    public void CreateAttacks(int buffIndex, int gemIndex) 
    {
        for (int i = 0; i < 3; i++) 
        {
            int firstBuff = (buffIndex * 2) - (2 * i);
            if (firstBuff < 0) 
            {
                firstBuff += wheelBuffs.Length;
            }
            int secondBuff = (firstBuff + 1);
            int gemI = gemIndex - i;
            if (gemI < 0) 
            {
                gemI += 6;
            }
            moves[i] = new PlayerAttack(this, wheelBuffs[firstBuff], wheelBuffs[secondBuff],wheelGems[gemI]);
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
    public void toggleModel(int model) 
    {
        if (model == 0)
        {
            modelWandering.SetActive(true);
            modelBattle.SetActive(false);
        }
        else if (model == 1) 
        {
            modelBattle.SetActive(true);
            modelWandering.SetActive(false);
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
            moves[selectedMoveIndex].Use();
        }
    }
    public override float GetSpeed() 
    {
        float speedTurn = 0;
        if (!isPassing)
        {
            PlayerAttack move = moves[selectedMoveIndex];
            speedTurn = baseStats[2] + statModifiers[move.gemType.gemTypeID, 2];
        }
        else 
        {
            float mod = 0;
            if (currentType != IType.ElementType.NoType) 
            {
                mod = statModifiers[(int)currentType, 2];
            }
            speedTurn = baseStats[2] + mod;
        }
        

        return speedTurn;
    }
}
