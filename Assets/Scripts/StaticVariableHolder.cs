using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariableHolder : MonoBehaviour
{
    public List<string> lines;
    public int levelsSinceLastAdd = 0;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        TextAsset theList = (TextAsset)Resources.Load("EnvironmentalFacts", typeof(TextAsset));

        lines = new List<string>(theList.text.Split('\n'));
    }
}
