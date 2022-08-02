using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool isGun;
    public GameObject[] pickupEffect;
    public int gunType = 0;

    private void Start()
    {
        if(isGun)
        {
            gunType = Random.Range(0, 3);
        }
        transform.GetChild(gunType).gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        Instantiate(pickupEffect[gunType], transform.position, transform.rotation);


        Destroy(gameObject);
    }
}
