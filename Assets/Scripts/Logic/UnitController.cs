using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class UnitController : BaseDisposable
{
    public struct Ctx
    {
        public IResourceLoader resourceLoader;
        public Vector3 playerSpawnPoint;
        public CinemachineVirtualCamera playerCamera;
        public Canvas canvasForJoystick;
        public ProjectileFactory projectileFactory;
        public RandomPointGenerator randomPointGenerator;
        public RandomResourceGenerator randomResourceGenerator;
        public float baseBorderCoord;
        public float timeDetweenEnemySpawn;
    }

    private readonly Ctx _ctx;
    private UnitFactory _unitFactory;
    private List<UnitPm> _enemies;
    private PlayerPm _player;
    private float _timer;
    private bool _currentHomeState;
    private bool _isPaused = false;
    public bool CurrentHomeState
    {
        get => _currentHomeState;
        private set
        {
            PlayerChangeHomeState?.Invoke(value);
            _currentHomeState = value;
        }
    }

    public UnitFactory Factory => _unitFactory;
    public event Action<bool> PlayerChangeHomeState;
    public event Action PlayerDied;
    public PlayerPm Player => _player;

    public UnitController(Ctx ctx)
    {
        _ctx = ctx;
        _enemies = new List<UnitPm>();
        Factory<UnitType, UnitView, UnitInfo, UnitPm>.BaseCtx unitFabricCtx =
            new Factory<UnitType, UnitView, UnitInfo, UnitPm>.BaseCtx
            {
                resourceLoader = _ctx.resourceLoader
            };
        UnitFactory.Ctx unitFactoryCtx = new UnitFactory.Ctx
        {
            playerCamera = _ctx.playerCamera,
            canvasForJoystick = _ctx.canvasForJoystick,
            baseBorderCoord = _ctx.baseBorderCoord
        };
        _unitFactory = new UnitFactory(unitFabricCtx, unitFactoryCtx);
        AddDispose(_unitFactory);
        CreatePlayer();
    }
    
    public void CreatePlayer()
    {
        UnitInfo playerInfo = new UnitInfo
        {
            damage = 50f,
            attackRadius = 8f,
            attackSpeed = 1f,
            maxHp = 100f,
            moveSpeed = 10f,
            projectileFactory = _ctx.projectileFactory,
            spawnPoint = _ctx.playerSpawnPoint,
            startResources = new List<ConsumableResourcePm>()
        };
        _player = (PlayerPm) _unitFactory.Create(UnitType.Player, playerInfo);
        CurrentHomeState = _player.InHome;
        _isPaused = false;
    }
    
    public void UpdateStates()
    {
        if(_isPaused)
            return;
        if (!_player.IsAlive)
        {
            PlayerDied?.Invoke();
            _isPaused = true;
        }
        _timer += Time.deltaTime;
        if (_timer > _ctx.timeDetweenEnemySpawn)
        {
            _timer = 0;
            AddEnemy();
        }
        ClearDisposed();
        UnitPm nearestEnemyToPlayer = null;
        float nearestDistance = float.MaxValue;
        if (CurrentHomeState != _player.InHome)
            CurrentHomeState = _player.InHome;
        foreach (var enemy in _enemies)
        {
            if (_player.InHome == false && _player.IsAlive)
            {
                enemy.SetTarget(_player);
                    
                if (_player.InAttackRadius(enemy.Position, out float sqrDistance))
                {
                    if (nearestDistance > sqrDistance)
                    {
                        nearestDistance = sqrDistance;
                        nearestEnemyToPlayer = enemy;
                    }
                }
            }
            else
            {
                enemy.SetTarget(null);
            }
                
            enemy.UpdateState();
        }
        
        _player.SetTarget(nearestEnemyToPlayer);
        _player.UpdateState();
    }
    
    private void ClearDisposed()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].IsDisposed)
                _enemies.Remove(_enemies[i]);
        }
    }

    private void AddEnemy()
    {
        UnitInfo enemyInfo = new UnitInfo
        {
            damage = 5f,
            attackRadius = 1.2f,
            attackSpeed = 2f,
            maxHp = 100f,
            moveSpeed = 1f,
            projectileFactory = _ctx.projectileFactory,
            spawnPoint = _ctx.randomPointGenerator.Get(),
            startResources = _ctx.randomResourceGenerator.GetResourceList()
        };
        _enemies.Add(_unitFactory.Create(UnitType.Enemy, enemyInfo));
    }
}