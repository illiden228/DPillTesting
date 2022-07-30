using System;
using UnityEngine;

public interface IResourceLoader
{
    public void LoadPrefab(ResourceInfo resourceInfo, Action<GameObject> onLoad);
}