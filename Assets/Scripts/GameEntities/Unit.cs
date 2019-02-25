using UnityEngine;
using System.Collections.Generic;

namespace GameEntitties {
	public class Unit : MonoBehaviour {

		public float DefaultSpeed;
		public int Lives;
		public TeamColor TeamColor;
		public Power UnitPower;
		public List<Unit> NearEnemies = new List<Unit>();
		public IUnitBehaviour UnitBehaviour { get; set; }
		public float CurrentSpeed { get; set; }
		public Team Team { get; set; }

		void FixedUpdate() {
			UnitBehaviour.Behave();
		}

		private void OnCollisionEnter2D(Collision2D other) {
			var otherUnit = other.gameObject.GetComponent<Unit>();
			if (otherUnit == null || TeamColor == otherUnit.TeamColor)
				return;

			if (IsPowerfullThen(otherUnit.UnitPower)) {
				Debug.Log(UnitPower + " is damage unit " + otherUnit.UnitPower);
				otherUnit.TakeDamage();
			}

			if (UnitPower == Power.mine || UnitPower == Power.tower)
				TakeDamage();
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit == null || TeamColor == unit.TeamColor)
				return;

			Debug.Log(UnitPower + ": enemy unit is entered trigger area: " + unit.UnitPower);
			if (!NearEnemies.Contains(unit))
				NearEnemies.Add(unit);
		}

		private void OnTriggerExit2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<Unit>();
			
			if (NearEnemies.Contains(unit))
				NearEnemies.Remove(unit);
		}

		private bool IsPowerfullThen(Power otherPower) {
			return UnitPower == Power.mine && otherPower != Power.mine
				|| UnitPower == Power.titan && otherPower != Power.titan
				|| UnitPower == Power.rock && otherPower == Power.scissors
				|| UnitPower == Power.scissors && otherPower == Power.paper
				|| UnitPower == Power.paper && otherPower == Power.rock
				|| UnitPower == Power.tower;
		}

		public void TakeDamage() {
			Lives--;
			Debug.Log(UnitPower + " is damaged: Lives:" + Lives);
			if (Lives <= 0) {
				Debug.Log(UnitPower + " no lives left");
				Destroy(gameObject);
			}
		}
	}
}
