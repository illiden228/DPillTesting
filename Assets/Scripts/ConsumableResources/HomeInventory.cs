using System;
using System.Collections.Generic;
using UnityEngine;

public class HomeInventory : BaseDisposable
{
    public struct Ctx
    {
        public UnitController unitController;
        public HomeResourcesView resourcesView;
    }

    private readonly Ctx _ctx;
    private BugPm _bug;

    public event Action<IReadOnlyList<ConsumableResourcePm>> HomeResourcesChanged;

    public HomeInventory(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.unitController.PlayerChangeHomeState += OnHomeStateChanged;
        _bug = new BugPm(new BugPm.Ctx());
        AddDispose(_bug);

        HomeResourcesView.Ctx resourcesViewCtx = new HomeResourcesView.Ctx
        {
            homeInventory = this
        };
        _ctx.resourcesView.Init(resourcesViewCtx);
    }

    private void OnHomeStateChanged(bool state)
    {
        _bug.Put(_ctx.unitController.Player.TakeAllResources());
        HomeResourcesChanged?.Invoke(_bug.GetResourceList());
    }

    protected override void OnDispose()
    {
        _ctx.unitController.PlayerChangeHomeState -= OnHomeStateChanged;
    }
}