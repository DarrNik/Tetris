using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 PointRotate;
    private float PrevTime;
    public float FallTime = 0.8f;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] Grid = new Transform[width, height];
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!Move())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!Move())
                transform.position -= new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(PointRotate), new Vector3(0, 0, 1), 90);
            if (!Move())
                transform.RotateAround(transform.TransformPoint(PointRotate), new Vector3(0, 0, 1), -90);

        }


        if (Time.time - PrevTime > ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) ? FallTime / 10 : FallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!Move())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddGrid();
                CheckLines();
                this.enabled = false;
                FindObjectOfType<Begin>().NewTetromino();
            }
            PrevTime = Time.time;
        }
    }

    void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (LineIs(i))
            {
                DeleteLine(i);
                LineDown(i);
            }
        }
    }

    bool LineIs(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (Grid[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(Grid[j, i].gameObject);
            Grid[j, i] = null;
        }
    }

    void LineDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (Grid[j, y] != null)
                {
                    Grid[j, y - 1] = Grid[j, y];
                    Grid[j, y] = null;
                    Grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddGrid()
    {
        foreach (Transform children in transform)
        {
            int RoundedX = Mathf.RoundToInt(children.transform.position.x);
            int RoundedY = Mathf.RoundToInt(children.transform.position.y);

            Grid[RoundedX, RoundedY] = children;
        }
    }

    bool Move()
    {
        foreach (Transform children in transform)
        {
            int RoundedX = Mathf.RoundToInt(children.transform.position.x);
            int RoundedY = Mathf.RoundToInt(children.transform.position.y);

            if (RoundedX < 0 || RoundedX >= width || RoundedY < 0 || RoundedY >= height)
            {
                return false;
            }

            if (Grid[RoundedX, RoundedY] != null)
            {
                return false;
            }
        }

        return true;
    }
}
