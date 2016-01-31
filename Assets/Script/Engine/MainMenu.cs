using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Engine
{
    public class MainMenu : MonoBehaviour
    {
        #region Unity
        void Awake()
        {
            Cardboard.Create();
            Cardboard.SDK.TapIsTrigger = true;

            StartCoroutine(LoadLevel());
        }

        void Update()
        {
            if (Cardboard.SDK.Triggered)
            {
                SceneManager.LoadScene(1);
            }
        }
        #endregion

        #region Routines
        IEnumerator LoadLevel()
        {
            yield return new WaitForSeconds(5.5f);
            SceneManager.LoadScene(1);
        }
        #endregion
    }
}
