using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataReturnInformation: NodeData
    {
        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {
            switch (nodeNumber)
            {
                case "I01":
                    PreviousNode = nodeGuidStr;
                    break;
                case "I02":
                    ValueInputNode = nodeGuidStr;
                    break;
            }
        }
        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
        }

        [JsonIgnore]
        public string PreviousNode { get; set; }
        [JsonIgnore]
        public string ValueInputNode { get; set; }

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {
            StartOneLine(level, ref script);
            script += "return ";
            NodeData valueInputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == ValueInputNode);
            valueInputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            script += "\n";
        }
    }
}
