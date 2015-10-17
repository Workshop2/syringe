﻿using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Syringe.Core.Configuration;
using Syringe.Core.Services;
using Syringe.Core.TestCases;

namespace Syringe.Client
{
	public class CasesClient : ICaseService
	{
		private readonly string _baseUrl;

		public CasesClient()
			: this(new ApplicationConfig())
		{
		}

		public CasesClient(IApplicationConfiguration appConfig)
		{
			_baseUrl = appConfig.ServiceUrl;
		}

		public IEnumerable<string> ListFilesForTeam(string teamName)
		{
			var client = new RestClient(_baseUrl);
			IRestRequest request = CreateRequest("ListForTeam");
			request.AddParameter("teamName", teamName);

			IRestResponse response = client.Execute(request);
			return DeserializeOrThrow<IEnumerable<string>>(response);
		}

		public Case GetTestCase(string filename, string teamName, int caseId)
		{
			var client = new RestClient(_baseUrl);
			IRestRequest request = CreateRequest("GetTestCase");
			request.AddParameter("filename", filename);
			request.AddParameter("caseId", caseId);
			request.AddParameter("teamName", teamName);

			IRestResponse response = client.Execute(request);
			return DeserializeOrThrow<Case>(response);
		}

		public CaseCollection GetTestCaseCollection(string filename, string teamName)
		{
			var client = new RestClient(_baseUrl);
			IRestRequest request = CreateRequest("GetTestCaseCollection");
			request.AddParameter("filename", filename);
			request.AddParameter("teamName", teamName);

			IRestResponse response = client.Execute(request);

			return DeserializeOrThrow<CaseCollection>(response);
		}

        public bool EditTestCase(Case testCase, string teamName)
		{
			var client = new RestClient(_baseUrl);
			IRestRequest request = CreateRequest("EditTestCase");
			request.Method = Method.POST;
			request.AddJsonBody(testCase);
			request.AddQueryParameter("teamName", teamName);

			IRestResponse response = client.Execute(request);
			return DeserializeOrThrow<bool>(response);
		}

        public bool CreateTestCase(Case testCase, string teamName)
        {
            var client = new RestClient(_baseUrl);
            IRestRequest request = CreateRequest("CreateTestCase");
            request.Method = Method.POST;
            request.AddJsonBody(testCase);
            request.AddQueryParameter("teamName", teamName);

            IRestResponse response = client.Execute(request);
            return DeserializeOrThrow<bool>(response);
        }

	    public bool DeleteTestCase(int testCaseId, string fileName, string teamName)
	    {
            var client = new RestClient(_baseUrl);
            IRestRequest request = CreateRequest("DeleteTestCase");
            request.Method = Method.POST;
            request.AddQueryParameter("fileName", fileName);
            request.AddQueryParameter("teamName", teamName);
            request.AddQueryParameter("testCaseId", testCaseId.ToString());

            IRestResponse response = client.Execute(request);
            return DeserializeOrThrow<bool>(response);
        }

	    public bool CreateTestFile(CaseCollection caseCollection, string teamName)
	    {
            var client = new RestClient(_baseUrl);
            IRestRequest request = CreateRequest("CreateTestFile");
            request.Method = Method.POST;
            request.AddJsonBody(caseCollection);
            request.AddQueryParameter("fileName", caseCollection.Filename);
            request.AddQueryParameter("teamName", teamName);

            IRestResponse response = client.Execute(request);
            return DeserializeOrThrow<bool>(response);
        }

	    public bool UpdateTestFile(CaseCollection caseCollection, string teamName)
	    {
            var client = new RestClient(_baseUrl);
            IRestRequest request = CreateRequest("UpdateTestFile");
            request.Method = Method.POST;
            request.AddJsonBody(caseCollection);
            request.AddQueryParameter("teamName", teamName);

            IRestResponse response = client.Execute(request);
            return DeserializeOrThrow<bool>(response);
        }

	    private T DeserializeOrThrow<T>(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
			{
				return JsonConvert.DeserializeObject<T>(response.Content);
			}

            throw new Exception(response.Content);
        }

	    private IRestRequest CreateRequest(string action)
		{
			return new RestRequest(string.Format("/api/cases/{0}", action));
		}
	}
}