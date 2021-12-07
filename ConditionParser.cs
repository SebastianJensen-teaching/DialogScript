using System;

namespace DialogScript
{
    public enum OperatorType {
        Undefined,
        Assign,
        Add,
        Subtract,
        Multiply,
        Divide,
        Equals,
        GreaterThan,
        LessThan,
        NotEquals
    }


    static class ConditionParser {


        public static bool Compare(OperatorType operatorType, int lvalue, int rvalue) {
            switch (operatorType) {
                case OperatorType.Equals: {
                    return lvalue == rvalue;
                }
                case OperatorType.LessThan: {
                    return lvalue < rvalue;
                }
                case OperatorType.GreaterThan: {
                    return lvalue > rvalue;
                }
                case OperatorType.NotEquals: {
                    return lvalue != rvalue;
                }
            }
            return false;
        }

        public static bool Compare(OperatorType operatorType, string lvalue, string rvalue) {
            switch (operatorType) {
                case OperatorType.Equals: {
                    return lvalue == rvalue;
                }
                case OperatorType.LessThan: {
                    return false;   // less than can not be used on strings
                }
                case OperatorType.GreaterThan: {
                    return false;   // greater than can not be used on strings
                }
                case OperatorType.NotEquals: {
                    return lvalue != rvalue;
                }
            }
            return false;
        }

        public static bool Parse(string condition) {
            string[] tokens = condition.Split(' ');
            if (tokens.Length != 3) {   // expect EXACTLY 3 tokens, lvalue, operator and rvalue
                return false;           // if we don't have a valid condition it is assumed to be false
            }

            // Our lvalue must be a value from the store. If it is not, the condition is false.
            if (!ScriptStore.GetItem(tokens[0].Substring(1), out string lvalue)) {
                return false;
            }

            OperatorType operatorType = OperatorType.Undefined;
            switch(tokens[1]) {
                case "==": {
                    operatorType = OperatorType.Equals;
                } break;
                case "<": {
                    operatorType = OperatorType.LessThan;
                } break;
                case ">": {
                    operatorType = OperatorType.GreaterThan;
                } break;
                case "!": {
                    operatorType = OperatorType.NotEquals;
                } break;
            }
            if (OperatorType.Undefined == operatorType) {
                return false;       // Conditions with an undefined operator are false
            }

            string rvalue = tokens[2];
            if ('$' == rvalue[0]) {     // The rvalue may optionally be another value from the store
                if (!ScriptStore.GetItem(rvalue.Substring(1), out rvalue)) {
                    return false;
                }
            }

            if (Char.IsDigit(rvalue, 0)) {
                if (!Int32.TryParse(lvalue, out int lvalue_num)) {
                    return false;   // if the rvalue is an integer, lvalue must also be an integer or its false
                }
                if (!Int32.TryParse(rvalue, out int rvalue_num)) {
                    return false;   // return false if rvalue could not be parsed as integer
                }
                return Compare(operatorType, lvalue_num, rvalue_num);
            }

            // No numbers, so do string comparison
            return Compare(operatorType, lvalue, rvalue);    
        }
    }
}
