﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum Team { red, blue }
public enum Power { none, paper, scissors, rock }

public class GameController : MonoBehaviour {

	private const string LIVES = "Lives: ";
	private const string ENERGY = "Energy: ";
	
	private static GameController _instance;

	public static GameController Instance {
		get { return _instance; }
	}

	[SerializeField] private Base _redBase;
	[SerializeField] private Base _blueBase;
	[SerializeField] private Transform _redArea;
	[SerializeField] private Transform _blueArea;
	
	[SerializeField] private Unit _rockPrefab;
	[SerializeField] private Unit _scissorsPrefab;
	[SerializeField] private Unit _paperPrefab;
	
	[SerializeField] private Text _redTeamEnergyText;
	[SerializeField] private Text _blueTeamEnergyText;
	[SerializeField] private Text _redBaseLives;
	[SerializeField] private Text _blueBaseLives;
	
	private int maxEnergy = 10;
	private Power _currentRedUnit;
	
	private Vector2 _redHalfScale;
	private Vector2 _blueHalfScale;
	private float _blueEnergy;
	private float _redEnergy;
	
	void Awake () {
		_instance = this;
		_redHalfScale = _redArea.localScale / 2f;
		_blueHalfScale = _blueArea.localScale / 2f;
		_blueEnergy = maxEnergy;
		_redEnergy = maxEnergy;
	}

	void FixedUpdate () {
		_redEnergy = UpdateEnergy(_redEnergy);
		_blueEnergy = UpdateEnergy(_blueEnergy);
		
		NpcSpawn();

		UpdateUI();
	}

	private void NpcSpawn() {
		if (_currentRedUnit == Power.none)
			_currentRedUnit = GetRandowPower();

		if (_currentRedUnit != Power.none && isSpawnAvailable(_currentRedUnit, _redEnergy)) {
			_redEnergy -= (int) _currentRedUnit;
			var power = GetRandowPower();
			var position = GetRandomPosition(_redArea.position, _redHalfScale);
			SpawnUnit(_currentRedUnit, Team.red, position, _blueBase.transform.position);
			_currentRedUnit = Power.none;
		}
	}
	
	public void PlayerSpawn(Power power, Vector2 position) {
		if (!isSpawnAvailable(power, _blueEnergy))
			return;
		
		SpawnUnit(power, Team.blue, position, _redBase.transform.position);
	}

	private float UpdateEnergy(float energy) {
		float newEnergy = energy + Time.deltaTime;
		return newEnergy > maxEnergy ? maxEnergy : newEnergy;
	}

	private bool isSpawnAvailable(Power power, float energy) {
		var intEnergy = (int) power;
		if (energy - intEnergy >= 0)
			return true;
		
		return false;
	}

	private void SpawnUnit(Power power, Team team, Vector2 spawnPosition, Vector2 targetPosition) {
		Unit prefab = _rockPrefab;
		switch (power) {
			case Power.scissors:
				prefab = _scissorsPrefab;
				break;
			case Power.paper:
				prefab = _paperPrefab;
				break;
		}
		
		var unit = Instantiate(prefab, spawnPosition, Quaternion.identity);
		
		unit.UnitTeam = team;
		unit.TargetPosition = targetPosition;
	}

	private void UpdateUI() {
		_redBaseLives.text = LIVES + _redBase.BaseLives;
		_blueBaseLives.text = LIVES + _blueBase.BaseLives;
		_redTeamEnergyText.text = ENERGY + Mathf.RoundToInt(_redEnergy);
		_blueTeamEnergyText.text = ENERGY + Mathf.RoundToInt(_blueEnergy);
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
