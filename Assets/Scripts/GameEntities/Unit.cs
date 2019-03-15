using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
    public abstract class Unit : MonoBehaviour {

        public Team Team;
        public Power SelfPower;
        public int Lives;
        public float CurrentSpeed;
        public bool IsAlive = true;

        public List<Unit> NearEnemies = new List<Unit>();

        private SpriteRenderer _unitColor;
        private SpriteRenderer _icon;

        private void Awake() {
            var sr = GetComponentsInChildren<SpriteRenderer>();
            _unitColor = sr[0];
            _icon = sr[1];
        }

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

#region Monobeheviour
        void Update() {
            if (!IsAlive) {
                Destroy(gameObject);
                return;
            }

            UnitMoveBehaviour();
		}

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
        protected void UnitMoveBehaviour() {
            if (IsEnemyFound()) {
                EnemyFoundBehaviour();
                return;
            }
               
            AttackEnemyBase();
        }

        protected virtual bool IsEnemyFound() {
            return NearEnemies.Any();
        }

        protected virtual void EnemyFoundBehaviour() { }

		protected virtual void AttackEnemyBase() {
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
