using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TestBtn : MonoBehaviour {
    public Button btn;
    public GameObject tile;

    // Use this for initialization
    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

    }

    void TaskOnClick()
    {
        /*
        for (int i = 0; i < 10; i++)
        {
            Map.Grid g = new Map.Grid(100, 100, 100, i);
        }
        */

        
        Map.Grid g = new Map.Grid(Application.dataPath + @"/1_1.txt");
        g.Filtering2(100);
        

        
        foreach (Map.Cell c in g.Cells)
        {
            GameObject cur;
            cur = Instantiate(tile, new Vector3(10 * c.X(), 0, 10 * c.Y()), Quaternion.identity);
            Color col = new Color((float)c.filter_results[50]*10,0,(float)(1-c.filter_results[50]));
            cur.GetComponent<Renderer>().material.color = col;
        }
        
        /*
        double[] avgs = new double[100];
        double[] readings = new double[100];

        string path = Application.dataPath + @"/";

        for(int i = 0; i < 10; i++)
        {
            string subpath = path + i.ToString() + "_";
            for(int j = 0; j < 10; j++)
            {
                string subpath2 =subpath +  j.ToString() + ".txt";
                Map.Grid g = new Map.Grid(subpath2);
                g.Filtering2(100);
                List<Map.Cell> cell_list = new List<Map.Cell>();
                foreach(Map.Cell c in g.Cells)
                {
                    cell_list.Add(c);
                }
                for(int k = 0; k < 100; k++)
                {
                    double maxval = cell_list.Max(a => a.filter_results[k]);
                    Map.Cell max = cell_list.First(x => x.filter_results[k] == maxval);
                    double diff = Map.Dist(max, g.actual_path[k + 1]);
                    avgs[k] += diff;

                    readings[k] += g.actual_path[k].filter_results[k];
                }
            }
        }
        Debug.Log("");
        */
        
    }
}
