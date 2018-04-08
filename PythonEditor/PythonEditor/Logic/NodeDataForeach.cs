using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataForeach : NodeData
    {
        enum workingMode
        {
            forMode,
            elementMode
        };

        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {
            switch (nodeNumber)
            {
                case "I01":
                    PreviousNode = nodeGuidStr;
                    break;
                case "I02":
                    GroupInputNode = nodeGuidStr;
                    break;
            }

        }
        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
            switch (nodeNumber)
            {
                case "O01":
                    ElementsInputNode.Add(nodeGuidStr);
                    break;
                case "O02":
                    StartNode = nodeGuidStr;
                    break;
                case "O03":
                    NextNode = nodeGuidStr;
                    break;
            }

        }

        [JsonIgnore]
        public string PreviousNode { get; set; }

        [JsonIgnore]
        public string GroupInputNode { get; set; }

        [JsonIgnore]
        public List<string> ElementsInputNode { get; set; } = new List<string>();

        private workingMode CurrentMode = workingMode.forMode;

        [JsonIgnore]
        public string NextNode { get; set; }

        [JsonIgnore]
        public string StartNode { get; set; }

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {
            switch (CurrentMode)
            {
                case workingMode.elementMode:
                    {
                        script += "element";
                        return;
                    }
                case workingMode.forMode:
                    {
                        //the for line
                        StartOneLine(level, ref script);
                        script += "for element in ";
                        NodeData groupInputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == GroupInputNode);
                        groupInputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
                        script += ":\n";

                        level++;

                        NodeData elementInputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == StartNode);
                        CurrentMode = workingMode.elementMode;
                        elementInputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
                        level--;
                        break;
                    }
            }

            NodeData nextNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == NextNode);
            CurrentMode = workingMode.forMode;
            if (nextNodeData != null)
            {
                nextNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            }
        }
    }
}
