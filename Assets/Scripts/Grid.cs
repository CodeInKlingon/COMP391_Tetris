using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Grid : MonoBehaviour {

    public List<GameObject> pieces;
    int nextPiece;
    GameObject nextBlockParent;

    bool isGameOver = false;

    Transform[,] blocks = new Transform[10, 24];

    public int score;
    int lines;

    GameObject linesGO;
    GameObject scoreGO;

    public void GameOver() {

        isGameOver = true;
        GameObject.Find("GameOver").GetComponent<Canvas>().enabled = true;

    }

    public LayerMask blocklayer;
    // Use this for initialization

    public bool checkGrid(List<Vector2> coords)
    {
        bool result = false;
        Vector2 inValidPosition;
        foreach (Vector2 coord in coords)
        {
            int x = Convert.ToInt32(coord.x - 0.5f);
            int y = Convert.ToInt32(coord.y - 0.5f);
            if (coord.y < 0 || coord.x > 10 || coord.x < 0)
            {
                result = true;            }
            else
            {
                
                if (blocks[x, y] == null)
                {
                    //result = false;
                }
                else
                {
                    result = true;
                    inValidPosition = new Vector2(x,y);
                }
            }
        }
        return result;

    }

    void updateUI()
    {
        linesGO.GetComponent<Text>().text = lines.ToString();
        scoreGO.GetComponent<Text>().text = score.ToString();
    }

    // Use this for initialization
    void Start () {
        score = 0;
        lines = 0;
        linesGO = GameObject.Find("Lines");
        scoreGO = GameObject.Find("Score");
        nextBlockParent = GameObject.Find("NextBlockP");
        NextBlock();
        DropBlock();
        print(pieces.Count);
	}

    void NextBlock() {

        //delete children if there are any
        foreach (Transform child in nextBlockParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        nextPiece = UnityEngine.Random.Range(0, 7);
        GameObject newPreveiw = Instantiate(pieces[nextPiece],nextBlockParent.transform);
        newPreveiw.GetComponent<block>().enabled = false;
        newPreveiw.transform.position = nextBlockParent.transform.position;
        //newPreveiw.transform.parent = nextBlockParent.transform;
    }

    public void DropBlock() {
        if (!isGameOver)
        {
            updateUI();
            Instantiate(pieces[nextPiece]);
            NextBlock();
        }
    }

    public void PlaceBlock(int x, int y, Transform blockTransform)
    {
        blocks[x, y] = blockTransform;
    }

    public void checkRow()
    {
        List<int> fullRows = new List<int>();
        int maxRow = 0;
        for (int y = 0; y < 24; y++)
        {
            bool isRowFull = true;
            for (int x = 0; x < 10; x++)
            {
                if (blocks[x, y] == null)
                {
                    isRowFull = false;
                    break;
                }
            }
            if (isRowFull)
            {
                if (y > maxRow)
                {
                    maxRow = y;
                }
                fullRows.Add(y);
            }
        }

        if (fullRows.Count > 0)
        {
            lines += fullRows.Count;
            int baseScore = 100;
            score += baseScore * (2 * fullRows.Count); 
            clearRows(fullRows);
            //UpdateList(fullRows, maxRow);
            /*
            DeleteRows(fullRows);
            MoveRows(fullRows, maxRow);*/
            //UpdateList(fullRows, maxRow);
        }
    }

    void clearRows(List<int> fullRows) {
        int dropDistance = 0; // distance to move blocks
        for (int y = 0; y < 24; y++) {
            if (fullRows.Contains(y))
            {
                //delete rows
                for (int x = 0; x < 10; x++)
                {
                    Destroy(blocks[x, y].gameObject);
                }
                dropDistance++;
            }
            else {
                for (int x = 0; x < 10; x++)
                {
                    if (blocks[x, y] != null)
                    {
                        if (dropDistance > 0)
                        {
                            //move actual blocks world position
                            blocks[x, y].position = new Vector3(blocks[x, y].position.x, blocks[x, y].position.y - dropDistance, blocks[x, y].position.z);

                            //move blocks array position
                            blocks[x, y - dropDistance] = blocks[x, y];
                            blocks[x, y] = null;
                        }
                    }
                }
            }
        }
    }


    void UpdateList(List<int> rows, int maxRow)
    {
        Transform[,] newBlocks = blocks;
        for (int y = maxRow + 1; y < 24; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                newBlocks[x, y - rows.Count] = blocks[x, y];
            }
        }
        blocks = newBlocks;

    }

    void MoveRows(List<int> deletedRows, int maxRow)
    {
        for (int y = maxRow; y < 24; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (blocks[x, y] != null)
                {
                    blocks[x, y].position = new Vector3(blocks[x, y].position.x, blocks[x, y].position.y - deletedRows.Count, blocks[x, y].position.z);
                }
            }
        }
    }

    void DeleteRows(List<int> rows)
    {
        foreach (int y in rows)
        {
            for (int x = 0; x < 10; x++)
            {
                Destroy(blocks[x, y].gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("space"))
        {

            string output = "";
            for (int y = 0; y < 24; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    output += blocks[x, y] == null ? "o" : "x";
                }
                output += "\n";
            }
            print(output);
        }

    }
}
