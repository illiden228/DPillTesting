using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HomeResourcesView : BaseMonoBehaviour
{
    public struct Ctx
    {
        public HomeInventory homeInventory;
    }

    private Ctx _ctx;
    [SerializeField] private TMP_Text _moneys;
    [SerializeField] private TMP_Text _crystals;

    public void Init(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.homeInventory.HomeResourcesChanged += OnHomeResourcesChanged;
        _moneys.text = "0";
        _crystals.text = "0";
    }

    private void OnHomeResourcesChanged(IReadOnlyList<ConsumableResourcePm> resources)
    {
        foreach (var resource in resources)
        {
            if (resource.Type == ConsumableType.Money)
                _moneys.text = resource.Count.ToString();
            if (resource.Type == ConsumableType.Crystal)
                _crystals.text = resource.Count.ToString();
        }
    }

    private void OnDisable()
    {
        _ctx.homeInventory.HomeResourcesChanged -= OnHomeResourcesChanged;
    }
}