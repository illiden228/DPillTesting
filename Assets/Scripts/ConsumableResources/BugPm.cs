using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BugPm : BaseDisposable
{
    public struct Ctx
    {
        public Transform ownerTransform;
    }

    private readonly Ctx _ctx;
    private List<ConsumableResourcePm> _resources;

    public BugPm(Ctx ctx)
    {
        _ctx = ctx;
        _resources = new List<ConsumableResourcePm>();
    }

    public void Put(ConsumableResourcePm newResource)
    {
        ConsumableResourcePm repeatResource = GetResourceByType(newResource.Type);
        
        if(repeatResource != null)
            repeatResource.Merge(newResource);
        else
            _resources.Add(newResource);
    }
    
    public void Put(List<ConsumableResourcePm> resources)
    {
        foreach (var resource in resources)
        {
            Put(resource);
        }
    }

    public ConsumableResourcePm Abort(ConsumableType resourceType)
    {
        ConsumableResourcePm abortResource = GetResourceByType(resourceType);

        if (abortResource != null)
        {
            _resources.Remove(abortResource);
            abortResource.SetInWorld(_ctx.ownerTransform.position);
        }

        return abortResource;
    }

    public List<ConsumableResourcePm> AbortAll()
    {
        for (int i = 0; i < _resources.Count; i++)
        {
            Abort(_resources[i].Type);
        }

        return _resources;
    }

    public IReadOnlyList<ConsumableResourcePm> GetResourceList()
    {
        return _resources;
    }

    public List<ConsumableResourcePm> TakeAll()
    {
        List<ConsumableResourcePm> takeResources = new List<ConsumableResourcePm>(_resources);
        _resources.Clear();
        return takeResources;
    }

    private ConsumableResourcePm GetResourceByType(ConsumableType resourceType)
    {
        foreach (var resource in _resources)
        {
            if (resource.Type == resourceType)
            {
                return resource;
            }
        }

        return null;
    }
}