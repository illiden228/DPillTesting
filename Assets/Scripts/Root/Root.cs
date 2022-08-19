using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Root : BaseMonoBehaviour
{
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] public CinemachineVirtualCamera _camera;
    [SerializeField] public HomeResourcesView _resourcesView;
    [SerializeField] public RestartView _restartView;

    [Header("Borders")] 
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;
    [SerializeField] private Transform _forward;
    [SerializeField] private Transform _back;

    [Header("Logic")] 
    [SerializeField] private float _timeBetweenSpawn;
    [SerializeField] private int _minRandomMoneyCount;
    [SerializeField] private int _maxRandomMoneyCount;
    [SerializeField] private int _minRandomCrystalCount;
    [SerializeField] private int _maxRandomCrystalCount;
    private List<IDisposable> _disposables;
    private ProjectileController _projectileController;
    private UnitController _unitController;
    
    private void Awake()
    {
        _disposables = new List<IDisposable>();
        ResourceLoader resourceLoader = new ResourceLoader();
        _disposables.Add(resourceLoader);

        ProjectileController.Ctx projectileControllerCtx = new ProjectileController.Ctx
        {
            resourceLoader = resourceLoader
        };
        _projectileController = new ProjectileController(projectileControllerCtx);
        _disposables.Add(_projectileController);

        RandomPointGenerator.Ctx pointGeneratorCtx = new RandomPointGenerator.Ctx
        {
            backBodrder = _back.position.z,
            forwardBodrder = _forward.position.z,
            leftBodrder = _left.position.x,
            rightBodrder = _right.position.x
        };
        RandomPointGenerator randomPointGenerator = new RandomPointGenerator(pointGeneratorCtx);
        _disposables.Add(randomPointGenerator);

        RandomResourceGenerator.Ctx resourceGeneratorCtx = new RandomResourceGenerator.Ctx
        {
            resourceLoader = resourceLoader,
            maxCountCrystal = _maxRandomCrystalCount,
            maxCountMoney = _maxRandomMoneyCount,
            minCountCrystal = _minRandomCrystalCount,
            minCountMoney = _minRandomMoneyCount
        };
        RandomResourceGenerator resourceGenerator = new RandomResourceGenerator(resourceGeneratorCtx);
        _disposables.Add(resourceGenerator);

        UnitController.Ctx unitControllerCtx = new UnitController.Ctx
        {
            resourceLoader = resourceLoader,
            playerCamera = _camera,
            projectileFactory = _projectileController.Factory,
            canvasForJoystick = _mainCanvas,
            playerSpawnPoint = _playerSpawnPoint.position,
            randomPointGenerator = randomPointGenerator,
            randomResourceGenerator = resourceGenerator,
            baseBorderCoord = _back.position.z,
            timeDetweenEnemySpawn = _timeBetweenSpawn,
        };
        _unitController = new UnitController(unitControllerCtx);
        _disposables.Add(_unitController);

        HomeInventory.Ctx homeInventoryCtx = new HomeInventory.Ctx
        {
            resourcesView = _resourcesView,
            unitController = _unitController
        };
        HomeInventory homeInventory = new HomeInventory(homeInventoryCtx);
        _disposables.Add(homeInventory);

        RestartView.Ctx restartViewCtx = new RestartView.Ctx
        {
            controller = _unitController,
            onRestart = () =>
            {
                _unitController.Player.Dispose();
                _unitController.CreatePlayer();
            },
        };
        _restartView.Init(restartViewCtx);
    }

    private void Update()
    {
        _projectileController.UpdateStates();
        _unitController.UpdateStates();
    }

    private void FixedUpdate()
    {
        _unitController.UpdatePhysicStates();
    }
}
