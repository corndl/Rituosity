using UnityEngine;

namespace Engine
{
    public class ScoreManager : MonoBehaviour
    {
        #region Properties
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
        #endregion

        #region API
        public void KillTarget()
        {
            Score++;
        }
        #endregion

        #region Private
        int m_Score = 0;
        #endregion
    }
}
