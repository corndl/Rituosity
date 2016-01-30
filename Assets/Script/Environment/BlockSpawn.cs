using UnityEngine;

namespace Environment
{
    public class BlockSpawn : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        Vector3[] m_SpawnPoints;
        #endregion

        #region API
        public Vector3[] SpawnPoints
        {
            get
            {
                return m_SpawnPoints;
            }
        }
        #endregion
    }
}
