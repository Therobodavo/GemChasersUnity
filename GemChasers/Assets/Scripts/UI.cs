using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject outerWheel;
    public GameObject innerWheel;
    public GameObject wheel;
    public GameObject battleUI;
    public GameObject wanderingUI;
    public GameObject playerHealthBarImage;
    public GameObject playerHealthBarText;
    public GameObject playerEnergyBarImage;
    public GameObject playerEnergyBarText;
    public GameObject playerCurrentTypeImage;
    public GameObject moveInfoHUD;
    public GameObject[] moveInfoHUDParts;

    public GameObject[] enemyHealthBarImages;
    public GameObject[] enemyHealthBarText;
    public GameObject[] enemyEnergyBarImages; 
    public GameObject[] enemyEnergyBarText;
    public GameObject[] enemyButtons;
    public GameObject[] enemyUI;
    public GameObject[] enemyTypeIcon;

    public GameObject[] gems;
    public GameObject[] buffs;

    public GameObject hover;
    public GameObject selected;
    public GameObject spinBtn;
    private GameObject playerInputUI;
    public GameObject player;
    public GameObject selectTargetText;

    private float halfWheel = 0;
    private float currentHoverAngle = -1;
    private float currentSelectedAngle = -1;

    private bool isHovering = false;
    private bool isSelected = false;
    private bool isLocked = false;
    private bool isSpinning = false;
    private bool hasSpun = false;

    private float spinTime = 2;
    private float startSpinTime = 0;

    private int outerIndex = -1;
    private int innerIndex = -1;
    public int selectedEnemyTarget = -1;

    private int[] possibleAngles = {0,-60,-120,-180,-240,-300};
    private int currentMoveIndex = -1;
    // Start is called before the first frame update
    private void Awake()
    {
        enemyHealthBarImages = GameObject.FindGameObjectsWithTag("EnemyBattleHealthBarFill");
        enemyHealthBarText = GameObject.FindGameObjectsWithTag("EnemyBattleHealthText");
        enemyEnergyBarImages = GameObject.FindGameObjectsWithTag("EnemyBattleEnergyBarFill");
        enemyEnergyBarText = GameObject.FindGameObjectsWithTag("EnemyBattleEnergyText");
        enemyButtons = GameObject.FindGameObjectsWithTag("EnemyBattleButton");
        enemyUI = GameObject.FindGameObjectsWithTag("EnemyBattleHUD");
        playerInputUI = GameObject.FindGameObjectWithTag("PlayerBattleInputUI");
        enemyTypeIcon = GameObject.FindGameObjectsWithTag("EnemyBattleElementTypeIcon");
    }
    void Start()
    {
        battleUI.SetActive(false);
        halfWheel = outerWheel.GetComponent<Image>().rectTransform.rect.width * outerWheel.GetComponent<Image>().rectTransform.localScale.x;
        switchButton("Spin", true, Spin);
    }
    public void SetUpWheel() 
    {
        for (int i = 0; i < buffs.Length; i++) 
        {
            buffs[i].transform.GetChild(0).GetComponent<Image>().sprite = player.GetComponent<PlayerManager>().wheelBuffs[i].buffImage;
        }
        for (int i = 0; i < gems.Length; i++)
        {
            gems[i].transform.GetChild(0).GetComponent<Image>().sprite = player.GetComponent<PlayerManager>().wheelGems[i].gemImage;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 angleVector = (Input.mousePosition - outerWheel.transform.position);
        float angle = Vector3.Angle(angleVector.normalized,Vector3.left);
        currentHoverAngle = -1;

        UpdatePlayerBars();
        UpdateEnemyUI();

        if (hasSpun && !isSpinning)
        {
            if (isHovering || isSelected)
            {
                int indexToUse = -1;
                if (isHovering)
                {
                    //Set HUD info to hover move
                    indexToUse = currentMoveIndex;
                }
                else 
                {
                    if (isSelected) 
                    {
                        //Set HUD info to selected move
                        indexToUse = player.GetComponent<PlayerManager>().selectedMoveIndex;
                    }
                }
                //Debug.Log("MOVE BUFF 2: " + ((IType.BuffType)player.GetComponent<PlayerManager>().moves[indexToUse].buffs[1].GetBuffID()).ToString());
                //SET HUD
                moveInfoHUDParts[1].GetComponent<Text>().text = ((IType.BuffType)player.GetComponent<PlayerManager>().moves[indexToUse].buffs[0].GetBuffID()).ToString();
                moveInfoHUDParts[2].GetComponent<Text>().text = ((IType.BuffType)player.GetComponent<PlayerManager>().moves[indexToUse].buffs[1].GetBuffID()).ToString();
                moveInfoHUDParts[3].GetComponent<Text>().text = ((IType.GemType)player.GetComponent<PlayerManager>().moves[indexToUse].gemType.gemTypeID).ToString();
                moveInfoHUDParts[4].GetComponent<Text>().text = player.GetComponent<PlayerManager>().moves[indexToUse].GetEnergyCost().ToString();
                moveInfoHUD.SetActive(true);
            }
            else 
            {
                moveInfoHUD.SetActive(false);
            }
            if (Input.mousePosition.y >= outerWheel.transform.position.y && angleVector.magnitude <= (halfWheel / 2))
            {
                OnHover(angle);
                if (Input.GetMouseButtonDown(0) && !isLocked && currentHoverAngle != currentSelectedAngle)
                {
                    OnSelected();
                }
                hover.transform.rotation = Quaternion.AngleAxis(currentHoverAngle, Vector3.forward);
            }
            else
            {
                isHovering = false;
            }
            //Hover active checks
            if (!isLocked && isHovering && (isSelected && (currentHoverAngle != currentSelectedAngle) || !isSelected))
            {
                if (!hover.activeSelf)
                {
                    hover.SetActive(true);
                }
            }
            else
            {
                if (hover.activeSelf)
                {
                    hover.SetActive(false);
                }
            }
            if (isSelected && selectedEnemyTarget != -1)
            {
                switchButton("Lock In", true, LockIn);
            }
        }
        else 
        {
            if (hover.activeSelf) 
            {
                hover.SetActive(false);
            }
            if (selected.activeSelf)
            {
                selected.SetActive(false);
            }

            if (hasSpun) 
            {
                if (Time.timeSinceLevelLoad - startSpinTime <= spinTime)
                {
                    wheel.transform.Rotate(new Vector3(0, 0, 100));
                }
                else
                {
                    wheel.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                    isSpinning = false;
                    outerWheel.transform.parent.rotation = Quaternion.AngleAxis(possibleAngles[outerIndex], Vector3.forward);
                    innerWheel.transform.rotation = Quaternion.AngleAxis(possibleAngles[innerIndex], Vector3.forward);
                }
            }
        }
    }

    //Button function to spin wheel
    public void Spin() 
    {
        hasSpun = true;
        isSpinning = true;
        startSpinTime = Time.timeSinceLevelLoad;

        outerIndex = Random.Range(0, 5);
        innerIndex = Random.Range(0, 5);
        player.GetComponent<PlayerManager>().CreateAttacks(outerIndex, innerIndex);


        spinBtn.SetActive(false);
    }

    //Button function to lock wheel in with move selected
    private void LockIn() 
    {
        isLocked = true;

        togglePlayerTurnInput(false);
        player.GetComponent<PlayerManager>().currentBattleArea.currentBattleState = BattleArea.GameState.PlayerMoveSelected;
    }
    public void Pass() 
    {
        player.GetComponent<PlayerManager>().isPassing = true;
        isLocked = true;
        togglePlayerTurnInput(false);
        player.GetComponent<PlayerManager>().currentBattleArea.currentBattleState = BattleArea.GameState.PlayerMoveSelected;
    }
    private void UpdatePlayerBars() 
    {
        playerHealthBarImage.GetComponent<Image>().fillAmount = player.GetComponent<PlayerManager>().GetHealth() / player.GetComponent<PlayerManager>().MAX_HEALTH;
        playerEnergyBarImage.GetComponent<Image>().fillAmount = player.GetComponent<PlayerManager>().GetEnergy() / player.GetComponent<PlayerManager>().MAX_ENERGY;
        playerHealthBarText.GetComponent<Text>().text = ((int)player.GetComponent<PlayerManager>().GetHealth()).ToString();
        playerEnergyBarText.GetComponent<Text>().text = ((int)player.GetComponent<PlayerManager>().GetEnergy()).ToString();

        //Update type
        playerCurrentTypeImage.GetComponent<Image>().sprite = player.GetComponent<PlayerManager>().lm.elementIcons[(int)player.GetComponent<PlayerManager>().currentType];
    }
    private void UpdateEnemyUI() 
    {
        PlayerManager playerScript;
        if (player) 
        {
            playerScript = player.GetComponent<PlayerManager>();
            if (playerScript) 
            {
                if (playerScript.currentBattleArea) 
                {
                    bool[] areEnemiesAlive = playerScript.currentBattleArea.GetEnemiesAlive();
                    for (int i = 0; i < 3; i++) 
                    {
                        UpdateEnemyBar(areEnemiesAlive, enemyUI[i], i);
                    }
                }
            }
        }
    }
    private void UpdateEnemyBar(bool[] alive,GameObject UI, int index) 
    {
        if (alive[index])
        {
            if (UI.activeSelf)
            {
                //Update Bar
                IBattle enemyScript = player.GetComponent<PlayerManager>().currentBattleArea.GetEnemyScripts()[index];
                if (enemyScript) 
                {
                    enemyHealthBarImages[index].GetComponent<Image>().fillAmount = enemyScript.GetHealth() / enemyScript.MAX_HEALTH;
                    enemyEnergyBarImages[index].GetComponent<Image>().fillAmount = enemyScript.GetEnergy() / enemyScript.MAX_ENERGY;
                    enemyHealthBarText[index].GetComponent<Text>().text = ((int)enemyScript.GetHealth()).ToString();
                    enemyEnergyBarText[index].GetComponent<Text>().text = ((int)enemyScript.GetEnergy()).ToString();
                }
            }
            else
            {
                UI.SetActive(true);
            }
        }
        else
        {
            if (UI.activeSelf)
            {
                UI.SetActive(false);
            }
        }
    }
    private void OnHover(float angle) 
    {
        isHovering = true;
        if (angle >= 0 && angle <= 60)
        {
            currentHoverAngle = 60;
            currentMoveIndex = 0;
        }
        else if (angle > 60 && angle <= 120)
        {
            currentHoverAngle = 0;
            currentMoveIndex = 1;
        }
        else
        {
            currentHoverAngle = -60;
            currentMoveIndex = 2;
        }
    }
    private void OnSelected() 
    {
        if (player.GetComponent<PlayerManager>().moves[currentMoveIndex].GetEnergyCost() <= player.GetComponent<PlayerManager>().GetEnergy())
        {
            isSelected = true;
            
            currentSelectedAngle = currentHoverAngle;
            player.GetComponent<PlayerManager>().selectedMoveIndex = currentMoveIndex;
            selected.transform.rotation = Quaternion.AngleAxis(currentSelectedAngle, Vector3.forward);
            if (!selected.activeSelf)
            {
                selected.SetActive(true);
            }

            //If selected move attacks all or move targets self
            if (player.GetComponent<PlayerManager>().moves[player.GetComponent<PlayerManager>().selectedMoveIndex].targetAllSide || player.GetComponent<PlayerManager>().moves[player.GetComponent<PlayerManager>().selectedMoveIndex].isTargetingSelf())
            {
                selectTargetText.SetActive(false);
                SwitchEnemyButtons(false);
                switchButton("Lock In", true, LockIn);
            }
            else 
            {
                //If player needs to select a target
                selectTargetText.SetActive(true);
                SwitchEnemyButtons(true);
                switchButton("Lock In", false, LockIn);
            }
        }
    }
    private void switchButton(string text, bool state, UnityEngine.Events.UnityAction funt = null) 
    {
        if (state)
        {
            spinBtn.transform.GetChild(0).GetComponent<Text>().text = text;
            spinBtn.SetActive(state);
        }
        else 
        {
            spinBtn.SetActive(state);
            spinBtn.transform.GetChild(0).GetComponent<Text>().text = text;
        }
        if (funt != null) 
        {
            spinBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            spinBtn.GetComponent<Button>().onClick.AddListener(funt);
        }
    }
    public void ResetTurn() 
    {
        selected.SetActive(false);
        hover.SetActive(false);
        isLocked = false;
        isSpinning = false;
        isSelected = false;
        isHovering = false;
        hasSpun = false;
        currentHoverAngle = -1;
        currentSelectedAngle = -1;
        startSpinTime = 0;
        outerIndex = -1;
        innerIndex = -1;
        selectedEnemyTarget = -1;
        player.GetComponent<PlayerManager>().isPassing = false;
        SwitchEnemyButtons(false);
        switchButton("Spin",true,Spin);

        //Make sure everything is active
        togglePlayerTurnInput(true);
    }
    public void SelectEnemy(int enemyNum) 
    {
        selectedEnemyTarget = enemyNum;
        selectTargetText.SetActive(false);
    }
    public void togglePlayerTurnInput(bool state) 
    {
        if (playerInputUI.activeSelf != state) 
        {
            playerInputUI.SetActive(state);
        }
    }
    private void SwitchEnemyButtons(bool state) 
    {
        for (int i = 0; i < 3; i++) 
        {
            enemyButtons[i].GetComponent<Button>().interactable = state;
        }
    }
}
