using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionGoalPanel : MonoBehaviour
{
	// collection goal 
    public CollectionGoal collectionGoal;

    // text indicating number to collect
    public Text numberLeftText;

    // icon for the GamePiece
    public Image prefabImage;

    public GameObject tickImage;
    public GameObject goalCollectedPrefab;
    bool animationPlayed = false;
    int previousNumberToCollect = -1;
    public GameObject tileAnimation;

    void Start()
    {
        SetupPanel();
        previousNumberToCollect = -1;
    }
	
	// setup the sprites and text
    public void SetupPanel()
    {
       
        if (collectionGoal != null && numberLeftText != null && prefabImage != null)
        {
            SpriteRenderer prefabSprite = collectionGoal.prefabToCollect.GetComponent<SpriteRenderer>(); 
            if (prefabSprite != null)
            {
                prefabImage.sprite = prefabSprite.sprite;
                prefabImage.color = prefabSprite.color;
            }

            numberLeftText.text = collectionGoal.numberToCollect.ToString();
        }
    }

    // update the text
    public void UpdatePanel(int x = -1, int y = 0, int z = 0)
    {
        
            if (collectionGoal != null && numberLeftText != null)
            {

           
                if (x != -1)
                {

                    if(collectionGoal.numberToCollect < previousNumberToCollect)
                    {
                    ////animation to move tile to goal layout
                    GameObject tile = Instantiate(tileAnimation, new Vector3(x, y), Quaternion.identity) as GameObject;

                    SpriteRenderer image = tile.GetComponentInChildren<SpriteRenderer>();
                    image.sprite = prefabImage.sprite;
                    
                    Move(tickImage.transform.position.x, tickImage.transform.position.y, 0.7f, tile);
                    }
                    
                }


                if (collectionGoal.numberToCollect > 0 || animationPlayed == false)
                {
                    if (collectionGoal.numberToCollect == 0)
                    {
                        numberLeftText.enabled = false;
                        if (tickImage != null)
                        {
                            tickImage.SetActive(true);
                            if (animationPlayed == false)
                            {
                                GoalCollectedFXAt(tickImage.transform.position.x, tickImage.transform.position.y, tickImage.transform.position.z - 2);
                                animationPlayed = true;


                            }

                        }

                    }
                    else
                    {
                        StartCoroutine(UpdateTextRoutine());
                        if (animationPlayed == true)
                        {
                            animationPlayed = false;
                        }
                        if (tickImage != null && tickImage.activeInHierarchy)
                        {
                            tickImage.SetActive(false);
                        }
                    }
                }

            previousNumberToCollect = collectionGoal.numberToCollect;


            }
        
    }

    void GoalCollectedFXAt(float x, float y, float z = 0, float waitTime = 0f)
    {
        StartCoroutine(GoalCollectedFXAtRoutine(x, y, z, waitTime));
    }

    IEnumerator GoalCollectedFXAtRoutine(float x, float y, float z = 0, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);

        if (goalCollectedPrefab != null)
        {
            GameObject goalFX = Instantiate(goalCollectedPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

            ParticlePlayer particlePlayer = goalFX.GetComponent<ParticlePlayer>();

            if (particlePlayer != null)
            {
                particlePlayer.Play();
            }
        }
    }

    void Move(float destX, float destY, float timeToMove, GameObject tile)
    {

        // only move if the GamePiece is not already moving
        
        StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0), timeToMove, tile));
        
    }

    // coroutine to handle movement
    IEnumerator MoveRoutine(Vector3 destination, float timeToMove, GameObject tile)
    {
        
        Vector3 startScale = tile.transform.localScale;

        // store our starting position
        Vector3 startPosition = tile.transform.position;

        float z = -8f;
        startPosition.z = z;

        // have we reached our destination?
        bool reachedDestination = false;

        // how much time has passed since we started moving
        float elapsedTime = 0f;
        // while we have not reached the destination, check to see if we are close enough
        while (!reachedDestination)
        {
            // if we are close enough to destination
            if (Vector3.Distance(tile.transform.position, destination) < 0.01f)
            {
                // we have reached the destination
                reachedDestination = true;
                Destroy(tile.gameObject);
                break;
            }

            // increment the total running time by the Time elapsed for this frame
            elapsedTime += Time.deltaTime;

            // calculate the Lerp value
            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);           
            
            t = t * t * t * (t * (t * 6 - 15) + 10);
                   
            

            // move the game piece
            tile.transform.position = Vector3.Lerp(startPosition, destination, t);
            tile.transform.localScale = Vector3.Lerp(startScale, startScale * 0.5f, t);

            // wait until next frame
            yield return null;
        }
       

    }

    IEnumerator UpdateTextRoutine()
    {
        numberLeftText.enabled = true;
        numberLeftText.text = collectionGoal.numberToCollect.ToString();
        if(collectionGoal.numberToCollect == 0)
        {
            numberLeftText.enabled = false;
        }

        yield return null;
    }

}
