using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : MonoBehaviour
{
    [SerializeField]
    GameObject[] Tetrominoes;
    [SerializeField]
    GameObject nextTetra;
    GameObject curTetra;

    bool first = true;
    
    //вызов нового тетромино
    void Start()
    {
        NewTetromino();
    }

    //случайный выбор тетромино
    public void NewTetromino()
    {
        if(first)
        {
            curTetra = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
            nextTetra = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], nextTetra.transform.position, Quaternion.identity);
            nextTetra.GetComponent<Block>().enabled = false;
            first = false;
        }
        else
        {
            curTetra = Instantiate(nextTetra, transform.position, Quaternion.identity);
            curTetra.GetComponent<Block>().enabled = true;
            Destroy(nextTetra);
            nextTetra = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], nextTetra.transform.position, Quaternion.identity);
            nextTetra.GetComponent<Block>().enabled = false;
        }
        
    }
    public void Restart()
    {
        first = true;
        Destroy(nextTetra);
        Destroy(curTetra);
    }
}
