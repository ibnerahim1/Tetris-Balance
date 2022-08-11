using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy gData;

    private void Awake()
    {
        gData = FindObjectOfType<DontDestroy>();
        if (gData != null && gData != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gData = GetComponent<DontDestroy>();
            DontDestroyOnLoad(gameObject);
        }
    }
}