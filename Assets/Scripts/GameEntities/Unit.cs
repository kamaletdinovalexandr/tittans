using UnityEngine;
using Strategy;

namespace GameEntitties {
	public class Unit : MonoBehaviour {

		public float Speed;
		public float AlternateSpeed { get; set; }
		public int Lives;
		public TeamColor Team;
		public Power UnitPower;
		public Vector2 TargetPosition;
		public Transform NearEnemyTransform;
		public bool RunAway;
		public ICollideBehaviour CollideBehaviour;

		private void Awake() {

		}

		void FixedUpdate() {
			if (Speed < 0.01f)
				return;

			Vector2 targetPosition = TargetPosition;

			if (NearEnemyTransform != null)
				targetPosition = NearEnemyTransform.position;

			if (RunAway && NearEnemyTransform != null) {
				targetPosition = transform.position - NearEnemyTransform.position;
			}

			MoveTo(targetPosition);
		}

		private void MoveTo(Vector2 position) {
			var speed = AlternateSpeed > 0 ? AlternateSpeed : Speed;
			transform.position = Vector2.MoveTowards(transform.position, position, speed * Time.deltaTime);
		}

		private void OnCollisionEnter2D(Collision2D other) {
			var otherUnit = other.gameObject.GetComponent<Unit>();
			if (otherUnit == null || Team == otherUnit.Team)
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
			if (unit == null || Team == unit.Team)
				return;

			CollideBehaviour.DoCollide(unit);
		}

		private void OnTriggerExit2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<Unit>();
			if (unit != null && Team == unit.Team) {
				return;
			}

			NearEnemyTransform = null;
			RunAway = false;
			AlternateSpeed = 0f;

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
