using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIController
{
    public Text warningtext;
    public void GameOver()
    {
        warningtext.text = "GameOver!";
    }
    public void ReStart()
    {
        warningtext.text = "";
    }
}
