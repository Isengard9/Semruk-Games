using System;

namespace NC.Strategy.Managers.Building.City
{
    public class TownCenter : CityBuildingController
    {
        private void Start()
        {
            OnPlaced += Placed;
        }

        private void OnDestroy()
        {
            OnPlaced -= Placed;
        }
    }
}