using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Configuration;
using System.Net;

namespace RestSharpAPI
{
    public class RestMethods
    {

        public IRestResponse CallGetAPIRequest(string APIURL)
        {
            
            var client = new RestClient(APIURL);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            //request.AddHeader("Authorization", "Bearer " + AuthToken);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);

        }

        public IRestResponse CallPostAPIRequest(string APIURL, string BodyData)
        {

            var client = new RestClient(APIURL);
            var request = new RestRequest(Method.POST);
            request.Parameters.Clear();
            //request.AddHeader("Authorization", "Bearer ");
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("Application/Json", BodyData, ParameterType.RequestBody);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);


        }

        public IRestResponse CallDeleteAPIRequest(string APIURL)
        {

            var client = new RestClient(APIURL);
            var request = new RestRequest(Method.DELETE);
            request.Parameters.Clear();
            //request.AddHeader("Authorization", "Bearer ");
            request.AddHeader("Content-Type", "application/json");
   
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);


        }

        public IRestResponse CallUpdateAPIRequest(string APIURL)
        {

            var client = new RestClient(APIURL);
            var request = new RestRequest(Method.PUT);
            request.Parameters.Clear();
            //request.AddHeader("Authorization", "Bearer ");
            request.AddHeader("Content-Type", "application/json");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            return client.Execute(request);
        }


    }
}
