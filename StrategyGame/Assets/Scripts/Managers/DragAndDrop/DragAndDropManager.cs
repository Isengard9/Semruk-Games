using System;
using System.Collections.Generic;
using NC.Strategy.Managers.Game;
using NC.Strategy.Managers.Game.UISystem;
using UnityEngine;

namespace NC.Strategy.Managers.DragAndDrop
{
    public class DragAndDropManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private bool isDragging = false;
        public bool IsDragging => isDragging;
        
        [SerializeField] private bool isDragStarted = false;

        private GameObject _tempBuilding;
        [SerializeField] private Building.Building draggingObject;

        [SerializeField] private List<GameObject> buildingPrefabs = new List<GameObject>();

        [SerializeField] private LayerMask gridLayer;

        private Vector2 DragStartPosition = Vector2.zero;

        #endregion

        #region Open Building Info

        public void OpenBuildingInfo(string buildingName)
        {
            var building = buildingPrefabs.Find(x => x.name == buildingName);

            if (building is null)
            {
                Debug.LogError($"Building not founded. Searched building: {buildingName}");
                return;
            }

            var details = new Details
            {
                SelectedObject = null,
                SelectedObjectInfo = building.GetComponent<Building.Building>().BuildingInfo
            };

            GameManager.instance.DetailsSection.OpenDetailsSection(details,
                DetailsSection.DetailSectionState.NewBuilding);
        }

        #endregion

        #region Choose Building

        public void ChooseBuilding(string buildingName)
        {
            if (draggingObject is not null)
            {
                return;
            }

            var building = buildingPrefabs.Find(x => x.name == buildingName);

            if (building is null)
            {
                Debug.LogError($"Building not founded. Searched building: {buildingName}");
                return;
            }

            _tempBuilding = building;

            isDragStarted = false;
            
            OpenBuildingInfo(buildingName);
        }

        #endregion

        #region Unity Update

        private void Update()
        {
            isDragging = ControlCheckIsDragging();

            #region Mouse Control

            if (Input.GetMouseButtonDown(0))
            {
                DragStartPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                if (isDragging)
                {
                    DragBuilding();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                DropPrefab();
            }

            #endregion
        }

        #endregion

        #region Create Building

        private void CreateBuilding()
        {
            draggingObject = Instantiate(_tempBuilding, GameManager.instance.BuildingParent)
                .GetComponent<Building.Building>();
            
            draggingObject.SelectObject();
        }

        #endregion

        #region Drag Drop Mechanics

        #region Control Check is dragging

        private bool ControlCheckDragStarted()
        {
            if (_tempBuilding is null)
            {
                return false;
            }

            if (isDragStarted)
            {
                return true;
            }

            else
            {
                if (Vector2.Distance(DragStartPosition, Input.mousePosition) < 20)
                {
                    return false;
                }
                else
                {
                    isDragStarted = true;
                    CreateBuilding();
                    GameManager.instance.SelectingManager.DeSelectObject();
                    return true;
                }
            }
        }

        private bool ControlCheckIsDragging()
        {
            if (isDragStarted)
            {
                return draggingObject != null && Input.GetMouseButton(0) ? true : false;
            }
            else
            {
                return ControlCheckDragStarted();
            }
        }

        #endregion

        #region Drag Prefab

        private void DragBuilding()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, gridLayer))
            {
                var objectHit = hit.transform;


                var draggingPosition = objectHit.transform.position;
                draggingPosition.y = 0.5f;
                draggingPosition.x += 0.5f;
                draggingPosition.z -= 0.5f;
                draggingObject.transform.position = draggingPosition;
            }
        }

        #endregion

        #region Drop prefabs

        private void DropPrefab()
        {
            isDragging = false;
            isDragStarted = false;
            _tempBuilding = null;
            
            if (draggingObject is null)
            {
                Debug.Log("There is no dragging object");
                return;
            }

            ControlCheckDropPerfectly();
            draggingObject = null;
            
        }

        private void ControlCheckDropPerfectly()
        {
            if (draggingObject.IsBuildable)
            {
                Debug.Log("Drop perfectly");
                draggingObject.BuildAnimation();
                draggingObject.IsPlaced = true;
                draggingObject.OnPlaced.Invoke();
                draggingObject.Placed();
                
            }
            else
            {
                Debug.Log("Can not drop here");
                draggingObject.IsDestroyed = true;
                Destroy(draggingObject.gameObject,0.2f);
            }
        }

        #endregion

        #endregion
    }
}