using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartView : BaseMonoBehaviour
{
    public struct Ctx
    {
        public Action onRestart;
        public UnitController controller;
    }

    private Ctx _ctx;

    [SerializeField] private Button _restartButton;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;
        gameObject.SetActive(false);
        _ctx.controller.PlayerDied += () => gameObject.SetActive(true);
    }

    private void OnRestartClick()
    {
        gameObject.SetActive(false);
        _ctx.onRestart?.Invoke();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartClick);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartClick);
    }

    protected override void OnDestroy()
    {
        _ctx.controller.PlayerDied -= () => gameObject.SetActive(false);
    }
}