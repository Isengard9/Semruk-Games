using System;
using System.Collections.Generic;
using DG.Tweening;
using NC.Strategy.Game.Info;
using NC.Strategy.Managers.Game;
using NC.Strategy.Managers.Game.InGameDynamics.Destroy;
using NC.Strategy.Managers.Game.InGameDynamics.Select;
using NC.Strategy.Managers.Game.UISystem;
using NC.Strategy.Managers.Grid;
using NC.Strategy.Tools;
using UnityEngine;

namespace NC.Strategy.Managers.Building
{
    public abstract class Building : MonoBehaviour, IBuilding, ISelect, IDestroy
    {
        public bool IsBuildable = true;
        public bool IsPlaced = false;
        public bool IsDestroyed = false;
        
        public List<Transform> BuildingPartList = new List<Transform>();
        public Info BuildingInfo;
        
        private List<GridPartManager> _gridParts = new List<GridPartManager>();
        public List<GridPartManager> GridPartManagers => _gridParts;

        public Action OnPlaced;

        public void Placed()
        {
            foreach (var part in GridPartManagers)
            {
                part.SetPlaced(true);
            }
        }

        public void Rotate()
        {
            this.transform.Rotate(Vector3.up * 90, Space.Self);
        }

        private void OnCollisionEnter(Collision collisionInfo)
        {
            if (IsPlaced)
            {
                return;
            }

            var gridPart = collisionInfo.transform.GetComponent<GridPartManager>();

            if (gridPart is not null)
            {

                if (!gridPart.IsPlaced)
                {
                    gridPart.SetAvailable(false);
                }
                
                if (!_gridParts.Contains(gridPart))
                {
                    _gridParts.Add(gridPart);
                }
            }
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            if (IsPlaced)
            {
                return;
            }

            var building = collisionInfo.transform.GetComponent<Building>();

            if (building is not null)
            {
                IsBuildable = false;
                ChangeNotBuildableColor();
            }

            var outOfGrid = collisionInfo.transform.GetComponent<ForbiddenArea>();

            if (outOfGrid is not null)
            {
                IsBuildable = false;
                ChangeNotBuildableColor();
            }
            
            var soldier = collisionInfo.transform.GetComponent<Soldier.Soldier>();

            if (soldier is not null)
            {
                IsBuildable = false;
                ChangeNotBuildableColor();
            }

            var gridPart = collisionInfo.transform.GetComponent<GridPartManager>();

            if (gridPart is not null)
            {
                if (!gridPart.IsPlaced)
                {
                    gridPart.SetAvailable(false);
                }
                
                if (!_gridParts.Contains(gridPart))
                {
                    _gridParts.Add(gridPart);
                }
            }
        }

        private void OnCollisionExit(Collision collisionInfo)
        {
            if (IsPlaced)
            {
                return;
            }

            var building = collisionInfo.transform.GetComponent<Building>();
            if (building is not null)
            {
                IsBuildable = true;
                ChangeBuildableColor();
            }

            var outOfGrid = collisionInfo.transform.GetComponent<ForbiddenArea>();

            if (outOfGrid is not null)
            {
                IsBuildable = true;
                ChangeBuildableColor();
            }
            
            var soldier = collisionInfo.transform.GetComponent<Soldier.Soldier>();

            if (soldier is not null)
            {
                IsBuildable = true;
                ChangeBuildableColor();
            }

            var gridPart = collisionInfo.transform.GetComponent<GridPartManager>();

            if (gridPart is not null)
            {
                if (!gridPart.IsPlaced)
                {
                    gridPart.SetAvailable(true);
                }

                if (_gridParts.Contains(gridPart))
                {
                    _gridParts.Remove(gridPart);
                }
            }
        }


        public void BuildAnimation()
        {
            if (IsPlaced)
            {
                return;
            }

            for (int i = 0; i < BuildingPartList.Count; i++)
            {
                var scale = BuildingPartList[i].transform.localScale;

                scale.y = 0;
                BuildingPartList[i].localScale = scale;

                BuildingPartList[i].DOScaleY(1, 0.2f).SetDelay(0.02f * i);
            }

            DeSelectObject();
        }


        public void SelectObject()
        {
            if (IsDestroyed)
            {
                return;
            }

            foreach (var obj in BuildingPartList)
            {
                var meshRenderer = obj.GetComponent<MeshRenderer>();
                meshRenderer.material.color = ColorHelper.SelectedColor;
            }

            if (IsPlaced)
            {
                var details = new Details
                {
                    SelectedObject = this.gameObject,
                    SelectedObjectInfo = this.BuildingInfo,
                };
                GameManager.instance.DetailsSection.OpenDetailsSection(details,
                    DetailsSection.DetailSectionState.SelectedBuilding);
            }
        }

        public void DeSelectObject()
        {
            if (IsDestroyed)
            {
                return;
            }

            foreach (var obj in BuildingPartList)
            {
                var meshRenderer = obj.GetComponent<MeshRenderer>();
                meshRenderer.material.color = ColorHelper.UnSelectedColor;
            }
        }

        private void ChangeBuildableColor()
        {
            if (IsDestroyed)
            {
                return;
            }

            if (IsPlaced)
            {
                return;
            }

            foreach (var obj in BuildingPartList)
            {
                var meshRenderer = obj.GetComponent<MeshRenderer>();
                meshRenderer.material.color = ColorHelper.SelectedColor;
            }
        }

        private void ChangeNotBuildableColor()
        {
            if (IsDestroyed)
            {
                return;
            }

            if (IsPlaced)
            {
                return;
            }


            foreach (var obj in BuildingPartList)
            {
                var meshRenderer = obj.GetComponent<MeshRenderer>();
                meshRenderer.material.color = ColorHelper.UnUsableColor;
            }
        }

        public void DestroyObject()
        {
            IsDestroyed = true;
            
            foreach (var part in GridPartManagers)
            {
                part.SetAvailable(true);
                part.SetPlaced(false);
            }
            
            Destroy(this.gameObject);
            
        }

        protected Transform ControlGrids()
        {
            foreach (var part in GridPartManagers)
            {
                var partLeft =
                    GameManager.instance.GridManager.GridPartList.Find(x =>
                        x.xValue == part.xValue - 1 && x.yValue == part.yValue);
                if (partLeft != null && partLeft.Available)
                {
                    return partLeft.transform;
                }

                var partRight =
                    GameManager.instance.GridManager.GridPartList.Find(x =>
                        x.xValue == part.xValue + 1 && x.yValue == part.yValue);
                if (partRight != null && partRight.Available)
                {
                    return partRight.transform;
                }

                var partUp =
                    GameManager.instance.GridManager.GridPartList.Find(x =>
                        x.xValue == part.xValue && x.yValue == part.yValue + 1);
                if (partUp != null && partUp.Available)
                {
                    return partUp.transform;
                }

                var partDown =
                    GameManager.instance.GridManager.GridPartList.Find(x =>
                        x.xValue == part.xValue && x.yValue == part.yValue - 1);
                if (partDown != null && partDown.Available)
                {
                    return partDown.transform;
                }
            }

            return null;
        }
        
    }
}