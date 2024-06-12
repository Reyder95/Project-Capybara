using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Struct that allows for importing prafabs into the inspector for a global lookup table
[System.Serializable]
public struct PrefabStruct
{
    public string prefabKey;
    public GameObject prefab;
}

// Struct that allows for importing images into the inspector for a global lookup table
[System.Serializable]
public struct ImageStruct
{
    public string imageKey;
    public Image image;
}

// The main Game object that handles everything necessary to run the game. 
public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public int currentAbilityIndex;
    public PrefabStruct[] prefabArray;
    public ImageStruct[] imageArray;

    public Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
    public Dictionary<string, Image> imageDictionary = new Dictionary<string, Image>();
    public Dictionary<string, Ability> abilityDictionary = new Dictionary<string, Ability>();

    public GameObject player;
    public GameObject playerPrefab;
    public Player playerInfo;
    public GameObject playerSpawner;

    public GameObject mainCamera;

    public Image abilityImage;

    public GeneralTimer blueWaveTimer;
    public GeneralTimer greenCrescentTimer;
    public GeneralTimer purpleCrystalTimer;
    public GeneralTimer redOrbTimer;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject); // Keeps the game controller loaded at all times

        // Singleton handler
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        SpawnPlayer();

        LoadDictionaries();

        playerInfo = player.GetComponent<Player>();

        playerInfo.InitializePlayer();
    }

    public void SpawnPlayer()
    {
        if (playerSpawner != null)
        {
            GameObject spawnedPlayer = Instantiate(playerPrefab, playerSpawner.transform.position, Quaternion.identity);
            player = spawnedPlayer;
            
            if (mainCamera != null)
            {
                mainCamera.GetComponent<CameraFollow>().target = player.transform;
            }
        }
    }

    public void LoadDictionaries()
    {
        foreach (PrefabStruct structElement in prefabArray)
        {
            prefabDictionary.Add(structElement.prefabKey, structElement.prefab);
        }

        foreach (ImageStruct structElement in imageArray)
        {
            imageDictionary.Add(structElement.imageKey, structElement.image);
        }

        // Load all abilities into global ability dictionary
        abilityDictionary.Add("Blue Wave", new BlueWave(false, imageDictionary["Blue Wave"], prefabDictionary["Blue Wave"]));
        abilityDictionary.Add("Green Crescent", new GreenCrescent(false, imageDictionary["Green Crescent"], prefabDictionary["Green Crescent"]));
        abilityDictionary.Add("Purple Crystal", new PurpleCrystal(false, imageDictionary["Purple Crystal"], prefabDictionary["Purple Crystal"]));
        abilityDictionary.Add("Red Orb", new RedOrb(false, imageDictionary["Red Orb"], prefabDictionary["Red Orb"]));
    }
}
