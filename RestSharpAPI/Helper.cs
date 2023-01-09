using System;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog.Internal;
using System.Threading;
using AventStack.ExtentReports;

namespace RestSharpAPI
{
    class Helper
    {
        public void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
              
                foreach (string file in Directory.GetFiles(path))
                {
                    File.Delete(file);

                }
                
                foreach (string directory in Directory.GetDirectories(path))
                {
                    DeleteDirectory(directory);
                }
               
                Directory.Delete(path);
                Thread.Sleep(400);
            }
        }
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        // Helper objHelper = new Helper();
        private Report_Method ReportMethods;
        string path;
        string target= Directory.GetCurrentDirectory();
        private Report_Method reportObj;
         
        public void InitializeLogger()
        {
            path = Directory.GetCurrentDirectory();
            target = path + "\\Log_Report\\" + DateTime.Now.ToString("ddmmyyyyhhmmss");
            DeleteDirectory(target);

            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = target + "\\log_${date:format=yyyyMMdd}.log" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);
            config.AddRule(NLog.LogLevel.Error, NLog.LogLevel.Fatal, logconsole);

            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);
            config.AddRule(NLog.LogLevel.Error, NLog.LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
        }
        public void LogInfo(String info)
        {
            logger.Info(info);
        }
        public void LogError(String Error)
        {
            logger.Error(Error);
        }


        #region Report
        public void InitializeReportingFrameWork()
        {

            logger.Info("InitializeReportFrameWork : " + DateTime.Now.ToString("HH:mm:ss:ffffff"));
            string reportPath = target + "\\" + "Report.html";
            // screenshot_path = target + "\\ScreenShots";
           string screenshot_path = target + "";
            string pathToLoadConfig = Directory.GetCurrentDirectory();
            pathToLoadConfig = pathToLoadConfig.Replace("\\bin\\Debug", "\\");
            ReportMethods = new Report_Method(logger, screenshot_path);
            ReportMethods.InitializeExtent(target, reportPath, "", "", "", "");
            logger.Info("InitializeReportFrameWork - Completed : " + DateTime.Now.ToString("HH:mm:ss:ffffff"));
        }
        
        public Report_Method getReportObj()
        {
            return reportObj=ReportMethods;
        }
        public void ReportInfo(string info, ExtentTest test, string TestCaseID)
        {
            LogInfo(info);
            reportObj.testInfo(test, TestCaseID, info);
        }
        public void ReportPass(string info, ExtentTest test, string TestCaseID)
        {
            LogInfo(info);
            reportObj.testPass(test, TestCaseID, info);
        }
        public void ReportFail(string Error, ExtentTest test, string TestCaseID)
        {
            LogError(Error);
            reportObj.testFail(test, TestCaseID, Error);
        }
        

        public void ReportFlush()
        {
            reportObj.flushExtent();
        }
        #endregion    
    }
}
