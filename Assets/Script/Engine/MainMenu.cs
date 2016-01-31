using UnityEngine;
using UnityEngine.SceneManagement;

namespace Engine
{
    public class MainMenu : MonoBehaviour
    {
        void Awake()
        {
            Cardboard.Create();
        }

        void Update()
        {
            if((Input.touchSupported && Input.GetTouch(0).tapCount > 0) || Input.GetMouseButton(0) || Cardboard.SDK.Triggered) { 
                Debug.Log("Trigger Chargement Scene");
                SceneManager.LoadScene(1);
            }
        }
    }
}
