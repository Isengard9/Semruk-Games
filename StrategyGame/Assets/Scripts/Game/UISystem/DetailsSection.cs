using System;
using NC.Strategy.Managers.Game.InGameDynamics.Destroy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NC.Strategy.Managers.Game.UISystem
{
    public class DetailsSection : MonoBehaviour
    {
        public enum DetailSectionState
        {
            None,
            NewBuilding,
            SelectedBuilding,
            Soldier
        }

        public DetailSectionState _DetailSectionState = DetailSectionState.None;

        public TMP_Text InfoAbout;
        public TMP_Text InfoTitle;

        public Button RemoveButton;
        
        private Details _currentDetails;

        private void Start()
        {
            CloseDetailsSection();
        }

        #region Details Section Functions

        public void OpenDetailsSection(Details details, DetailSectionState state)
        {
            _currentDetails = details;
            SetSection(state);
        }

        public void CloseDetailsSection()
        {
            SetSection(DetailSectionState.None);
        }

        private void SetSection(DetailSectionState state)
        {
            _DetailSectionState = state;
            ChangeState();
        }

        private void ChangeState()
        {
            switch (_DetailSectionState)
            {
                case DetailSectionState.None:
                    CloseDetails();
                    break;
                
                case DetailSectionState.NewBuilding:
                    OpenDescriptionUI();
                    break;
                
                case DetailSectionState.SelectedBuilding:
                    OpenBuildingUI();
                    break;

                case DetailSectionState.Soldier:
                    OpenSoldierUI();
                    break;
            }
        }

        private void OpenBuildingUI()
        {
            RemoveButton.gameObject.SetActive(true);
            
            InfoTitle.gameObject.SetActive(true);
            InfoTitle.text = _currentDetails.SelectedObjectInfo.Title;
            
            InfoAbout.gameObject.SetActive(true);
            InfoAbout.text = _currentDetails.SelectedObjectInfo.AboutInfo;
            
        }

        private void OpenSoldierUI()
        {
            RemoveButton.gameObject.SetActive(true);
            
            InfoTitle.gameObject.SetActive(true);
            InfoTitle.text = _currentDetails.SelectedObjectInfo.Title;
            
            InfoAbout.gameObject.SetActive(true);
            InfoAbout.text = _currentDetails.SelectedObjectInfo.AboutInfo;
            
        }

        private void OpenDescriptionUI()
        {
            RemoveButton.gameObject.SetActive(false);
            
            InfoTitle.gameObject.SetActive(true);
            InfoTitle.text = _currentDetails.SelectedObjectInfo.Title;
            
            InfoAbout.gameObject.SetActive(true);
            InfoAbout.text = _currentDetails.SelectedObjectInfo.AboutInfo;
        }

        private void CloseDetails()
        {
            RemoveButton.gameObject.SetActive(false);
            
            InfoTitle.gameObject.SetActive(false);
            
            InfoAbout.gameObject.SetActive(false);
        }

        #endregion

        #region Destroy

        public void DestroyObject()
        {
            _currentDetails?.SelectedObject.GetComponent<IDestroy>().DestroyObject();
            CloseDetailsSection();
        }

        #endregion
    }
}