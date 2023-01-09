using System;
using RestSharp;
using System.IO;
using System.Collections.Generic;
using AventStack.ExtentReports;

namespace RestSharpAPI
{
    class APIs
    {
        private string PositiveFlow = "Positive";
        private string pathToExcelFiles = "Configurations/";
        private string ExcelFileName = "AppConfiguration.xlsx";
        ExtentTest TestMaster;

        Helper HelpObj = new Helper();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private string environment = string.Empty;
        private string build_number = string.Empty;
        private string build_ID = string.Empty;
        private string build_username = string.Empty;
        private string path = string.Empty;
        private string target = string.Empty;
        private string reportPath = string.Empty;
        private string screenshot_path = string.Empty;
        private string log_path = string.Empty;
        private string log_name = string.Empty;
        private string AppConfiguration_ExcelFileName = string.Empty;
        private Report_Method ReportMethods;
        public void Run()
        {

           

            List<DataCollection> dataCol = ExcelLib.PopulateInCollection(pathToExcelFiles + ExcelFileName, PositiveFlow);
            HelpObj.InitializeLogger();
            HelpObj.InitializeReportingFrameWork();
             TestMaster = HelpObj.getReportObj().CreateTest("RestSharp Cases Positive");

            RestMethods obj = new RestMethods();
            for (int i = 1; i <= dataCol[0].totalRowCount; i++)

            { string TestCaseID = ExcelLib.ReadData(dataCol, i, "TestCaseID");
                string RequestType = ExcelLib.ReadData(dataCol, i, "RequestType");
                string RequestURL = ExcelLib.ReadData(dataCol, i, "RequestURL");
                string RequestBody = ExcelLib.ReadData(dataCol, i, "RequestBody");


                if (!String.IsNullOrEmpty(TestCaseID))
                {

                    ExtentTest test = TestMaster.CreateNode("Amazon Flow").AssignCategory("Amazon");


                    try
                    {


                        HelpObj.LogInfo("TestCase ID: " + TestCaseID);


                        if (RequestType == "POST")
                        {
                            IRestResponse postreq = obj.CallPostAPIRequest(RequestURL, RequestBody);
                            var responsePost = postreq.Content;
                            logger.Info("Information about Post URL : " + RequestURL + Environment.NewLine +
                                "Body: " + RequestBody + Environment.NewLine +
                                 "Response: " + responsePost + Environment.NewLine);
                            if (responsePost== "I'm a Teapot")
                            { HelpObj.ReportPass("Success for RequestType" + responsePost, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for RequestType" + responsePost, test, TestCaseID); }
                        }

                        if (RequestType == "GET")
                        {
                            IRestResponse gettreq = obj.CallGetAPIRequest(RequestURL);
                            var responseGET = gettreq.Content;
                            logger.Info("Information about Get URL : " + RequestURL + Environment.NewLine +
                                 "Gettreq: " + gettreq + Environment.NewLine +
                                  "Response: " + responseGET + Environment.NewLine);
                            Console.WriteLine("Get Request:" + responseGET);
                            if (responseGET == "I'm a Teapot")
                            { HelpObj.ReportPass("Success for RequestType" + responseGET, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for RequestType" + responseGET, test, TestCaseID); }
                        }

                        if (RequestType == "DELETE")
                        {
                            
                            IRestResponse deletereq = obj.CallDeleteAPIRequest(RequestURL);
                            var responsedelete = deletereq.Content;
                            Console.WriteLine("Get Request:" + responsedelete);
                            if (responsedelete == "I'm a Teapot")
                            { HelpObj.ReportPass("Success for RequestType" + responsedelete, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for RequestType" + responsedelete, test, TestCaseID); }
                        }

                        if (RequestType == "PUT")
                        {

                            IRestResponse putreq = obj.CallPostAPIRequest(RequestURL, RequestBody);
                            var responsePost = putreq.Content;
                            logger.Info("Information about Post URL : " + RequestURL + Environment.NewLine +
                                "Body: " + RequestBody + Environment.NewLine +
                                 "Response: " + responsePost + Environment.NewLine);
                            if (responsePost == "I'm a Teapot")
                            { HelpObj.ReportPass("Success for RequestType" + responsePost, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for RequestType" + responsePost, test, TestCaseID); }
                        }
                        HelpObj.ReportFlush();
                    }
                    catch (Exception ex)
                    { HelpObj.ReportFail(ex.ToString(), test, TestCaseID); }
                } 









                    }




            //string PostURL = "https://restful-booker.herokuapp.com/booking";
            //string Body = " {\"firstname\" : \"Jim13\",\"lastname\" : \"Brown\", \"totalprice\" : 111, \"depositpaid\" : true, \"bookingdates\" : " +
            //    "{\"checkin\" : \"2018-01-01\",\"checkout\" : \"2019-01-01\"},\"additionalneeds\" : \"Breakfast\"}";

            //IRestResponse postreq = obj.CallPostAPIRequest(PostURL, Body);
            //var responsePost = postreq.Content;
            //logger.Info("Information about Post URL : " + PostURL + Environment.NewLine +
            //    "Body: " + Body + Environment.NewLine +
            //     "Response: " + responsePost + Environment.NewLine);
            //Console.WriteLine("Post Request:" + responsePost);


           // string Geturl = "https://restful-booker.herokuapp.com/booking/487";
            //IRestResponse gettreq = obj.CallGetAPIRequest(Geturl);
            //var responseGET = gettreq.Content;
            //logger.Info("Information about Get URL : " + Geturl + Environment.NewLine +
            //     "Gettreq: " + gettreq + Environment.NewLine +
            //      "Response: " + responseGET + Environment.NewLine);
            //Console.WriteLine("Get Request:" + responseGET);


            //string deleteurl = "https://restful-booker.herokuapp.com/booking/487";
            //IRestResponse deletereq = obj.CallDeleteAPIRequest(deleteurl);
            //var responsedelete = deletereq.Content;
            //Console.WriteLine("Get Request:" + responsedelete);


            Console.ReadKey();
        }
        void InitializeReportingFrameWork()
        {

            logger.Info("InitializeReportFrameWork : " + DateTime.Now.ToString("HH:mm:ss:ffffff"));
            reportPath = target + "\\" + "Report.html";
            // screenshot_path = target + "\\ScreenShots";
            screenshot_path = target + "";
            string pathToLoadConfig = Directory.GetCurrentDirectory();
            pathToLoadConfig = pathToLoadConfig.Replace("\\bin\\Debug", "\\");
            ReportMethods = new Report_Method(logger, screenshot_path);
            ReportMethods.InitializeExtent(target, reportPath, build_username, build_ID, build_number, environment);
            logger.Info("InitializeReportFrameWork - Completed : " + DateTime.Now.ToString("HH:mm:ss:ffffff"));
        }
    }
}
