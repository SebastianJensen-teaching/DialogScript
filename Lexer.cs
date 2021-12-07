using System;
using System.IO;
using System.Collections.Generic;

namespace DialogScript
{
    static class Lexer {

        static readonly Dictionary<char, ScriptNodeType> dict = new Dictionary<char, ScriptNodeType>() {
            {'#', ScriptNodeType.Label},
            {'<', ScriptNodeType.Goto},
            {'>', ScriptNodeType.Link},
            {'[', ScriptNodeType.Choice},
            {']', ScriptNodeType.EndChoice},
            {'{', ScriptNodeType.Option},
            {'}', ScriptNodeType.EndOption},
            {'(', ScriptNodeType.If},
            {')', ScriptNodeType.Endif},
            {'$', ScriptNodeType.Set},
            {'+', ScriptNodeType.Add},
            {'^', ScriptNodeType.Toggle},
            {'*', ScriptNodeType.End},
        };

        public static ScriptNode Lex(string path) {
            ScriptNode root = new ScriptNode();
            Stack<ScriptNode> currentRoot = new Stack<ScriptNode>();
            currentRoot.Push(root);
            string[] lines = File.ReadAllLines(path);
            for(int i = 0; i < lines.Length; i ++) lines[i] = lines[i].Trim();
            foreach(string line in lines) {
                if (line == String.Empty || line == null) continue;
                ScriptNode newNode = new ScriptNode();
                newNode.type = dict.ContainsKey(line[0]) ? dict[line[0]] : ScriptNodeType.Dialog;
                
                if (newNode.type == ScriptNodeType.Dialog ||
                    newNode.type == ScriptNodeType.Set) {
                    newNode.contents= line;
                } else {
                    newNode.contents = line.Substring(1).TrimStart();
                }
                newNode.indentLevel = currentRoot.Count - 1; 
                currentRoot.Peek().AppendChild(newNode);
                if (newNode.type == ScriptNodeType.Choice ||
                    newNode.type == ScriptNodeType.Option ||
                    newNode.type ==  ScriptNodeType.If ) {
                        currentRoot.Push(newNode);
                }
                if (newNode.type == ScriptNodeType.EndChoice ||
                    newNode.type == ScriptNodeType.EndOption ||
                    newNode.type == ScriptNodeType.Endif) {
                        currentRoot.Pop();
                }
            }
            return root;
        }

        public static void DebugPrint(ScriptNode root) {
            foreach(var node in root.children) {
                for(int i = 0; i < node.indentLevel; i++) {
                    Console.Write("\t");
                }
                Console.Write($"[{node.type}]: {node.contents}\n");
                DebugPrint(node);
            }
        }
    }
}
