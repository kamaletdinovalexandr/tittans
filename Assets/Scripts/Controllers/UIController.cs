using UnityEngine;
using UnityEngine.UI;
using System;
using GameCore;

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
			_gameOver.text = String.Empty;
		}

		private void InitCosts() {
			_titanCost.text = GameController.Instance.GetCost(Power.titan).ToString();
			_rockCost.text = GameController.Instance.GetCost(Power.rock).ToString();
			_scissorsCost.text = GameController.Instance.GetCost(Power.scissors).ToString();
			_paperCost.text = GameController.Instance.GetCost(Power.paper).ToString();
			_towerCost.text = GameController.Instance.GetCost(Power.tower).ToString();
			_mineCost.text = GameController.Instance.GetCost(Power.mine).ToString();
		}

		public void GameOver(TeamColor team) {
			_gameOver.text = team + Globals.WINS;
		}

		public void UpdateUI(int redLives, float redEnergy, int blueLives, float blueEnergy) {
			_redBaseLives.text = Globals.LIVES + redLives;
			_blueBaseLives.text = Globals.LIVES + blueLives;
			_redTeamEnergyText.text = Globals.ENERGY + Mathf.RoundToInt(redEnergy);
			_blueTeamEnergyText.text = Globals.ENERGY + Mathf.RoundToInt(blueEnergy);

			UpdateStoreButtons(redLives, redEnergy, blueLives, blueEnergy);
		}

		private void UpdateStoreButtons(int redLives, float redEnergy, int blueLives, float blueEnergy) {
			bool canBuy;
			canBuy = GameController.Instance.IsEnergyAvailable(Power.titan,blueEnergy);
			_titanBuyImage.color = canBuy
				? SetAlpha(_titanBuyImage.color, 1f)
				: SetAlpha(_titanBuyImage.color, 0.2f);

			canBuy = GameController.Instance.IsEnergyAvailable(Power.paper, blueEnergy);
			_paperBuyImage.color = canBuy
				? SetAlpha(_paperBuyImage.color, 1f)
				: SetAlpha(_paperBuyImage.color, 0.2f);

			canBuy = GameController.Instance.IsEnergyAvailable(Power.scissors, blueEnergy);
			_scissorsBuyImage.color = canBuy
				? SetAlpha(_scissorsBuyImage.color, 1f)
				: SetAlpha(_scissorsBuyImage.color, 0.2f);

			canBuy = GameController.Instance.IsEnergyAvailable(Power.rock, blueEnergy);
			_rockBuyImage.color = canBuy
				? SetAlpha(_rockBuyImage.color, 1f)
				: SetAlpha(_rockBuyImage.color, 0.2f);

			canBuy = GameController.Instance.IsEnergyAvailable(Power.tower, blueEnergy);
			_towerBuyImage.color = canBuy
				? SetAlpha(_towerBuyImage.color, 1f)
				: SetAlpha(_towerBuyImage.color, 0.2f);

			canBuy = GameController.Instance.IsEnergyAvailable(Power.mine, blueEnergy);
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
}
