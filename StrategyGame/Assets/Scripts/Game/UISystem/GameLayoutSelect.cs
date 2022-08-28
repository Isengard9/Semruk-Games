using UnityEngine;
using UnityEngine.SceneManagement;

namespace NC.Strategy.Managers.Game.UISystem
{
    public class GameLayoutSelect : MonoBehaviour
    {
        public void SelectLayout(int val)
        {
            if (val == 0)
            {
                return;
            }
            PlayerPrefs.SetInt("Layout",val-1);

            SceneManager.LoadScene(1);
        }
    }
}