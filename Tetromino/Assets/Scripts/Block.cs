using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] Vector3 pointRotate;
    [SerializeField] float fallTime = 0.8f;
    [SerializeField] static int height = 20;
    [SerializeField] static int width = 10;

    private float prevTime;
    private static Transform[,] grid = new Transform[width, height];
    private static int score = 0;
    private static int speed = 1;
    private static int lines = 0;
    private static int k=1;
    private static int countRows = 0;

    //определение информации
    void Start()
    {
        FindObjectOfType<Info>().TextForLines(lines);
        FindObjectOfType<Info>().TextForScore(score);
        FindObjectOfType<Info>().TextForSpeed(speed);
    }

    // Нажатие на клавиши 
    void Update()
    {
        //движение влево
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!Move())
                transform.position -= new Vector3(-1, 0, 0);
        }
        //движение вправо
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!Move())
                transform.position -= new Vector3(1, 0, 0);
        }
        //вращение
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(pointRotate), new Vector3(0, 0, 1), 90);
            if (!Move())
                transform.RotateAround(transform.TransformPoint(pointRotate), new Vector3(0, 0, 1), -90);

        }
        //увеличение скорости
        else if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (speed < 10)
            {
                speed++;
                FindObjectOfType<Info>().TextForSpeed(speed);
            }
        }
        //уменьшение скорости
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (speed > 1)
            {
                speed--;
                FindObjectOfType<Info>().TextForSpeed(speed);
            }
        }

        //выход
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        //движение вниз
        if (Time.time - prevTime > ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) ? fallTime / (10*speed) : fallTime/speed))
        {
            transform.position += new Vector3(0, -1, 0);
            countRows++;
            if (!Move())
            {
                transform.position -= new Vector3(0, -1, 0);
                if (countRows<2)
                {
                    this.enabled = false;
                    GameOver();
                }
                else
                {
                    AddGrid();
                    CheckLines();
                    this.enabled = false;
                    FindObjectOfType<Begin>().NewTetromino();
                }
                countRows = 0;
            }
            prevTime = Time.time;
        }
    }

    //перезапуск игры
    void GameOver()
    {
        System.Threading.Thread.Sleep(3000);
        
        speed = 1;
        score = 0;
        lines = 0;
        countRows = 0;
        FindObjectOfType<Begin>().Restart();
        for (int h = 0; h < height; h++)
        {
            DeleteLine(h);
        }
        FindObjectOfType<Begin>().NewTetromino();
    }

    //проверка линии
    void CheckLines()
    {
        int ln = 0;
        for (int i = height - 1; i >= 0; i--)
        {
            if (LineIs(i))
            {
                DeleteLine(i);
                LineDown(i);
                ln++;
            }
        }
        if (ln!=0)
        {
            lines = ln;
            FindObjectOfType<Info>().TextForLines(lines);
            CheckScore(lines);
        }
    }

    //счет игры
    void CheckScore(int l)
    {
        switch (l)
        {
            case 1:
                score += 100;
                break;
            case 2:
                score += 300;
                break;
            case 3:
                score += 500;
                break;
            case 4:
                score += 800;
                break;
        }
        //увеличение скорости от времени (счета)
        if (score >= 700*k)
        {
            if (speed<10)
            {
                speed++;
                k++;
            }
        }
        FindObjectOfType<Info>().TextForScore(score);
    }

    //проверка на наличие линии
    bool LineIs(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }
        return true;
    }

    //удаление линии
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] != null)
            {
                Destroy(grid[j, i].gameObject);
            }
            grid[j, i] = null;
        }
    }

    //смещение после удаления линии
    void LineDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    //сохранение позиции тетромино
    void AddGrid()
    {
        foreach (Transform children in transform)
        {
            int RoundedX = Mathf.RoundToInt(children.transform.position.x);
            int RoundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[RoundedX, RoundedY] = children;
        }
    }

    //проверка движения тетромино
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

            if (grid[RoundedX, RoundedY] != null)
            {
                return false;
            }
        }

        return true;
    }
}
