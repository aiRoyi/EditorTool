using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonEditor.Services
{
    [Route("/script/generate")]
    public class GenerateScriptRequest
    {
        public string ClassName { get; set; }
        public string SessionData { get; set; }
    }

    public class GenerateScriptResponse
    {
        public string ErrorMsg { get; set; }
        public string Script { get; set; }
    }

    public class ScriptService : Service
    {
        public GenerateScriptResponse Post(GenerateScriptRequest request)
        {
            string cardScript;
            try
            {
                ScriptData cardScriptData = ScriptData.ParseFromString(request.ClassName, request.SessionData);
                cardScript = cardScriptData.GenerateScript();
            }
            catch (Exception e)
            {
                return new GenerateScriptResponse()
                {
                    ErrorMsg = e.ToString()
                };
            }
            GenerateScriptResponse response = new GenerateScriptResponse()
            {
                ErrorMsg = "",
                Script = cardScript
            };
            return response;
        }
    }
}
