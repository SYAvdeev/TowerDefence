using System;

namespace Presentation.LevelObjects
{
    public interface IPoolObject
    {
        event Action<IPoolObject> AddToPool;
    }
}