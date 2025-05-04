using UnityEngine;

    public static class Chunk
    {
        public const int chunkSize = 16;

        public static Vector2Int GetChunkCoord(Vector3 worldPos)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPos.x / chunkSize),
                Mathf.FloorToInt(worldPos.y / chunkSize)
            );
        }

        public static Vector3 GetWorldPosition(Vector2Int chunkCoord)
        {
            return new Vector3(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, 0);
        }

        public static Vector2 GetLocalPos(Vector3 worldPos, Vector2Int chunkCoord)
        {
            return new Vector2(
                worldPos.x - chunkCoord.x * chunkSize,
                worldPos.y - chunkCoord.y * chunkSize
            );
        }
    }

