using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class LoadInfo 
{
    // boosters
    static string booster1Type = "onePieceRemove";
    static string booster3Type = "colorBomb";
    static string booster4Type = "switch";
    static string booster5Type = "meteor";
    static string booster6Type = "jumping";
    static string booster7Type = "big";

    static string removeAds = "removeAds";

    public static int booster1No = 2;
    public static int booster3No = 2;
    public static int booster4No = 2;
    public static int booster5No = 2;
    public static int booster6No = 2;
    public static int booster7No = 2;

   
    public static bool adsRemoved = false;

    // stars in each level
    public static int numberOfLevels = 50;

    // sound settings
    public static bool isMusicMute = false;
    public static bool isFXMute = false;

    public static void Awake()
    {
        //Awake();
        LoadSoundSettings("music");
        LoadSoundSettings("fx");
        LoadNonConsumables(removeAds);
    }

    public static void LoadData()
    {
        Load(booster1Type);
        Load(booster3Type);
        Load(booster4Type);
        Load(booster5Type);
        Load(booster6Type);
        Load(booster7Type);
              
        //Debug.Log(Application.persistentDataPath);

    }

    public static int StarsInLevel(int i)
    {
        if(i < numberOfLevels)
        {
            int stars = -1;
            stars = LoadStarsInEachLevel(i);
            if (stars > -1)
            {
                return stars;
            }         
        }
        return -1;
    }

    private static void Load(string boosterType)
    {
        if (File.Exists(Application.persistentDataPath + "/" + boosterType))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + boosterType, FileMode.Open);

            BoosterData data = (BoosterData)bf.Deserialize(file);
            file.Close();
            if (boosterType == booster1Type)
            {
                booster1No = data.numberOfBooster;
                if(booster1No < 0)
                {
                    booster1No = 0;
                }
            }
            else if (boosterType == booster3Type)
            {
                booster3No = data.numberOfBooster;
                if (booster3No < 0)
                {
                    booster3No = 0;
                }
            }
            else if (boosterType == booster4Type)
            {
                booster4No = data.numberOfBooster;
                if (booster4No < 0)
                {
                    booster4No = 0;
                }
            }
            else if (boosterType == booster5Type)
            {
                booster5No = data.numberOfBooster;
                if (booster5No < 0)
                {
                    booster5No = 0;
                }
            }
            else if (boosterType == booster6Type)
            {
                booster6No = data.numberOfBooster;
                if (booster6No < 0)
                {
                    booster6No = 0;
                }
            }
            else if (boosterType == booster7Type)
            {
                booster7No = data.numberOfBooster;
                if (booster7No < 0)
                {
                    booster7No = 0;
                }
            }

        }
    }

    private static int LoadStarsInEachLevel(int idx)
    {
        if (File.Exists(Application.persistentDataPath + "/" + idx))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + idx, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            return data.stars;
        }

        return -1;
    }

    private static void LoadNonConsumables(string removeAds)
    {
        if (File.Exists(Application.persistentDataPath + "/" + removeAds))
        {
            //Debug.Log("ads are not displayed");
            adsRemoved = true;
        }
    }

    private static void LoadSoundSettings(string muteType)
    {
        if (File.Exists(Application.persistentDataPath + "/" + muteType))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + muteType, FileMode.Open);

            SoundData data = (SoundData)bf.Deserialize(file);
            file.Close();
            if (muteType == "fx")
            {
                isFXMute = data.mute;
            }
            if (muteType == "music")
            {
                isMusicMute = data.mute;
            }
        }
    }

    public static void Save(int numberOfBooster, string boosterType)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + boosterType);

        BoosterData data = new BoosterData();
        data.numberOfBooster = numberOfBooster;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void SaveNonConsumable(bool state, string boosterType)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + boosterType);

        NonConsumableData data = new NonConsumableData();
        data.adsRemoved = state;

        bf.Serialize(file, data);
        file.Close();
    }
}
