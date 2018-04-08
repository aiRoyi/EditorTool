using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataCreateArray: NodeData
    {
        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {
            PreviousNode = nodeGuidStr;
        }

        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
            NextNode = nodeGuidStr;
        }

        [JsonIgnore]
        public string PreviousNode { get; set; }

        public string ArrayName { get; set; }

        [JsonIgnore]
        public string NextNode { get; set; }

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {

            StartOneLine(level, ref script);
            script += (ArrayName + " = []");
            script += "\n";

            NodeData nextNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == NextNode);
            if (nextNodeData != null)
            {
                nextNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            }
        }
    }
}
