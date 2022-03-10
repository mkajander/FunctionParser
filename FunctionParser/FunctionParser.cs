namespace FunctionParser;

public class FunctionParser
{
    // Define possible operators
    private static string[] operators = { "+", "-", "*", "/", "^" };
    private static string[] priorityOperators = { "^", "*", "/" };
    public FunctionParser()
    {
        
    }

    public Decimal Parse(string input)
    {
        Decimal result = 0;
        // Check if input is empty
        if (input == "")
        {
            throw new System.ArgumentException("Input is empty");
        }
        // Check if input is a number
        if (Decimal.TryParse(input, out result))
        {
            return result;
        }
        // check if input contains any operators
        if (!operators.Any(input.Contains))
        {
            throw new System.ArgumentException("Input does not contain any operators");
        }
        // split input into tokens by operators but include the delimiter
        List<string> tokens = SplitAndKeepDelimiters(input, operators).ToList();
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
        
        // Calculate the result
        result = Calculate(tokens);
        return result;

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
        return tokens;
    }
}