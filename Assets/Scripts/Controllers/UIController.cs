using UnityEngine;
using UnityEngine.UI;
using System;
using GameCore;
using Factory;

namespace UI {
	public class UIController : MonoBehaviour {

		private static UIController _instance;

#region UnitySetups
		public Text _titanCost;
		public Image _titanBuyImage;
		public Text _rockCost;
		public Image _rockBuyImage;
		public Text _scissorsCost;
		public Image _scissorsBuyImage;
		public Text _paperCost;
		public Image _paperBuyImage;
		public Text _towerCost;
		public Image _towerBuyImage;
		public Text _mineCost;
		public Image _mineBuyImage;

		public Text _redTeamEnergyText;
		public Text _blueTeamEnergyText;
		public Text _redBaseLives;
		public Text _blueBaseLives;
		public Text _gameOver;
#endregion

		public static UIController Instance { get { return _instance; } }

		private void Awake() {
			_instance = this;
		}

		private void Start() {
			_gameOver.text = String.Empty;
			InitCosts();
		}

		private void InitCosts() {
			_titanCost.text = UnitFactory.Instance.GetUnitCost(Power.titan).ToString();
			_rockCost.text = UnitFactory.Instance.GetUnitCost(Power.rock).ToString();
			_scissorsCost.text = UnitFactory.Instance.GetUnitCost(Power.scissors).ToString();
			_paperCost.text = UnitFactory.Instance.GetUnitCost(Power.paper).ToString();
			_towerCost.text = UnitFactory.Instance.GetUnitCost(Power.tower).ToString();
			_mineCost.text = UnitFactory.Instance.GetUnitCost(Power.mine).ToString();
		}

		public void TeamWin(TeamColor team) {
			_gameOver.text = team + Globals.WINS;
		}

		public void UpdateUI(int redLives, float redEnergy, int blueLives, float blueEnergy) {
			_redBaseLives.text = Globals.LIVES + redLives;
			_blueBaseLives.text = Globals.LIVES + blueLives;
			_redTeamEnergyText.text = Globals.ENERGY + Mathf.RoundToInt(redEnergy);
			_blueTeamEnergyText.text = Globals.ENERGY + Mathf.RoundToInt(blueEnergy);

			UpdateStoreButtons();
		}

		private void UpdateStoreButtons() {
            _titanBuyImage.color = GetColor(Power.titan, _titanBuyImage.color);
            _paperBuyImage.color = GetColor(Power.paper, _paperBuyImage.color);
            _scissorsBuyImage.color = GetColor(Power.scissors, _scissorsBuyImage.color);
            _rockBuyImage.color = GetColor(Power.rock, _rockBuyImage.color);
            _towerBuyImage.color = GetColor(Power.tower, _towerBuyImage.color);
            _mineBuyImage.color = GetColor(Power.mine, _mineBuyImage.color);
		}

        private Color GetColor(Power power, Color color) {
            return GameController.Instance.IsBlueEnergyAvailable(power)
                ? SetAlpha(color, 1f)
                : SetAlpha(color, 0.2f);
        }

		private Color SetAlpha(Color color, float alpha) {
			Color newColor = color;
			newColor.a = alpha;
			return newColor;
		}
	}
}
