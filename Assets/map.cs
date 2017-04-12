using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Map : MonoBehaviour {
    static System.Random rand = new System.Random();
    public class Cell
    {
        int x;
        int y;
        char type;
        public double[] filter_results;
        public double[] viterbi_results;

        public Cell(int x, int y, char type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public int X()
        {
            return this.x;
        }

        public int Y()
        {
            return this.y;
        }

        public char Type()
        {
            return this.type;
        }

    }

    public class Grid
    {
        int width;
        int height;
        public Cell[,] Cells;
        int n_count;
        int h_count;
        int t_count;
        int b_count;
        public List<Cell> actual_path;
        string actions;
        string observations;

        public Grid(string path)
        {
            string[] buffer = System.IO.File.ReadAllLines(path);
            this.width = Convert.ToInt32(buffer[0]);
            this.height = Convert.ToInt32(buffer[1]);
            Cells = new Cell[height, width];
            int action_count = Convert.ToInt32(buffer[2]);

            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    char t = buffer[i + 3][j];
                    Cells[i, j] = new Cell(i, j, t);
                    Cells[i,j].filter_results = new double[action_count + 1];
                }
            }
            actions = buffer[3 + height];
            observations = buffer[4 + height];
            int initX, initY;
            initX = Convert.ToInt32(buffer[5 + height]);
            initY = Convert.ToInt32(buffer[6 + height]);

            actual_path = new List<Cell>();

            for(int i = 0; i < action_count+1; i++)
            {
                string coordinate_line = buffer[7 + height + i];
                string[] coord = coordinate_line.Split(' ');
                int curX, curY;
                curX = Convert.ToInt32(coord[0]);
                curY = Convert.ToInt32(coord[1]);
                Cell cur = Cells[curX, curY];
                actual_path.Add(cur);
            }
            Debug.Log("");
        }

        public Grid(int width, int height, int action_count,int no)
        {
            string path;
            path = Application.dataPath;
            path += @"/"+no.ToString()+"_";
            this.width = width;
            this.height = height;
            Cells = new Cell[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    char t;
                    double r = rand.NextDouble();
                    if (r < 0.5)
                    {
                        t = 'N';
                        n_count++;
                    }
                    else if (r > 0.5 && r < 0.7)
                    {
                        t = 'H';
                        h_count++;
                    }
                    else if (r > 0.7 && r < 0.9)
                    {
                        t = 'T';
                        t_count++;
                    }
                    else
                    {
                        t = 'B';
                        b_count++;
                    }

                    Cells[i, j] = new Cell(i, j, t);
                    Cells[i, j].filter_results = new double[action_count + 1];
                }
            }

            for(int k = 0; k < 10; k++)
            {
                string subpath = path + k.ToString() + ".txt";
                string actions_string = "";
                for (int i = 0; i < action_count; i++)
                {
                    double r = rand.NextDouble();
                    if (r < 0.25)
                    {
                        actions_string += 'L';
                    }
                    else if (r > 0.25 && r < 0.5)
                    {
                        actions_string += 'R';
                    }
                    else if (r > 0.5 && r < 0.75)
                    {
                        actions_string += 'U';
                    }
                    else
                    {
                        actions_string += 'D';
                    }
                }

                //path generation
                int initX = 0;
                int initY = 0;

                do
                {
                    initX = rand.Next(height);
                    initY = rand.Next(width);
                } while (Cells[initX, initY].Type() == 'B');

                actual_path = new List<Cell>();
                actual_path.Add(Cells[initX, initY]);

                Cell curC = Cells[initX, initY];
                for (int i = 0; i < action_count; i++)
                {
                    Cell next;
                    char curA = actions_string[i];
                    int nextX = curC.X();
                    int nextY = curC.Y();

                    switch (curA)
                    {
                        case 'L':
                            nextX--;
                            break;
                        case 'R':
                            nextX++;
                            break;
                        case 'U':
                            nextY--;
                            break;
                        case 'D':
                            nextY++;
                            break;
                    }

                    if (nextX < 0 || nextX >= height || nextY < 0 || nextY >= width)
                    {
                        next = curC;
                    }
                    else if (Cells[nextX, nextY].Type() == 'B')
                    {
                        next = curC;
                    }
                    else
                    {
                        next = Cells[nextX, nextY];
                    }

                    actual_path.Add(next);
                    curC = next;
                }
                actions = actions_string;
                observations = "";


                foreach (Cell c in actual_path)
                {
                    List<char> obs = new List<char>();
                    obs.Add('N');
                    obs.Add('T');
                    obs.Add('H');
                    double r = rand.NextDouble();
                    if (r < 0.9)
                    {
                        observations += c.Type();
                    }
                    else
                    {
                        obs.Remove(c.Type());
                        int r2 = rand.Next(2);
                        observations += obs[r2];
                    }
                }
                observations = observations.Substring(1);

                List<string> towrite = new List<string>();

                towrite.Add(width.ToString());
                towrite.Add(height.ToString());
                towrite.Add(action_count.ToString());

                for (int i = 0; i < height; i++)
                {
                    string line = "";
                    for (int j = 0; j < width; j++)
                    {
                        line += Cells[i, j].Type();
                    }
                    towrite.Add(string.Copy(line));
                }

                towrite.Add(actions);
                towrite.Add(observations);
                towrite.Add(initX.ToString());
                towrite.Add(initY.ToString());

                foreach (Cell c in actual_path)
                {
                    towrite.Add(c.X().ToString() + " " + c.Y().ToString());
                }


                System.IO.File.WriteAllLines(subpath, towrite.ToArray());
            }


        }

        public Grid(int width,int height,int action_count)
        {
            this.width = width;
            this.height = height;
            Cells = new Cell[height, width];

            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    char t;
                    double r = rand.NextDouble();
                    if (r < 0.5)
                    {
                        t = 'N';
                        n_count++;
                    }
                    else if (r > 0.5 && r < 0.7)
                    {
                        t = 'H';
                        h_count++;
                    }
                    else if (r > 0.7 && r < 0.9)
                    {
                        t = 'T';
                        t_count++;
                    }
                    else
                    {
                        t = 'B';
                        b_count++;
                    }

                    Cells[i, j] = new Cell(i, j, t);
                    Cells[i, j].filter_results = new double[action_count + 1];
                }
            }
            string actions_string = "";
            for (int i = 0; i < action_count; i++)
            {
                double r = rand.NextDouble();
                if (r < 0.25)
                {
                    actions_string += 'L';
                }
                else if (r > 0.25 && r < 0.5)
                {
                    actions_string += 'R';
                }
                else if (r > 0.5 && r < 0.75)
                {
                    actions_string += 'U';
                }
                else
                {
                    actions_string += 'D';
                }
            }

            //path generation
            int initX = 0;
            int initY = 0;

            do
            {
                initX = rand.Next(height);
                initY = rand.Next(width);
            } while (Cells[initX, initY].Type() == 'B');

            actual_path = new List<Cell>();
            actual_path.Add(Cells[initX, initY]);

            Cell curC = Cells[initX, initY];
            for(int i = 0; i < action_count; i++)
            {
                Cell next;
                char curA = actions_string[i];
                int nextX = curC.X();
                int nextY = curC.Y();

                switch (curA)
                {
                    case 'L':
                        nextX--;
                        break;
                    case 'R':
                        nextX++;
                        break;
                    case 'U':
                        nextY--;
                        break;
                    case 'D':
                        nextY++;
                        break;
                }

                if(nextX<0 || nextX>=height || nextY<0 || nextY >= width)
                {
                    next = curC;
                }
                else if (Cells[nextX, nextY].Type() == 'B')
                {
                    next = curC;
                }
                else
                {
                    next = Cells[nextX, nextY];
                }

                actual_path.Add(next);
                curC = next;
            }
            actions = actions_string;
            observations = "";


            foreach(Cell c in actual_path)
            {
                List<char> obs = new List<char>();
                obs.Add('N');
                obs.Add('T');
                obs.Add('H');
                double r = rand.NextDouble();
                if (r < 0.9)
                {
                    observations += c.Type();
                }
                else
                {
                    obs.Remove(c.Type());
                    int r2 = rand.Next(2);
                    observations += obs[r2];
                }
            }
            observations = observations.Substring(1);

            List<string> towrite = new List<string>();

            towrite.Add(width.ToString());
            towrite.Add(height.ToString());
            towrite.Add(action_count.ToString());

            for(int i = 0; i < height; i++)
            {
                string line = "";
                for(int j = 0; j < width; j++)
                {
                    line += Cells[i, j].Type();
                }
                towrite.Add(string.Copy(line));
            }

            towrite.Add(actions);
            towrite.Add(observations);
            towrite.Add(initX.ToString());
            towrite.Add(initY.ToString());
            
            foreach(Cell c in actual_path)
            {
                towrite.Add(c.X().ToString() + " " + c.Y().ToString());
            }

            string path;
            path = Application.dataPath;
            Debug.Log(path);
            path += @"/test.txt";
            System.IO.File.WriteAllLines(path,towrite.ToArray());


        }

        public void Filtering(int steps)
        {
            int notblocked = width*height - b_count;
            double initial_prob = 1 / (double)notblocked;

            foreach(Cell c in Cells)
            {
                if (c.Type() != 'B')
                {
                    c.filter_results[0] = initial_prob;
                }
                else
                {
                    c.filter_results[0] = 0;
                }
            }

            for (int i = 1; i < steps+1; i++)
            {
                foreach(Cell c in Cells)
                {
                    double newProb = c.filter_results[i-1];
                    foreach(Cell c2 in Cells)
                    {
                        newProb += Transition(c, c2, actions[i - 1])*c2.filter_results[i-1];
                    }
                    newProb *= Observation(c, observations[i - 1]);
                    c.filter_results[i] = newProb;
                }

                double sum = 0;
                foreach(Cell c in Cells)
                {
                    sum += c.filter_results[i];
                }
                double alpha = 1 / sum;
                foreach(Cell c in Cells)
                {
                    c.filter_results[i] *= alpha;
                }
            }


        }

        public void Filtering2(int steps)
        {
            int notblocked = width * height - b_count;
            double initial_prob = 1 / (double)notblocked;

            foreach (Cell c in Cells)
            {
                if (c.Type() != 'B')
                {
                    c.filter_results[0] = initial_prob;
                }
                else
                {
                    c.filter_results[0] = 0;
                }
            }

            for (int i = 1; i < steps + 1; i++)
            {
                foreach (Cell c in Cells)
                {
                    int newX = c.X(), newY = c.Y();
                    char action = actions[i - 1];
                    switch (action)
                    {
                        case 'L':
                            newX--;
                            break;
                        case 'R':
                            newX++;
                            break;
                        case 'U':
                            newY--;
                            break;
                        case 'D':
                            newY++;
                            break;
                    }
                    Cell destination = null;
                    if(newX<0 || newX>=height || newY<0 || newY>=width)
                    {
                        destination = c;
                    }else if (Cells[newX, newY].Type() == 'B')
                    {
                        destination = c;
                    }
                    else
                    {
                        destination = Cells[newX, newY];
                    }

                    c.filter_results[i] += 0.1 * c.filter_results[i - 1];
                    destination.filter_results[i] += 0.9 * c.filter_results[i - 1];
                }


                foreach(Cell c in Cells)
                {
                    c.filter_results[i] *= Observation(c, observations[i - 1]);
                }


                double sum = 0;
                foreach (Cell c in Cells)
                {
                    sum += c.filter_results[i];
                }
                double alpha = 1 / sum;
                foreach (Cell c in Cells)
                {
                    c.filter_results[i] *= alpha;
                }
            }

            Debug.Log('1');

        }

    }

    public static double Transition(Cell to, Cell from, char action)
    {
        if(to.Type() == 'B' || from.Type() == 'B')
        {
            return 0;
        }

        int offset_x, offset_y, ex = 0, ey = 0;
        offset_x = to.X() - from.X();
        offset_y = to.Y() - from.Y();


        if(offset_x == 0 && offset_y == 0)
        {
            return 0.1;
        }

        switch (action)
        {
            case 'L':
                ex = -1;
                ey = 0;
                break;
            case 'R':
                ex = 1;
                ey = 0;
                break;
            case 'U':
                ex = 0;
                ey = -1;
                break;
            case 'D':
                ex = 0;
                ey = 1;
                break;
        }

        if(offset_x == ex && offset_y == ey)
        {
            return 0.9;
        }
        else
        {
            return 0;
        }


    }

    public static double Observation(Cell c, char observation)
    {
        if(c.Type() == observation)
        {
            return 0.9;
        }
        else
        {
            return 0.05;
        }
    }
    
    public static double Dist(Cell a, Cell b)
    {
        double aX, aY, bX, bY,dX,dY;
        aX = (double)a.X();
        aY = (double)a.Y();
        bX = (double)b.X();
        bY = (double)b.Y();
        dX = aX - bX;
        dY = aY - bY;


        return Math.Sqrt(dX * dX + dY * dY);
    }
}
