using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTiles : MonoBehaviour
{
	// x and y position in the array
	int xIndex;
	int yIndex;
	public Sprite[] alternatingSprites;
	SpriteRenderer m_spriteRenderer;
	// reference to our Board
	Board m_board;

    private void Awake()
    {
		// initialize our SpriteRenderer
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

    public void Init(int x, int y, Board board)
	{
		xIndex = x;
		yIndex = y;
		m_board = board;
		if(m_spriteRenderer != null && alternatingSprites[0] != null && alternatingSprites[1] != null)
        {
			if (x % 2 == 0)
			{
				if (y % 2 == 0)
				{ 				
					m_spriteRenderer.sprite = alternatingSprites[1];										
				}
                else
                {
					m_spriteRenderer.sprite = alternatingSprites[0];
				}
			}
            else
            {
				if (y % 2 == 0)
				{
					m_spriteRenderer.sprite = alternatingSprites[0];
				}
				else
				{
					m_spriteRenderer.sprite = alternatingSprites[1];
				}
			}
		}
		
	}


}
