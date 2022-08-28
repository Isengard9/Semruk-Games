using UnityEngine;

namespace NC.Strategy.Managers.Building
{
    [CreateAssetMenu(fileName = "BuildingInfo", menuName = "Strategy/Building", order = 0)]
    public class BuildingInfo : ScriptableObject
    {
        public string Info = "";
    }
}