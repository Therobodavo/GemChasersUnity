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
    public GameObject enemy1HealthBarImage;
    public GameObject enemy2HealthBarImage;
    public GameObject enemy3HealthBarImage;
    public GameObject enemy1UI;
    public GameObject enemy2UI;
    public GameObject enemy3UI;
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

    private int[] possibleAngles = {0,-60,-120,-180,-240,-300};
    // Start is called before the first frame update
    void Start()
    {
        halfWheel = outerWheel.GetComponent<Image>().rectTransform.rect.width * outerWheel.GetComponent<Image>().rectTransform.localScale.x;
        spinBtn.GetComponent<Button>().onClick.AddListener(Spin);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angleVector = (Input.mousePosition - outerWheel.transform.position);
        float angle = Vector3.Angle(angleVector.normalized,Vector3.left);
        currentHoverAngle = -1;

        UpdatePlayerBars();
        UpdateEnemyBars();

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
                    currentSelectedAngle = currentHoverAngle;
                    selected.transform.rotation = Quaternion.AngleAxis(currentSelectedAngle, Vector3.forward);

                    if (!selected.activeSelf)
                    {
                        selected.SetActive(true);

                        spinBtn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Lock In";
                        spinBtn.GetComponent<Button>().onClick.RemoveAllListeners();
                        spinBtn.GetComponent<Button>().onClick.AddListener(LockIn);
                        spinBtn.SetActive(true);
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
        spinBtn.SetActive(false);
        player.GetComponent<PlayerManager>().currentBattleArea.GetEnemyScripts()[1].TakeDamage(100);
        spinBtn.transform.GetChild(0).GetComponent<Text>().text = "Spin";
    }
    private void UpdatePlayerBars() 
    {
        playerHealthBarImage.GetComponent<Image>().fillAmount = player.GetComponent<PlayerManager>().GetHealth() / 100;
        playerEnergyBarImage.GetComponent<Image>().fillAmount = player.GetComponent<PlayerManager>().GetEnergy() / 100;
    }
    private void UpdateEnemyBars() 
    {
        PlayerManager playerScript;
        if (player) 
        {
            playerScript = player.GetComponent<PlayerManager>();
            if (playerScript) 
            {
                if (playerScript.currentBattleArea) 
                {
                    bool[] areEnemiesAlive = playerScript.currentBattleArea.enemiesAlive();
                    UpdateEnemyBar(areEnemiesAlive, enemy1UI, enemy1HealthBarImage, 0);
                    UpdateEnemyBar(areEnemiesAlive, enemy2UI, enemy2HealthBarImage, 1);
                    UpdateEnemyBar(areEnemiesAlive, enemy3UI, enemy3HealthBarImage, 2);
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
}
