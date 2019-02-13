using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum TeamColor { red, blue }
public enum Power { none, paper, scissors, rock, titan, tower, mine }

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
	[SerializeField] private Unit _towerPrefab;
	[SerializeField] private Unit _minePrefab;
	
	[SerializeField] private Text _redTeamEnergyText;
	[SerializeField] private Text _blueTeamEnergyText;
	[SerializeField] private Text _redBaseLives;
	[SerializeField] private Text _blueBaseLives;
	[SerializeField] private Text _gameOver;

	[SerializeField] private Text _titanCost;
	[SerializeField] private Image _titanBuyImage;
	[SerializeField] private Text _rockCost;
	[SerializeField] private Image _rockBuyImage;
	[SerializeField] private Text _scissorsCost;
	[SerializeField] private Image _scissorsBuyImage;
	[SerializeField] private Text _paperCost;
	[SerializeField] private Image _paperBuyImage;
	[SerializeField] private Text _towerCost;
	[SerializeField] private Image _towerBuyImage;
	[SerializeField] private Text _mineCost;
	[SerializeField] private Image _mineBuyImage;
	
	#endregion
	
	public Dictionary<Power, int> Costs = new Dictionary<Power, int>() {
		{ Power.rock , 3 },
		{ Power.paper, 4 },
		{ Power.scissors, 6 },
		{ Power.titan, 8 },
		{ Power.tower, 9 },
		{ Power.mine, 9 }
	};	
	
	private bool _gameRunning;
	private Team _redTeam;
	private Team _blueTeam;
	
	void Awake () {
		_instance = this;
		Init();
		SetUICosts();
		_gameRunning = true;
	}

	private void Init() {
		_redTeam = new Team(_blueBase.transform.position, _redArea.position, _redArea.localScale / 2f);
		_redTeam.ActionBehaviour = new NpcUpdate(_redTeam);
		_blueTeam = new Team(_redBase.transform.position, _blueArea.position, _blueArea.localScale / 2f);
		
		_gameOver.text = String.Empty;
	}

	private void SetUICosts() {
		_titanCost.text = Costs[Power.titan].ToString();
		_rockCost.text = Costs[Power.rock].ToString();
		_scissorsCost.text = Costs[Power.scissors].ToString();
		_paperCost.text = Costs[Power.paper].ToString();
		_towerCost.text = Costs[Power.tower].ToString();
		_mineCost.text = Costs[Power.mine].ToString();
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
			case Power.tower:
				prefab = _towerPrefab;
				break;
			case Power.mine:
				prefab = _minePrefab;
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

		UpdateStoreButtons();
	}

	private void UpdateStoreButtons() {
		bool canBuy;
		canBuy = IsEnergyAvailable(Power.titan, _blueTeam.Energy);
		_titanBuyImage.color = canBuy
			? SetAlpha(_titanBuyImage.color, 1f)
			: SetAlpha(_titanBuyImage.color, 0.2f);

		canBuy = IsEnergyAvailable(Power.paper, _blueTeam.Energy);
		_paperBuyImage.color = canBuy
			? SetAlpha(_paperBuyImage.color, 1f)
			: SetAlpha(_paperBuyImage.color, 0.2f);

		canBuy = IsEnergyAvailable(Power.scissors, _blueTeam.Energy);
		_scissorsBuyImage.color = canBuy
			? SetAlpha(_scissorsBuyImage.color, 1f)
			: SetAlpha(_scissorsBuyImage.color, 0.2f);

		canBuy = IsEnergyAvailable(Power.rock, _blueTeam.Energy);
		_rockBuyImage.color = canBuy
			? SetAlpha(_rockBuyImage.color, 1f)
			: SetAlpha(_rockBuyImage.color, 0.2f);
		
		canBuy = IsEnergyAvailable(Power.tower, _blueTeam.Energy);
		_towerBuyImage.color = canBuy
			? SetAlpha(_towerBuyImage.color, 1f)
			: SetAlpha(_towerBuyImage.color, 0.2f);
		
		canBuy = IsEnergyAvailable(Power.mine, _blueTeam.Energy);
		_mineBuyImage.color = canBuy
			? SetAlpha(_mineBuyImage.color, 1f)
			: SetAlpha(_mineBuyImage.color, 0.2f);
	}

	private Color SetAlpha(Color color, float alpha) {
		Color newColor = color;
		newColor.a = alpha;
		return newColor;
	}
}
