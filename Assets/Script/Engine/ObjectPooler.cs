using UnityEngine;
using System.Collections.Generic;

namespace Engine
{
    public class ObjectPooler : MonoBehaviour
    {
        #region Properties
        [SerializeField]
        int m_PoolSize = 100;
        #endregion

        #region API
        public int PoolSize
        {
            get
            {
                return m_PoolSize;
            }
        }

        /// <summary>
        /// Instantiate all the objects we need to fill the pool.
        /// </summary>
        /// <param name="prefabs">The prefab to instantiate.</param>
        public List<GameObject> GeneratePool(GameObject[] prefabs, int poolSize)
        {
            m_PoolSize = poolSize;

            // GeneratePool should only be called once
            if (m_Available.Count + m_InUse.Count == m_PoolSize)
            {
                return null;
            }

            // Instantiate m_poolSize GameObjects
            for (int i = 0; i < m_PoolSize; i++)
            {
                Generate(prefabs[Random.Range(0, prefabs.Length)]);
            }

            return m_Available;
        }

        /// <summary>
        /// Return an instance of the prefab.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public GameObject GetObject(GameObject[] prefabs)
        {
            // Returns first available element of the pool
            // If there's no available object, create one
            lock (m_Available)
            {
                // Check if there are objects available
                if (m_Available.Count != 0)
                {
                    // Get first available element and place it in m_inUse
                    GameObject obj = m_Available[0];
                    m_InUse.Add(obj);
                    m_Available.RemoveAt(0);

                    return obj;
                }

                else
                {
                    // If there's no object available, generate one and add it 
                    // to the pool
                    GameObject obj = Generate(prefabs[Random.Range(0, prefabs.Length)]);
                    m_InUse.Add(obj);
                    m_Available.RemoveAt(0);
                    m_PoolSize++;
                    return obj;
                }
            }
        }

        /// <summary>
        /// Remove an object from inUse and put it in the available list.
        /// </summary>
        /// <param name="obj"></param>
        public void ReleaseObject(GameObject obj)
        {
            if (m_InUse.Contains(obj))
            {
                lock (m_Available)
                {
                    m_Available.Add(obj);
                    m_InUse.Remove(obj);
                }
            }

        }

        #endregion

        #region Private
        List<GameObject> m_Available = new List<GameObject>();
        List<GameObject> m_InUse = new List<GameObject>();

        private GameObject Generate(GameObject prefab)
        {
            GameObject obj = Instantiate(prefab);
            m_Available.Add(obj);
            return obj;
        }
        #endregion
    }
}
