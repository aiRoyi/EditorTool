using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataCompair: NodeData
    {
        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {
            switch (nodeNumber)
            {
                case "I01":
                    PreviousNode = nodeGuidStr;
                    break;
                case "I02":
                    Val1InputNode = nodeGuidStr;
                    break;
                case "I03":
                    Val2InputNode = nodeGuidStr;
                    break;
            }
        }
        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
            switch (nodeNumber)
            {
                case "O01":
                    NextNode = nodeGuidStr;
                    break;
                case "O02":
                    TrueOutputNode = nodeGuidStr;
                    break;
                case "O03":
                    FalseOutputNode = nodeGuidStr;
                    break;
            }
        }

        [JsonIgnore]
        public string PreviousNode { get; set; }
        [JsonIgnore]
        public string Val1InputNode { get; set; }
        [JsonIgnore]
        public string Val2InputNode { get; set; }
        public string Compair { get; set; }
        [JsonIgnore]
        public string TrueOutputNode { get; set; }
        [JsonIgnore]
        public string FalseOutputNode { get; set; }
        [JsonIgnore]
        public string NextNode { get; set; }

        private static Dictionary<string, string> CompareSymbolLookup = new Dictionary<string, string>()
        {
            { "Greater", ">" },
            { "Lesser", "<" },
            { "Equal", "==" },
            { "NotEqual", "!=" },
            { "And", "and" },
            { "Or", "or"},
            { "GreaterOrEqual", ">=" },
            { "LesserOrEqual", "<=" }
        };

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {
            //condition
            StartOneLine(level, ref script);
            script += "if (";
            NodeData val1InputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == Val1InputNode);
            val1InputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            script += ") ";
            script += CompareSymbolLookup[Compair];
            script += " (";
            NodeData val2InputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == Val2InputNode);
            val2InputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            script += "):\n";

            level++;

            //true
            NodeData TrueOutputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == TrueOutputNode);
            TrueOutputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);

            level--;


            //false
            NodeData FalseOutputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == FalseOutputNode);
            if (FalseOutputNodeData != null)
            {
                StartOneLine(level, ref script);
                script += "else:\n";
                level++;
                FalseOutputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
                level--;
            }

            NodeData nextNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == NextNode);
            if (nextNodeData != null)
            {
                nextNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            }
        }
    }
}
