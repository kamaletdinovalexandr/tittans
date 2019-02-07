using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Team { red, blue }
public enum Power { rock, scissors, paper }

public class GameController : MonoBehaviour {

	[SerializeField] private GameObject _redBase;
	[SerializeField] private GameObject _blueBase;
	[SerializeField] private Transform _redArea;
	[SerializeField] private Transform _blueArea;
	[SerializeField] private GameObject _unitPrefab;
	[SerializeField] private float SpawnDeltaTime;
	private float _spawnCooldownTimer;
	private Vector2 _redHalfScale;
	private Vector2 _blueHalfScale;

	void Awake () {
		_spawnCooldownTimer = 0f;
		_redHalfScale = _redArea.localScale / 2f;
		_blueHalfScale = _blueArea.localScale / 2f;

	}

	void Update () {
		_spawnCooldownTimer += Time.deltaTime;

		if (_spawnCooldownTimer > SpawnDeltaTime) {
			SpawnRed();
			SpawnBlue();

			_spawnCooldownTimer = 0f;
		}
	}

	private void SpawnRed() {
		var unit = Instantiate(_unitPrefab, GetRandomPosition(_redArea.transform.position, _redHalfScale), 
		                       Quaternion.identity).GetComponent<Unit>();
		unit.Speed = 0.5f;
		unit.UnitTeam = Team.red;
		unit.UnitPower = GetRandowPower();
		unit.TargetPosition = _blueBase.transform.position;
		SetColorByPower(unit);
	}

	private void SpawnBlue() {
		var unit = Instantiate(_unitPrefab, GetRandomPosition(_blueArea.transform.position, _blueHalfScale),
							   Quaternion.identity).GetComponent<Unit>();
		unit.Speed = 0.5f;
		unit.UnitTeam = Team.blue;
		unit.UnitPower = GetRandowPower();
		unit.TargetPosition = _redBase.transform.position;
		SetColorByPower(unit);
	}

	private Power GetRandowPower() {
		return (Power)Random.Range(0, System.Enum.GetValues(typeof(Power)).Length);
	}

	private Vector2 GetRandomPosition(Vector2 areaPosition, Vector2 halfScale) {
		var randomX = Random.Range(areaPosition.x - halfScale.x, areaPosition.x + halfScale.x);
		var randomY = Random.Range(areaPosition.y - halfScale.y, areaPosition.y + halfScale.y);
		return new Vector2(randomX, randomY);
	}

	private void SetColorByPower(Unit unit) {
		Color color = Color.white;
		switch(unit.UnitPower) {
			case Power.rock:
				color = Color.gray;
				break;
			case Power.paper:
				break;
			case Power.scissors:
				color = Color.blue;
				break;
		}

		unit.GetComponent<SpriteRenderer>().color = color;
	}
}
