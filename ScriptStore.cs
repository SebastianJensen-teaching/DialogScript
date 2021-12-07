using System;
using System.Collections.Generic;

namespace DialogScript
{
    static class ScriptStore {
        static Dictionary<string, string> data = new Dictionary<string, string>();

        public static void SetString(string key, string value) => data[key] = value;
        public static bool GetItem(string key, out string value) => 
            data.TryGetValue(key, out value);

        public static void DebugPrint() {
            foreach(KeyValuePair<string, string> keyValuePair in data) {
                Console.WriteLine($"{keyValuePair.Key} : {keyValuePair.Value}");
            }
        }
    }
}
