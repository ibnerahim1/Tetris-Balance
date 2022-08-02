using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private GameManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Finish") && gManager.currBlock == GetComponent<Rigidbody>())
        {
            gManager.HapticManager(GameManager.hapticTypes.soft);
            gManager.currBlock = null;
            gManager.Invoke("SpawnBlock", 0.5f);
        }
        if (collision.transform.CompareTag("ground") && gManager.gameStarted)
        {
            gManager.LevelFail();
        }
    }
}
