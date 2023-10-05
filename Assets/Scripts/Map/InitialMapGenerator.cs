using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class InitialMapGenerator : GameSystem
{
    
    private const float c_spawnAnimationSpeed = 25;
    [SerializeField, Tooltip("Would be sorted on initialization by height")] RoadData[] _heightDependentRoadVariants;
    [Header("General road cycle settings")]
    [SerializeField] private int _roadCircleRadiusInPercents = 40;
    [SerializeField] private int _protoQuadsCount = 4;

    [Header("Perlin settings"), SerializeField] private float _perlinSize = 1;
    
    public override bool AsyncInitialization => true;
    public override void Initialize(System.Action initializationEndedCallback)
    {
        Map _map = SystemsManager.GetSystemOfType<Map>();
        Randomness randomness = SystemsManager.GetSystemOfType<Randomness>();

        _heightDependentRoadVariants = _heightDependentRoadVariants.OrderBy(obj => obj.MaxHeight).ToArray();
        for (int i = 0; i < _heightDependentRoadVariants.Length; i++)
        {
            _heightDependentRoadVariants[i].InitRandomness(randomness.Seed + i);
            if(i == _heightDependentRoadVariants.Length - 1)
            {
                //ensure that for every height road variant would exist
                _heightDependentRoadVariants[_heightDependentRoadVariants.Length - 1].MaxHeight = 1;
            }
        }

        List<Vector3Int> roadLoop = new MapGenerator().GenerateRoadLoop(randomness.Random, _map.Size, _roadCircleRadiusInPercents, _protoQuadsCount);

        //spawn road along longest loop
        var road = new List<Road>();
        float delay;
        int longestLoopLength = roadLoop.Count;
        for (int i = 0; i < longestLoopLength; i++)
        {
            var node = _map[roadLoop[i].x, roadLoop[i].z];
            int id = i;
            var roadTile = SpawnRoad(node, randomness);
            _map.AddLocation(roadTile);
            node.OnLocationUpdated += (newLocation) => _map.UpdateRoad(id, newLocation as Road);
            delay = (float)road.Count / c_spawnAnimationSpeed;

            StartCoroutine(DoWithDelay(() => {
                roadTile.gameObject.SetActive(true);
                roadTile.ShowSpawnAnimation();
                node.SetSelfGraphicsActive(false);
            },
            delay));
            road.Add(roadTile);
        }

        _map.Road = road;

        //end initialization after animation of last road tiles ends
        StartCoroutine(DoWithDelay(initializationEndedCallback, road.Count / c_spawnAnimationSpeed));
    }

    [ContextMenu("Draw perlin")]
    public void DrawPerlinOnMap()
    {
        Randomness randomness = SystemsManager.GetSystemOfType<Randomness>();
        Map _map = SystemsManager.GetSystemOfType<Map>();
        for (int x = 0; x < _map.Size; x++)
        {
            for (int y = 0; y < _map.Size; y++)
            {
                _map[x, y].Colorize(Color.HSVToRGB(0, 0, GetHeightForNodeAtPosition(x,y, randomness)));
            }
        }
    }
    private float GetHeightForNodeAtPosition(int x, int y, Randomness randomness)
    {
        float offset = randomness.Seed / int.MaxValue;
        return Mathf.PerlinNoise((x + offset) / _perlinSize, (y+offset) / _perlinSize);
    }

    public class MapGenerator
    {
        public List<Vector3Int> GenerateRoadLoop(System.Random random, int mapSize = 25, int roadLoopRadiusInPercentage = 40, int protoQuadsCount = 4)
        {

            int middle = Mathf.RoundToInt((float)mapSize / 2);
            int circleRadius = mapSize * roadLoopRadiusInPercentage / 100;
            int quadCentersRadius = Mathf.RoundToInt((float)circleRadius / 2);

            //fill mask with intersecting rectangles
            int[,] mask = new int[mapSize, mapSize];
            for (int i = 0; i < protoQuadsCount; i++)
            {
                var quadSize = new Vector2Int(random.Next(3, circleRadius), random.Next(3, circleRadius));
                var center = new Vector2Int(middle + random.Next(-quadCentersRadius, quadCentersRadius), middle + random.Next(-quadCentersRadius, quadCentersRadius));

                for (int x = center.x - quadSize.x; x < center.x + quadSize.x; x++)
                {
                    for (int y = center.y - quadSize.y; y < center.y + quadSize.y; y++)
                    {
                        mask[x, y]++;
                    }
                }
            }

            //get positions from mask on borders
            HashSet<Vector3Int> uncheckedRoadCoords = new HashSet<Vector3Int>();
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    if (mask[x, y] >= 1 && HasAnEmptyNeighbous(x, y, mask))
                    {
                        uncheckedRoadCoords.Add(new Vector3Int(x,0,y));
                    }
                }
            }

            //collect loops
            HashSet<Vector3Int> checkedRoadCoords = new HashSet<Vector3Int>();
            Vector3Int currentPos = uncheckedRoadCoords.ElementAt(0);
            bool nextTileFound = false;
            List<List<Vector3Int>> loops = new List<List<Vector3Int>>() { new List<Vector3Int>() };
            while (uncheckedRoadCoords.Count > 0)
            {
                uncheckedRoadCoords.Remove(currentPos);
                checkedRoadCoords.Add(currentPos);

                loops[0].Add(currentPos);

                var strightNeighbourCoords = GetStrightNeighbourCoords(currentPos.x, currentPos.z);
                nextTileFound = false;
                foreach (var neighbour in strightNeighbourCoords)
                {
                    if (uncheckedRoadCoords.Contains(neighbour))
                    {
                        currentPos = neighbour;
                        nextTileFound = true;
                        break;
                    }
                }
                if (!nextTileFound && uncheckedRoadCoords.Count > 0)
                {
                    currentPos = uncheckedRoadCoords.ElementAt(0);
                    loops.Insert(0, new List<Vector3Int>());
                }
            }

            //get longest loop from collected previously loops
            int longestLoop = 0;
            for (int i = 0; i < loops.Count; i++)
            {
                if (loops[i].Count > loops[longestLoop].Count)
                    longestLoop = i;
            }

            return loops[longestLoop];
        }

        private bool HasAnEmptyNeighbous(int x, int y, int[,] mask)
        {
            bool front = y < mask.GetLength(1) - 2;
            bool right = x < mask.GetLength(0) - 2;
            bool back = y > 1;
            bool left = x > 1;

            if (right && mask[x + 1, y] == 0) return true;
            if (left && mask[x - 1, y] == 0) return true;

            if (front && mask[x, y + 1] == 0) return true;
            if (back && mask[x, y - 1] == 0) return true;

            if (front && right && mask[x + 1, y + 1] == 0) return true;
            if (front && left && mask[x - 1, y + 1] == 0) return true;

            if (back && right && mask[x + 1, y - 1] == 0) return true;
            if (back && left && mask[x - 1, y - 1] == 0) return true;

            return false;
        }
        private Vector3Int[] GetStrightNeighbourCoords(int x, int z)
        {
            Vector3Int[] neighbourCoords = new Vector3Int[4]
            {
            new Vector3Int(x+1,0,z),
            new Vector3Int(x-1, 0, z),
            new Vector3Int(x, 0, z+1),
            new Vector3Int(x, 0, z-1)
            };

            return neighbourCoords;
        }
    }
    private Road SpawnRoad(Node node, Randomness randomness)
    {
        //select the apropriate variant based on ground height
        float height = GetHeightForNodeAtPosition(node.GridPosition.x, node.GridPosition.z, randomness);
        int selectedRoadVariant = -1;
        for (int i = 0; i < _heightDependentRoadVariants.Length; i++)
        {
            if (_heightDependentRoadVariants[i].MaxHeight <= height) continue;
            else
            {
                selectedRoadVariant = i;
                break;
            }
        }
        if(selectedRoadVariant < 0) selectedRoadVariant = _heightDependentRoadVariants.Length - 1;
        var r = _heightDependentRoadVariants[selectedRoadVariant].RoadPrefab;
        //spawn road tile
        var road = Instantiate(r, node.GridPosition, Quaternion.identity, node.transform);
        road.gameObject.SetActive(false);
        node.ReplaceLocation(road, false);
        return road;
    }
    IEnumerator DoWithDelay(System.Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    [System.Serializable]
    struct RoadData
    {
        [Range(0,1)] public float MaxHeight;
        [SerializeField] LootTable<Road> _roadPrefabVariants;
        public void InitRandomness(int seed)
        {
            _roadPrefabVariants.ProvideSpecificRandomnessSeed(seed);
        }
        public Road RoadPrefab
        {
            get
            {
                if(_roadPrefabVariants.GetLoot(out Road road) == LootTable<Road>.LootRollResult.DroppedLessThanRequested)
                {
                    road = _roadPrefabVariants.GetFirst();
                }
                return road;
            }
        }
    }
}
