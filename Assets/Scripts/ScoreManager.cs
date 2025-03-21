using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text throwsNbText;
    public TMP_Text fallenPinsNbText;
    public TMP_Text totalPinsNbText;


    private int throwsNb = 0;
    private int fallenPinsNb = 0;



    public void IncreaseThrowNumber()
    {
        throwsNb++;
        throwsNbText.text = throwsNb.ToString();
    }

    public void IncreaseFallenPinsNb()
    {
        fallenPinsNb++;
        fallenPinsNbText.text = fallenPinsNb.ToString();
    }

    public void setTotalPinsNb(int nb)
    {
        totalPinsNbText.text = "/" + nb.ToString();
    }
}
