using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using MoreMountains.NiceVibrations;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
    public Transform levels;
    public Transform[] block;
    public Color[] colors;
    public bool gameStarted;
    public int level, blockCount, maxBlock;

    [Header("UI")]
    public Image hand;
    public GameObject menuPanel, gamePanel, winPanel, failPanel, tut1, tut2;
    public TextMeshProUGUI blockText, levelText;
    public enum hapticTypes{ soft, success, medium, fail};
    public hapticTypes haptics;

    [HideInInspector]
    public Rigidbody currBlock;

    private Camera cam;
    private bool once;
    private Transform currLevel;
    private Vector3 prevPos, mousePos, mousePos1, offset;
    private float time, tutTime;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        level = PlayerPrefs.HasKey("level")? PlayerPrefs.GetInt("level") : 1;
        //currLevel = levels.GetChild(level < 11 ? level - 1 : Random.Range(3, 10));
        //currLevel.gameObject.SetActive(true);
        maxBlock = level < 3 ? 4 : (level < 5 ? 5 : (level < 9 ? 6 : (level < 13 ? 7 : 7 + ((level - 15) / 3))));
        blockText.text = blockCount + "/" + maxBlock.ToString();
        blockText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = blockCount + "/" + maxBlock.ToString();

        levelText.text = "LEVEL " + level;
        levelText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "LEVEL " + level;
        cam.backgroundColor = Color.HSVToRGB(Random.Range(0f, 1f), 0.05f, 1);
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level ", level);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1))
            Restart();
        if (Input.GetKeyDown("b"))
            Debug.Break();
        if (Input.GetKeyDown("d"))
            PlayerPrefs.DeleteKey("level");
        if (Input.GetKeyDown("n"))
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        if (Input.GetKeyDown("w"))
            LevelComplete();
        if (Input.GetKeyDown("f"))
            LevelFail();
#endif
        if (gameStarted)
        {
            time += Time.deltaTime;
            if (currBlock)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    time = 0;
                    mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
                    prevPos = currBlock.position;
                }
                if (Input.GetMouseButton(0))
                {
                    mousePos1 = cam.ScreenToViewportPoint(Input.mousePosition);
                    offset = mousePos1 - mousePos;
                    currBlock.MovePosition(new Vector3(prevPos.x + (offset.x * 5), currBlock.position.y, currBlock.position.z));
                    print(offset.x);
                }
                if (Input.GetMouseButtonUp(0) && time < 0.2f && offset.sqrMagnitude < 0.01f)
                {
                    currBlock.transform.eulerAngles += Vector3.forward * 90;
                }
            }
        }
        else if (!once && Input.GetMouseButtonDown(0))
        {
            once = true;
            gameStarted = true;
            menuPanel.SetActive(false);
            SpawnBlock();
            tutTime = Time.timeSinceLevelLoad;
        }
        if (level < 3 && gameStarted)
        {
            tut1.SetActive(Time.timeSinceLevelLoad < tutTime + 3);
            tut2.SetActive(Time.timeSinceLevelLoad < tutTime + 5 && Time.timeSinceLevelLoad > tutTime + 3.5f);
        }
    }

    public void LevelComplete()
    {
        if (gameStarted)
        {
            gameStarted = false;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level ", level);
            level++;
            PlayerPrefs.SetInt("level", level);
            HapticManager(hapticTypes.success);

            winPanel.SetActive(true);
            winPanel.transform.GetChild(0).DOLocalMove(new Vector3(0, 500, 0), 0.2f).SetEase(Ease.OutBack);
        }
    }
    public void LevelFail()
    {
        if (gameStarted)
        {
            HapticManager(hapticTypes.fail);
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level ", level);

            gameStarted = false;
            failPanel.SetActive(true);
            failPanel.transform.GetChild(0).DOLocalMove(new Vector3(0, 500, 0), 0.2f).SetEase(Ease.OutBack);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
    public void SpawnBlock()
    {
        if (gameStarted)
        {
            if (blockCount < maxBlock)
            {
                blockCount++;
                blockText.text = blockCount.ToString() + "/" + maxBlock.ToString();
                blockText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = blockCount.ToString() + "/" + maxBlock.ToString();

                currBlock = Instantiate(block[Random.Range(0, block.Length)], new Vector3(Random.Range(-1f, 1f), 6.5f, 0), Quaternion.Euler(0, 0, Random.Range(0, 4) * 90)).GetComponent<Rigidbody>();
                currBlock.GetComponent<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];
                prevPos = currBlock.position;
                if (Input.GetMouseButtonDown(0))
                {
                    mousePos = cam.ScreenToViewportPoint(Input.mousePosition);
                }
            }
            else
                Invoke("LevelComplete", 2);
        }
    }

    public void HapticManager(hapticTypes types)
    {
        switch (types)
        {
            case hapticTypes.soft:
                MMVibrationManager.Haptic(HapticTypes.SoftImpact);
                break;
            case hapticTypes.success:
                MMVibrationManager.Haptic(HapticTypes.Success);
                break;
            case hapticTypes.medium:
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                break;
            case hapticTypes.fail:
                MMVibrationManager.Haptic(HapticTypes.Failure);
                break;

        }
    }
}
