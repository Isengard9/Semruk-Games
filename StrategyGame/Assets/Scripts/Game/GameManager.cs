using System;
using NC.Strategy.Managers.DragAndDrop;
using NC.Strategy.Managers.Game.UISystem;
using NC.Strategy.Managers.Grid;
using NC.Strategy.Managers.Selecting;
    using UnityEngine;

namespace NC.Strategy.Managers.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [Header("Building")]
        private Transform buildingParent;
        public Transform BuildingParent => buildingParent;

        

        [Header("UI System")] 
        public DetailsSection DetailsSection;


        [Header("Managers")] 
        
        public DragAndDropManager DragAndDropManager;

        public SelectingManager SelectingManager;
        public GridManager GridManager;
        
        private void Awake()
        {
            if (instance is not null)
            {
                GameManager.instance.DeInitialize();
            }

            instance = this;
            Initialize();
        }


        private void Initialize()
        {
            #region Building Parent

            buildingParent = new GameObject().transform;
            buildingParent.name = "Building Parent";
            buildingParent.transform.position = Vector3.zero;

            #endregion
            
        }

        private void DeInitialize()
        {
            Destroy(buildingParent);
            Destroy(this.gameObject);
        }
    }
}