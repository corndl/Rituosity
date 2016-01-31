using UnityEngine;
using UnityEngine.SceneManagement;

namespace Engine
{
    public class MainMenu : MonoBehaviour
    {
        #region Unity
        void Awake()
        {
            Cardboard.Create();
            Cardboard.SDK.TapIsTrigger = true;
        }

        void Update()
        {
            if (Cardboard.SDK.Triggered)
            {
                SceneManager.LoadScene(1);
            }
        }
        #endregion
    }
}
