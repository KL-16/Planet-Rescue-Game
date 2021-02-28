using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rate : MonoBehaviour
{
    public void RateGame()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.PlanetRescue.PlanetRescueGemsMatch3SlidingPuzzle");
    }

    public void LikeGame()
    {
        Application.OpenURL("https://www.facebook.com/Planet-Rescue-113149827153666");
    }
}
