using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum Team { red, blue }
public enum Power { none, paper, scissors, rock }

public class GameController : MonoBehaviour {

	[SerializeField] private GameObject _redBase;
	[SerializeField] private GameObject _blueBase;
	[SerializeField] private Transform _redArea;
	[SerializeField] private Transform _blueArea;
	[SerializeField] private Unit UnitRockPrefab;
	[SerializeField] private Unit UnitScissorsPrefab;
	[SerializeField] private Unit UnitPaperPrefab;
	[SerializeField] private Text _redTeamEnergyText;
	[SerializeField] private Text _blueTeamEnergyText;
	
	private int maxEnergy = 10;
	private float _redTeamEnergy;
	private float _blueTeamEnergy;
	private Power _currentRedUnit;
	
	
	private Vector2 _redHalfScale;
	private Vector2 _blueHalfScale;
	private float _blueEnergy;
	private float _redEnergy;

	void Awake () {
		
		_redHalfScale = _redArea.localScale / 2f;
		_blueHalfScale = _blueArea.localScale / 2f;
		_redTeamEnergy = maxEnergy;
		_blueTeamEnergy = maxEnergy;

	}

	void FixedUpdate () {
		_redEnergy = GetEnergy(_redEnergy);
		_redTeamEnergyText.text = Mathf.RoundToInt(_redEnergy).ToString();
		
		if (_currentRedUnit == Power.none)
			_currentRedUnit = GetRandowPower();

		if (_currentRedUnit != Power.none && isSpawnAvailable(_currentRedUnit, _redEnergy)) {
			_redEnergy -= (int) _currentRedUnit;
			SpawnRandomRed();
			_currentRedUnit = Power.none;

		}
	}

	private float GetEnergy(float energy) {
		float newEnergy = energy + Time.deltaTime;
		return newEnergy > maxEnergy ? maxEnergy : newEnergy;
	}

	private bool isSpawnAvailable(Power power, float energy) {
		if (energy - (int) power >= 0)
			return true;
		
		return false;
	}
	
	private void SpawnRandomRed() {
		Unit redUnit = UnitRockPrefab;
		switch (GetRandowPower()) {
			case Power.scissors:
				redUnit = UnitScissorsPrefab;
				break;
			case Power.paper:
				redUnit = UnitScissorsPrefab;
				break;
		}

		SpawnUnit(redUnit, Team.red,
			GetRandomPosition(_redArea.position, _redHalfScale), _blueBase.transform.position);
	}

	private void SpawnUnit(Unit unitPrefab, Team team, Vector2 spawnPosition, Vector2 targetPosition) {
		var unit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
		
		unit.UnitTeam = team;
		unit.TargetPosition = targetPosition;
	}

	private Power GetRandowPower() {
		return (Power)Random.Range(1, System.Enum.GetValues(typeof(Power)).Length);
	}

	private Vector2 GetRandomPosition(Vector2 areaPosition, Vector2 halfScale) {
		var randomX = Random.Range(areaPosition.x - halfScale.x, areaPosition.x + halfScale.x);
		var randomY = Random.Range(areaPosition.y - halfScale.y, areaPosition.y + halfScale.y);
		return new Vector2(randomX, randomY);
	}
}
