﻿using UnityEngine;
using System.Collections;

// each GamePiece has a MatchValue to determine if it forms a match with its neighbors
public enum MatchValue
{
	Yellow,
	Blue,
	Green,
	White,
    Purple,
    Red,
	Litter,
	Barrel,
	BrokenBarrel,
	None
}

// this is a basic dot GamePiece
[RequireComponent(typeof(SpriteRenderer))]
public class GamePiece : MonoBehaviour {

    // x and y index used for determining position in the Board's array
	public int xIndex;
	public int yIndex;

    // reference to the Board
	Board m_board;

    // are we currently moving?
	public bool m_isMoving = false;

	// interpolation type when we move from one position to another 
	public InterpType interpolation = InterpType.SmootherStep;

	//barrel pieces
	public int movesBeforeExplosion;
	public Sprite[] barrelSprites;
	public SpriteRenderer m_spriteRenderer;


	public enum InterpType
	{
		Linear,
		EaseOut,
		EaseIn,
		SmoothStep,
		SmootherStep
	};

    // our current MatchValue
	public MatchValue matchValue;

    // how much this GamePiece is worth when it is cleared
	public int scoreValue = 20;

    // the sound the GamePiece makes when it clears
    public AudioClip clearSound;

	void Awake()
	{
		// initialize our SpriteRenderer
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// initialize the GamePiece with a reference to the Board
	public void Init(Board board)
	{
		m_board = board;
	}

    // sets the x and y index of the GamePiece
	public void SetCoord(int x, int y)
	{
		xIndex = x;
		yIndex = y;
	}

    // move the GamePiece
	public void Move (int destX, int destY, float timeToMove, int interp = 0)
	{

        // only move if the GamePiece is not already moving
		if (!m_isMoving)
		{

			StartCoroutine(MoveRoutine(new Vector3(destX, destY,0), timeToMove, interp));	
		}
	}

    // coroutine to handle movement
	IEnumerator MoveRoutine(Vector3 destination, float timeToMove, int interp = 0)
	{
        // store our starting position
		Vector3 startPosition = transform.position;

        // have we reached our destination?
		bool reachedDestination = false;

        // how much time has passed since we started moving
		float elapsedTime = 0f;

		Color c = new Color(1F, 1F, 1F, 0F);
		Color d = new Color(1F, 1F, 1F, 1F);

		// we are moving the GamePiece
		m_isMoving = true;

		if(interp == 1)
		{
			interpolation = InterpType.Linear;
		}

        // while we have not reached the destination, check to see if we are close enough
		while (!reachedDestination)
		{
			// if we are close enough to destination
			if (Vector3.Distance(transform.position, destination) < 0.01f)
			{			
					// we have reached the destination
					reachedDestination = true;

					// explicitly set the GamePiece at the new location in the Board
					if (m_board != null)
					{
						m_board.PlaceGamePiece(this, (int)destination.x, (int)destination.y);

					}
				
				break;             			
			}

			// increment the total running time by the Time elapsed for this frame
			elapsedTime += Time.deltaTime;
			
			
			// calculate the Lerp value
			float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

			switch (interpolation)
			{
				case InterpType.Linear:
					break;
				case InterpType.EaseOut:
					t = Mathf.Sin(t * Mathf.PI * 0.5f);
					break;
				case InterpType.EaseIn:
					t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
					break;
				case InterpType.SmoothStep:
					t = t*t*(3 - 2*t);
					break;
				case InterpType.SmootherStep:
					t =  t*t*t*(t*(t*6 - 15) + 10);
					break;
			}

			// move the game piece
			transform.position = Vector3.Lerp(startPosition, destination, t);

			if (transform.position.y > m_board.height)
			{
				m_spriteRenderer.color = c;
			}

			else
			{
				m_spriteRenderer.color = d;
			}
			// wait until next frame
			yield return null;
		}

        // GamePiece is no longer moving
		m_isMoving = false;


	}

	// Change the color of the GamePiece to match another GamePiece
	public void ChangeColor(GamePiece pieceToMatch)
	{
		SpriteRenderer rendererToChange = GetComponent<SpriteRenderer>();

		if (pieceToMatch !=null)
		{
			SpriteRenderer rendererToMatch = pieceToMatch.GetComponent<SpriteRenderer>();

			if (rendererToMatch !=null && rendererToChange !=null)
			{
				rendererToChange.color = rendererToMatch.color;
			}

			matchValue = pieceToMatch.matchValue;
		}

	}

	public void BreakBarrel(int x, int y)
	{
		StartCoroutine(BreakBarrelRoutine(x, y));
	}

	// decrement the breakable value, switch to the appropriate sprite
	// and conver the Tile to become normal once the breakableValue reaches 0
	IEnumerator BreakBarrelRoutine(int x, int y)
	{
		movesBeforeExplosion = Mathf.Clamp(--movesBeforeExplosion, 0, movesBeforeExplosion);	

		if (barrelSprites[movesBeforeExplosion] != null)
		{
			m_spriteRenderer.sprite = barrelSprites[movesBeforeExplosion];
		}

		if (movesBeforeExplosion == 0)
		{
			//m_board.ClearPieceAt(x, y);
			Board.barrelBroken = true;
			// function to spread oil

		}
		yield return null;
	}
}
