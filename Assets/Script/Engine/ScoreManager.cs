using UnityEngine;
using System.Collections;

namespace Engine
{
    public class ScoreManager : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        int m_TimePerTarget = 15;

        [SerializeField]
        Motor m_Player = null;

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
                onScoreChanged.Invoke(value.ToString());
            }
        }
        #endregion

        #region Events
        public StringEvent onScoreChanged = new StringEvent();
        public StringEvent onTimer = new StringEvent();
        public UnityEngine.Events.UnityEvent onGameOver = new UnityEngine.Events.UnityEvent();
        #endregion

        #region API
        public void KillTarget()
        {
            Score++;
            StopCoroutine(Timer());
            StartCoroutine(Timer());
            m_Player.Speed *= 1.1f;
        }
        #endregion

        #region Unity
        void Start()
        {
            m_Player = GameObject.FindObjectOfType<Motor>();
            StartCoroutine(Timer());
        }
        #endregion

        #region Routines
        IEnumerator Timer()
        {
            int timeLeft = m_TimePerTarget;

            while (timeLeft >= 0)
            {
                onTimer.Invoke(timeLeft.ToString());
                timeLeft--;

                yield return new WaitForSeconds(1f);
            }

            onGameOver.Invoke();
        }
        #endregion

        #region Private
        int m_Score = 0;
        #endregion
    }
}
