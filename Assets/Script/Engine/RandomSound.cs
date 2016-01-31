using UnityEngine;

namespace Engine
{
    public class RandomSound : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        AudioSource[] m_AudioSources = new AudioSource[0];
        #endregion

        #region Unity
        void Awake()
        {
            m_AudioSources = gameObject.GetComponentsInChildren<AudioSource>();
        }
        #endregion

        #region API
        public void PlayRandom()
        {
            m_AudioSources[(int)Random.Range(0, m_AudioSources.Length)].Play();
        }
        #endregion
    }
}
