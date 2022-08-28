using UnityEngine;

namespace NC.Strategy.Managers.Building
{
    public abstract class MilitaryBuildingController : Building
    {
        public GameObject SoldierPrefab;
        public abstract void CreateSoldier();
        public abstract void Placed();

    }
    
}