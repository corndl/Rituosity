using UnityEngine;
using UnityEngine.SceneManagement;

namespace Engine
{
    public class MainMenu : MonoBehaviour
    {
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
    }
}
