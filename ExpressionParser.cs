using System;

namespace DialogScript
{
    static class ExpressionParser {
        public static bool Parse(string expression, out string result) {
            string[] tokens = expression.Split(' ');
            OperatorType operatorType = OperatorType.Undefined;
            
            if (tokens.Length != 3) {    // Requires exactly 3 tokens: lvalue, operator and rvalue
                Console.WriteLine("Invalid expression: requires exactly three tokens");
                result = String.Empty;
                return false;
            }

            { // Is the variable already declared?
                if (false == ScriptStore.GetItem(tokens[0].Substring(1), out string temp)) {
                    // If not, create it and initialize to zero:
                    ScriptStore.SetString(tokens[0].Substring(1), "0");
                    ScriptStore.GetItem(tokens[0].Substring(1), out temp);
                }
                tokens[0] = temp;
            }

            // Is the rvalue another variable?
            if (tokens[2][0] == '$') 
            {
                // Attempt to find and replace with variable from store:
                if (!ScriptStore.GetItem(tokens[2].Substring(1), out tokens[2])) {
                    result = String.Empty;
                    return false;   // Attempting to use undeclared variable
                }
            }
            
            if (false == Char.IsDigit(tokens[2][0])) {
                result = tokens[2];
                return true;
            }

            // We are manipulating a number, so get that number
            if (false == Int32.TryParse(tokens[0], out int lvalue_num)) {
                result = String.Empty;
                return false;
            }

            // If the lvalue is a number then the rvalue needs to be a number too
            if (false == Int32.TryParse(tokens[2], out int rvalue_num)) {
                result = String.Empty;
                return false;
            }

            // Select the appropriate operator
            switch(tokens[1][0]) {      
                case '=': {
                    operatorType = OperatorType.Assign;
                } break;
                case '+': {
                    operatorType = OperatorType.Add;
                } break;
                case '-': {
                    operatorType = OperatorType.Subtract;
                } break;
                case '*': {
                    operatorType = OperatorType.Multiply;
                } break;
                case '/': {
                    operatorType = OperatorType.Divide;
                } break;
            }

            int result_num = lvalue_num;
            switch (operatorType) {
                case OperatorType.Assign: {
                    result_num = rvalue_num;
                } break;
                case OperatorType.Add: {
                    result_num = lvalue_num + rvalue_num;
                } break;
                case OperatorType.Subtract: {
                    result_num = lvalue_num - rvalue_num;
                } break;
                case OperatorType.Multiply: {
                    result_num = lvalue_num * rvalue_num;
                } break;
                case OperatorType.Divide: {
                    if (rvalue_num != 0) {
                        result_num = lvalue_num / rvalue_num;
                    }
                } break;
            }

            result = result_num.ToString();
            return true;
        }
    }
}
