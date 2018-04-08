using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataTrigger: NodeData
    {
        public string Session { get; set; }
        [JsonIgnore]
        public string OutputNode { get; set; }

        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {

        }
        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
            OutputNode = nodeGuidStr;
        }

        private static Dictionary<string, string> TriggerNameLookup = new Dictionary<string, string>()
        {
            { "Init", "__init__" },
        };

        private static Dictionary<string, List<string>> TriggerParameterNamesLookup = new Dictionary<string, List<string>>()
        {
            { "Init", new List<string>() },
        };

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {
            StartOneLine(level, ref script);

            if (TriggerNameLookup.ContainsKey(Session))
            {
                List<string> parameterNames = TriggerParameterNamesLookup[Session];
                script += "def " + TriggerNameLookup[Session] + "(self";
                for (int i = 0; i < parameterNames.Count; i++)
                {
                    script += ", ";
                    script += parameterNames[i];
                }
                script += "):\n";
                level++;
            }
            else
            {
                script += "def " + Session.ToLower() + "(self):\n";
                level++;
            }

            NodeData outputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == OutputNode);
            if (outputNodeData != null)
            {
                outputNodeData.GenerateScriptCode(nodeDatas, ref level, ref script);
            }
            else
            {
                StartOneLine(level, ref script);
                script += "pass\n";
            }
        }
    }
}
