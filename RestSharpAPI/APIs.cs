using System;
using RestSharp;
using System.IO;
using System.Collections.Generic;
using AventStack.ExtentReports;
using Newtonsoft.Json.Linq;

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
        string jsonData;
        dynamic data;
        string bookingid;
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
                string Token = ExcelLib.ReadData(dataCol, i, "Token");
                string TokenBody = ExcelLib.ReadData(dataCol, i, "TokenBody");
                


                if (!String.IsNullOrEmpty(TestCaseID))
                {

                    ExtentTest test = TestMaster.CreateNode("Amazon Flow").AssignCategory("Amazon");


                    try
                    {


                        HelpObj.LogInfo("TestCase ID: " + TestCaseID);


                        if (RequestType == "POST")
                        {
                            IRestResponse postreq = obj.CallPostAPIRequest(RequestURL, RequestBody);
                            var StatusCode = postreq.StatusCode;
                            if (StatusCode.Equals("OK"))
                            { HelpObj.ReportPass("StatusCode for POST: " + StatusCode, test, TestCaseID); }
                            else { HelpObj.ReportFail("StatusCode for POST :" + StatusCode, test, TestCaseID); }


                            var responsePost = postreq.Content;
                            
                              jsonData = responsePost;
                             data = JObject.Parse(jsonData);
                             bookingid = data.bookingid;

                            logger.Info("Information about Post URL : " + RequestURL + Environment.NewLine +
                                "Body: " + RequestBody + Environment.NewLine +
                                 "Response: " + responsePost + Environment.NewLine);
                            if (responsePost.Contains("bookingid"))
                            { HelpObj.ReportPass("Success for POST" + responsePost, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for POST" + responsePost, test, TestCaseID); }
                        }

                        if (RequestType == "GET")
                        {
                            IRestResponse gettreq = obj.CallGetAPIRequest(RequestURL);
                            var StatusCode = gettreq.StatusCode;
                            if (StatusCode.Equals("OK"))
                            { HelpObj.ReportPass("StatusCode for Request: " + StatusCode, test, TestCaseID); }
                            else { HelpObj.ReportFail("StatusCode for Request :" + StatusCode, test, TestCaseID); }

                            var responseGET = gettreq.Content;
                            logger.Info("Information about Get URL : " + RequestURL + Environment.NewLine +
                                 "Gettreq: " + gettreq + Environment.NewLine +
                                  "Response: " + responseGET + Environment.NewLine);
                            Console.WriteLine("Get Request:" + responseGET);
                            if (responseGET.Contains("Josh") || responseGET.Contains("Not Found"))
                            { HelpObj.ReportPass("Success for GET " + responseGET, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for GET " + responseGET, test, TestCaseID); }
                        }
                        if (RequestType == "PUT")
                        {
                            var tokenValue = TokenCall(Token, TokenBody, test);

                             IRestResponse putreq = obj.CallPostAPIRequest(RequestURL, RequestBody);
                            
                            var responsePost = putreq.Content;
                            logger.Info("Information about Post URL : " + RequestURL + Environment.NewLine +
                                "Body: " + RequestBody + Environment.NewLine +
                                 "Response: " + responsePost + Environment.NewLine);
                            if (responsePost.Contains("bookingid"))
                            { HelpObj.ReportPass("Success for PUT " + responsePost, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for PUT " + responsePost, test, TestCaseID); }
                        }
                        if (RequestType == "DELETE")
                        {
                            var tokenValue = TokenCall(Token, TokenBody, test);
                           
                           

                            IRestResponse deletereq = obj.CallDeleteAPIRequest(RequestURL, tokenValue);
                            var StatusCode = deletereq.StatusCode;
                            if (StatusCode.Equals("OK")  )
                            { HelpObj.ReportPass("StatusCode for Request: " + StatusCode, test, TestCaseID); }
                            else if(StatusCode.Equals("Forbidden"))
                            { HelpObj.ReportPass("StatusCode for Request: " + StatusCode+ " ID already deleted", test, TestCaseID); }
                            else { HelpObj.ReportFail("StatusCode for Request :" + StatusCode, test, TestCaseID); }


                            var responsedelete = deletereq.Content;
                            Console.WriteLine("Get Request:" + responsedelete);
                            if (responsedelete == "Created" || responsedelete == "Forbidden")
                            { HelpObj.ReportPass("Success for DELETE" + responsedelete, test, TestCaseID); }
                            else { HelpObj.ReportFail("Failed for DELETE" + responsedelete, test, TestCaseID); }
                        }

                       
                        HelpObj.ReportFlush();
                    }
                    catch (Exception ex)
                    { HelpObj.ReportFail(ex.ToString(), test, TestCaseID); }
                } 









                    }




            Console.ReadKey();
        }
        string TokenCall( string Token, string TokenBody,ExtentTest test)
        {
            RestMethods obj = new RestMethods();
            IRestResponse tokenReq = obj.CallPostAPIRequest(Token, TokenBody);
            var tokenValue = tokenReq.Content;
            tokenValue = tokenValue.Replace("{\"", "");
            tokenValue = tokenValue.Replace("\"}", "");
            tokenValue = tokenValue.Replace("\":\"", "=");
            HelpObj.ReportPass("Success for token: " + tokenValue, test, "");
            return tokenValue;
        }
    }
}
