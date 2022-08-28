using System;
using System.Collections.Generic;
using NC.Strategy.Managers.Game;
using NC.Strategy.Managers.Game.InGameDynamics.Select;
using NC.Strategy.Managers.Grid;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NC.Strategy.Managers.Selecting
{
    public class SelectingManager : MonoBehaviour
    {
        public LayerMask SelectableObjectLayer;
        public LayerMask SelectableGridLayer;
        [SerializeField] private ISelect _tempSelectedObject;

        public bool SoldierSelected = false;

        public Action<Transform> StartMove;
        
        private void Update()
        {
            if (Input.GetMouseButton(0) && IsPointerOverUIElement())
            {
                return;
            }
            
            if (SoldierSelected && Input.GetMouseButtonUp(0))
            {
                ControlCheckGrid();
            }
            
            if (Input.GetMouseButtonUp(0) && !GameManager.instance.DragAndDropManager.IsDragging)
            {
               ControlIsThereAnySelectableObject();
            }

            
        }
        
        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        private void ControlIsThereAnySelectableObject()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, SelectableObjectLayer))
            {
                var objectHit = hit.transform;

                var selectedObject = objectHit.GetComponent<ISelect>();

                if (selectedObject != null)
                {
                    _tempSelectedObject?.DeSelectObject();;
                    selectedObject.SelectObject();
                    _tempSelectedObject = selectedObject;
                }

            }
            else
            {
                Debug.Log("Not selected anything");
                _tempSelectedObject?.DeSelectObject();
                GameManager.instance.DetailsSection.CloseDetailsSection();
            }
        }

        private void ControlCheckGrid()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000, SelectableGridLayer))
            {
                var objectHit = hit.transform;

                var selectedObject = objectHit.GetComponent<GridPartManager>();

                if (selectedObject != null && selectedObject.Available && !selectedObject.IsPlaced)
                {
                     StartMove?.Invoke(objectHit); 
                }

            }
            else
            {
                Debug.Log("Not selected anything");
                _tempSelectedObject?.DeSelectObject();
                GameManager.instance.DetailsSection.CloseDetailsSection();
            }
        }

        public void DeSelectObject()
        {
            _tempSelectedObject?.DeSelectObject();
        }
    }
}