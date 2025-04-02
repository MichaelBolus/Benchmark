//This example will validate not only for one delimter line but for multiple.
//Examples below should explain different scinarious.
//I know you have asked for a simple solution wit h a specific schenarios.
//I have created a solution that hopefully handles all scenarious for you.
//Whether there are multiple lines or not it wont matter.
//It will also handles exceptions and duplicate delimiters.
//I hope you don't mind and I hope it is ok, covering more scenrious than requested.


using System.Text.RegularExpressions;

#region Valid examples
validate("");
validate("1");
validate("1,2");
validate("//*%\n1*2%3");
validate("//*%\n1*2%3\n4");
validate("//*%\n1*2%3\n4*1001%5");
validate("//*%\n1*2%3//;,\n1;2,3");
validate("//*%\n1*2%3//;,\n1;2,3\n4");
#endregion

#region Invalid examples
validate("1,\n");
validate("//*%\n1*%2%3");
validate("//*%\n1*2%3*-2");
validate("//-\n1--2");
#endregion


void validate(string numbers)
{
	try
	{
		if (string.IsNullOrWhiteSpace(numbers))
            Console.WriteLine(0);

        var partitions = numbers.Split("//");

		if(partitions.Length > 0)
		{
			foreach (var part in partitions)
			{
                int newlineIndex = part.IndexOf('\n');
				if (newlineIndex != -1)
				{
                    var delimiters = $"{part.Substring(0, newlineIndex)}";
					var values = part.Substring(newlineIndex + 1);

					if ((delimiters.Contains("-") && values.Contains("--")) || (!delimiters.Contains('-') && values.Contains("-"))) 
					{
                        throw new Exception("Negatives not allowed");
                    }

					values = NormalizeValues(delimiters, values);
                    if (values.Contains("-"))
                        throw new Exception("Negatives not allowed");
                    else if (!string.IsNullOrWhiteSpace(values))
                    {
                        PrintValues(",", values);
                        continue;
                    }
                }

				var value = NormalizeValues(",", part);
                if (value.Contains("-"))
                    throw new Exception("Negatives not allowed");
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    PrintValues(",", value);
                    continue;
                }
            }
		}


    }
	catch (Exception ex)
	{
        Console.WriteLine($"An error has occurred: {ex.Message}");
    }
}

string NormalizeValues(string delimiters, string value)
{
	try
	{
		if (string.IsNullOrWhiteSpace(value)) return value;

        string validation = "[" + Regex.Escape(delimiters) + "\n" + "]";
		var values = Regex.Replace(value, validation, "|");

		if (values.Contains("||"))
		{
            Console.WriteLine($"A partition with the following delimter(s) [{delimiters}] failed for the following reason:\nPartition includes a combined delimter with a new line OR two delimters following each other.");
            return "";
		}

		return values;
    }
	catch
	{
		return "";
	}
}

void PrintValues(string delimiters,	string values)
{
    if (!string.IsNullOrWhiteSpace(values))
    {
        try
        {
            int sum = values.Split('|')
                             .Select(n => int.Parse(n))
                             .Sum(num => num > 1000 ? 0 : num);

            Console.WriteLine(sum);
        }
        catch
        {
            Console.WriteLine($"A partition with the following delimter(s) [{delimiters}] failed for the following reason:\nPartition includes non numerical character.");
        }
    }
}
