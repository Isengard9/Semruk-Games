using NC.Strategy.Tools;
using UnityEngine;

namespace NC.Strategy.Managers.Grid
{
    public class GridPartManager : MonoBehaviour
    {
        private bool available = true;

        public bool Available
        {
            get
            {
                return IsPlaced ? false : available;
            }
        }
        
        public int xValue, yValue;

        private bool isPlaced = false;
        public bool IsPlaced => isPlaced;

        [SerializeField] private MeshRenderer centerObject;
        
        public void SetAvailable(bool _available = false)
        {
            available = _available;
        }

        public void SetPlaced(bool placed = true)
        {
            isPlaced = placed;
        }

        public void SetSelected(bool selected = false)
        {
            if (selected)
            {
                centerObject.material.color = ColorHelper.MoveColor;
            }
            else
            {
                centerObject.material.color = Color.white;
            }
        }

    }
}