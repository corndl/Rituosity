using Engine;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace Procedural
{
    public class CityGenerator : MonoBehaviour
    {
        #region Properties
        [Header("Blocks")]
        [SerializeField]
        ObjectPooler m_BlockPool = null;
        [SerializeField]
        GameObject[] m_BlockPrefabs = new GameObject[0];
        [SerializeField]
        int m_GridSize = 5;
        [SerializeField]
        float m_BlockSize = 50f;
        [SerializeField]
        GameObject m_CityParent = null;

        [Header("Mobs")]
        [SerializeField]
        ObjectPooler m_MobPool = null;
        [SerializeField]
        GameObject[] m_MobPrefabs = new GameObject[0];

        [Header("Player")]
        [SerializeField]
        Motor m_Player = null;
        #endregion

        #region Unity
        void Awake()
        {
            m_Player = GameObject.FindObjectOfType<Motor>();

            Assert.IsNotNull(m_BlockPool, "Block Pool is null");
            Assert.IsNotNull(m_MobPool, "Mob Pool is null");
            Assert.IsNotNull(m_Player, "Player is null");

            GenerateCity();
        }

        void Update()
        {
            // foreach block, if > distance max, release
            int playerX = (int)m_Player.transform.position.x % (int)m_BlockSize;
            int playerZ = (int)m_Player.transform.position.z % (int)m_BlockSize;
        }
        #endregion

        #region Private
        GameObject[,] m_Tiles;

        void GenerateCity()
        {
            List<GameObject> blocks = m_BlockPool.GeneratePool(m_BlockPrefabs, (int)Mathf.Pow(m_GridSize, 2));
            m_Tiles = new GameObject[m_GridSize, m_GridSize];
            int randRotation = 0;
            GameObject block;
            
            // Tile
            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    block = m_BlockPool.GetObject(m_BlockPrefabs);
                    // Random block rotation
                    randRotation = Random.Range(0, 4);
                    block.transform.rotation = Quaternion.Euler(0, randRotation * 90, 0);

                    m_Tiles[i, j] = block;
                    m_Tiles[i, j].transform.SetParent(m_CityParent.transform, false);
                    m_Tiles[i, j].transform.position = new Vector3(i * m_BlockSize, 0, j * m_BlockSize);
                }
            }
        }
        #endregion
    }
}
