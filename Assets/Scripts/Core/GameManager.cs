using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Loading Scene Settings")] 
    public bool isLoading; 
    public GameObject LoadingScreenObj;
    public LoadingScreen LoadingScreen;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    private float totalSceneProgress;
    public AudioMixer audioMixer;



    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void FirstInitialize()
    {
        Debug.Log("This message will output BEFORE awake - Load sound/video settings here");
        
        SceneManager.LoadScene("PersistentScene", LoadSceneMode.Additive);
        
        //
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying GameManager");
            Destroy(this.gameObject);
        }
        else
            Instance = this;
        InitializePlayerProfile();
        LoadingScreen = LoadingScreenObj.GetComponent<LoadingScreen>(); 
        
    }

    public void InitializePlayerProfile()
    {
        string profilePath = Application.persistentDataPath + "/SaveData/PlayerProfile.save";
        if (File.Exists(profilePath))
        {
            Debug.Log("Loading data at " + profilePath);
            SaveData.current = (SaveData)SerializationManager.Load(profilePath);
            Debug.Log("Fullscreen on savedata is " + SaveData.current.profile.isFullScreen + " So setting fullscreen status to that");
            Screen.fullScreen = SaveData.current.profile.isFullScreen;
        }
        else
        {
            CreateNewProfile();
        }
    }

    private void Start()
    {
        
    }

    private static void CreateNewProfile()
    {
        Debug.Log("Creating new Save data at " + Application.persistentDataPath + "/SaveData/PlayerProfile.save");
        SaveData newSaveData = new SaveData();
        SerializationManager.Save(Application.persistentDataPath + "/SaveData/PlayerProfile.save", newSaveData);
    }

    public void LoadScene(string targetScene)
    {
        LoadingScreenObj.SetActive(true);
        LoadingScreen.SetScreenHider(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name));
        scenesLoading.Add(SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive));

        StartCoroutine(SceneLoadProgress_Co(targetScene));
    }

    public IEnumerator SceneLoadProgress_Co(string targetScene)
    {
        Debug.Log("starting scene load progress");
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                Debug.Log("Loading scene still...");
                yield return null;
            }
        }

        Debug.Log("Scene is done loading");
        Debug.Log("Setting Active scene to " + targetScene);
        
        while (!Input.GetKey(KeyCode.Space))
        {
            yield return null;
        }
        LoadingScreen.autosaveImage.gameObject.SetActive(false);
        
        
        LoadingScreen.SetScreenHider(false);
        yield return new WaitForSeconds(2f);
        LoadingScreenObj.SetActive(false);
        LoadingScreen.loadingCamera.gameObject.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

        
    }
}
