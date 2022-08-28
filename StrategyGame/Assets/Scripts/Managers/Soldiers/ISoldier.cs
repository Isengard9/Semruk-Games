using UnityEngine;

namespace NC.Strategy.Managers.Soldier
{
    public interface ISoldier
    {
        void Move(Transform movePoint);
    }
}