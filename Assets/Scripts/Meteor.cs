using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    public void MoveMeteor(int destX, int destY, float timeToMove)
    {

        // only move if the GamePiece is not already moving
        
            StartCoroutine(MoveMeteorRoutine(new Vector3(destX, destY, -5), timeToMove));
        
    }

    IEnumerator MoveMeteorRoutine(Vector3 destination, float timeToMove)
    {
        // store our starting position
        Vector3 startPosition = gameObject.transform.position;

        // have we reached our destination?
        bool reachedDestination = false;

        // how much time has passed since we started moving
        float elapsedTime = 0f;

        // while we have not reached the destination, check to see if we are close enough
        while (!reachedDestination)
        {
            // if we are close enough to destination
            if (Vector3.Distance(gameObject.transform.position, destination) < 0.01f)
            {
                // we have reached the destination
                reachedDestination = true;
                yield return new WaitForSeconds(1.7f);
                Destroy(gameObject);
                break;
            }

            // increment the total running time by the Time elapsed for this frame
            elapsedTime += Time.deltaTime;

            // calculate the Lerp value
            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);
            

            t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);


            // move the game piece
            gameObject.transform.position = Vector3.Lerp(startPosition, destination, t);
            

            // wait until next frame
            yield return null;
        }
    }
}
