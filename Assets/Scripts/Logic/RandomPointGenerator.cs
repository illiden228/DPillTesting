using UnityEngine;

public class RandomPointGenerator : BaseDisposable
{
    public struct Ctx
    {
        public float leftBodrder;
        public float rightBodrder;
        public float forwardBodrder;
        public float backBodrder;
    }

    private readonly Ctx _ctx;

    public RandomPointGenerator(Ctx ctx)
    {
        _ctx = ctx;
    }

    public Vector3 Get()
    {
        float x = Random.Range(_ctx.leftBodrder, _ctx.rightBodrder);
        float z = Random.Range(_ctx.forwardBodrder, _ctx.backBodrder);
        return new Vector3(x, 0f, z);
    }
}