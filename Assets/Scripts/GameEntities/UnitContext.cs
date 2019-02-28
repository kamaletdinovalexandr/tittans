using UnityEngine;
using System.Collections.Generic;
using FlyWeight;

namespace GameEntitties {
	public class UnitContext : MonoBehaviour {


		public Team Team { get; set; }public int Lives;
		public List<UnitContext> NearEnemies = new List<UnitContext>();
		public IUnitBehaviour UnitBehaviour { get; set; }
        [SerializeField] private SpriteRenderer _unitColor;
		[SerializeField] private SpriteRenderer _icon;

		private UnitFlyweight _unitFlyweight;

		public UnitFlyweight UnitFlyweight {
			
			get { return _unitFlyweight; }

			set {
				_unitFlyweight = value;
				_icon.sprite = _unitFlyweight.Sprite;
				_unitColor.color = _unitFlyweight.Color;
                transform.localScale = new Vector2(_unitFlyweight.Scale, _unitFlyweight.Scale);
			}
		}

		void FixedUpdate() {
            if (UnitBehaviour != null)
                UnitBehaviour.Behave();
		}

		private void OnCollisionEnter2D(Collision2D other) {
			var otherUnit = other.gameObject.GetComponent<UnitContext>();
			if (otherUnit == null || Team == otherUnit.Team)
				return;

			if (IsPowerfullThen(otherUnit.UnitFlyweight.Power)) {
				Debug.Log(UnitFlyweight.Power + " is damage unit " + otherUnit.UnitFlyweight.Power);
				otherUnit.TakeDamage();
			}

			if (UnitFlyweight.Power == Power.mine || UnitFlyweight.Power == Power.tower)
				TakeDamage();
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<UnitContext>();
			if (unit == null || Team == unit.Team)
				return;

			Debug.Log(UnitFlyweight.Power + ": enemy unit is entered trigger area: " + unit.UnitFlyweight.Power);
			if (!NearEnemies.Contains(unit))
				NearEnemies.Add(unit);
		}

		private void OnTriggerExit2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<UnitContext>();
			if (NearEnemies.Contains(unit)) {
				NearEnemies.Remove(unit);
				Debug.Log(UnitFlyweight.Power + ": enemy unit is exited trigger area");
			}
		}

		private bool IsPowerfullThen(Power otherPower) {
			return UnitFlyweight.Power == Power.mine && otherPower != Power.mine
                || UnitFlyweight.Power == Power.titan && otherPower != Power.titan
                || UnitFlyweight.Power == Power.rock && otherPower == Power.scissors
                || UnitFlyweight.Power == Power.scissors && otherPower == Power.paper
                || UnitFlyweight.Power == Power.paper && otherPower == Power.rock
                || UnitFlyweight.Power == Power.tower;
		}

		public void TakeDamage() {
			Lives--;
			Debug.Log(UnitFlyweight.Power + " is damaged: Lives:" + Lives);
			if (Lives <= 0) {
				Debug.Log(UnitFlyweight.Power + " no lives left");
				Destroy(gameObject);
			}
		}
	}
}
