using UnityEngine;
using UI;
using GameEntitties;
using NPCInput;
using Factory;

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

		private bool _gameRunning;
		private Team _redTeam;
		private Team _blueTeam;

		void Awake() {
			_instance = this;
			Init();
		}

		private void Init() {
			_redTeam = new Team(TeamColor.red, _blueBase.transform.position, _redArea.position, _redArea.localScale / 2f);
			_blueTeam = new Team(TeamColor.blue, _redBase.transform.position, _blueArea.position, _blueArea.localScale / 2f);
            _redTeam.ActionBehaviour = new NpcUpdate(_redTeam);
            _gameRunning = true;
		}

		void FixedUpdate() {
			if (!_gameRunning)
				return;

			_redTeam.UpdateEnergy();
			_redTeam.ActionBehaviour.MakeAction();
			_blueTeam.UpdateEnergy();

			UIController.Instance.UpdateUI(_redBase.BaseLives, _redTeam.Energy, _blueBase.BaseLives, _blueTeam.Energy);

		}

		public void GameOver(TeamColor team) {
            var winTeam = GetOppositeTeamColor(team);
			UIController.Instance.TeamWin(winTeam);
			_gameRunning = false;
		}

        private TeamColor GetOppositeTeamColor(TeamColor team) {
            return team == TeamColor.blue ? TeamColor.red : TeamColor.blue;
        }

		public void PlayerSpawn(Power power, Vector2 position) {
			UnitFactory.Instance.CreateUnit(power, _blueTeam, position);
			_blueTeam.Energy -= UnitFactory.Instance.GetUnitCost(power);

		}

		public bool IsBlueSpawnAvailable(Power power, Vector3 position) {
			return IsBlueEnergyAvailable(power) && _blueTeam.IsInsideArea(position);
		}

		public bool IsBlueEnergyAvailable(Power power) {
			return _blueTeam.IsEnergyAvailable(power);
		}
	}
}
