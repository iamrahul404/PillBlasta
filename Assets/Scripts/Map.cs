﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Map {
  public Coordinate size;
  public Vector2 maxSize;
  [Range(0, 1)]
  public int seed;
  public float tileSeparatorWidth;
  public float tileSize;
  public Obstacle obstacleData;
  [Range(0, 1)]
  public float obstaclePercent;

  public Coordinate center {
    get {
      return new Coordinate(size.x / 2, size.y / 2);
    }
  }

  // This is an implementation of the Flood-fill 4 algorithm.
  public bool IsMapCompletelyAccessible(bool[,] obstacleMap, int obstacleCount) {
    bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
    Queue<Coordinate> queue = new Queue<Coordinate>();

    queue.Enqueue(center);
    mapFlags[center.x, center.y] = true;
    int accessibleTileCount = 0;

    while (queue.Count > 0) {
      Coordinate tile = queue.Dequeue();

      for (int x = -1; x < 2; x++) {
        for (int y = -1; y < 2; y++) {
          int neighborX = tile.x + x;
          int neighborY = tile.y + y;

          if (x == 0 || y == 0) {
            if (neighborX >= 0 && neighborX < obstacleMap.GetLength(0) &&
                neighborY >= 0 && neighborY < obstacleMap.GetLength(1)) {

              if (!mapFlags[neighborX, neighborY] && !obstacleMap[neighborX, neighborY]) {
                mapFlags[neighborX, neighborY] = true;
                queue.Enqueue(new Coordinate(neighborX, neighborY));
                accessibleTileCount += 1;
              }
            }
          }
        }
      }
    }

    int expectedAccessibleTileCount = (int)(size.x * size.y - obstacleCount);

    return expectedAccessibleTileCount == accessibleTileCount;
  }

  public Vector3 CoordinateToPosition(Coordinate coordinate) {
    return new Vector3(-size.x / 2.0f + 0.5f + coordinate.x, 0, -size.y / 2.0f + 0.5f + coordinate.y) * tileSize;
  }
}