using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class ScriptException: Exception
    {
        public string NodeType { get; set; }

        public ScriptException(string nodeType, string message): base(message)
        {
            NodeType = nodeType;
        }

        public override string ToString()
        {
            string prefix = string.IsNullOrEmpty(NodeType) ? "" : (NodeType + ": ");
            return prefix + base.ToString();
        }
    }

    public class NodeLineData
    {
        public string InputNodeGuid { get; set; }
        public string OutputNodeGuid { get; set; }

        public string GetInputNodeGuid()
        {
            int lastIndex = InputNodeGuid.LastIndexOf('-');
            return InputNodeGuid.Substring(0, lastIndex);
        }

        public string GetInputNodeNumber()
        {
            int lastIndex = InputNodeGuid.LastIndexOf('-');
            return InputNodeGuid.Substring(lastIndex + 1);
        }

        public string GetOutputNodeGuid()
        {
            int lastIndex = OutputNodeGuid.LastIndexOf('-');
            return OutputNodeGuid.Substring(0, lastIndex);
        }

        public string GetOutputNodeNumber()
        {
            int lastIndex = OutputNodeGuid.LastIndexOf('-');
            return OutputNodeGuid.Substring(lastIndex + 1);
        }
    }

    public class ScriptData
    {
        public string ClassName { get; set; }

        public List<List<NodeData>> NodeDataSet { get; set; }

        public static ScriptData ParseFromString(string className, string inputStr)
        {

            ScriptData scriptData = new ScriptData()
            {
                ClassName = className,
                NodeDataSet = new List<List<NodeData>>()
            };
            var parsedResults = JsonConvert.DeserializeObject<Dictionary<string, object>>(inputStr);
            foreach (var parsedResultTemp in parsedResults)
            {
                var parsedResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(parsedResultTemp.Value.ToString());
                var nodesResult = JsonConvert.DeserializeObject<List<object>>(parsedResult["NodeDataList"].ToString());
                List<NodeData> nodeDatas = new List<NodeData>();
                foreach (var node in nodesResult)
                {
                    var nodeDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(node.ToString());
                    NodeData nodeData = null;
                    switch (nodeDict["DataType"].ToString())
                    {
                        case "NodeDataBooleanInput":
                            nodeData = JsonConvert.DeserializeObject<NodeDataBooleanInput>(node.ToString());
                            break;
                        case "NodeDataNumberInput":
                            nodeData = JsonConvert.DeserializeObject<NodeDataNumberInput>(node.ToString());
                            break;
                        case "NodeDataTextInput":
                            nodeData = JsonConvert.DeserializeObject<NodeDataTextInput>(node.ToString());
                            break;
                        case "NodeDataAddToMemory":
                            nodeData = JsonConvert.DeserializeObject<NodeDataAddToMemory>(node.ToString());
                            break;
                        case "NodeDataCompair":
                            nodeData = JsonConvert.DeserializeObject<NodeDataCompair>(node.ToString());
                            break;
                        case "NodeDataCreateArray":
                            nodeData = JsonConvert.DeserializeObject<NodeDataCreateArray>(node.ToString());
                            break;
                        case "NodeDataForeach":
                            nodeData = JsonConvert.DeserializeObject<NodeDataForeach>(node.ToString());
                            break;
                        case "NodeDataMath":
                            nodeData = JsonConvert.DeserializeObject<NodeDataMath>(node.ToString());
                            break;
                        case "NodeDataReadArray":
                            nodeData = JsonConvert.DeserializeObject<NodeDataReadArray>(node.ToString());
                            break;
                        case "NodeDataReadFromMemory":
                            nodeData = JsonConvert.DeserializeObject<NodeDataReadFromMemory>(node.ToString());
                            break;
                        case "NodeDataReturnInformation":
                            nodeData = JsonConvert.DeserializeObject<NodeDataReturnInformation>(node.ToString());
                            break;
                    }
                    if (nodeData != null)
                    {
                        nodeDatas.Add(nodeData);
                    }
                }
                var nodeLineDatas = JsonConvert.DeserializeObject<List<NodeLineData>>(parsedResult["NodeLineList"].ToString());
                foreach (var nodeLineData in nodeLineDatas)
                {
                    string inputNodeGuid = nodeLineData.GetInputNodeGuid();
                    string outputNodeGuid = nodeLineData.GetOutputNodeGuid();
                    NodeData inputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == inputNodeGuid);
                    if (inputNodeData != null)
                    {
                        inputNodeData.AddOutputNode(outputNodeGuid, nodeLineData.GetInputNodeNumber());
                    }
                    NodeData outputNodeData = nodeDatas.Find(nodeData => nodeData.DataGuid == outputNodeGuid);
                    if (outputNodeData != null)
                    {
                        outputNodeData.AddInputNode(inputNodeGuid, nodeLineData.GetOutputNodeNumber());
                    }
                }
                scriptData.NodeDataSet.Add(nodeDatas);
            }
            return scriptData;
        }
        public string GenerateScript()
        {
            string script = "";
            script += "class " + ClassName + ": \n";
            foreach (List<NodeData> nodeDatas in NodeDataSet)
            {
                NodeDataTrigger triggerData = null;
                foreach (NodeData nodeData in nodeDatas)
                {
                    if (nodeData is NodeDataTrigger)
                    {
                        triggerData = (NodeDataTrigger)nodeData;
                        break;
                    }
                }
                if (triggerData == null)
                {
                    continue;
                }
                int level = 1;
                script += "\n";
                triggerData.GenerateScriptCode(nodeDatas, ref level, ref script);
            }
            return script;
        }
    }
}
