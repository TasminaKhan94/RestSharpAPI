using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAPI
{
    class Program
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
     
      
        static void Main(string[] args)
        {
           



            APIs obj = new APIs();
            obj.Run();
        }


       
       
        
    }
}
