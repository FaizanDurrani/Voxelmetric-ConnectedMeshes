using UnityEngine;
using System.Collections;
[System.Serializable]
public class DirectionsEnabled
{
    public bool up, down, north, south, east, west;
    public DirectionsEnabled(bool u, bool d, bool n, bool s, bool e, bool w)
    {
        up = u;
        down = d;
        north = n;
        south = s;
        east = e;
        west = w;
    }
}