using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this manager class handles particle effects
public class ParticleManager : MonoBehaviour
{
    // prefab GameObject for clearing a GamePiece
	public GameObject clearBlueFXPrefab;
	public GameObject clearGreenFXPrefab;
	public GameObject clearRedFXPrefab;
	public GameObject clearPurpleFXPrefab;
	public GameObject clearYellowFXPrefab;
	public GameObject clearWhiteFXPrefab;

	public GameObject clearTBagFXPrefab;
	public GameObject clearBarrelFXPrefab;
	public GameObject clearOiledPieceFXPrefab;

	// prefab GameObject for clearing a GamePiece
	public GameObject rotationFXPrefab;
	public GameObject rotationOnFXPrefab;

	public GameObject explosionFXPrefab;

	public GameObject shuffleLightFXPrefab;
	public GameObject shuffleDarkFXPrefab;

	public GameObject barrelExplodeFXPrefab;

	// prefab GameObject for breaking a Tile 
	public GameObject breakFXPrefab;

    // prefab GameObject for breaking a Doublebreak Tile effect
	public GameObject doubleBreakFXPrefab;

    // prefab GameObject for the bomb explosion effect
	public GameObject bombFXPrefab;

	public GameObject tileBreakNuclearFXPrefab;
	public GameObject tileNuclearGoneFXPrefab;
	public GameObject tileNuclearRBreakFXPrefab;
	public GameObject tileFalloutFXPrefab;
	public GameObject tileBreakFossilFXPrefab;
	public GameObject tileBreakOilPumpFXPrefab;
	public GameObject tileBreakMiningPitFXPrefab;
	public GameObject tileMiningAppearFXPrefab;
	public GameObject tileLitterAppearFXPrefab;
	public GameObject tileBarrelAppearFXPrefab;

	public GameObject tileFossilGoneFXPrefab;
	public GameObject tileCityGoneFXPrefab;
	public GameObject tileOilMineGoneFXPrefab;

	public GameObject tileSolarOnFXPrefab;

	public GameObject tileWasteDumbWithTrashFXPrefab;
	public GameObject tileWasteDumbFXPrefab;

	//public GameObject waterDamPrefab;

	public GameObject verticalBlueFXPrefab;
	public GameObject verticalGreenFXPrefab;
	public GameObject verticalYellowFXPrefab;
	public GameObject verticalRedFXPrefab;
	public GameObject verticalPurpleFXPrefab;
	public GameObject verticalWhiteFXPrefab;

	public GameObject horizontalBlueFXPrefab;
	public GameObject horizontalGreenFXPrefab;
	public GameObject horizontalYellowFXPrefab;
	public GameObject horizontalRedFXPrefab;
	public GameObject horizontalPurpleFXPrefab;
	public GameObject horizontalWhiteFXPrefab;

	public GameObject adjacentBlueFXPrefab;
	public GameObject adjacentGreenFXPrefab;
	public GameObject adjacentRedFXPrefab;
	public GameObject adjacentYellowFXPrefab;
	public GameObject adjacentPurpleFXPrefab;
	public GameObject adjacentWhiteFXPrefab;

	public GameObject adjacentBigBlueFXPrefab;
	public GameObject adjacentBigGreenFXPrefab;
	public GameObject adjacentBigPurpleFXPrefab;
	public GameObject adjacentBigRedFXPrefab;
	public GameObject adjacentBigYellowFXPrefab;
	public GameObject adjacentBigWhiteFXPrefab;

	public GameObject adjacentBigBlueFastFXPrefab;
	public GameObject adjacentBigGreenFastFXPrefab;
	public GameObject adjacentBigPurpleFastFXPrefab;
	public GameObject adjacentBigRedFastFXPrefab;
	public GameObject adjacentBigWhiteFastFXPrefab;
	public GameObject adjacentBigYellowFastFXPrefab;

	public GameObject mixedBigBlueFXPrefab;
	public GameObject mixedBigGreenFXPrefab;
	public GameObject mixedBigPurpleFXPrefab;
	public GameObject mixedBigRedFXPrefab;
	public GameObject mixedBigYellowFXPrefab;
	public GameObject mixedBigWhiteFXPrefab;

	public GameObject crossBlueFXPrefab;
	public GameObject crossGreenFXPrefab;
	public GameObject crossPurpleFXPrefab;
	public GameObject crossRedFXPrefab;
	public GameObject crossYellowFXPrefab;
	public GameObject crossWhiteFXPrefab;

	public GameObject colorFXPrefab;
	public GameObject colorPickedFXPrefab;

	public GameObject DoubleColorFXPrefab;

	public GameObject brokenBarrelFXPrefab;

	//roads animations
	public GameObject roadUpFXprefab;
	public GameObject roadUp0_3FXprefab;
	public GameObject roadUp9_12FXprefab;

	public GameObject roadDownFXprefab;
	public GameObject roadDown3_6FXprefab;
	public GameObject roadDown6_9FXprefab;

	public GameObject roadLeftFXprefab;
	public GameObject roadLeft6_9FXprefab;
	public GameObject roadLeft9_12FXprefab;

	public GameObject roadRightFXprefab;
	public GameObject roadRight0_3FXprefab;
	public GameObject roadRight3_6FXprefab;

	////points

	public GameObject FXprefab20;
	public GameObject FXprefab30;
	public GameObject FXprefab40;
	public GameObject FXprefab60;
	public GameObject FXprefab80;
	public GameObject FXprefab90;
	public GameObject FXprefab120;
	public GameObject FXprefab160;
	public GameObject FXprefab50;
	public GameObject FXprefab100;
	public GameObject FXprefab150;
	public GameObject FXprefab200;
	public GameObject FXprefab500;
	public GameObject FXprefab1000;
	public GameObject FXprefab1500;
	public GameObject FXprefab2000;
	public GameObject FXprefab400;
	public GameObject FXprefab600;
	public GameObject FXprefab800;
	public GameObject FXprefab240;
	public GameObject FXprefab360;
	public GameObject FXprefab480;
	public GameObject FXprefab180;
	public GameObject FXprefab70;
	public GameObject FXprefab140;
	public GameObject FXprefab210;
	public GameObject FXprefab280;
	public GameObject FXprefab300;

	//combos
	public GameObject FX2x;
	public GameObject FX3x;
	public GameObject FX4x;
	public GameObject FX5x;
	public GameObject FX6x;
	public GameObject FXultrax;


	/// //////////////


	public GameObject greenHouseGassesFXPrefab;
	public GameObject greenHouseGoneFXPrefab;

	public GameObject smoke1FXPrefab;
	public GameObject smoke2FXPrefab;
	public GameObject smoke3FXPrefab;

	public GameObject recycleFXPrefab;

	public GameObject fireworksFXPrefab;
	public GameObject fireworks2FXPrefab;
	public GameObject fireworks3FXPrefab;

	public GameObject thunderFXPrefab;

	public GameObject biogasPlantFXPrefab;
	public GameObject chainBrakeFXPrefab;

	public GameObject waterPlant1FXPrefab;
	public GameObject waterPlant2FXPrefab;
	public GameObject waterPlant3FXPrefab;

	public GameObject releaseForestFXPrefab;

	List<GameObject> matchFXs = new List<GameObject>();


	public GameObject matchGlowPrefab;

	//notes
	public GameObject shufflingNoteFXPrefab;





	// play the clear GamePiece effect
	public void ClearPieceFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(ClearPieceFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator ClearPieceFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		if (waitTime != 0)
		{
			yield return new WaitForSeconds(waitTime);
		}
		
		GameObject clearFX = null;

		if(matchValue == MatchValue.Blue)
		{
			if (clearBlueFXPrefab != null)
			{
				clearFX = Instantiate(clearBlueFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (clearGreenFXPrefab != null)
			{
				clearFX = Instantiate(clearGreenFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}
		if (matchValue == MatchValue.Red)
		{
			if (clearRedFXPrefab != null)
			{
				clearFX = Instantiate(clearRedFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}
		if (matchValue == MatchValue.Purple)
		{
			if (clearPurpleFXPrefab != null)
			{
				clearFX = Instantiate(clearPurpleFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}
		if (matchValue == MatchValue.Yellow)
		{
			if (clearYellowFXPrefab != null)
			{
				clearFX = Instantiate(clearYellowFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}

		if(matchValue == MatchValue.White)
		{
			if (clearWhiteFXPrefab != null)
			{
				clearFX = Instantiate(clearWhiteFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Litter)
		{
			if (clearTBagFXPrefab != null)
			{
				clearFX = Instantiate(clearTBagFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Barrel)
		{
			if (clearBarrelFXPrefab != null)
			{
				clearFX = Instantiate(clearBarrelFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.BrokenBarrel)
		{
			if (brokenBarrelFXPrefab != null)
			{
				clearFX = Instantiate(brokenBarrelFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}

		if (clearFX != null)
		{
			ParticlePlayer particlePlayer = clearFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}	
		
	}

	public void ColorPickedFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(ColorPickedFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ColorPickedFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (colorPickedFXPrefab != null)
		{
			GameObject colorPickedFX = Instantiate(colorPickedFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = colorPickedFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void RecycleFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(RecycleFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator RecycleFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (recycleFXPrefab != null)
		{
			GameObject recycleFX = Instantiate(recycleFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = recycleFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void ComboFXAt(float x, float y, float scale, int multiplier, int z = -8, float waitTime = 0f)
	{
		StartCoroutine(ComboFXAtRoutine(x, y, scale, multiplier, z, waitTime));
	}

	IEnumerator ComboFXAtRoutine(float x, float y, float scale, int multiplier, int z = -8, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		GameObject comboFX = null;

		switch (multiplier)
		{
			case 2:
				if(FX2x != null)
				{
					comboFX = Instantiate(FX2x, new Vector3(x, y, z), Quaternion.identity) as GameObject;				
				}
				break;
			case 3:
				if (FX3x != null)
				{
					comboFX = Instantiate(FX3x, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;
			case 4:
				if (FX4x != null)
				{
					comboFX = Instantiate(FX4x, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;
			case 5:
				if (FX5x != null)
				{
					comboFX = Instantiate(FX5x, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;
			case 6:
				if (FX2x != null)
				{
					comboFX = Instantiate(FX6x, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;
			case 7:
				if (FXultrax != null)
				{
					comboFX = Instantiate(FXultrax, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;
		}
		
		if(comboFX != null)
		{
			comboFX.transform.localScale = new Vector3(scale, scale, 1f);
			ParticlePlayer particlePlayer = comboFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
					
	}


	public void ShufflingNoteFXAt(float x, float y, float scale, int z = -8, float waitTime = 0f)
	{
		StartCoroutine(ShufflingNoteFXAtRoutine(x, y, scale, z, waitTime));
	}

	IEnumerator ShufflingNoteFXAtRoutine(float x, float y, float scale, int z = -8, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (shufflingNoteFXPrefab != null)
		{
			
			GameObject FX = Instantiate(shufflingNoteFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			 
			FX.transform.localScale = new Vector3(scale, scale, 1f); 

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void ReleaseForestFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(ReleaseForestFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ReleaseForestFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (releaseForestFXPrefab != null)
		{
			GameObject forestFX = Instantiate(releaseForestFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = forestFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void ClearOiledPieceFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(ClearOiledPieceFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ClearOiledPieceFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (clearOiledPieceFXPrefab != null)
		{
			GameObject FX = Instantiate(clearOiledPieceFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void GreenHouseGasesFXAt(float x, float y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(GreenHouseGasesFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator GreenHouseGasesFXAtRoutine(float x, float y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (greenHouseGassesFXPrefab != null)
		{
			GameObject FX = Instantiate(greenHouseGassesFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void GreenHouseGoneFXAt(float x, float y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(GreenHouseGoneFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator GreenHouseGoneFXAtRoutine(float x, float y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (greenHouseGoneFXPrefab != null)
		{
			GameObject FX = Instantiate(greenHouseGoneFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void BarrelExplodeFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(BarrelExplodeFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator BarrelExplodeFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (barrelExplodeFXPrefab != null)
		{
			GameObject colorPickedFX = Instantiate(barrelExplodeFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = colorPickedFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void WasteDumpWithTrashFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(WasteDumpWithTrashFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator WasteDumpWithTrashFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileWasteDumbWithTrashFXPrefab != null)
		{
			GameObject smoke1FX = Instantiate(tileWasteDumbWithTrashFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke1FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void MoveOnRoadFXAt(int x, int y, int type, int z = 2, float waitTime = 0f)
	{
		StartCoroutine(MoveOnRoadFXAtRoutine(x, y, type, z, waitTime));
	}

	IEnumerator MoveOnRoadFXAtRoutine(int x, int y, int type, int z = 2, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject roadFX = null;

		switch (type)
		{
			//up
			case 1:
				if (roadUpFXprefab != null)
				{
					roadFX = Instantiate(roadUpFXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 2:
				if (roadUp0_3FXprefab != null)
				{
					roadFX = Instantiate(roadUp0_3FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 3:
				if (roadUp9_12FXprefab != null)
				{
					roadFX = Instantiate(roadUp9_12FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			//down
			case 4:
				if (roadDownFXprefab != null)
				{
					roadFX = Instantiate(roadDownFXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 5:
				if (roadDown3_6FXprefab != null)
				{
					roadFX = Instantiate(roadDown3_6FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 6:
				if (roadDown6_9FXprefab != null)
				{
					roadFX = Instantiate(roadDown6_9FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			//left
			case 7:
				if (roadLeftFXprefab != null)
				{
					roadFX = Instantiate(roadLeftFXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 8:
				if (roadLeft6_9FXprefab != null)
				{
					roadFX = Instantiate(roadLeft6_9FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 9:
				if (roadLeft9_12FXprefab != null)
				{
					roadFX = Instantiate(roadLeft9_12FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			//right
			case 10:
				if (roadRightFXprefab != null)
				{
					roadFX = Instantiate(roadRightFXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 11:
				if (roadRight0_3FXprefab != null)
				{
					roadFX = Instantiate(roadRight0_3FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

			case 12:
				if (roadRight3_6FXprefab != null)
				{
					roadFX = Instantiate(roadRight3_6FXprefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
				break;

		}

		if(roadFX != null)
		{
			ParticlePlayer particlePlayer = roadFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
		
	}


	public void SolarOnFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(SolarOnFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator SolarOnFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileSolarOnFXPrefab != null)
		{
			GameObject smoke1FX = Instantiate(tileSolarOnFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke1FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void WaterPlant1FXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(WaterPlant1FXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator WaterPlant1FXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (waterPlant1FXPrefab != null)
		{
			GameObject smoke1FX = Instantiate(waterPlant1FXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke1FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void WaterPlant2FXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(WaterPlant2FXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator WaterPlant2FXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (waterPlant2FXPrefab != null)
		{
			GameObject smoke1FX = Instantiate(waterPlant2FXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke1FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void WaterPlant3FXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(WaterPlant3FXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator WaterPlant3FXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (waterPlant3FXPrefab != null)
		{
			GameObject smoke1FX = Instantiate(waterPlant3FXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke1FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void WasteDumpFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(WasteDumpFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator WasteDumpFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileWasteDumbFXPrefab != null)
		{
			GameObject smoke1FX = Instantiate(tileWasteDumbFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke1FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void Smoke1FXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(Smoke1FXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator Smoke1FXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (smoke1FXPrefab != null)
		{
			GameObject smoke1FX = Instantiate(smoke1FXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke1FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void Smoke2FXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(Smoke2FXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator Smoke2FXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (smoke2FXPrefab != null)
		{
			GameObject smoke2FX = Instantiate(smoke2FXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke2FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void Smoke3FXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(Smoke3FXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator Smoke3FXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (smoke3FXPrefab != null)
		{
			GameObject smoke3FX = Instantiate(smoke3FXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = smoke3FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void FireworkFXAt(int width, int height, float waitTime = 0f)
	{
		StartCoroutine(FireworkFXAtRoutine(width, height, waitTime));
	}

	IEnumerator FireworkFXAtRoutine(int width, int height, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (fireworksFXPrefab != null && fireworks2FXPrefab != null && fireworks3FXPrefab != null)
		{
			for(int iteration = 1; iteration < 20; iteration++)
			{
				int randomX = UnityEngine.Random.Range(0, width + 1);
				int randomY = UnityEngine.Random.Range(0, height + 1);
				int randomType = UnityEngine.Random.Range(0, 3);

				if(randomType == 0)
				{
					GameObject fireworkFX = Instantiate(fireworksFXPrefab, new Vector3(randomX, randomY, -5), Quaternion.identity) as GameObject;

					ParticlePlayer particlePlayer = fireworkFX.GetComponent<ParticlePlayer>();

					if (particlePlayer != null)
					{
						particlePlayer.Play();
					}
				}

				if(randomType == 1)
				{
					GameObject firework2FX = Instantiate(fireworks2FXPrefab, new Vector3(randomX, randomY, -5), Quaternion.identity) as GameObject;

					ParticlePlayer particlePlayer2 = firework2FX.GetComponent<ParticlePlayer>();

					if (particlePlayer2 != null)
					{
						particlePlayer2.Play();
					}
				}

				if(randomType == 2)
				{
					GameObject firework3FX = Instantiate(fireworks3FXPrefab, new Vector3(randomX, randomY, -5), Quaternion.identity) as GameObject;

					ParticlePlayer particlePlayer3 = firework3FX.GetComponent<ParticlePlayer>();

					if (particlePlayer3 != null)
					{
						particlePlayer3.Play();
					}
				}

				yield return new WaitForSeconds(0.07f);
			}		

		}
	}


	public void RotationFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(RotationFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator RotationFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (rotationFXPrefab != null)
		{
			GameObject rotationFX = Instantiate(rotationFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = rotationFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void RotationOnFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(RotationOnFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator RotationOnFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (rotationOnFXPrefab != null)
		{
			GameObject rotationFX = Instantiate(rotationOnFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = rotationFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}


	public void BiogasPlantFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(BiogasPlantFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator BiogasPlantFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (biogasPlantFXPrefab != null)
		{
			GameObject biogasFX = Instantiate(biogasPlantFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = biogasFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void ChainBrakeFXAt(int x, int y, int z = 2, float waitTime = 0f)
	{
		StartCoroutine(ChainBrakeFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ChainBrakeFXAtRoutine(int x, int y, int z = 2, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (chainBrakeFXPrefab != null)
		{
			GameObject biogasFX = Instantiate(chainBrakeFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = biogasFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void ShuffleFXAt(int x, int y, int z = -6, float waitTime = 1f)
	{
		StartCoroutine(ShuffleFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ShuffleFXAtRoutine(int x, int y, int z = -1, float waitTime = 1f)
	{
		yield return new WaitForSeconds(waitTime);


		if (x % 2 == 0)
		{
			if (y % 2 == 0)
			{
				if (shuffleLightFXPrefab != null)
				{
					GameObject shuffleFX = Instantiate(shuffleLightFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

					ParticlePlayer particlePlayer = shuffleFX.GetComponent<ParticlePlayer>();

					if (particlePlayer != null)
					{
						particlePlayer.Play();
					}
				}
			}
			else
			{
				if (shuffleDarkFXPrefab != null)
				{
					GameObject shuffleFX = Instantiate(shuffleDarkFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

					ParticlePlayer particlePlayer = shuffleFX.GetComponent<ParticlePlayer>();

					if (particlePlayer != null)
					{
						particlePlayer.Play();
					}
				}
			}
		}
		else
		{
			if (y % 2 == 0)
			{
				if (shuffleDarkFXPrefab != null)
				{
					GameObject shuffleFX = Instantiate(shuffleDarkFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

					ParticlePlayer particlePlayer = shuffleFX.GetComponent<ParticlePlayer>();

					if (particlePlayer != null)
					{
						particlePlayer.Play();
					}
				}
			}
			else
			{
				if (shuffleLightFXPrefab != null)
				{
					GameObject shuffleFX = Instantiate(shuffleLightFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

					ParticlePlayer particlePlayer = shuffleFX.GetComponent<ParticlePlayer>();

					if (particlePlayer != null)
					{
						particlePlayer.Play();
					}
				}
			}
		}

		
	}


	public void RemoveMatchFX(int numberOfGlows, float waitTime = 0f)
	{
		StartCoroutine(RemoveMatchFXRoutine(numberOfGlows, waitTime));
	}

	IEnumerator RemoveMatchFXRoutine(int numberOfGlows, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		for (int i = matchFXs.Count - 1; i >= matchFXs.Count - numberOfGlows; i--)
		{
			if (i > 0)
			{
				if (matchFXs[i] != null)
				{
					ParticlePlayer particlePlayer = matchFXs[i].GetComponent<ParticlePlayer>();
					particlePlayer.Stop();
					Destroy(matchFXs[i].gameObject);
				}
			}
		}

		matchFXs.Clear();
	}

	public void PossibleMatchFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(PossibleMatchFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator PossibleMatchFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (matchFXs.Count > 25)
		{
			for (int i = 0; i < 13; i++)
			{
				matchFXs.RemoveAt(i);
			}
		}
		if (rotationFXPrefab != null)
		{
			GameObject glowFX = Instantiate(matchGlowPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = glowFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
				matchFXs.Add(glowFX);
			}
		}
	}

	public void ExplosionFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(ExplosionFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ExplosionFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (explosionFXPrefab != null)
		{
			GameObject explosionFX = Instantiate(explosionFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = explosionFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void DoubleColorFXAt(float x, float y, float z = 0, float waitTime = 0f)
	{
		StartCoroutine(DoubleColorFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator DoubleColorFXAtRoutine(float x, float y, float z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (DoubleColorFXPrefab != null)
		{
			GameObject explosionFX = Instantiate(DoubleColorFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = explosionFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void VerticalFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(VerticalFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator VerticalFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		GameObject verticalFX = null;

		if (matchValue == MatchValue.Blue)
		{
			if (verticalBlueFXPrefab != null)
			{
				verticalFX = Instantiate(verticalBlueFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (verticalGreenFXPrefab != null)
			{
				verticalFX = Instantiate(verticalGreenFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Red)
		{
			if (verticalRedFXPrefab != null)
			{
				verticalFX = Instantiate(verticalRedFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}
		if (matchValue == MatchValue.Yellow)
		{
			if (verticalYellowFXPrefab != null)
			{
				verticalFX = Instantiate(verticalYellowFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}
		if (matchValue == MatchValue.Purple)
		{
			if (verticalPurpleFXPrefab != null)
			{
				verticalFX = Instantiate(verticalPurpleFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.White)
		{
			if (verticalWhiteFXPrefab != null)
			{
				verticalFX = Instantiate(verticalWhiteFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (verticalFX != null)
		{
			ParticlePlayer particlePlayer = verticalFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
				
	}

	public void HorizontalFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(HorizontalFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator HorizontalFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject horizontalFX = null;

		if(matchValue == MatchValue.Blue)
		{
			if (horizontalBlueFXPrefab != null)
			{
				horizontalFX = Instantiate(horizontalBlueFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (horizontalGreenFXPrefab != null)
			{
				horizontalFX = Instantiate(horizontalGreenFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Red)
		{
			if (horizontalRedFXPrefab != null)
			{
				horizontalFX = Instantiate(horizontalRedFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Yellow)
		{
			if (horizontalYellowFXPrefab != null)
			{
				horizontalFX = Instantiate(horizontalYellowFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Purple)
		{
			if (horizontalPurpleFXPrefab != null)
			{
				horizontalFX = Instantiate(horizontalPurpleFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.White)
		{
			if (horizontalWhiteFXPrefab != null)
			{
				horizontalFX = Instantiate(horizontalWhiteFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (horizontalFX != null)
		{
			ParticlePlayer particlePlayer = horizontalFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}	
		
	}

	public void AdjacentFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(AdjacentFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator AdjacentFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject adjacentFX = null;

		if(matchValue == MatchValue.Blue)
		{
			if (adjacentBlueFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBlueFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (adjacentGreenFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentGreenFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Red)
		{
			if (adjacentRedFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentRedFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Yellow)
		{
			if (adjacentYellowFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentYellowFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Purple)
		{
			if (adjacentPurpleFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentPurpleFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.White)
		{
			if (adjacentWhiteFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentWhiteFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (adjacentFX != null)
		{
			ParticlePlayer particlePlayer = adjacentFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}	
		
	}

	public void AdjacentBigFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(AdjacentBigFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator AdjacentBigFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject adjacentFX = null;

		if (matchValue == MatchValue.Blue)
		{
			if (adjacentBigBlueFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigBlueFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (adjacentBigGreenFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigGreenFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Red)
		{
			if (adjacentBigRedFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigRedFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Yellow)
		{
			if (adjacentBigYellowFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigYellowFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Purple)
		{
			if (adjacentBigPurpleFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigPurpleFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.White)
		{
			if (adjacentBigWhiteFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigWhiteFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}


		if (adjacentFX != null)
		{
			ParticlePlayer particlePlayer = adjacentFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}

	}

	public void AdjacentBigFastFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(AdjacentBigFastFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator AdjacentBigFastFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject adjacentFX = null;

		if (matchValue == MatchValue.Blue)
		{
			if (adjacentBigBlueFastFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigBlueFastFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (adjacentBigGreenFastFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigGreenFastFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Red)
		{
			if (adjacentBigRedFastFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigRedFastFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Yellow)
		{
			if (adjacentBigYellowFastFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigYellowFastFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Purple)
		{
			if (adjacentBigPurpleFastFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigPurpleFastFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.White)
		{
			if (adjacentBigWhiteFastFXPrefab != null)
			{
				adjacentFX = Instantiate(adjacentBigWhiteFastFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}


		if (adjacentFX != null)
		{
			ParticlePlayer particlePlayer = adjacentFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}

	}

	public void MixedBigFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(MixedBigFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator MixedBigFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject adjacentFX = null;

		if (matchValue == MatchValue.Blue)
		{
			if (mixedBigBlueFXPrefab != null)
			{
				adjacentFX = Instantiate(mixedBigBlueFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (mixedBigGreenFXPrefab != null)
			{
				adjacentFX = Instantiate(mixedBigGreenFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Red)
		{
			if (mixedBigRedFXPrefab != null)
			{
				adjacentFX = Instantiate(mixedBigRedFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Yellow)
		{
			if (mixedBigYellowFXPrefab != null)
			{
				adjacentFX = Instantiate(mixedBigYellowFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Purple)
		{
			if (mixedBigPurpleFXPrefab != null)
			{
				adjacentFX = Instantiate(mixedBigPurpleFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.White)
		{
			if (mixedBigWhiteFXPrefab != null)
			{
				adjacentFX = Instantiate(mixedBigWhiteFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}


		if (adjacentFX != null)
		{
			ParticlePlayer particlePlayer = adjacentFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}

	}

	public void CrossFXAt(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(CrossFXAtRoutine(x, y, matchValue, z, waitTime));
	}

	IEnumerator CrossFXAtRoutine(int x, int y, MatchValue matchValue, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);
		GameObject crossFX = null;

		if (matchValue == MatchValue.Blue)
		{
			if (crossBlueFXPrefab != null)
			{
				crossFX = Instantiate(crossBlueFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Green)
		{
			if (crossGreenFXPrefab != null)
			{
				crossFX = Instantiate(crossGreenFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Red)
		{
			if (crossRedFXPrefab != null)
			{
				crossFX = Instantiate(crossRedFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Yellow)
		{
			if (crossYellowFXPrefab != null)
			{
				crossFX = Instantiate(crossYellowFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.Purple)
		{
			if (crossPurpleFXPrefab != null)
			{
				crossFX = Instantiate(crossPurpleFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}

		if (matchValue == MatchValue.White)
		{
			if (crossWhiteFXPrefab != null)
			{
				crossFX = Instantiate(crossWhiteFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;
			}
		}


		if (crossFX != null)
		{
			ParticlePlayer particlePlayer = crossFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}

	}

	public void ColorFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(ColorFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ColorFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (colorFXPrefab != null)
		{
			GameObject colorFX = Instantiate(colorFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = colorFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void ThunderFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(ThunderFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator ThunderFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(0f);

		if (thunderFXPrefab != null)
		{
			GameObject thunderFX = Instantiate(thunderFXPrefab, new Vector3(x, y + 4, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = thunderFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}


	/*public void WaterDamFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(WaterDamFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator WaterDamFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (waterDamPrefab != null)
		{
			GameObject waterDamFX = Instantiate(waterDamPrefab, new Vector3(x, y - 0.2f, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = waterDamFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	} */

	public void TileBreakNuclearFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileBreakNuclearFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileBreakNuclearFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileBreakNuclearFXPrefab != null)
		{
			GameObject TBFX = Instantiate(tileBreakNuclearFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = TBFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileNuclearGoneFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileNuclearGoneFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileNuclearGoneFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileNuclearGoneFXPrefab != null)
		{
			GameObject TBFX = Instantiate(tileNuclearGoneFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = TBFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileNuclearRBreakFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileNuclearRBreakFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileNuclearRBreakFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileNuclearRBreakFXPrefab != null)
		{
			GameObject TBFX = Instantiate(tileNuclearRBreakFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = TBFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileBreakFossilFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileBreakFossilFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileBreakFossilFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileBreakFossilFXPrefab != null)
		{
			GameObject FX = Instantiate(tileBreakFossilFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileFossilGoneFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileFossilGoneFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileFossilGoneFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileFossilGoneFXPrefab != null)
		{
			GameObject FX = Instantiate(tileFossilGoneFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileFalloutFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileFalloutFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileFalloutFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileFalloutFXPrefab != null)
		{
			GameObject FX = Instantiate(tileFalloutFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}


	public void TileOilMineGoneFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileOilMineGoneFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileOilMineGoneFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileOilMineGoneFXPrefab != null)
		{
			GameObject FX = Instantiate(tileOilMineGoneFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileBreakOilPumpFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileBreakOilPumpFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileBreakOilPumpFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileBreakOilPumpFXPrefab != null)
		{
			GameObject FX = Instantiate(tileBreakOilPumpFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileBreakMiningPitFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileBreakMiningPitFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileBreakMiningPitFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileBreakMiningPitFXPrefab != null)
		{
			GameObject FX = Instantiate(tileBreakMiningPitFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileMiningAppearFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileMiningAppearFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileMiningAppearFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileMiningAppearFXPrefab != null)
		{
			GameObject FX = Instantiate(tileMiningAppearFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileLitterAppearFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileLitterAppearFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileLitterAppearFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileLitterAppearFXPrefab != null)
		{
			GameObject FX = Instantiate(tileLitterAppearFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileBarrelAppearFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileBarrelAppearFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileBarrelAppearFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileBarrelAppearFXPrefab != null)
		{
			GameObject FX = Instantiate(tileBarrelAppearFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}

	public void TileCityGoneFXAt(int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(TileCityGoneFXAtRoutine(x, y, z, waitTime));
	}

	IEnumerator TileCityGoneFXAtRoutine(int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		if (tileCityGoneFXPrefab != null)
		{
			GameObject FX = Instantiate(tileCityGoneFXPrefab, new Vector3(x, y, -7), Quaternion.identity) as GameObject;

			ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}


	// play the Break Tile effect (either single or double effect depending on the Tile)
	public void BreakTileFXAt(int breakableValue, int x, int y, int z = 0, float waitTime = 0f)
	{
		StartCoroutine(BreakTileFXAtRoutine(breakableValue, x, y, z, waitTime));
	}

	IEnumerator BreakTileFXAtRoutine(int breakableValue, int x, int y, int z = 0, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		GameObject breakFX = null;
		ParticlePlayer particlePlayer = null;

		if (breakableValue > 1)
		{
			if (doubleBreakFXPrefab != null)
			{
				breakFX = Instantiate(doubleBreakFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}
		else
		{
			if (breakFXPrefab != null)
			{
				breakFX = Instantiate(breakFXPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject;
			}
		}

		if (breakFX != null)
		{
			particlePlayer = breakFX.GetComponent<ParticlePlayer>();

			if (particlePlayer != null)
			{
				particlePlayer.Play();
			}
		}
	}


	public void PointsFXAt(int x, int y, int points, MatchValue match = MatchValue.None, int z = -4, float waitTime = 0f)
	{
		StartCoroutine(PointsFXAtRoutine(x, y, points, match, z, waitTime));
	}

	IEnumerator PointsFXAtRoutine(int x, int y, int points, MatchValue match = MatchValue.None, int z = -4, float waitTime = 0f)
	{
		yield return new WaitForSeconds(waitTime);

		
			GameObject FX = null;

			if (points == 20)
			{
				if (FXprefab20 != null)
				{
					FX = Instantiate(FXprefab20, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 40)
			{
				if (FXprefab40 != null)
				{
					FX = Instantiate(FXprefab40, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 60)
			{
				if (FXprefab60 != null)
				{
					FX = Instantiate(FXprefab60, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 80)
			{
				if (FXprefab80 != null)
				{
					FX = Instantiate(FXprefab80, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 30)
			{
				if (FXprefab30 != null)
				{
					FX = Instantiate(FXprefab30, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 90)
			{
				if (FXprefab90 != null)
				{
					FX = Instantiate(FXprefab90, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 120)
			{
				if (FXprefab120 != null)
				{
					FX = Instantiate(FXprefab120, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 160)
			{
				if (FXprefab160 != null)
				{
					FX = Instantiate(FXprefab160, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 50)
			{
				if (FXprefab50 != null)
				{
					FX = Instantiate(FXprefab50, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 100)
			{
				if (FXprefab100 != null)
				{
					FX = Instantiate(FXprefab100, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 150)
			{
				if (FXprefab150 != null)
				{
					FX = Instantiate(FXprefab150, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 200)
			{
				if (FXprefab200 != null)
				{
					FX = Instantiate(FXprefab200, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 500)
			{
				if (FXprefab500 != null)
				{
					FX = Instantiate(FXprefab500, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 1000)
			{
				if (FXprefab1000 != null)
				{
					FX = Instantiate(FXprefab1000, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 1500)
			{
				if (FXprefab1500 != null)
				{
					FX = Instantiate(FXprefab1500, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 2000)
			{
				if (FXprefab2000 != null)
				{
					FX = Instantiate(FXprefab2000, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 400)
			{
				if (FXprefab400 != null)
				{
					FX = Instantiate(FXprefab400, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 600)
			{
				if (FXprefab600 != null)
				{
					FX = Instantiate(FXprefab600, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 800)
			{
				if (FXprefab800 != null)
				{
					FX = Instantiate(FXprefab800, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 240)
			{
				if (FXprefab240 != null)
				{
					FX = Instantiate(FXprefab240, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 360)
			{
				if (FXprefab360 != null)
				{
					FX = Instantiate(FXprefab360, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 480)
			{
				if (FXprefab480 != null)
				{
					FX = Instantiate(FXprefab480, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 180)
			{
				if (FXprefab180 != null)
				{
					FX = Instantiate(FXprefab180, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 70)
			{
				if (FXprefab70 != null)
				{
					FX = Instantiate(FXprefab70, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 140)
			{
				if (FXprefab140 != null)
				{
					FX = Instantiate(FXprefab140, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 210)
			{
				if (FXprefab210 != null)
				{
					FX = Instantiate(FXprefab210, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}
			else if (points == 280)
			{
				if (FXprefab280 != null)
				{
					FX = Instantiate(FXprefab280, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			else if (points == 300)
			{
				if (FXprefab300 != null)
				{
					FX = Instantiate(FXprefab300, new Vector3(x, y, z), Quaternion.identity) as GameObject;
				}
			}

			if(FX != null)
			{
				ParticlePlayer particlePlayer = FX.GetComponent<ParticlePlayer>();

				if (particlePlayer != null)
				{
					particlePlayer.PlayPoints(match);
				}
			}
						
		
	}

}
