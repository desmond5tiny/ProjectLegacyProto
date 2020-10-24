using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UIHoverTest : MonoBehaviour
{
    public string myString;
    public Text myText;
    Color textColor;


    void Start()
    {
        myText.text = myString;
        textColor = myText.color;
        myText.color = Color.clear;
    }

    
    private void OnMouseEnter()
    {
        myText.color = textColor;
    }
    private void OnMouseExit()
    {
        myText.color = Color.clear;
    }
}
