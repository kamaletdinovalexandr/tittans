using System.Collections.Generic;
using UnityEngine;
using UI;
using GameEntitties;
using NPCInput;
using Factory;
using FlyWeight;

namespace GameCore {
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

		#endregion

		public Dictionary<Power, UnitFlyweight> Units = new Dictionary<Power, UnitFlyweight>();

		private bool _gameRunning;
		private Team _redTeam;
		private Team _blueTeam;

		void Awake() {
			_instance = this;
			Init();

			_gameRunning = true;
		}

		private void Init() {
			_redTeam = new Team(TeamColor.red, _blueBase.transform.position, _redArea.position, _redArea.localScale / 2f);
			_redTeam.ActionBehaviour = new NpcUpdate(_redTeam);
			_blueTeam = new Team(TeamColor.blue, _redBase.transform.position, _blueArea.position, _blueArea.localScale / 2f);
		}

		void FixedUpdate() {
			if (!_gameRunning)
				return;

			_redTeam.UpdateEnergy();
			_redTeam.ActionBehaviour.MakeAction();
			_blueTeam.UpdateEnergy();

			UIController.Instance.UpdateUI(_redBase.BaseLives, _redTeam.Energy, _blueBase.BaseLives, _blueTeam.Energy);

			if (_redBase.BaseLives <= 0) {
				GameOver(_blueBase.CurrentTeam);
			}
			else {
				if (_blueBase.BaseLives <= 0)
					GameOver(_redBase.CurrentTeam);
			}
		}

		private void GameOver(TeamColor team) {
			UIController.Instance.GameOver(team);
			_gameRunning = false;
		}

		public void PlayerSpawn(Power power, Vector2 position) {
			UnitFactory.Instance.SpawnUnit(power, _blueTeam, position);
			//_blueTeam.Energy -= Costs[power];

		}
	}
}
