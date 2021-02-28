﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    public GameObject collectionGoalLayout;

    public int collectionGoalBaseWidth = 125;

    CollectionGoalPanel[] m_collectionGoalPanels;

    // reference to graphic that fades in and out
    public ScreenFader screenFader;

    // UI.Text that stores the level name
    public Text levelNameText;

    // UI.Text that shows how many moves are left
    public Text movesLeftText;

    // reference to three-star score meter
    public ScoreMeter scoreMeter;

    // reference to the custom UI window
    public MessageWindow messageWindow;

    public GameObject movesCounter;

    public Timer timer;

    public override void Awake()
    {
        base.Awake();

        if (messageWindow != null)
        {
            messageWindow.gameObject.SetActive(true);
        }

        if (screenFader != null)
        {
            screenFader.gameObject.SetActive(true);
        }
    }

    public void SetupCollectionGoalLayout(List<CollectionGoal> collectionGoals, GameObject goalLayout, int spacingWidth)
    {
        if (goalLayout != null && collectionGoals != null && collectionGoals.Count != 0)
        {
            RectTransform rectXform = goalLayout.GetComponent<RectTransform>();
            rectXform.sizeDelta = new Vector2(collectionGoals.Count * spacingWidth, rectXform.sizeDelta.y);

            CollectionGoalPanel[] panels = goalLayout.GetComponentsInChildren<CollectionGoalPanel>();

            for (int i = 0; i < panels.Length; i++)
            {
                if (i < collectionGoals.Count && collectionGoals[i] != null)
                {
                    panels[i].gameObject.SetActive(true);
                    panels[i].collectionGoal = collectionGoals[i];
                    panels[i].SetupPanel();
                }
            }
        }
    }

    public void SetupCollectionGoalLayout(List<CollectionGoal> collectionGoals)
    {
        SetupCollectionGoalLayout(collectionGoals, collectionGoalLayout, collectionGoalBaseWidth);
    }

    public void UpdateCollectionGoalLayout(GameObject goalLayout, int x = -1, int y = 0, int z = 0)
    {
        if (goalLayout != null)
        {
            CollectionGoalPanel[] panels = goalLayout.GetComponentsInChildren<CollectionGoalPanel>();

            if (panels != null && panels.Length != 0)
            {
                foreach (CollectionGoalPanel panel in panels)
                {
                    if (panel != null && panel.isActiveAndEnabled)
                    {
                        panel.UpdatePanel(x, y, z);
                    }
                }
            }
        }
    }

    public void UpdateCollectionGoalLayout1(int x = -1, int y = 0, int z = 0)
    {
        UpdateCollectionGoalLayout(collectionGoalLayout, x, y, z);
    }

    public void EnableTimer(bool state)
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(state);
        }
    }

    public void EnableMovesCounter(bool state)
    {
        if (movesCounter != null)
        {
            movesCounter.SetActive(state);
        }
    }

    public void EnableCollectionGoalLayout(bool state)
    {
        if (collectionGoalLayout != null)
        {
            collectionGoalLayout.SetActive(state);
        }
    }
}
