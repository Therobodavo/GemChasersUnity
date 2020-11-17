using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public IType.PickupType healType;
    float rotateAmount = 75;
    public PickUpSpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateAmount * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            bool toDestroy = false;
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            if (!player.inBattle) 
            {
                if (healType == IType.PickupType.Health)
                {
                    if (player.GetHealth() < player.MAX_HEALTH)
                    {
                        player.Heal(player.MAX_HEALTH);
                        toDestroy = true;

                        if (spawner)
                        {
                            RemoveFromSpawner(spawner.activeHealthPickups);
                        }
                    }
                }
                else
                {
                    if (player.GetEnergy() < player.MAX_ENERGY)
                    {
                        player.Relax(player.MAX_ENERGY);
                        toDestroy = true;

                        if (spawner)
                        {
                            RemoveFromSpawner(spawner.activeEnergyPickups);
                        }
                    }
                }
                if (toDestroy)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public void RemoveFromSpawner(List<GameObject> list) 
    {
        if (spawner)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(this.gameObject))
                {
                    list.RemoveAt(i);
                    break;
                }
            }
        }
    }

}
