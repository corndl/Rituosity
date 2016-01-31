using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Engine
{
    public class GameOver : MonoBehaviour
    {
        #region Events
        public StringEvent onScore = new StringEvent();
        #endregion

        #region Unity
        void Awake()
        {
            Cardboard.Create();
            Cardboard.SDK.TapIsTrigger = true;

            if (PlayerPrefs.HasKey("Score"))
            {
                onScore.Invoke(PlayerPrefs.GetInt("Score").ToString());
            }

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
            SceneManager.LoadScene(0);
        }
        #endregion
    }
}
