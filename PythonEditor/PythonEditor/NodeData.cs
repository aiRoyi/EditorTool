using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor
{
    public abstract class NodeData
    {
        public const string IndentStr = "    ";

        public string DataGuid { get; set; }

        public abstract void AddInputNode(string nodeGuid, string nodeNumber);
        public abstract void AddOutputNode(string nodeGuid, string nodeNumber);
        public abstract void GenerateScriptCode(List<NodeData> nodeDatas, ref int level, ref string script);

        protected void StartOneLine(int level, ref string script)
        {
            for (int i = 0; i < level; i++)
            {
                script += IndentStr;
            }
        }
    }
}
