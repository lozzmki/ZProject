using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DSizeN
{
    public int d_nWidth, d_nHeight;
    public DSizeN(int width = 0, int height = 0)
    {
        d_nHeight = height;
        d_nWidth = width;
    }
}

public class DPointN
{
    public int x, y;
    public DPointN(int xval = 0, int yval = 0)
    {
        x = xval; y = yval;
    }
    public Vector2 ToVec2()
    {
        return new Vector2(x, y);
    }
    public bool Equals(DPointN other)
    {
        return x == other.x && y == other.y;
    }
}

