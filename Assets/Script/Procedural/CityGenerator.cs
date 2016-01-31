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
        [SerializeField]
        int m_MobPoolSize = 50;

        [Header("Targets")]
        [SerializeField]
        GameObject m_TargetPrefab = null;
        [SerializeField]
        int m_NbTargets = 10;
        [SerializeField]
        SelectTarget m_SelectTarget = null;

        [Header("Player")]
        [SerializeField]
        Motor m_Player = null;

        [Header("Score")]
        [SerializeField]
        ScoreManager m_ScoreManager = null;
        #endregion

        #region Unity
        void Awake()
        {
            m_Player = GameObject.FindObjectOfType<Motor>();
            playerTile = GetPlayerTile();
            newPlayerTile = GetPlayerTile();
            m_HalfCitySize = (int)Mathf.Ceil(m_GridSize / 2);

            m_ScoreManager = GameObject.FindObjectOfType<ScoreManager>();
            m_SelectTarget = GameObject.FindObjectOfType<SelectTarget>();

            Assert.IsNotNull(m_BlockPool, "Block Pool is null");
            Assert.IsNotNull(m_MobPool, "Mob Pool is null");
            Assert.IsNotNull(m_Player, "Player is null");

            GenerateCity();
            GenerateMobs();
        }

        void Update()
        {
            MoveBlocks();
            GenerateTargets();
        }
        #endregion

        #region Handlers
        void OnTargetDestroyed(Characters.Target target)
        {
            m_Targets.Remove(target);
            Destroy(target.gameObject);
            m_ScoreManager.KillTarget();
        }

        void OnTargetTooFar(Characters.Target target)
        {
            m_Targets.Remove(target);
            Destroy(target.gameObject);
        }
        #endregion

        #region API
        public List<Characters.Target> Targets
        {
            get
            {
                return m_Targets;
            }
        }
        #endregion

        #region Private
        Dictionary<Vector2, GameObject> m_Tiles = new Dictionary<Vector2, GameObject>();
        Vector2 playerTile;
        Vector2 newPlayerTile;
        int m_HalfCitySize;
        List<Characters.Target> m_Targets = new List<Characters.Target>();

        #region City Blocks
        /// <summary>
        /// Called on init. Generate city blocks pool, randomize their rotations
        /// and place them in a tile grid.
        /// </summary>
        void GenerateCity()
        {
            int randRotation = 0;
            int posX;
            int posZ;
            Vector2 coordinates;
            GameObject block;
            GameObject initialBlock = null;

            // Instantiate blocks
            m_BlockPool.GeneratePool(m_BlockPrefabs, (int)Mathf.Pow(m_GridSize, 2));
            
            // Assign all blocks to a tile in the grid
            for (int i = 0; i < m_GridSize; i++)
            {
                for (int j = 0; j < m_GridSize; j++)
                {
                    block = m_BlockPool.GetObject(m_BlockPrefabs);
                    if (i == 0 && j == 0)
                    {
                        initialBlock = block;
                    }        

                    // Random block rotation to add diversity to the map
                    randRotation = Random.Range(0, 4);
                    block.transform.rotation = Quaternion.Euler(0, randRotation * 90, 0);

                    // Get tile coordinates
                    posX = (int)(i - Mathf.Ceil(m_GridSize / 2));
                    posZ = (int)(j - Mathf.Ceil(m_GridSize / 2));
                    coordinates = new Vector2(posX, posZ);

                    m_Tiles[coordinates] = block;

                    // Set block as a child of City and place it correctly
                    block.transform.SetParent(m_CityParent.transform, false);
                    block.transform.position = new Vector3(posX * m_BlockSize, 0, posZ * m_BlockSize);
                }
            }

            GameObject centerBlock = m_Tiles[Vector2.zero];
            centerBlock.transform.position = initialBlock.transform.position;

            m_Tiles[Vector2.zero] = initialBlock;
            initialBlock.transform.position = Vector3.zero;

            posX = (int)(- Mathf.Ceil(m_GridSize / 2));
            posZ = (int)(- Mathf.Ceil(m_GridSize / 2));

            m_Tiles[new Vector2(posX, posZ)] = centerBlock;
        }

        /// <summary>
        /// Called in Update. Moves rows and columns of blocks as the player
        /// changes tile, so that the player is always at the center of the
        /// grid.
        /// </summary>
        void MoveBlocks()
        {
            // Check if player changed tile
            playerTile = newPlayerTile;
            newPlayerTile = GetPlayerTile();

            if (playerTile == newPlayerTile)
            {
                return;
            }

            // If the player has changed tile
            int direction = 1;
            Vector3 newPosition;

            // If player changed tile on the x axis...
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

                // Take the farthest row in their back and set it as the farthest
                // in front of the player
                for (int i = -m_HalfCitySize; i <= m_HalfCitySize; i++)
                { 
                    Z = (int)playerTile.y + i;

                    m_Tiles[new Vector2(newRowX, Z)] = m_Tiles[new Vector2(oldRowX, Z)];
                    m_Tiles.Remove(new Vector2(oldRowX, Z));

                    newPosition = new Vector3(newRowX * m_BlockSize, 0, Z * m_BlockSize);
                    m_Tiles[new Vector2(newRowX, Z)].transform.position = newPosition;
                }
            }

            // If player changed tile on the y axis...
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

                // Take the farthest column in their back and set it as the farthest
                // in front of the player
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
        #endregion

        #region Mobs
        void GenerateMobs()
        {
            List<GameObject> mobs = m_MobPool.GeneratePool(m_MobPrefabs, m_MobPoolSize);

            Vector2 tile;
            foreach (GameObject mob in mobs)
            {
                tile = new Vector2(Random.Range(-m_HalfCitySize, m_HalfCitySize + 1), Random.Range(-m_HalfCitySize, m_HalfCitySize + 1));
                mob.transform.SetParent(m_Tiles[tile].transform, false);
                
                Environment.BlockSpawn spawn = m_Tiles[tile].GetComponent<Environment.BlockSpawn>();
                if (spawn != null) 
                {
                    mob.transform.position += spawn.SpawnPoints[Random.Range(0, spawn.SpawnPoints.Length)];
                }
            }
        }

        void GenerateTargets()
        {
            if (m_Targets.Count == m_NbTargets)
            {
                return;
            }

            while (m_Targets.Count < m_NbTargets)
            {
                SpawnTarget();
            }
        }

        void SpawnTarget()
        {
            Vector2 playerTile = GetPlayerTile();
            Vector2 targetTile = playerTile;
            int rand = (int)Random.Range(0, 8);

            switch (rand)
            {
                case 1:
                    targetTile = new Vector2(playerTile.x + 1, playerTile.y);
                    break;
                case 2:
                    targetTile = new Vector2(playerTile.x + 1, playerTile.y + 1);
                    break;
                case 3:
                    targetTile = new Vector2(playerTile.x + 1, playerTile.y - 1);
                    break;
                case 4:
                    targetTile = new Vector2(playerTile.x - 1, playerTile.y);
                    break;
                case 5:
                    targetTile = new Vector2(playerTile.x - 1, playerTile.y + 1);
                    break;
                case 6:
                    targetTile = new Vector2(playerTile.x - 1, playerTile.y - 1);
                    break;
                case 7:
                    targetTile = new Vector2(playerTile.x, playerTile.y + 1);
                    break;
                case 8:
                    targetTile = new Vector2(playerTile.x, playerTile.y - 1);
                    break;
            }

            GameObject target = Instantiate(m_TargetPrefab);
            target.transform.SetParent(m_Tiles[targetTile].transform, false);

            Environment.BlockSpawn spawn = m_Tiles[targetTile].GetComponent<Environment.BlockSpawn>();
            if (spawn != null)
            {
                target.transform.position += spawn.SpawnPoints[Random.Range(0, spawn.SpawnPoints.Length)];
            }

            Characters.Target targ = target.GetComponent<Characters.Target>();
            m_Targets.Add(targ);
            targ.onDestroyed.AddListener(OnTargetDestroyed);
            targ.onTooFar.AddListener(OnTargetTooFar);
        }
        #endregion

        #region Tile helper functions
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