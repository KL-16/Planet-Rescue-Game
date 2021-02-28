using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class Ocean : MonoBehaviour
{
	SpriteRenderer m_spriteRenderer;

	public int xIndex;
	public int yIndex;
	// reference to our Board
	Board m_board;
	public int scoreValue = 20;
	// current "health" of a Breakable tile before it is removed
	public int breakableValue = 0;

	// array of Sprites used to show damage on Breakable Tile
	public Sprite[] breakableSprites;

	// Start is called before the first frame update
	private void Awake()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void PlaceOcean(int x, int y, Board board)
	{
		xIndex = x;
		yIndex = y;
		m_board = board;
		
			if (breakableSprites[breakableValue] != null)
			{
				m_spriteRenderer.sprite = breakableSprites[breakableValue];
			}
			
	}

	public void CleanOcean(float waitTime = 0f)
	{
		StartCoroutine(CleanOceanRoutine(waitTime));
	}

	IEnumerator CleanOceanRoutine(float waitTime = 0f)
	{
		breakableValue = Mathf.Clamp(--breakableValue, 0, breakableValue);

		yield return new WaitForSeconds(waitTime);

		if (breakableSprites[breakableValue] != null)
		{
			m_spriteRenderer.sprite = breakableSprites[breakableValue];
		}

		if (breakableValue == 0)
		{
			m_board.MakeNewTile(m_board.tileNormalPrefab, xIndex, yIndex);
			m_board.MakeNewTile(m_board.tileNormalPrefab, xIndex + 1, yIndex);
			m_board.MakeNewTile(m_board.tileNormalPrefab, xIndex, yIndex + 1);
			m_board.MakeNewTile(m_board.tileNormalPrefab, xIndex + 1, yIndex + 1);

			//tsunami wave
			Destroy(gameObject);
		}
		yield return null;
	}
}
