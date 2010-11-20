using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace IronPython.Web
{
    public class IronPythonHttpHandlerFactory : IHttpHandlerFactory
    {
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (!File.Exists(pathTranslated))
                throw new HttpException(404, "File not found");

            // create ScriptEngine
            var engine = Python.CreateEngine();

            // process python code, create class object 
            var scope = engine.ExecuteFile(pathTranslated);

            // get the class object
            var className = Path.GetFileNameWithoutExtension(pathTranslated);
            var clazz = scope.GetVariable(className);

            // create the instance
            return (IHttpHandler)engine.Operations.Invoke(clazz);
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
