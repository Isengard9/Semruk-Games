using System;
using System.Collections.Generic;
using NC.Strategy.Game.Info;
using NC.Strategy.Managers.Game;
using NC.Strategy.Managers.Game.InGameDynamics.Destroy;
using NC.Strategy.Managers.Game.InGameDynamics.Select;
using NC.Strategy.Managers.Game.UISystem;
using NC.Strategy.Tools;
using UnityEngine;
using UnityEngine.AI;

namespace NC.Strategy.Managers.Soldier
{
    public abstract class Soldier : MonoBehaviour, ISoldier, ISelect,IDestroy
    {
        public NavMeshAgent Agent;
        public Color SoldierColor;

        private bool isDestroyed;
        
        [SerializeField] private List<Transform> SoldierTransforms = new List<Transform>();

        public Info SoldierInfo;

        public void Move(Transform movePoint)
        {
            Agent.SetDestination(movePoint.position);
            GameManager.instance.SelectingManager.SoldierSelected = false;
            DeSelectObject();
        }
        
        public void SelectObject()
        {
            if (isDestroyed)
            {
                return;
            }
            
            ChangeSelectedColor();
            
            var details = new Details
            {
                SelectedObject = this.gameObject,
                SelectedObjectInfo = this.SoldierInfo,
            };
            GameManager.instance.DetailsSection.OpenDetailsSection(details,
                DetailsSection.DetailSectionState.Soldier);

            GameManager.instance.SelectingManager.SoldierSelected = true;
            GameManager.instance.SelectingManager.StartMove += Move;
        }

        public void DeSelectObject()
        {
            if (isDestroyed)
            {
                return;
            }
            ChangeDeSelectedColors();
            GameManager.instance.SelectingManager.StartMove -= Move;
        }

        public void DestroyObject()
        {
            isDestroyed = true;
            Destroy(this.gameObject);
        }

        void ChangeSelectedColor()
        {
            foreach (var part in SoldierTransforms)
            {
                var renderer = part.GetComponent<MeshRenderer>();
                renderer.material.color = ColorHelper.SelectedColor;
            }
        }

        void ChangeDeSelectedColors()
        {
            foreach (var part in SoldierTransforms)
            {
                var renderer = part.GetComponent<MeshRenderer>();
                renderer.material.color = SoldierColor;
            }
        }
        
        
        
    }
}