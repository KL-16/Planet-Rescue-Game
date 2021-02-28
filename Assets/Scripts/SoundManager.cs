using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;

// Singletone manager class to handle sounds
public class SoundManager : Singleton<SoundManager>
{
    //buttons for music and fx
    public Button musicBtn;
    public Button fxBtn;

    public Sprite imgfxmute;
    public Sprite imgfxOn;
    public Sprite imgmusicmute;
    public Sprite imgmusicOn;

    // array of sound clips for music
    public AudioClip[] musicClips;

    // array of sound clips for winning the game
    public AudioClip[] winClips;

    // array of sound clips for losing the game
    public AudioClip[] loseClips;

    // array of sounds for bonus events (chain reaction clears)
    public AudioClip[] bonusClips;
    public AudioClip[] chainClips;
    public AudioClip[] starClips;

    public AudioClip lightningSound;
    public AudioClip meteorSound;
    public AudioClip explosionSound;
    public AudioClip wooshSound;
    public AudioClip colorBombSound;
    public AudioClip bigBombSound;
    public AudioClip spawnBombSound;
    public AudioClip bounceSound;
    public AudioClip roadSound;
    public AudioClip wateringCanSound;
    public AudioClip fireworksSound;

    // music volume
    [Range(0,1)]
    public float musicVolume = 0.5f;

    // sound effects volume
    [Range(0,1)]
    public float fxVolume = 1.0f;

    // boundaries for random variation in pitch
    public float lowPitch = 0.95f;
    public float highPitch = 1.05f;

    private bool isMusicPlaying = false;
    private bool isMusicMute = false;
    private bool isFXMute = false;

    GameObject goMusic;
    GameObject goFX;

    private void Start()
    {
        isMusicMute = LoadInfo.isMusicMute;
        isFXMute = LoadInfo.isFXMute;
        if (isMusicMute)
        {
            musicBtn.GetComponent<Image>().sprite = imgmusicmute;
        }
        else
        {
            musicBtn.GetComponent<Image>().sprite = imgmusicOn;
        }

        if (isFXMute)
        {
            fxBtn.GetComponent<Image>().sprite = imgfxmute;
        }
        else
        {
            fxBtn.GetComponent<Image>().sprite = imgfxOn;
        }

    }

    private void Update()
    {
        if (!isMusicPlaying)
        {
            if (!isMusicMute)
            {
                // play a random music clip
                PlayRandomMusic();
                isMusicPlaying = true;
            } 
        }
    }

    public void MuteMusic()
    {
        Destroy(goMusic);
        isMusicMute = !isMusicMute;
        Save(isMusicMute, "music");
        LoadInfo.isMusicMute = isMusicMute;
        if (!isMusicMute)
        {
            isMusicPlaying = false;
        }

        if (isMusicMute)
        {
            musicBtn.GetComponent<Image>().sprite = imgmusicmute;
        }
        else
        {
            musicBtn.GetComponent<Image>().sprite = imgmusicOn;
        }
    }

    public void MuteFX()
    {
        Destroy(goFX);
        isFXMute = !isFXMute;
        Save(isFXMute, "fx");
        LoadInfo.isFXMute = isFXMute;
        if (isFXMute)
        {
            fxBtn.GetComponent<Image>().sprite = imgfxmute;
        }
        else
        {
            fxBtn.GetComponent<Image>().sprite = imgfxOn;
        }
    }

    // this replaces the native PlayClipAtPoint to play an AudioClip at a world space position
    // this allows a third volume parameter to specify the volume unlike the native version
    // and allows for some random variation so the sound is less monotonous
    public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, bool randomizePitch = true)
    {
        if (!isFXMute)
        {
            if (clip != null)
            {
                // create a new GameObject at the specified world space position
                goFX = new GameObject("SoundFX" + clip.name);
                goFX.transform.position = position;

                // add an AudioSource component and set the AudioClip
                AudioSource source = goFX.AddComponent<AudioSource>();
                source.clip = clip;

                // change the pitch of the sound within some variation
                if (randomizePitch)
                {
                    float randomPitch = UnityEngine.Random.Range(lowPitch, highPitch);
                    source.pitch = randomPitch;
                }

                // set the volume
                source.volume = volume;

                // play the sound
                source.Play();

                // destroy the AudioSource after the clip is done playing
                Destroy(goFX, clip.length);

                // return our AudioSource out of the method
                return source;
            }
        }
        return null;
    }

    public AudioSource PlayMusicClipAtPoint(AudioClip clip, Vector3 position, float volume = 1f, bool randomizePitch = true)
    {
        if (clip != null)
        {
            // create a new GameObject at the specified world space position
            goMusic = new GameObject("SoundFX" + clip.name);
            goMusic.transform.position = position;

            // add an AudioSource component and set the AudioClip
            AudioSource source = goMusic.AddComponent<AudioSource>();
            source.clip = clip;

            // change the pitch of the sound within some variation
            if (randomizePitch)
            {
                float randomPitch = UnityEngine.Random.Range(lowPitch, highPitch);
                source.pitch = randomPitch;
            }

            // set the volume
            source.volume = volume;
            source.loop = true;
            // play the sound
            source.Play();

            // destroy the AudioSource after the clip is done playing
            //Destroy(goMusic, clip.length);

            // return our AudioSource out of the method
            return source;
        }

        return null;
    }

    // play a random sound from an array of sounds
    public AudioSource PlayRandom(AudioClip[] clips, Vector3 position, float volume = 1f)
    {
        if (clips != null)
        {
            if (clips.Length != 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, clips.Length);

                if (clips[randomIndex] != null)
                {
                    AudioSource source = PlayClipAtPoint(clips[randomIndex], position, volume);
                    return source;
                }
            }
        }
        return null;
    }

    public AudioSource PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip != null)
        {       
               AudioSource source = PlayClipAtPoint(clip, position, volume);
               return source;
               
        }
        return null;
    }

    public AudioSource PlayRandomMusic(AudioClip[] clips, Vector3 position, float volume = 1f)
    {
        if (clips != null)
        {
            if (clips.Length != 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, clips.Length);

                if (clips[randomIndex] != null)
                {
                    AudioSource source = PlayMusicClipAtPoint(clips[randomIndex], position, volume);
                    return source;
                }
            }
        }
        return null;
    }

    // play a random music clip
    public void PlayRandomMusic()
    {
        PlayRandomMusic(musicClips, Vector3.zero, musicVolume);
    }

    // play a random win sound
    public void PlayWinSound()
    {
        PlayRandom(winClips, Vector3.zero, fxVolume);
    }

    // play a random lose sound
    public void PlayLoseSound()
    {
        PlayRandom(loseClips, Vector3.zero, fxVolume * 0.5f);
    }

    // play a random bonus sound
    public void PlayBonusSound()
    {
        PlayRandom(bonusClips, Vector3.zero, fxVolume);
    }

    public void PlayLightningSound()
    {
        PlaySound(lightningSound, Vector3.zero, fxVolume/2f);
    }

    public void PlayMeteorSound()
    {
        PlaySound(meteorSound, Vector3.zero, fxVolume / 2f);
    }

    public void PlayExplosionSound()
    {
        PlaySound(explosionSound, Vector3.zero, fxVolume / 2f);
    }

    public void PlayWooshSound()
    {
        PlaySound(wooshSound, Vector3.zero, fxVolume / 2f);
    }

    public void PlayColorBombSound()
    {
        PlaySound(colorBombSound, Vector3.zero, fxVolume / 2f);
    }

    public void PlayBigBombSound()
    {
        PlaySound(bigBombSound, Vector3.zero, fxVolume * 2f);
    }

    public void PlaySpawnBombSound()
    {
        PlaySound(spawnBombSound, Vector3.zero, fxVolume / 2f);
    }

    public void PlayStar1Sound()
    {
        PlaySound(starClips[0], Vector3.zero, fxVolume * 2f);
    }

    public void PlayStar2Sound()
    {
        PlaySound(starClips[1], Vector3.zero, fxVolume * 2f);
    }

    public void PlayStar3Sound()
    {
        PlaySound(starClips[2], Vector3.zero, fxVolume * 2f);
    }

    public void PlayBounceSound()
    {
        PlaySound(bounceSound, Vector3.zero, fxVolume / 2f);
    }
    public void PlayRoadSound()
    {
        PlaySound(roadSound, Vector3.zero, fxVolume);
    }
    public void PlayWateringCanSound()
    {
        PlaySound(wateringCanSound, Vector3.zero, fxVolume);
    }
    public void PlayFireworksSound()
    {
        PlaySound(fireworksSound, Vector3.zero, fxVolume);
    }




    private void Save(bool mute, string muteType)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + muteType);

        SoundData data = new SoundData();
        data.mute = mute;

        bf.Serialize(file, data);
        file.Close();
    }
}

[Serializable]
class SoundData
{
    public bool mute;
}
