using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Earth : MonoBehaviour
{
    public Sprite[] earthSprites;
    SpriteRenderer m_spriteRenderer;

	public int xIndex;
	public int yIndex;
	// reference to our Board
	Board m_board;
	public int scoreValue = 20;
	public AudioClip clearSound;

	// Start is called before the first frame update
	private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

	public void PlaceEarth(int x, int y, Board board)
	{
		xIndex = x;
		yIndex = y;
		m_board = board;

		if (earthSprites[0] != null)
			{
				m_spriteRenderer.sprite = earthSprites[0];
			}
		
	}
}
