using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum TeamColor { red, blue }
public enum Power { none, paper, scissors, rock, titan }

public class GameController : MonoBehaviour {
	private static GameController _instance;

	public static GameController Instance {
		get { return _instance; }
	}

	#region EditorSetups

	[SerializeField] private Base _redBase;
	[SerializeField] private Base _blueBase;
	[SerializeField] private Transform _redArea;
	[SerializeField] private Transform _blueArea;
	
	[SerializeField] private Unit _rockPrefab;
	[SerializeField] private Unit _scissorsPrefab;
	[SerializeField] private Unit _paperPrefab;
	[SerializeField] private Unit _titanPrefab;
	
	[SerializeField] private Text _redTeamEnergyText;
	[SerializeField] private Text _blueTeamEnergyText;
	[SerializeField] private Text _redBaseLives;
	[SerializeField] private Text _blueBaseLives;
	[SerializeField] private Text _gameOver;
	
	#endregion
	
	public Dictionary<Power, int> Costs = new Dictionary<Power, int>() {
		{ Power.rock , 3 },
		{ Power.paper, 4 },
		{ Power.scissors, 6 },
		{ Power.titan, 8 }
	};	
	
	private bool _gameRunning;
	private Team _redTeam;
	private Team _blueTeam;
	
	void Awake () {
		_instance = this;
		_redTeam = new Team(_blueBase.transform.position, _redArea.position, _redArea.localScale / 2f);
		var npcAction = new NpcUpdate(_redTeam);
		_redTeam.ActionBehaviour = npcAction;
		
		_blueTeam = new Team(_redBase.transform.position, _blueArea.position, _blueArea.localScale / 2f);
		_gameOver.text = String.Empty;
		_gameRunning = true;
	}

	void FixedUpdate () {
		if (!_gameRunning)
			return;
		
		_redTeam.UpdateEnergy();
		_redTeam.ActionBehaviour.MakeAction();
		_blueTeam.UpdateEnergy();
		

		UpdateUI();

		if (_redBase.BaseLives <= 0) {
			GameOver(_blueBase.CurrentTeam);
		}
		else {
			if (_blueBase.BaseLives <= 0)
				GameOver(_redBase.CurrentTeam);
		}
	}

	private void GameOver(TeamColor team) {
		_gameOver.text = team + Globals.WINS;
		_gameRunning = false;
	}

	
	
	public void PlayerSpawn(Power power, Vector2 position) {
		SpawnUnit(power, TeamColor.blue, position, _redBase.transform.position);
		_blueTeam.Energy -= Costs[power];
		
	}



	public bool IsEnergyAvailable(Power power, float energy) {
		return Costs.ContainsKey(power) && energy - Costs[power] >= 0;
	}

	

	public bool IsBlueSpawnAvailable(Power power, Vector3 position) {
		return IsEnergyAvailable(power, _blueTeam.Energy) && _blueTeam.IsInsideArea(position);
	}

	public void SpawnUnit(Power power, TeamColor teamColor, Vector2 spawnPosition, Vector2 targetPosition) {
		Unit prefab = _rockPrefab;
		switch (power) {
			case Power.scissors:
				prefab = _scissorsPrefab;
				break;
			case Power.paper:
				prefab = _paperPrefab;
				break;
			case Power.titan:
				prefab = _titanPrefab;
				break;
		}
		
		var unit = Instantiate(prefab, spawnPosition, Quaternion.identity);
		unit.UnitPower = power;
		unit.Team = teamColor;
		unit.TargetPosition = targetPosition;
	}

	private void UpdateUI() {
		_redBaseLives.text = Globals.LIVES + _redBase.BaseLives;
		_blueBaseLives.text = Globals.LIVES + _blueBase.BaseLives;
		_redTeamEnergyText.text = Globals.ENERGY + Mathf.RoundToInt(_redTeam.Energy);
		_blueTeamEnergyText.text = Globals.ENERGY + Mathf.RoundToInt(_blueTeam.Energy);
	}
	
	
}
