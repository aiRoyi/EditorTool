using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataBooleanInput: NodeData
    {
        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {
        }

        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
            OutputNode = nodeGuidStr;
        }

        private static Dictionary<string, string> BooleanLookup = new Dictionary<string, string>()
        {
            { "True", "True" },
            { "False", "False" }
        };

        public string TrueAndFalse { get; set; }

        [JsonIgnore]
        public string OutputNode { get; set; }

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {
            script += BooleanLookup[TrueAndFalse];
        }
    }
}
