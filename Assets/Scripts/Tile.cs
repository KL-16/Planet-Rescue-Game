using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;


// a Tile can be an empty space, an Obstacle preventing movement, or a removeable Breakable tile
public enum TileType 
{
	Normal,
	Teleport,
	Breakable,
	FossilFuels,
	House,
	Windmill,
	RoadDown,
	RoadUp,
	RoadLeft,
	RoadRight,
	NuclearPP,
	Fallout,
	SolarPanel,
	OilMine,
	Plants,
	NuclearRuins,
	Ocean,
	Pipeline,
	BiogasPlant,
	Wall,
	WindmillOn,
	NuclearPPAlarm,
	DoubleBreakable,
	TripleBreakable
}

public enum WasFactoryAttacked
{
	Attacked,
	NotAttacked
}

// a Tile represents one space of the Board

[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour {

    // x and y position in the array
	public int xIndex;
	public int yIndex;

    // reference to our Board
	Board m_board;

    // our current TileType
	public TileType tileType = TileType.Normal;

	public WasFactoryAttacked wasAttacked = WasFactoryAttacked.NotAttacked;

	// the Sprite for this Tile
	SpriteRenderer m_spriteRenderer;

	// current "health" of a Breakable tile before it is removed
	public int breakableValue = 0;

	float pointsWaitTime = 0.5f;

    // array of Sprites used to show damage on Breakable Tile
	public Sprite[] breakableSprites;

    // sets the color of Breakable Tile back to normal once it is removed
	public Color normalColor;

	public int scoreValue = 20;

	//variable to store windmill rotation
	public int windmillRotation = 0;
	public Sprite[] windmillSprites;

	public AudioClip[] clearSound;
	bool m_isDestroyed;
	
	// Use this for initialization
	void Awake () 
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
		m_isDestroyed = false;

		// if the Tile is breakable, set its Sprite
		if (tileType == TileType.Breakable || tileType == TileType.DoubleBreakable || tileType == TileType.TripleBreakable || tileType == TileType.FossilFuels || tileType == TileType.House 
			|| tileType == TileType.NuclearPP || tileType == TileType.SolarPanel || tileType == TileType.OilMine || tileType == TileType.Plants || tileType == TileType.Fallout)
		{
			if (breakableSprites[breakableValue] !=null)
			{
				m_spriteRenderer.sprite = breakableSprites[breakableValue];
			}
		}
		if(tileType == TileType.Windmill)
        {
			if (windmillSprites[windmillRotation] != null)
			{
				m_spriteRenderer.sprite = windmillSprites[windmillRotation];
			}
		}
		
	}

    // if the mouse clicks the Collider on this Tile, run ClickTile on the Board
	void OnMouseDown()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
        {
			if (GameManager.Instance.BoosterIsActive == true)
			{
				GameManager.Instance.BoosterTile(this);
			}
			else
			{
				if (m_board != null && Board.m_playerInputEnabled == true)
				{
					m_board.ClickTile(this);
				}
			}
		}
			
	}

    // if the mousebutton is held and then the pointer is dragged into the Collider on this Tile...
    // run DragToTile on the Board, passing in this component 
	void OnMouseEnter()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (GameManager.Instance.BoosterIsActive == true)
			{
				GameManager.Instance.BoosterTile(this);
			}
			else
			{
				if (m_board != null && Board.m_playerInputEnabled == true)
				{
					m_board.DragToTile(this);
				}
			}
		}
	}

    // if we let go of the mouse button while on this Tile, run ReleaseTile on the Board
	void OnMouseUp()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (GameManager.Instance.BoosterIsActive == true)
			{
				GameManager.Instance.BoosterGoOff();
			}
			else
			{
				if (m_board != null && Board.m_playerInputEnabled == true)
				{
					m_board.ReleaseTile();
				}
			}
		}
	}

	// starts the coroutine to break a Breakable Tile
	public void BreakTile(float waitTime = 0f)
	{
		if (tileType != TileType.Breakable && tileType != TileType.FossilFuels 
			&& tileType != TileType.House && tileType != TileType.Fallout
		    && tileType != TileType.OilMine && tileType != TileType.DoubleBreakable && tileType != TileType.TripleBreakable)
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
			breakableValue = Mathf.Clamp(--breakableValue, 0, breakableValue);

			if (waitTime > 0f)
			{
				yield return new WaitForSeconds(waitTime);
			}

			if (breakableSprites[breakableValue] != null)
			{
				m_spriteRenderer.sprite = breakableSprites[breakableValue];
			}

			if (tileType == TileType.FossilFuels)
			{
				wasAttacked = WasFactoryAttacked.Attacked;
			}

			else if (tileType == TileType.DoubleBreakable && breakableValue == 1)
			{
				ParticleSystem ps = GetComponentInChildren<ParticleSystem>();

				if (ps != null)
				{
					ParticleSystem.MainModule ma = ps.main;

					ma.startColor = Color.white;

				}
			}

			else if (tileType == TileType.TripleBreakable && breakableValue == 2)
			{
				ParticleSystem ps = GetComponentInChildren<ParticleSystem>();

				if (ps != null)
				{
					ParticleSystem.MainModule ma = ps.main;

					ma.startColor = Color.black;

				}
			}

			if (breakableValue == 0 && m_isDestroyed == false)
			{
				m_isDestroyed = true;
				if (GameManager.Instance != null)
				{
					GameManager.Instance.UpdateCollectionGoalsTiles(this);
					GameManager.Instance.ScorePointsTiles(this);
					if (m_board.m_particleManager != null)
					{
						m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, pointsWaitTime);
					}
				}

				if (tileType == TileType.FossilFuels)
				{
					Board.factoriesOnBoard--;
				}

				if (tileType == TileType.House)
				{
					Board.citiesOnBoard--;
				}

				if (tileType == TileType.OilMine)
				{
					Board.oilMinesOnBoard--;
				}

				m_board.MakeNewTile(m_board.tileNormalPrefab, xIndex, yIndex);

			}
		}
		
		yield return null;
	}

	public void BreakNuclearTile(int x, int y, float waitTime = 0f)
	{
		StartCoroutine(BreakNuclearTileRoutine(x, y, waitTime));
	}


	IEnumerator BreakNuclearTileRoutine(int x, int y, float waitTime = 0f)
	{
		if(breakableValue > 0)
        {
			breakableValue = Mathf.Clamp(--breakableValue, 1, breakableValue);

			if (waitTime > 0f)
			{
				yield return new WaitForSeconds(waitTime);
			}

			if (breakableValue > 1)
			{
				if (breakableSprites[breakableValue] != null)
				{
					m_spriteRenderer.sprite = breakableSprites[breakableValue];
				}
			}
			if (breakableValue == 1 && m_isDestroyed == false)
			{
				m_isDestroyed = true;
				if (breakableSprites[1] != null)
				{
					m_spriteRenderer.sprite = breakableSprites[1];
				}

				m_board.MakeNewTile(m_board.tileAlarmNuclearPPPrefab, x, y);
			}
		}
		
		yield return null;
	}


	public void BreakAlarmNuclearTile(int x, int y, float waitTime = 0f)
	{
		StartCoroutine(BreakAlarmNuclearTileRoutine(x, y, waitTime));
	}


	IEnumerator BreakAlarmNuclearTileRoutine(int x, int y, float waitTime = 0f)
	{
		if(m_isDestroyed == false)
        {
			m_isDestroyed = true;

			if (waitTime > 0f)
			{
				yield return new WaitForSeconds(waitTime);
			}
			if (GameManager.Instance != null)
			{
				GameManager.Instance.UpdateCollectionGoalsTiles(this);
				GameManager.Instance.ScorePointsTiles(this);
				if (m_board.m_particleManager != null)
				{
					m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, waitTime);
				}
			}

			Board.nuclearDisasters++;

			Board.nuclearListX.Add(x);
			Board.nuclearListY.Add(y);

			yield return null;
		}
		
	}

	public void RotateTile(int x, int y, float waitTime = 0f)
	{
		if (tileType != TileType.Windmill)
		{
			return;
		}

		StartCoroutine(RotateTileRoutine(x, y, waitTime));
	}

	IEnumerator RotateTileRoutine(int x, int y, float waitTime = 0f)
	{
		windmillRotation = Mathf.Clamp(++windmillRotation, 0, 2);

		if (waitTime > 0f)
		{
			yield return new WaitForSeconds(waitTime);
		}
		if (windmillRotation == 2 && m_isDestroyed == false)
		{
			m_isDestroyed = true;
			if (GameManager.Instance != null)
			{
				GameManager.Instance.UpdateCollectionGoalsTiles(this);
				GameManager.Instance.ScorePointsTiles(this);
				if (m_board.m_particleManager != null)
				{
					m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, waitTime);
				}
			}
			windmillRotation = 0;
			m_board.MakeNewTile(m_board.tileWindmillOnPrefab, x, y);
		}

		if (windmillSprites[windmillRotation] != null && windmillRotation < windmillSprites.Length)
		{
			m_spriteRenderer.sprite = windmillSprites[windmillRotation];
		}
	}

	public void TurnOffWindmillTile(int x, int y, float waitTime = 0f)
	{

		if (tileType != TileType.WindmillOn)
		{
			return;
		}

		StartCoroutine(TurnOffWindmillTileRoutine(x, y, waitTime));
	}

	IEnumerator TurnOffWindmillTileRoutine(int x, int y, float waitTime = 0f)
	{
		if(m_isDestroyed == false)
        {
			m_isDestroyed = true;
			if (waitTime > 0f)
			{
				yield return new WaitForSeconds(waitTime);
			}
			foreach (CollectionGoal collectionGoal in m_board.level.collectionGoals)
			{
				if (collectionGoal != null)
				{
					if (collectionGoal.prefabToCollect == m_board.tileWindmillPrefab)
					{
						collectionGoal.numberToCollect++;
						if (UIManager.Instance != null)
						{
							UIManager.Instance.UpdateCollectionGoalLayout1(-1, 0);
						}
					}
				}
			}
			m_board.MakeNewTile(m_board.tileWindmillPrefab, x, y);
			yield return null;
		}
	}

	public void TurnOnSolarPanel(float waitTime = 0f)
	{
			if (tileType != TileType.SolarPanel)
			{
				return;
			}

			StartCoroutine(TurnOnSolarPanelRoutine(waitTime));	
	}

	IEnumerator TurnOnSolarPanelRoutine(float waitTime = 0f)
	{
		if(breakableValue > 0 && m_isDestroyed == false)
        {
			m_isDestroyed = true;
			breakableValue = Mathf.Clamp(--breakableValue, 0, breakableValue);

			if (waitTime > 0)
			{
				yield return new WaitForSeconds(waitTime);
			}

			if (GameManager.Instance != null)
			{
				GameManager.Instance.UpdateCollectionGoalsTiles(this);
				GameManager.Instance.ScorePointsTiles(this);
				if (m_board.m_particleManager != null)
				{
					m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, waitTime);
				}
			}

			m_spriteRenderer.sprite = breakableSprites[0];
		}

		yield return null;
	}

	public void GrowPlants(float waitTime = 0f)
	{
			if (tileType != TileType.Plants)
			{
				return;
			}

			StartCoroutine(GrowPlantsRoutine(waitTime));
	}

	IEnumerator GrowPlantsRoutine(float waitTime = 0f)
	{ 
		if(breakableValue > 0)
        {
			breakableValue = Mathf.Clamp(--breakableValue, 0, breakableValue);

			yield return new WaitForSeconds(0.01f + waitTime);

			if (SoundManager.Instance != null)
			{
                if (!Board.isWateringSoundPlaying)
                {
					Board.isWateringSoundPlaying = true;
					SoundManager.Instance.PlayWateringCanSound();
				}
				
			}

			if (breakableValue == 0 && m_isDestroyed == false)
            {
				m_isDestroyed = true;
				if (GameManager.Instance != null)
				{
					GameManager.Instance.UpdateCollectionGoalsTiles(this);
					GameManager.Instance.ScorePointsTiles(this);
					if (m_board.m_particleManager != null)
					{
						m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, waitTime);
					}
				}
			}
			
			if (breakableSprites[breakableValue] != null)
			{
				m_spriteRenderer.sprite = breakableSprites[breakableValue];
			}
		}	
		yield return null;
	}

	public void BreakNuclearRuinsTile(int x, int y, float waitTime = 0f)
	{
		StartCoroutine(BreakNuclearRuinsTileRoutine(x, y, waitTime));
	}


	IEnumerator BreakNuclearRuinsTileRoutine(int x, int y, float waitTime = 0f)
	{
		if(breakableValue > 0)
        {
			breakableValue = Mathf.Clamp(--breakableValue, 0, breakableValue);

			if(waitTime > 0f)
            {
				yield return new WaitForSeconds(waitTime);
			}
		
			if (breakableValue > 0)
			{
				if (breakableSprites[breakableValue] != null)
				{
					m_spriteRenderer.sprite = breakableSprites[breakableValue];
				}
			}
			if (breakableValue == 0 && m_isDestroyed == false)
			{
				m_isDestroyed = true;
				if (GameManager.Instance != null)
				{
					GameManager.Instance.UpdateCollectionGoalsTiles(this);
					GameManager.Instance.ScorePointsTiles(this);
					if (m_board.m_particleManager != null)
					{
						m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, waitTime);
					}
				}
				m_board.MakeNewTile(m_board.tileNormalPrefab, x, y);
			}
		}
		
		yield return null;
	}

	public void TurnGasFlow(float waitTime = 0f)
	{
		if (tileType != TileType.Pipeline)
		{
			return;
		}

		StartCoroutine(TurnGasFlowRoutine(waitTime));
	}

	IEnumerator TurnGasFlowRoutine(float waitTime = 0f)
	{
		if(breakableValue > 0)
		{
			//yield return new WaitForSeconds(0.4f);

			breakableValue = 0;
			if (GameManager.Instance != null)
			{
				GameManager.Instance.UpdateCollectionGoalsTiles(this);
				GameManager.Instance.ScorePointsTiles(this);
				if (m_board.m_particleManager != null)
				{
					m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, waitTime);
				}
			}
			if (breakableSprites[0] != null)
			{
				m_spriteRenderer.color = Color.white;
				m_spriteRenderer.sprite = breakableSprites[0];
				
			}
		}

		yield return null;
	}

	public void TurnOnBiogasPlant(float waitTime = 0f)
	{
		if (tileType != TileType.BiogasPlant)
		{
			return;
		}

		StartCoroutine(TurnOnBiogasPlantRoutine(waitTime));
	}

	IEnumerator TurnOnBiogasPlantRoutine(float waitTime = 0f)
	{
		
		if(breakableValue > 0)
		{
			if(waitTime > 0f)
            {
				yield return new WaitForSeconds(waitTime);
			}
			
			breakableValue = 0;

			if (breakableSprites[0] != null)
			{
				m_spriteRenderer.sprite = breakableSprites[0];
			}

			if (GameManager.Instance != null)
			{
				GameManager.Instance.UpdateCollectionGoalsTiles(this);
				GameManager.Instance.ScorePointsTiles(this);
				if (m_board.m_particleManager != null)
				{
					m_board.m_particleManager.PointsFXAt(xIndex, yIndex, scoreValue, MatchValue.None, -4, waitTime);
				}
			}
		}
		
		yield return null;
	}
}
