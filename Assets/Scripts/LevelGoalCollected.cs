using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoalCollected : LevelGoal
{

    public List<CollectionGoal> collectionGoals = new List<CollectionGoal>();

    public void UpdateGoals(GamePiece pieceToCheck)
    {
        if (pieceToCheck != null)
        {
            foreach (CollectionGoal goal in collectionGoals)
            {
                if (goal != null)
                {
                    goal.CollectPiece(pieceToCheck);
                }
            }

            UpdateUI(pieceToCheck.xIndex, pieceToCheck.yIndex);
        }
       
    }

    public void UpdateGoalsTiles(Tile tileToCheck)
    {
        
        if (tileToCheck != null)
        {
            foreach (CollectionGoal goal in collectionGoals)
            {
                if (goal != null)
                {
                    goal.CollectTile(tileToCheck);
                }
            }

            UpdateUI(tileToCheck.xIndex, tileToCheck.yIndex);
        }
    }

    public void UpdateGoalsTiles2nd(Tiles2ndLayer tileToCheck)
    {

        if (tileToCheck != null)
        {
            foreach (CollectionGoal goal in collectionGoals)
            {
                if (goal != null)
                {
                    goal.CollectTile2nd(tileToCheck);
                }
            }

            UpdateUI(tileToCheck.xIndex, tileToCheck.yIndex);
        }
    }

    public void UpdateGoalsEarth(Earth earth)
    {
        if (earth != null)
        {
            foreach (CollectionGoal goal in collectionGoals)
            {
                if (goal != null)
                {
                    goal.CollectEarth(earth);
                }
            }

            UpdateUI(earth.xIndex, earth.yIndex);
        }
    }

    public void UpdateGoalsPlanet(Ocean ocean)
    {
        if (ocean != null)
        {
            foreach (CollectionGoal goal in collectionGoals)
            {
                if (goal != null)
                {
                    goal.CollectPlanet(ocean);
                }
            }

            UpdateUI(ocean.xIndex, ocean.yIndex);
        }
    }

    public void UpdateUI(int x = -1, int y = 0, int z = 0)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCollectionGoalLayout1(x, y, z);
        }
    }

    bool AreGoalsComplete(List<CollectionGoal> goals)
    {
        foreach (CollectionGoal g in goals)
        {
            if (g == null | goals == null)
            {
                return false;
            }

            if (goals.Count == 0)
            {
                return false;
            }

            if (g.numberToCollect != 0)
            {
                return false;
            }
        }
        return true;
    }

    public override bool IsGameOver()
    {
        if(Board.meteorsImpacting == false)
        {
            if (AreGoalsComplete(collectionGoals))
            {
               
                    return true;
                
            }
            if (Board.boardShuffles >= 1 || Board.locked == true)
            {
                return true;
            }
            if (levelCounter == LevelCounter.Moves)
            {
                return (movesLeft <= 0);
            }
            else
            {
                return (timeLeft <= 0);
            }
        }

        return false;
    }

    public override bool IsWinner()
    {
        if (ScoreManager.Instance != null)
        {
            return (AreGoalsComplete(collectionGoals));
        }
        return false;
    }
}
