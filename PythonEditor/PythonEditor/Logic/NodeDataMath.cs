using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataMath: NodeData
    {
        private static Dictionary<string, string> MathSymbolLookup = new Dictionary<string, string>()
        {
            { "Add", "+" },
            { "Subtract", "-" },
            { "Time", "*" },
            { "Divide", "/" }
        };

        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {
            switch (nodeNumber)
            {
                case "I01":
                    Val1InputNode = nodeGuidStr;
                    break;
                case "I02":
                    Val2InputNode = nodeGuidStr;
                    break;
            }
        }

        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
            OutputNode = nodeGuidStr;
        }

        [JsonIgnore]
        public string Val1InputNode { get; set; }
        [JsonIgnore]
        public string Val2InputNode { get; set; }
        [JsonIgnore]
        public string OutputNode { get; set; }
        public string Math { get; set; }

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {
            script += "(";
            NodeData val1InputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == Val1InputNode);
            val1InputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            script += ") ";
            script += MathSymbolLookup[Math];
            script += " (";
            NodeData val2InputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == Val2InputNode);
            val2InputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            script += ")";
        }
    }
}
