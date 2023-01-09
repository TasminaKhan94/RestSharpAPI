using System;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using AventStack.ExtentReports;
using AventStack.ExtentReports.MarkupUtils;

using AventStack.ExtentReports.Reporter;

namespace RestSharpAPI
{
   

    public class Report_Method
    {
        private NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private ExtentReports extent;
        private string screenshot_path;
        public Report_Method(NLog.Logger logger, string screenshot_path)
        {
            this.logger = logger;
            this.screenshot_path = screenshot_path;
        }
        public ExtentReports getExtent()
        {
            return extent;
        }
        public void flushExtent()
        {
            extent.Flush();
        }
        public ExtentTest CreateTest(string desc)
        {
            return extent.CreateTest(desc);
        }
        public void InitializeExtent(string target, string reportPath, string build_username, string build_ID, string build_number, string environment)
        {
            var reporter = new ExtentHtmlReporter(reportPath);
            extent = new ExtentReports();
            extent.AddSystemInfo("Build username", build_username);
            extent.AddSystemInfo("Build id", build_ID);
            extent.AddSystemInfo("Build number", build_number);
            extent.AddSystemInfo("Report Folder", target);
            extent.AddSystemInfo("Environment", environment);
            extent.AttachReporter(reporter);
        }
      
        public void testFail(ExtentTest test, string test_case_id, string test_fail_details)
        {
            logger.Error("Test Failed: Test Case: " + test_case_id + " <br>" + test_fail_details);
            test.Log(Status.Fail, "Test Case: " + test_case_id + " <br>" + test_fail_details);
            extent.Flush();
        }
        public void testInfo(ExtentTest test, string test_case_id, string test_fail_details)
        {
            logger.Info("Test Info: Test Case: " + test_case_id + " <br>" + test_fail_details);
            test.Log(Status.Info, "Test Case: " + test_case_id + " <br>" + test_fail_details);
            extent.Flush();
        }
        public void testPass(ExtentTest test, string test_case_id, string test_pass_details)
        {
            logger.Info("Test Passed: Test Case: " + test_case_id + " <br>" + test_pass_details);
            test.Log(Status.Pass, "Test Case: " + test_case_id + " <br>" + test_pass_details);
            extent.Flush();
        }
       
    }
}