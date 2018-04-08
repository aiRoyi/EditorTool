﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public class NodeDataNumberInput: NodeData
    {
        public override void AddInputNode(string nodeGuidStr, string nodeNumber)
        {

        }
        public override void AddOutputNode(string nodeGuidStr, string nodeNumber)
        {
            OutputNode = nodeGuidStr;
        }

        public float NumberInput { get; set; }
        [JsonIgnore]
        public string OutputNode { get; set; }

        public override void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script)
        {
            script += NumberInput;
        }
    }
}
