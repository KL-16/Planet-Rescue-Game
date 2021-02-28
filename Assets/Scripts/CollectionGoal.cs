using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : MonoBehaviour
{
    public GameObject prefabToCollect;

    [Range(1,100)]
    public int numberToCollect = 5;

    SpriteRenderer m_spriteRenderer;
    GamePiece m_gamePiece;
    Tile m_tile;
    Tiles2ndLayer m_tile2nd;
    Earth m_earth;
    Ocean m_ocean;

    // Use this for initialization
    void Start()
    {
        if (prefabToCollect != null)
        {
            m_spriteRenderer = prefabToCollect.GetComponent<SpriteRenderer>();
        }
    }

    public void CollectPiece(GamePiece piece)
    {
        if (piece != null)
        {
            SpriteRenderer spriteRenderer = piece.GetComponent<SpriteRenderer>();
            m_gamePiece = prefabToCollect.GetComponent<GamePiece>();
            if(m_spriteRenderer != null)
            {
                if(m_gamePiece != null)
                {
                    if (m_gamePiece.matchValue == MatchValue.None)
                    {
                        Bomb bomb = m_gamePiece.GetComponent<Bomb>();
                        if (bomb != null)
                        {
                            Bomb bomb1 = piece.GetComponent<Bomb>();
                            if(bomb1 != null && bomb.bombType == bomb1.bombType)
                            {
                                //numberToCollect--;
                                numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);
                            }

                        }
                        else
                        {
                            if (m_spriteRenderer.sprite == spriteRenderer.sprite && m_gamePiece.matchValue == piece.matchValue)
                            {
                                //numberToCollect--;
                                numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);
                            }
                        }                      
                    }

                    else
                    {
                        Bomb bomb = m_gamePiece.GetComponent<Bomb>();

                        if(bomb != null)
                        {
                            Bomb bomb1 = piece.GetComponent<Bomb>();
                            if(bomb1 != null && bomb.bombType == bomb1.bombType && m_gamePiece.matchValue == piece.matchValue)
                            {                             
                                    //numberToCollect--;
                                    numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);                              
                            }
                            
                        }
                        else
                        {
                            if (m_gamePiece.matchValue == piece.matchValue)
                            {
                                //numberToCollect--;
                                numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);
                            }
                        }
                        
                    }
                }
                               
            }           
        }
    }

    public void CollectTile(Tile tile)
    {
        if (tile != null)
        {
            m_tile = prefabToCollect.GetComponent<Tile>();
            if (m_tile != null && m_tile.tileType == tile.tileType)
            {
                //numberToCollect--;
                numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);
            }
        }
    }

    public void CollectTile2nd(Tiles2ndLayer tile)
    {
        if (tile != null)
        {
            m_tile2nd = prefabToCollect.GetComponent<Tiles2ndLayer>();
            if (m_tile2nd != null && m_tile2nd.tileType == tile.tileType)
            {
                //numberToCollect--;
                numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);
            }
        }
    }

    public void CollectEarth(Earth earth)
    {
        if (earth != null)
        {
            m_earth = prefabToCollect.GetComponent<Earth>();

            if (m_earth != null)
            {
                //numberToCollect--;
                numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);
            }
        }
    }

    public void CollectPlanet(Ocean ocean)
    {
        if (ocean != null)
        {
            m_ocean = prefabToCollect.GetComponent<Ocean>();

            if (m_ocean != null)
            {
                //numberToCollect--;
                numberToCollect = Mathf.Clamp(--numberToCollect, 0, numberToCollect);
            }
        }
    }

    public void MakeNewCollection(GameObject gameObject, int number)
    {
        prefabToCollect = gameObject;
        numberToCollect = number;
    }
}
