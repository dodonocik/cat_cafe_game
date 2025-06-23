using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndOfDayManager : MonoBehaviour
{
    public TextMeshProUGUI summary;
    public TextMeshProUGUI debtRate;
    public TextMeshProUGUI popupSummary;
    public GameObject popup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisplaySummary();
    }

    public void DisplaySummary()
    {
        string todayMoney = MoneyManager.daysAmount.ToString();
        string totalMoney = MoneyManager.fullAmount.ToString();
        string remainingDebt = MoneyManager.debt.ToString();
        string rate = MoneyManager.debtRate.ToString();
        string text = "Money earned today: " + todayMoney + "\r\nMoney in total: " + totalMoney+"\r\nRemaining debt: "+remainingDebt;
        string rateText = "You ned to pay "+rate+" in credit rate to continue";

        summary.text = text;
        debtRate.text = rateText;
    }

    public void DisplayPopup()
    {
        popup.SetActive(true);
        int currentDebt = MoneyManager.debt;
        int shortfall = MoneyManager.debtRate - MoneyManager.fullAmount;
        int loanValue = (int)(shortfall * 1.5f);
        int postLoanDebt = currentDebt + loanValue*2;
        int currentRate = MoneyManager.debtRate;
        int postLoanRate = (int)(currentRate*1.1);
        string text = "Current debt: " + currentDebt.ToString()+"\r\nPost-loan debt: "+ postLoanDebt.ToString()+"\r\nCurrent payment: " + currentRate.ToString()+"\r\nPost-loan payment: "+postLoanRate.ToString();

        popupSummary.text = text;
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        if (MoneyManager.debtRate > MoneyManager.fullAmount)
        {
            DisplayPopup();
            return;
        }
        if (MoneyManager.debt > 0)
        {
            MoneyManager.fullAmount -= MoneyManager.debtRate;
            if(MoneyManager.debtRate > MoneyManager.debt)
                MoneyManager.debt = 0;
            else
                MoneyManager.debt -= MoneyManager.debtRate;
            MoneyManager.debtRate = (int)(MoneyManager.debtRate * 1.1);
        }
        SceneManager.LoadSceneAsync(1);
    }
    public void GetLoan()
    {
        int rate = MoneyManager.debtRate;
        rate = (int)(rate * 1.1);
        int shortfall = MoneyManager.debtRate - MoneyManager.fullAmount;
        int loanValue = (int)(shortfall * 1.5f);
        MoneyManager.fullAmount += loanValue;
        MoneyManager.debt += loanValue * 2;
        MoneyManager.debtRate = rate;
        popup.SetActive(false);
    }

    public void CancelPopup()
    { 
        popup.SetActive(false);
        DisplaySummary();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
