using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace NC.Strategy.Managers.Grid
{
    public class GridManager : MonoBehaviour
    {
        #region Grid Variables

        private enum GridLayout
        {
            [Description("5x5")] _5x5 = 7 * 7,

            [Description("7x7")] _7x7 = 9 * 9,

            [Description("9x9")] _9x9 = 11 * 11,

            [Description("11x11")] _11x11 = 13 * 13,

            [Description("13x13")] _13x13 = 15 * 15,

            [Description("15x15")] _15x15 = 17 * 17,
        }

        [SerializeField] private GridLayout gridLayout = GridLayout._5x5;

        private float GridCount
        {
            get { return (int)gridLayout; }
        }

        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GameObject forbiddenAreaPrefab;

        [SerializeField] private List<GridPartManager> gridPartList = new List<GridPartManager>();
        [SerializeField] public List<GridPartManager> GridPartList => gridPartList;
        [SerializeField] private Transform _ground;
        
        #endregion

        private void Start()
        {
            InitializeLayout();
            CreateGrid();
            _ground.localScale = (Vector3.right + Vector3.forward) * Mathf.Sqrt((int)gridLayout);
        }

        private void InitializeLayout()
        {
            switch (PlayerPrefs.GetInt("Layout"))
            {
                case 0:
                    gridLayout = GridLayout._5x5;
                    break;
                case 1:
                    gridLayout = GridLayout._7x7;
                    break;
                case 2:
                    gridLayout = GridLayout._9x9;
                    break;
                case 3:
                    gridLayout = GridLayout._11x11;
                    break;
                case 4:
                    gridLayout = GridLayout._13x13;
                    break;
                case 5:
                    gridLayout = GridLayout._15x15;
                    break;
                
            }
        }

        private void CreateGrid()
        {
            var gridParent = new GameObject();
            gridParent.transform.position = Vector3.zero;
            gridParent.name = "GridParent";


            var count = Mathf.Sqrt(GridCount);
            var distance = (int)Mathf.Floor(count / 2) * -1;

            for (int i = distance; i <= Mathf.Abs(distance); i++)
            {
                for (int j = distance; j <= Mathf.Abs(distance); j++)
                {
                    if (Mathf.Abs(i) == Mathf.Abs(distance) || Mathf.Abs(j) == Mathf.Abs(distance))
                    {
                        CreateForbiddenArea(gridParent.transform, i, j);
                    }
                    else
                    {
                        CreateGridPart(gridParent.transform, i, j);
                    }
                }
            }

            gridParent.transform.position = new Vector3(0.5f, 0.0f, 0.5f);
        }

        private void CreateGridPart(Transform gridParent, int x, int y)
        {
            var gridPart = Instantiate(gridPrefab, gridParent);
            gridPart.transform.position = new Vector3(x, 0, y);
            gridPart.name = $"Grid: {x} x {y}";
            var gridPartManager = gridPart.GetComponent<GridPartManager>();
            gridPartList.Add(gridPartManager);
            gridPartManager.xValue = x;
            gridPartManager.yValue = y;
        }

        private void CreateForbiddenArea(Transform gridParent, int x, int y)
        {
            var gridPart = Instantiate(forbiddenAreaPrefab, gridParent);
            gridPart.transform.position = new Vector3(x, 0, y);
            gridPart.name = $"Forbidden Area: {x} x {y}";
        }

        
    }
}