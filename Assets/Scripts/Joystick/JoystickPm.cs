using UnityEngine;

public class JoystickPm : BaseDisposable
{
    public struct Ctx
    {
        public IResourceLoader resourceLoader;
        public Canvas parent;
    }

    private PlayerInput _playerInput;
    private JoystickView _joistickView;
    private readonly Ctx _ctx;
    
    private readonly ResourceInfo _joystickView = new ResourceInfo
    {
        StorageName = "Prefabs/",
        Name = "JoystickView"
    };

    public Vector3 Direction
    {
        get
        {
            Vector2 clampedDirection = _playerInput.ClampedMoveDirection;
            return new Vector3(clampedDirection.x, 0, clampedDirection.y);
        }
    }

    public JoystickPm(Ctx ctx)
    {
        _ctx = ctx;
        _playerInput = new PlayerInput();
        _ctx.resourceLoader.LoadPrefab(_joystickView, OnLoad);
    }

    private void OnLoad(GameObject prefab)
    {
        _joistickView = GameObject.Instantiate(prefab, _ctx.parent.transform).GetComponentInChildren<JoystickView>();
        _joistickView.Init();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        GameObject.Destroy(_joistickView.gameObject); 
    }
}