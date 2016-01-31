using UnityEngine;

namespace Engine
{
    public class DeathSound : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        AudioSource m_AudioSource = null;
        #endregion

        #region API
        public void Play()
        {
            m_AudioSource.Play();
            Destroy(gameObject, 2f);
        }
        #endregion

        #region Unity
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            m_AudioSource = GetComponent<AudioSource>();
        }
        #endregion
    }
}
