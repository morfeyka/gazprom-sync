using System;

namespace Sofia.Domain
{
    [Serializable]
    public abstract class Entity : EntityWithTypedId<int>
    {
    }
}