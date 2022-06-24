using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public float[,] CurrentPosition;
    public float[,] NextPosition;

    public Color[,] CurrentColor;
    public Color[,] NextColor;

    public GameObject PlatformBase;
    public GameObject[,] PlatformNode;

    public int M = 16;
    public int N = 9;

    public float height = 1.0f;
    public float spaceX = 0.0f;
    public float spaceZ = 0.0f;

    public bool simulation = false;

    public enum Colors
    {
        grey, red, green, blue, random
    }
    public Colors shade = Colors.grey;

    public float y = 0.0f;
    public float r = 0.0f;
    public float g = 0.0f;
    public float b = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        PlatformNode = new GameObject[M, N];
        CurrentPosition = new float[M, N];
        NextPosition = new float[M, N];
        CurrentColor = new Color[M, N];
        NextColor = new Color[M, N];

        for (int i = 0; i < M; i++)
        {
            spaceZ = 0.0f;
            for (int j = 0; j < N; j++)
            {
                PlatformBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
                PlatformBase.transform.localScale = new Vector3(1, 0.1f, 1);
                GameObject.Find("Cube").GetComponent<Renderer>().enabled = false;

                float x = i + spaceX;
                float z = j + spaceZ;

                var nodes = Instantiate(PlatformBase, new Vector3(x, 0, z), Quaternion.identity); // generates copies

                PlatformNode[i, j] = nodes;
                CurrentPosition[i, j] = 0.0f;
                NextPosition[i, j] = 0.0f;
                CurrentColor[i, j] = new Color(1.0f, 1.0f, 1.0f);
                spaceZ += 0.1f;
            }
            spaceX += 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            simulation = !simulation;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            height++;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            height--;
            if (height < 0)
            {
                height = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            quit();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            shade = Colors.red;
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            shade = Colors.green;
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            shade = Colors.blue;
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            shade = Colors.grey;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            shade = Colors.random;
        }


        if (simulation)
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (almost(CurrentPosition[i, j], NextPosition[i, j]))
                    {
                        NextPosition[i, j] = y;

                        //random floats within -1 to 1 inclusive
                        y = UnityEngine.Random.Range(-height, height);

                        //random floats for different shades in color
                        r = UnityEngine.Random.Range(0.0f, 1.0f);
                        g = UnityEngine.Random.Range(0.0f, 1.0f);
                        b = UnityEngine.Random.Range(0.0f, 1.0f);

                        if (shade == Colors.grey)
                        {
                            NextColor[i, j] = new Color(r, r, r);
                        }

                        if (shade == Colors.red)
                        {
                            NextColor[i, j] = new Color(r, 0, 0);
                        }

                        if (shade == Colors.green)
                        {
                            NextColor[i, j] = new Color(0, g, 0);
                        }

                        if (shade == Colors.blue)
                        {
                            NextColor[i, j] = new Color(0, 0, b);
                        }

                        if (shade == Colors.random)
                        {
                            NextColor[i, j] = new Color(r, g, b);
                        }
                    }

                    PlatformNode[i, j].transform.position = Vector3.Lerp(PlatformNode[i, j].transform.position,
                        new Vector3(PlatformNode[i, j].transform.position.x, NextPosition[i, j], PlatformNode[i, j].transform.position.z), Time.deltaTime);

                    CurrentPosition[i, j] = PlatformNode[i, j].transform.position.y;

                    PlatformNode[i, j].transform.GetComponent<Renderer>().material.color = Color.Lerp(CurrentColor[i, j], NextColor[i, j], Time.deltaTime);

                    CurrentColor[i, j] = PlatformNode[i, j].transform.GetComponent<Renderer>().material.color;
                }
            }
        }
        else //Else lerp back to original position and change color back to white
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {

                    PlatformNode[i, j].transform.position = Vector3.Lerp(PlatformNode[i, j].transform.position,
                        new Vector3(PlatformNode[i, j].transform.position.x, 0, PlatformNode[i, j].transform.position.z), Time.deltaTime);

                    PlatformNode[i, j].transform.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                }
            }
        }
    }

    public bool almost(float? current, float? next)
    {
        if (current != next)
        {
            return Mathf.Abs(current.Value - next.Value) < 0.1f;
        }

        return true;
    }

    public void quit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}