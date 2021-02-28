using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelSelector : MonoBehaviour
{
    public GameObject levelHolder;
    public Image levelIcon;
    public GameObject thisCanvas;
    public int numberOfLevels = 50;
    public Vector2 iconSpacing;
    private Rect panelDimensions;
    private Rect iconDimensions;
    private int amountPerPage;
    private int currentLevelCount;
    private int starsInArray;
    private int previousLevelNumOfStars = 0;
    public Sprite unlockedIcon;
    int unlockedPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
        int maxInARow = Mathf.FloorToInt((panelDimensions.width + iconSpacing.x) / (iconDimensions.width + iconSpacing.x));
        int maxInACol = Mathf.FloorToInt((panelDimensions.height + iconSpacing.y) / (iconDimensions.height + iconSpacing.y)) - 2;
        amountPerPage = maxInARow * maxInACol;
        int totalPages = Mathf.CeilToInt((float)numberOfLevels / amountPerPage);
        LoadPanels(totalPages);
    }
    void LoadPanels(int numberOfPanels)
    {
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        PageSwiper swiper = levelHolder.AddComponent<PageSwiper>();
        swiper.totalPages = numberOfPanels;

        for (int i = 1; i <= numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(levelHolder.transform);
            panel.name = "Page-" + i;
            panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i - 1), 0);
            SetUpGrid(panel);
            int numberOfIcons = i == numberOfPanels ? numberOfLevels - currentLevelCount : amountPerPage;
            LoadIcons(numberOfIcons, panel, i);

            
        }
        Destroy(panelClone);
        swiper.InitialPosition(unlockedPage);
    }
    void SetUpGrid(GameObject panel)
    {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(iconDimensions.width, iconDimensions.height);
        grid.childAlignment = TextAnchor.MiddleCenter;
        grid.spacing = iconSpacing;
    }
    void LoadIcons(int numberOfIcons, GameObject parentObject, int panelNumber)
    {
        currentLevelCount = (panelNumber - 1) * amountPerPage;
        for (int i = 1; i <= numberOfIcons; i++)
        {
            currentLevelCount++;
            Image icon = Instantiate(levelIcon) as Image;
            icon.transform.SetParent(thisCanvas.transform, false);
            icon.transform.SetParent(parentObject.transform);
            int x = i + 1;
            starsInArray = 0;
            icon.name = "Level " + currentLevelCount;
            
            

            var starIm1 = icon.transform.GetChild(0).GetComponent<Image>();
            var starIm2 = icon.transform.GetChild(1).GetComponent<Image>();
            var starIm3 = icon.transform.GetChild(2).GetComponent<Image>();
            var button = icon.transform.GetChild(3).GetComponent<Button>();
            button.enabled = false;
            Load(currentLevelCount - 1);
            if(starsInArray > 0 || previousLevelNumOfStars > 0 || currentLevelCount == 1)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().SetText(currentLevelCount.ToString());
                button.enabled = true;
                button.onClick.AddListener(delegate () { LoadScene((panelNumber - 1) * amountPerPage + x); });
                button.GetComponent<Image>().sprite = unlockedIcon;
                unlockedPage = panelNumber - 1;

            }
            switch (starsInArray)
            {
                case 0:
                    starIm1.enabled = false;
                    starIm2.enabled = false;
                    starIm3.enabled = false;
                    break;

                case 1:
                    starIm1.enabled = true;
                    starIm2.enabled = false;
                    starIm3.enabled = false;
                    break;

                case 2:
                    starIm1.enabled = true;
                    starIm2.enabled = true;
                    starIm3.enabled = false;
                    break;

                case 3:
                    starIm1.enabled = true;
                    starIm2.enabled = true;
                    starIm3.enabled = true;
                    break;

            }
            previousLevelNumOfStars = starsInArray;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadScene(int index)
    {
        int buttonIndex = index - 1;
        FindObjectOfType<ProgressSceneLoader>().LoadScene("Level " + buttonIndex);
    }

    private void Load(int idx)
    {
        if (File.Exists(Application.persistentDataPath + "/" + idx))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + idx, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            starsInArray = data.stars;
        }
    }
}