namespace MyMauiApp;

public partial class MainPage : ContentPage
{
    private string currentInput = "0";
    private string pendingOperator = "";
    private double? firstNumber = null;
    private bool newInputExpected = false;
    private bool calculationPerformed = false;
    
    public MainPage()
    {
        InitializeComponent();
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        Label.Text = currentInput;
    }
    
    private void OnDigitClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string digit = button.Text;
        
        if (currentInput == "0" || currentInput == "Error" || newInputExpected || calculationPerformed)
        {
            currentInput = digit;
            newInputExpected = false;
            calculationPerformed = false;
        }
        else
        {
            currentInput += digit;
        }
        
        UpdateDisplay();
    }
    
    private void OnDecimalClicked(object sender, EventArgs e)
    {
        if (newInputExpected || calculationPerformed)
        {
            currentInput = "0.";
            newInputExpected = false;
            calculationPerformed = false;
        }
        else if (!currentInput.Contains("."))
        {
            currentInput += ".";
        }
        
        UpdateDisplay();
    }
    
    private void OnSignClicked(object sender, EventArgs e)
    {
        if (currentInput == "0" || currentInput == "Error")
            return;
            
        if (currentInput.StartsWith("-"))
            currentInput = currentInput.Substring(1);
        else
            currentInput = "-" + currentInput;
        
        UpdateDisplay();
    }
    
    private void OnBackspaceClicked(object sender, EventArgs e)
    {
        if (currentInput == "Error")
        {
            currentInput = "0";
        }
        else if (currentInput.Length > 1 && currentInput != "0")
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
        }
        else
        {
            currentInput = "0";
        }
        
        UpdateDisplay();
    }
    
    private void OnClearClicked(object sender, EventArgs e)
    {
        currentInput = "0";
        firstNumber = null;
        pendingOperator = "";
        newInputExpected = false;
        calculationPerformed = false;
        UpdateDisplay();
    }
    
    private void OnOperatorClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string newOperator = button.Text;
        
        if (firstNumber == null)
        {
            firstNumber = double.Parse(currentInput);
            pendingOperator = newOperator;
            newInputExpected = true;
        }
        else
        {
            if (!newInputExpected && !calculationPerformed)
            {
                CalculateResult();
            }
            pendingOperator = newOperator;
            newInputExpected = true;
            calculationPerformed = false;
        }
    }
    
    private void OnEqualsClicked(object sender, EventArgs e)
    {
        if (firstNumber != null && !string.IsNullOrEmpty(pendingOperator))
        {
            CalculateResult();
            pendingOperator = "";
            calculationPerformed = true;
        }
    }
    
    private void CalculateResult()
    {
        try
        {
            double secondNumber = double.Parse(currentInput);
            double result = 0;
            
            switch (pendingOperator)
            {
                case "+":
                    result = firstNumber.Value + secondNumber;
                    break;
                case "-":
                    result = firstNumber.Value - secondNumber;
                    break;
                case "×":
                case "*":
                    result = firstNumber.Value * secondNumber;
                    break;
                case "÷":
                case "/":
                    if (secondNumber == 0)
                        throw new DivideByZeroException();
                    result = firstNumber.Value / secondNumber;
                    break;
            }
            
            currentInput = FormatResult(result);
            firstNumber = result;
        }
        catch
        {
            currentInput = "Error";
            firstNumber = null;
            pendingOperator = "";
            newInputExpected = false;
            calculationPerformed = false;
        }
        
        UpdateDisplay();
    }
    
    private string FormatResult(double result)
    {
        if (double.IsInfinity(result) || double.IsNaN(result))
            return "Error";
            
        string strResult = result.ToString();
        
        if (strResult.Contains("."))
        {
            strResult = strResult.TrimEnd('0').TrimEnd('.');
        }
        
        if (strResult.Length > 15)
        {
            return result.ToString("E10");
        }
        
        return strResult;
    }
}
