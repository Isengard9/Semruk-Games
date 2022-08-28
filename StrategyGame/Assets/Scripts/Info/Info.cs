using UnityEngine;

namespace NC.Strategy.Game.Info
{
    [CreateAssetMenu(fileName = "Info", menuName = "Strategy/Info", order = 0)]
    public class Info : ScriptableObject
    {
        public string Title = "";
        public string AboutInfo = "";
    }
}