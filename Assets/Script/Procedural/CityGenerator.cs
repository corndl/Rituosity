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
            playerTile = GetPlayerTile();
            newPlayerTile = GetPlayerTile();
            m_HalfCitySize = (int)Mathf.Ceil(m_GridSize / 2);

            Assert.IsNotNull(m_BlockPool, "Block Pool is null");
            Assert.IsNotNull(m_MobPool, "Mob Pool is null");
            Assert.IsNotNull(m_Player, "Player is null");

            GenerateCity();
        }

        void Update()
        {
            MoveBlocks();
        }
        #endregion

        #region Private
        Dictionary<Vector2, GameObject> m_Tiles = new Dictionary<Vector2, GameObject>();
        Vector2 playerTile;
        Vector2 newPlayerTile;
        int m_HalfCitySize;

        void GenerateCity()
        {
            List<GameObject> blocks = m_BlockPool.GeneratePool(m_BlockPrefabs, (int)Mathf.Pow(m_GridSize, 2));
            int randRotation = 0;
            int posX;
            int posZ;
            Vector2 coordinates;
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

                    posX = (int)(i - Mathf.Ceil(m_GridSize / 2));
                    posZ = (int)(j - Mathf.Ceil(m_GridSize / 2));
                    coordinates = new Vector2(posX, posZ);

                    m_Tiles[coordinates] = block;

                    block.transform.SetParent(m_CityParent.transform, false);
                    block.transform.position = new Vector3(posX * m_BlockSize, 0, posZ * m_BlockSize);
                }
            }
        }

        void MoveBlocks()
        {
            playerTile = newPlayerTile;
            newPlayerTile = GetPlayerTile();

            if (playerTile == newPlayerTile)
            {
                return;
            }

            // If the player has changed tile
            int direction = 1;
            Vector3 newPosition;

            if (newPlayerTile.x != playerTile.x)
            {
                direction = (newPlayerTile.x > playerTile.x) ? 1 : -1;

                int oldRowX;
                int newRowX;
                int Z;
                int minRow = GetMinRow();
                int maxRow = GetMaxRow();
                oldRowX = (direction == 1) ? minRow : maxRow;
                newRowX = (direction == 1) ? oldRowX + m_GridSize : oldRowX - m_GridSize;

                for (int i = -m_HalfCitySize; i <= m_HalfCitySize; i++)
                { 
                    Z = (int)playerTile.y + i;

                    m_Tiles[new Vector2(newRowX, Z)] = m_Tiles[new Vector2(oldRowX, Z)];
                    m_Tiles.Remove(new Vector2(oldRowX, Z));

                    newPosition = new Vector3(newRowX * m_BlockSize, 0, Z * m_BlockSize);
                    m_Tiles[new Vector2(newRowX, Z)].transform.position = newPosition;
                }
            }

            if (newPlayerTile.y != playerTile.y)
            {
                direction = (newPlayerTile.y > playerTile.y) ? 1 : -1;

                int oldColumnZ;
                int newColumnZ;
                int X;
                int minCol = GetMinColumn();
                int maxCol = GetMaxColumn();
                oldColumnZ = (direction == 1) ? minCol : maxCol;
                newColumnZ = (direction == 1) ? oldColumnZ + m_GridSize : oldColumnZ - m_GridSize;

                for (int i = -m_HalfCitySize; i <= m_HalfCitySize; i++)
                {
                    X = (int)playerTile.x + i;

                    m_Tiles[new Vector2(X, newColumnZ)] = m_Tiles[new Vector2(X, oldColumnZ)];                    
                    m_Tiles.Remove(new Vector2(X, oldColumnZ));

                    newPosition = new Vector3(X * m_BlockSize, 0, newColumnZ * m_BlockSize);
                    m_Tiles[new Vector2(X, newColumnZ)].transform.position = newPosition;
                }
            }
        }

        #region Tiles
        Vector2 GetPlayerTile()
        {
            return new Vector2
            (
                (int)m_Player.transform.position.x / (int)m_BlockSize,
                (int)m_Player.transform.position.z / (int)m_BlockSize
            );
        }

        int GetMaxColumn()
        {
            Vector2 pos = GetPlayerTile();
            while (m_Tiles.ContainsKey(pos))
            {
                pos.y++;
            }
            return (int)pos.y - 1;
        }

        int GetMinColumn()
        {
            Vector2 pos = GetPlayerTile();
            while (m_Tiles.ContainsKey(pos))
            {
                pos.y--;
            }
            return (int)pos.y + 1;
        }

        int GetMaxRow()
        {
            Vector2 pos = GetPlayerTile();
            while (m_Tiles.ContainsKey(pos))
            {
                pos.x++;
            }
            return (int)pos.x - 1;
        }

        int GetMinRow()
        {
            Vector2 pos = GetPlayerTile();
            while (m_Tiles.ContainsKey(pos))
            {
                pos.x--;
            }
            return (int)pos.x + 1;
        }
        #endregion
        #endregion
    }
}