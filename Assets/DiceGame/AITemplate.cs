using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Your AI will need to access the GameManager script on the object in the scene.
// You will have access to the following methods:
// void RollDice(): This method with roll the dice for a couple seconds.
// bool IsRolling(): This method will return a bool that tells you if the dice are currently rolling.
// void SetComboActive(int index, bool state): This method will set the interactable state of a combo button at a specified index. It will only do this if the combo has not yet been selected.
// void SelectCombo(int index): This will try to select the combo by index. You can use the enum DiceCombos and cast it to an int. eg. (int)GameManager.DiceCombos.LargeStraight
// void KeepDie(int index): This will toggle the keep button at the index.
// void GetDiceValues(ref int[] values): This will point the array given to the diceValues in the GameManager.
// bool IsComboSelected(int index): This will return if the combo has been selected 


public class AITemplate : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [SerializeField] AIStates currentState = AIStates.RollDice1;

    [SerializeField] Button aiButton;

   

    enum AIStates
    {
        RollDice1,
        EvaluateDice1,
        KeepDice,
        RollDice2,
        EvaluateDice2
    }

    [SerializeField] int[] diceValues = new int[5];
    [SerializeField] int[] diceCount = new int[6];

    [SerializeField] bool onePair, twoPair, threeKind, fourKind, fullHouse, smallStraight, largeStraight;

    [SerializeField] int numInSequence;

    private void Start()
    {
        gameManager.GetDiceValues(ref diceValues);
        aiButton.GetComponentInChildren<TextMeshProUGUI>().text = "Roll";
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsRolling() && !aiButton.interactable)
        {
            aiButton.interactable = true;
        }
    }

    public void TakeAIStep()
    {
        switch (currentState)
        {
            case AIStates.RollDice1:
                // Behavior
                Roll();

                // Transitions

                currentState = AIStates.EvaluateDice1;
                break;
            case AIStates.EvaluateDice1:
                CountDice();

                CheckCombos();

                // If select combo go to AIStates.RollDice1

                // If you are keeping dice go to AIStates.KeepDice
                aiButton.GetComponentInChildren<TextMeshProUGUI>().text = "Keep";
                currentState = AIStates.KeepDice;
                break;
            case AIStates.KeepDice:
                currentState = AIStates.RollDice2;
                aiButton.GetComponentInChildren<TextMeshProUGUI>().text = "Roll";
                break;
            case AIStates.RollDice2:
                Roll();

                currentState = AIStates.EvaluateDice2;
                break;
            case AIStates.EvaluateDice2:
                CountDice();

                CheckCombos();

                currentState = AIStates.RollDice1;
                aiButton.GetComponentInChildren<TextMeshProUGUI>().text = "Roll";
                break;
            default:
                break;
        }
    }

    void Roll()
    {
        gameManager.RollDice();

        for (int i = 0; i < 6; i++)
        {
            gameManager.SetComboActive(i, false);
        }

        aiButton.interactable = false;
        aiButton.GetComponentInChildren<TextMeshProUGUI>().text = "Eval";
    }

    void CountDice()
    {
        // Reset array
        for (int i = 0; i < diceCount.Length; i++)
        {
            diceCount[i] = 0;
        }
        // Count the dice values
        for (int i = 0; i < diceValues.Length; i++)
        {
            diceCount[diceValues[i]]++;
        }
    }
    void CheckCombos()
    {
        // Initialize combo variables
        onePair = false;
        twoPair = false;
        threeKind = false;
        fourKind = false;
        fullHouse = false;
        smallStraight = false;
        largeStraight = false;

        numInSequence = 0;

        for (int i = 0; i < diceCount.Length; i++)
        {
            // Find matches of dice values
            if (diceCount[i] >= 2)
            {
                if (!onePair)
                {
                    onePair = true;
                }
                else
                {
                    twoPair = true;
                    gameManager.SetComboActive((int)GameManager.DiceCombos.TwoPair, true);
                }
            }

            if (diceCount[i] >= 3)
            {
                threeKind = true;
                gameManager.SetComboActive((int)GameManager.DiceCombos.ThreeKind, true);
            }

            if (diceCount[i] >= 4)
            {
                fourKind = true;
                gameManager.SetComboActive((int)GameManager.DiceCombos.FourKind, true);
            }

            if (twoPair &&  threeKind)
            {
                fullHouse = true;
                gameManager.SetComboActive((int)GameManager.DiceCombos.FullHouse, true);
            }

            // Find sequence of dice values
            if (diceCount[i] > 0)
            {
                numInSequence++;

                if (numInSequence == 4)
                {
                    smallStraight = true;
                    gameManager.SetComboActive((int)GameManager.DiceCombos.SmallStraight, true);
                }
                if (numInSequence == 5)
                {
                    largeStraight = true;
                    gameManager.SetComboActive((int)GameManager.DiceCombos.LargeStraight, true);
                }
            }
            else
            {
                numInSequence = 0;
            }
        }
    }

    void KeepDice()
    {

    }

    void ChooseCombo()
    {
        
    }
}
