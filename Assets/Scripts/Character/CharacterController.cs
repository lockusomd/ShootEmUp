using UnityEngine;

namespace ShootEmUp
{
    public sealed class CharacterController : MonoBehaviour
    {
        [SerializeField] private GameObject _character; 
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private BulletSystem _bulletSystem;
        [SerializeField] private BulletConfig _bulletConfig;
        
        public bool _fireRequired;

        private void OnEnable()
        {
            this._character.GetComponent<HitPointsComponent>().HpEmpty += this.OnCharacterDeath;
        }

        private void OnDisable()
        {
            this._character.GetComponent<HitPointsComponent>().HpEmpty -= this.OnCharacterDeath;
        }

        private void OnCharacterDeath(GameObject _) => this._gameManager.FinishGame();

        private void FixedUpdate()
        {
            if (this._fireRequired)
            {
                this.OnFlyBullet();
                this._fireRequired = false;
            }
        }

        private void OnFlyBullet()
        {
            var weapon = this._character.GetComponent<WeaponComponent>();
            _bulletSystem.FlyBulletByArgs(new BulletSystem.Args
            {
                IsPlayer = true,
                PhysicsLayer = (int) this._bulletConfig.physicsLayer,
                Color = this._bulletConfig.color,
                Damage = this._bulletConfig.damage,
                Position = weapon.Position,
                Velocity = weapon.Rotation * Vector3.up * this._bulletConfig.speed
            });
        }
    }
}