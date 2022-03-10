namespace FunctionParser;

public class FunctionParser
{
    // Define possible operators
    private static string[] operators = { "+", "-", "*", "/", "^" };
    private static string[] priorityOperators = { "^", "*", "/" };
    public FunctionParser()
    {
        
    }

    public bool StringContainsOnlyOperatorsOrNumbers(string input)
    {
        // Check if the string contains only operators or numbers or spaces
        foreach (char c in input)
        {
            if (!char.IsDigit(c) && !char.IsWhiteSpace(c) && !operators.Contains(c.ToString()))
            {
                return false;
            }
        }
        return true;
    }

    public void ValidateInput(string input)
    {
        if (input == "")
        {
            throw new System.ArgumentException("Input is empty");
        }
        // check if input contains any operators
        if (!operators.Any(input.Contains))
        {
            throw new System.ArgumentException("Input does not contain any operators");
        }
        // Check if input contains only operators or numbers
        if (!StringContainsOnlyOperatorsOrNumbers(input))
        {
            throw new System.ArgumentException("Input contains invalid characters");
        }
    }

    public Decimal Parse(string input)
    {
        ValidateInput(input);
        Decimal result = 0;
        // Check if input is a number
        if (Decimal.TryParse(input, out result))
        {
            return result;
        }
        var tokens = Tokenize(input);
        // Calculate the result
        result = Calculate(tokens);
        return result;
    }

    private List<string> Tokenize(string input)
    {
        // split input into tokens by operators but include the delimiter
        List<string> tokens = SplitAndKeepDelimiters(input, operators).ToList();
        ValidateTokens(tokens);
        return tokens;
    }

    private void ValidateTokens(List<string> tokens)
    {
        Decimal result = 0;
        // ensure that every second token is a number
        for (int i = 0; i < tokens.Count; i += 2)
        {
            if (!Decimal.TryParse(tokens[i], out result))
            {
                throw new System.ArgumentException("Function format is invalid");
            }
        }
        // ensure that every odd token is an operator
        for (int i = 1; i < tokens.Count; i += 2)
        {
            if (!operators.Contains(tokens[i]))
            {
                throw new System.ArgumentException("Function format is invalid");
            }
        }
    }

    private decimal Calculate(List<string> tokens)
    {
        // Calculate the sub-expressions with priority operators first
        for (int i = 0; i < tokens.Count; i += 1)
        {
            if (priorityOperators.Any(tokens[i].Contains))
            {
                decimal left = Decimal.Parse(tokens[i - 1]);
                decimal right = Decimal.Parse(tokens[i + 1]);
                decimal result = 0;
                switch (tokens[i])
                {
                    case "^":
                        result = (decimal)Math.Pow((double)left, (double)right);
                        break;
                    case "*":
                        result = left * right;
                        break;
                    case "/":
                        result = left / right;
                        break;
                }
                tokens.RemoveRange(i - 1, 3);
                tokens.Insert(i - 1, result.ToString());
                i = 0;
            }
        }
        // Calculate the sub-expressions with no priority operators
        for (int i = 1; i < tokens.Count; i += 1)
        {
            if (!priorityOperators.Contains(tokens[i]))
            {
                decimal left = Decimal.Parse(tokens[i - 1]);
                decimal right = Decimal.Parse(tokens[i + 1]);
                decimal result = 0;
                switch (tokens[i])
                {
                    case "+":
                        result = left + right;
                        break;
                    case "-":
                        result = left - right;
                        break;
                }
                tokens.RemoveRange(i - 1, 3);
                tokens.Insert(i - 1, result.ToString());
                i = 0;
            }
        }
        // Return the result
        return Decimal.Parse(tokens[0]);
        
    }
        
        
    public Decimal Parse(string input, Dictionary<string, Decimal> replaceValues)
    {
        // replace values in the input then call parse
        foreach (KeyValuePair<string, Decimal> kvp in replaceValues)
        {
            input = input.Replace(kvp.Key, kvp.Value.ToString());
        }
        // check that the input is not empty
        if (input == "")
        {
            throw new System.ArgumentException("Input is empty");
        }
        // validate the input
        ValidateInput(input);
        return Parse(input);
    }


    public static IList<string> SplitAndKeepDelimiters(string input, string[] operators)
    {
        List<string> tokens = new List<string>();
        string token = "";
        for (int i = 0; i < input.Length; i++)
        {
            if (operators.Contains(input[i].ToString()))
            {
                if (token != "")
                {
                    tokens.Add(token);
                    token = "";
                }
                tokens.Add(input[i].ToString());
            }
            else
            {
                token += input[i];
            }
        }
        if (token != "")
        {
            tokens.Add(token);
        }
        return tokens.Select(x => x.Trim()).ToList();
    }
}