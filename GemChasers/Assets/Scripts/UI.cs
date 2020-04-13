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
    public GameObject playerEnergyBarImage;

    public GameObject[] enemyHealthBarImages;
    public GameObject[] enemyHealthBarText;
    public GameObject[] enemyEnergyBarImages; 
    public GameObject[] enemyEnergyBarText;
    public GameObject[] enemyButtons;
    public GameObject[] enemyUI;

    public GameObject[] gems;
    public GameObject[] buffs;
    public GameObject hover;
    public GameObject selected;
    public GameObject spinBtn;

    public GameObject player;

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
    private int selectedEnemyTarget = -1;

    private int[] possibleAngles = {0,-60,-120,-180,-240,-300};

    // Start is called before the first frame update
    private void Awake()
    {
        enemyHealthBarImages = GameObject.FindGameObjectsWithTag("EnemyBattleHealthBarFill");
        enemyHealthBarText = GameObject.FindGameObjectsWithTag("EnemyBattleHealthText");
        enemyEnergyBarImages = GameObject.FindGameObjectsWithTag("EnemyBattleEnergyBarFill");
        enemyEnergyBarText = GameObject.FindGameObjectsWithTag("EnemyBattleEnergyText");
        enemyButtons = GameObject.FindGameObjectsWithTag("EnemyBattleButton");
        enemyUI = GameObject.FindGameObjectsWithTag("EnemyBattleHUD");
    }
    void Start()
    {
        battleUI.SetActive(false);

        halfWheel = outerWheel.GetComponent<Image>().rectTransform.rect.width * outerWheel.GetComponent<Image>().rectTransform.localScale.x;
        switchButton("Spin", true, Spin);
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
            if (Input.mousePosition.y >= outerWheel.transform.position.y && angleVector.magnitude <= (halfWheel / 2))
            {
                isHovering = true;
                if (angle >= 0 && angle <= 60)
                {
                    currentHoverAngle = 60;
                }
                else if (angle > 60 && angle <= 120)
                {
                    currentHoverAngle = 0;
                }
                else
                {
                    currentHoverAngle = -60;
                }

                if (Input.GetMouseButtonDown(0) && !isLocked && currentHoverAngle != currentSelectedAngle)
                {
                    isSelected = true;
                    SwitchEnemyButtons(true);
                    currentSelectedAngle = currentHoverAngle;
                    selected.transform.rotation = Quaternion.AngleAxis(currentSelectedAngle, Vector3.forward);

                    if (!selected.activeSelf)
                    {
                        selected.SetActive(true);
                    }
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

        spinBtn.SetActive(false);
    }

    //Button function to lock wheel in with move selected
    private void LockIn() 
    {
        isLocked = true;

        //Temporary damage dealt
        if (player.GetComponent<PlayerManager>().currentBattleArea && selectedEnemyTarget != -1) 
        {
            if (player.GetComponent<PlayerManager>().currentBattleArea.GetEnemiesAlive()[selectedEnemyTarget])
            {
                player.GetComponent<PlayerManager>().currentBattleArea.GetEnemyScripts()[selectedEnemyTarget].TakeDamage(50);
            }
        }

        switchButton("Spin", true, Spin);
        ResetTurn();
    }
    private void UpdatePlayerBars() 
    {
        playerHealthBarImage.GetComponent<Image>().fillAmount = player.GetComponent<PlayerManager>().GetHealth() / 100;
        playerEnergyBarImage.GetComponent<Image>().fillAmount = player.GetComponent<PlayerManager>().GetEnergy() / 100;
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
                        UpdateEnemyBar(areEnemiesAlive, enemyUI[i], enemyHealthBarImages[i], i);
                    }
                }
            }
        }
    }
    private void UpdateEnemyBar(bool[] alive,GameObject UI, GameObject UIImage, int index) 
    {
        if (alive[index])
        {
            if (UI.activeSelf)
            {
                //Update Bar
                Enemy enemyScript = player.GetComponent<PlayerManager>().currentBattleArea.GetEnemyScripts()[index];
                if (enemyScript) 
                {
                    UIImage.GetComponent<Image>().fillAmount = enemyScript.GetHealth() / 100;
                    UIImage.GetComponent<Image>().fillAmount = enemyScript.GetHealth() / 100;
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
        SwitchEnemyButtons(false);
    }
    public void SelectEnemy(int enemyNum) 
    {
        selectedEnemyTarget = enemyNum;
    }
    private void SwitchEnemyButtons(bool state) 
    {
        for (int i = 0; i < 3; i++) 
        {
            enemyButtons[i].GetComponent<Button>().interactable = state;
        }
    }
}
