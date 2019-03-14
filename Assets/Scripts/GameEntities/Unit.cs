using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
	public class Unit : MonoBehaviour {

        public Team Team;
        public Power SelfPower;
        public int Lives;
        public float CurrentSpeed;
        public bool IsAlive = true;

		public List<Unit> NearEnemies = new List<Unit>();

        [SerializeField] private SpriteRenderer _unitColor;
		[SerializeField] private SpriteRenderer _icon;

#region Init		
        public void Init(Team team, UnitSetup setup) {
            SelfPower = setup.Power;
            Team = team;
            CurrentSpeed = setup.DefaultSpeed;
            Lives = setup.StartLives;
            InitGameObject(setup);
        }

        private void InitGameObject(UnitSetup setup) {
            gameObject.name = setup.Power.ToString();
            _icon.sprite = setup.Sprite;
            _unitColor.color = GetCurrentTeamColor();
            transform.localScale = new Vector2(setup.Scale, setup.Scale);
            SetTriggerRadius(setup.TriggerColliderRadius);
        }

        private Color GetCurrentTeamColor() {
            return Team.TeamColor == TeamColor.blue ? Color.blue : Color.red;
        }

        public void SetTriggerRadius(float radius) {
			var triggerColliders = GetComponents<CircleCollider2D>();
			if (triggerColliders == null)
				return;

			foreach (var coll in triggerColliders) {
				if (coll.isTrigger) {
					coll.radius = radius;
					return;
				}
			}
		}
#endregion

        void Update() {
            if (!IsAlive) {
                Destroy(gameObject);
                return;
            }

			switch (SelfPower) {
				case Power.mine:
				DetectEnemyAndAttack();
				break;

				case Power.paper:
				MoveBackwardIfScissors();
				break;

				case Power.rock:
				AttackScissors();
				break;

				case Power.tower:
				SlowDownNearestEnemy();
				break;

				default:
				AttackEnemyBase();
				break;
			}
		}

#region Collision
        private void OnCollisionEnter2D(Collision2D other) {
            var otherUnit = other.gameObject.GetComponent<Unit>();
            if (IsNullOrFriendly(otherUnit))
                return;

            TryGiveDamage(otherUnit);
        }

        private void OnTriggerEnter2D(Collider2D other) {
			var otherUnit = other.gameObject.GetComponent<Unit>();
            if (IsNullOrFriendly(otherUnit))
                return;

            Debug.Log(SelfPower + ": enemy unit is entered trigger area: " + otherUnit.SelfPower);
            if (!NearEnemies.Contains(otherUnit))
                NearEnemies.Add(otherUnit);
		}

		private void OnTriggerExit2D(Collider2D other) {
			var unit = other.gameObject.GetComponent<Unit>();
			RemoveNearEnemy(unit);
		}
#endregion

		

#region UnitBehaviours
		private void DetectEnemyAndAttack() {
			var enemies = NearEnemies.Where(u => u != null && (u.SelfPower != Power.tower || u.SelfPower != Power.mine));
			if (!enemies.Any()) {
                CurrentSpeed = UnitFactory.Instance.GetSetupForUnit(SelfPower).DefaultSpeed;
				return;
			}

			var enemy = enemies.First();
            CurrentSpeed = 2 * enemy.CurrentSpeed;
            transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, CurrentSpeed * Time.deltaTime);
		}

		private void MoveBackwardIfScissors() {
			var scissors = NearEnemies.Where(u => u != null && (u.SelfPower == Power.scissors));
			var target = Team.EnemyBasePosition;
			if (scissors.Any())
				target = target * -1;

            transform.position = Vector2.MoveTowards(transform.position, target, CurrentSpeed * Time.deltaTime);
		}

		private void AttackScissors() {
			var scissors = NearEnemies.Where(u => u != null && (u.SelfPower == Power.scissors));
			var target = Team.EnemyBasePosition;
			if (scissors.Any())
				target = scissors.First().transform.position;

            transform.position = Vector2.MoveTowards(transform.position, target, CurrentSpeed * Time.deltaTime);
		}

        private void SlowDownNearestEnemy() {
			var enemies = NearEnemies.Where(u => u != null && (u.SelfPower != Power.tower || u.SelfPower != Power.mine));
			if (!enemies.Any()) {
				return;
			}

            enemies.ToList().ForEach(u => u.CurrentSpeed = 0.5f);
		}

		private void AttackEnemyBase() {
            transform.position = Vector2.MoveTowards(transform.position, Team.EnemyBasePosition, CurrentSpeed * Time.deltaTime);
		}
#endregion

		private bool CanGiveDamage(Power otherPower) {
			return SelfPower == Power.mine && otherPower != Power.mine
                || SelfPower == Power.titan && otherPower != Power.titan
                || SelfPower == Power.rock && otherPower == Power.scissors
                || SelfPower == Power.scissors && otherPower == Power.paper
                || SelfPower == Power.paper && otherPower == Power.rock
                || SelfPower == Power.tower;
		}

		public void TakeDamage() {
			Lives--;
            Debug.Log(SelfPower + " is damaged");
			if (Lives <= 0) {
				Debug.Log(SelfPower + " no lives left");
                IsAlive = false;
			}
		}

        private void TryGiveDamage(Unit enemy) {
            if (CanGiveDamage(enemy.SelfPower)) {
                enemy.TakeDamage();
            }

            if (SelfPower == Power.mine || SelfPower == Power.tower)
                TakeDamage();
        }

        private void RemoveNearEnemy(Unit unit) {
            if (NearEnemies.Contains(unit)) {
                NearEnemies.Remove(unit);
            }
        }

        private bool IsNullOrFriendly(Unit unit) {
            return unit == null || Team == unit.Team;
        }
	}
}
