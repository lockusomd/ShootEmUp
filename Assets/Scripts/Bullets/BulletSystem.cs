using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class BulletSystem : MonoBehaviour
    {
        [SerializeField] private int _initialCount = 50;
        
        [SerializeField] private Transform _container;
        [SerializeField] private Bullet _prefab;
        [SerializeField] private Transform _worldTransform;
        [SerializeField] private LevelBounds _levelBounds;

        private readonly Queue<Bullet> m_bulletPool = new();
        private readonly HashSet<Bullet> m_activeBullets = new();
        private readonly List<Bullet> m_cache = new();
        
        private void Awake()
        {
            for (var i = 0; i < this._initialCount; i++)
            {
                var bullet = Instantiate(this._prefab, this._container);
                this.m_bulletPool.Enqueue(bullet);
            }
        }
        
        private void FixedUpdate()
        {
            this.m_cache.Clear();
            this.m_cache.AddRange(this.m_activeBullets);

            for (int i = 0, count = this.m_cache.Count; i < count; i++)
            {
                var bullet = this.m_cache[i];
                if (!this._levelBounds.InBounds(bullet.transform.position))
                {
                    this.RemoveBullet(bullet);
                }
            }
        }

        public void FlyBulletByArgs(Args args)
        {
            if (this.m_bulletPool.TryDequeue(out var bullet))
            {
                bullet.transform.SetParent(this._worldTransform);
            }
            else
            {
                bullet = Instantiate(this._prefab, this._worldTransform);
            }

            bullet.SetPosition(args.Position);
            bullet.SetColor(args.Color);
            bullet.SetPhysicsLayer(args.PhysicsLayer);
            bullet.Damage = args.Damage;
            bullet.IsPlayer = args.IsPlayer;
            bullet.SetVelocity(args.Velocity);
            
            if (this.m_activeBullets.Add(bullet))
            {
                bullet.OnCollisionEntered += this.OnBulletCollision;
            }
        }
        
        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            BulletUtils.DealDamage(bullet, collision.gameObject);
            this.RemoveBullet(bullet);
        }

        private void RemoveBullet(Bullet bullet)
        {
            if (this.m_activeBullets.Remove(bullet))
            {
                bullet.OnCollisionEntered -= this.OnBulletCollision;
                bullet.transform.SetParent(this._container);
                this.m_bulletPool.Enqueue(bullet);
            }
        }
        
        public struct Args
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public Color Color;
            public int PhysicsLayer;
            public int Damage;
            public bool IsPlayer;
        }
    }
}