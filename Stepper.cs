namespace DialogScript
{
    class Stepper {
        public readonly ScriptNode rootNode;
        public ScriptNode currentNode;
        const int maxDepth = 128;

        public Stepper(ScriptNode start) {
            currentNode = start;
            rootNode = start;

            int itr = 0;
            while (ScriptNodeType.Root != currentNode.type) {
                rootNode = rootNode.parent;
                if (itr++ == maxDepth) {
                    throw new System.Exception("Error! Script has no root node or is too deep.");
                }
            }

        }

        public bool TryGoToLabel(string label) {
            Stepper subStepper = new Stepper(rootNode);
            while (false == subStepper.Finished()) {
                if (ScriptNodeType.Label == subStepper.currentNode.type &&
                    subStepper.currentNode.contents == label) {
                        currentNode = subStepper.currentNode;
                        return true;
                }
                subStepper.Advance();
            }
            return false;   //Invalid label
        }

        public bool Finished() {
            if (currentNode.children.Count > 0) {
                return false;
            }
            if (currentNode.parent.children.IndexOf(currentNode) != currentNode.parent.children.Count - 1) {
                return false;
            }
            if (null != currentNode.parent.parent) {
                int indexOfParent = currentNode.parent.parent.children.IndexOf(currentNode.parent);
                if (indexOfParent < currentNode.parent.parent.children.Count - 1) {
                    return false;
                }
            }
            return true;
        }

        public bool Advance() {
            if (currentNode.children.Count > 0) {
                currentNode = currentNode.children[0];
                return true;    //advanced to first child of current node
            }
            if (currentNode.parent.children.IndexOf(currentNode) != currentNode.parent.children.Count - 1) {
                int indexOfNode = currentNode.parent.children.IndexOf(currentNode);
                currentNode = currentNode.parent.children[indexOfNode + 1];
                return true;    //advanced to sibling of current node
            }
            if (null != currentNode.parent.parent) {
                int indexOfParent = currentNode.parent.parent.children.IndexOf(currentNode.parent);
                if (indexOfParent < currentNode.parent.parent.children.Count - 1) {
                    currentNode = currentNode.parent.parent.children[indexOfParent + 1];
                    return true;    //advanced to sibling of parent node
                }
            }
            return false; // end of script, nothing more to parse
        }
    }
}
