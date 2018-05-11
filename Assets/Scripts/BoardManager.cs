using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int maximum;
        public int minimum;

        public Count(int min, int max)
        {
            this.minimum = min;
            this.maximum = max;
        }
    }

    public int rows = 8;
    public int cols = 8;

    // 随机生成内墙和食物的数量
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    // 退出对象
    public GameObject exit;

    // 多个游戏对象使用数组进行管理
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] floorTiles;
    public GameObject[] outerwallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    /// <summary>
    /// 初始化内墙和迷宫的位置
    /// </summary>
    void InitList()
    {
        for (int i = 1; i < cols - 1; i++)
            for (int j = 1; j < rows - 1; j++)
                gridPositions.Add(new Vector3(i, j, 0f));
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        // 初始化外墙和地板
        for (int i = -1; i < cols + 1; i++)
            for (int j = -1; j < rows + 1; j++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (i == -1 || j == -1 || i == cols || j == rows)
                    toInstantiate = outerwallTiles[Random.Range(0, outerwallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
    }

    // 生成用于摆放物体的随机位置
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    //
    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

	// 进入新关卡的时候供 GameManager 调用
	public void SetupScene (int level)
    {
        BoardSetup();
        InitList();

        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        Instantiate(exit, new Vector3(cols-1, rows-1, 0f), Quaternion.identity);
	}
	
}
