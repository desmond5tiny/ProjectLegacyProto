using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    static Texture2D _rectFillTexture;
    public static Texture2D rectFillTexture
    {
        get
        {
            if (_rectFillTexture == null)
            {
                _rectFillTexture = new Texture2D(1, 1);
                _rectFillTexture.SetPixel(0, 0, Color.white);
                _rectFillTexture.Apply();
            }

            return _rectFillTexture;
        }
    }

    public static void DrawScreenRect(Rect _rect, Color _color)
    {
        GUI.color = _color;
        GUI.DrawTexture(_rect, rectFillTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect _rect, float _width, Color _color)
    {
        Utils.DrawScreenRect(new Rect(_rect.xMin, _rect.yMin, _rect.width, _width),_color); //top
        Utils.DrawScreenRect(new Rect(_rect.xMin, _rect.yMin, _width, _rect.height),_color); //left
        Utils.DrawScreenRect(new Rect(_rect.xMax - _width, _rect.yMin, _width, _rect.height), _color); //left
        Utils.DrawScreenRect(new Rect(_rect.xMin, _rect.yMax - _width, _rect.width, _width), _color); //left
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
        // Create Rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }
}
