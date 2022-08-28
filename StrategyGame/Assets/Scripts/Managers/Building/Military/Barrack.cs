namespace NC.Strategy.Managers.Building.Military
{
    public class Barrack: MilitaryBuildingController
    {
        private void Start()
        {
            OnPlaced += Placed;
        }

        public override void CreateSoldier()
        {
            var gridPoints = ControlGrids();
            if (gridPoints is null)
            {
                return;
            }

            var soldier = Instantiate(SoldierPrefab);
            var position = gridPoints.transform.position;
            position.y = 1;
            soldier.transform.position = position;
        }

        public override void Placed()
        {
            CreateSoldier();
        }
        
        private void OnDestroy()
        {
            OnPlaced -= Placed;
        }

    }
}