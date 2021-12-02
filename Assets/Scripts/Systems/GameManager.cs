using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public EnemyLibrary enemyLibrary;
    public AudioManager audioManager;

    public List<Sprite> upgradeLevels;

    public int currentWorldId = 0;
    //list containing different level stats and enemies waves 
    internal List<WorldInfo> worldList = new List<WorldInfo>();
    internal bool initiated = false;
    [DllImport("__Internal")]
    private static extern bool IsMobile();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        AudioManager.instance.Play("mainMenuSong");//Pls change asap
        if (isMobile())
        {
            Console.WriteLine("is mobile");
        }
        else
        {
            Console.WriteLine("not mobile");
        }
    }

    public void SetNextLevelWorld(int nextWorld)
    {
        currentWorldId = nextWorld;
    }

    public bool IncreaseCurrentWorldId()
    {
        if (currentWorldId >= worldList.Count - 1)
            return false;

        currentWorldId++;
        return true;
    }

    public WorldInfo GetCurrentWorld()
    {
        return worldList[currentWorldId];
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetColor("_Tint", Color.white);
    }

    public bool isMobile()
    {
        //Debug.Log(SystemInfo.deviceModel);
#if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobile();
#endif
        return false;

    }
}
