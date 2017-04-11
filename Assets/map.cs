using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class map : MonoBehaviour {
    public class Cell
    {
        int x;
        int y;
        public string type;
        double prob;
    }

    public class Grid
    {
        int width;
        int length;
        Cell[,] Cells;
        int n_count = 0;
        int h_count = 0;
        int t_count = 0;
        int b_count = 0;

        public Grid(int x, int y)
        {
            width = x;
            length = y;
            Cells = new Cell[x, y];
            foreach(Cell c in Cells)
            {
                float r = Random.Range(0, 1);
                if (r < 0.5)
                {
                    c.type = "N";
                    n_count++;
                }else if(r>0.5 && r < 0.7)
                {
                    c.type = "H";
                    h_count++;
                }else if(r>0.7 && r < 0.9)
                {
                    c.type = "T";
                    t_count++;
                }else
                {
                    c.type = "B";
                    b_count++;
                }
            }
        }
    }
}
