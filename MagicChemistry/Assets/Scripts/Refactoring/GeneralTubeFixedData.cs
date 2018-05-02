using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GeneralTubeFixedData : GeneralTubeData
{

    [SerializeField]
    private int Number;

    [SerializeField]
    private Text text;

    private void Start()
    {
        Value = Number;
        Replacable = false;
        text.text = Number.ToString();
    }

    public void SetNumber(int number)
    {
        Number = number;
        text.text = Number.ToString();
    }
}
