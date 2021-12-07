using System.Collections.Generic;

namespace DialogScript
{
    class ScriptNode {
        public ScriptNodeType type = ScriptNodeType.Root;
        public ScriptNode parent = null;
        public List<ScriptNode> children = new List<ScriptNode>();
        public string contents;
        public int indentLevel;

        public void AppendChild(ScriptNode node) {
            if (node.parent != null) node.parent.children.Remove(node);
            node.parent = this;
            children.Add(node);
        }
    }
}
