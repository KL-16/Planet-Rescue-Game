using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile2ndLayerType
	{
	Normal,
	MiningPit,
	Oil,
	WasteDump
}

public class Tiles2ndLayer : MonoBehaviour
{
	// x and y position in the array
	public int xIndex;
	public int yIndex;

	// reference to our Board
	Board m_board;
	public int scoreValue = 20;

	// our current TileType
	public Tile2ndLayerType tileType = Tile2ndLayerType.Normal;

	// the Sprite for this Tile
	SpriteRenderer m_spriteRenderer;

	// current "health" of a Breakable tile before it is removed
	public int breakableValue = 0;
	
	float pointsWaitTime = 0.5f;

	// array of Sprites used to show damage on Breakable Tile
	public Sprite[] breakableSprites;

	// sets the color of Breakable Tile back to normal once it is removed
	public Color normalColor;
	public AudioClip[] clearSound;


	// Use this for initialization
	void Awake()
		{
		// initialize our SpriteRenderer
		m_spriteRenderer = GetComponent<SpriteRenderer>();
		}

	// initialze the Tile's array index and cache a reference to the Board
	public void Init(int x, int y, Board board)
		{
		xIndex = x;
		yIndex = y;
		m_board = board;

		// if the Tile is breakable, set its Sprite
		}

	// if the mouse clicks the Collider on this Tile, run ClickTile on the Board

	void OnMouseDown()
	{
		if (GameManager.Instance.BoosterIsActive == true)
		{
			GameManager.Instance.BoosterTile(m_board.m_allTiles[xIndex, yIndex]);
		}
		
	}

	// if the mousebutton is held and then the pointer is dragged into the Collider on this Tile...
	// run DragToTile on the Board, passing in this component 
	void OnMouseEnter()
	{
		if (GameManager.Instance.BoosterIsActive == true)
		{
			GameManager.Instance.BoosterTile(m_board.m_allTiles[xIndex, yIndex]);
		}
		
	}

	// if we let go of the mouse button while on this Tile, run ReleaseTile on the Board
	void OnMouseUp()
	{
		if (GameManager.Instance.BoosterIsActive == true)
		{
			GameManager.Instance.BoosterGoOff();
		}
		
	}

	// starts the coroutine to break a Breakable Tile
	public void BreakTile(float waitTime = 0f)
		{
		if (tileType != Tile2ndLayerType.MiningPit && tileType != Tile2ndLayerType.Oil && tileType != Tile2ndLayerType.WasteDump)
			{
			return;
			}

		StartCoroutine(BreakTileRoutine(waitTime));
		}
	
	// decrement the breakable value, switch to the appropriate sprite
	// and conver the Tile to become normal once the breakableValue reaches 0
	IEnumerator BreakTileRoutine(float waitTime = 0f)
		{
		if(breakableValue > 0)
        {
			if (waitTime > 0f)
			{
				yield return new WaitForSeconds(waitTime);
			}

			breakableValue = Mathf.Clamp(--breakableValue, 0, breakableValue);


			if (breakableSprites[breakableValue] != null)
			{
				m_spriteRenderer.sprite = breakableSprites[breakableValue];
			}

			if (breakableValue == 0)
			{
				if (GameManager.Instance != null)
				{
					GameManager.Instance.UpdateCollectionGoalsTiles2nd(this);
					GameManager.Instance.ScorePoints2ndTiles(this);
					if (m_board.m_particleManager != null)
					{
						m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, pointsWaitTime);
					}
				}

				Destroy(gameObject);

			}
		}
		
			yield return null;
		}
	}