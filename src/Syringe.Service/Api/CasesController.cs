﻿using System.Collections.Generic;
using System.Web.Http;
using Syringe.Core;
using Syringe.Core.Repositories;
using Syringe.Core.Services;
using Syringe.Core.TestCases;

namespace Syringe.Service.Api
{
	// TODO: Tests
	public class CasesController : ApiController, ICaseService
	{
		private readonly ICaseRepository _caseRepository;

		public CasesController()
			: this(new CaseRepository())
		{
		}

		internal CasesController(ICaseRepository caseRepository)
		{
			_caseRepository = caseRepository;
		}

		[Route("api/cases/ListForTeam")]
		[HttpGet]
		public IEnumerable<string> ListFilesForTeam(string teamName)
		{
			return _caseRepository.ListCasesForTeam(teamName);
		}

		[Route("api/cases/GetTestCase")]
		[HttpGet]
		public Case GetTestCase(string filename, string teamName, int caseId)
		{
			return _caseRepository.GetTestCase(filename, teamName, caseId);
		}

		[Route("api/cases/GetTestCaseCollection")]
		[HttpGet]
		public CaseCollection GetTestCaseCollection(string filename, string teamName)
		{
			return _caseRepository.GetTestCaseCollection(filename, teamName);
		}
        [Route("api/cases/GetPagedTestCaseCollection")]
        [HttpGet]
        public CaseCollection GetPagedTestCaseCollection(string filename, string teamName, int pageNumber, int take)
	    {
            return _caseRepository.GetPagedTestCaseCollection(filename, teamName,pageNumber,take);
        }

	    [Route("api/cases/AddTestCase")]
		[HttpPost]
		public bool AddTestCase([FromBody]Case testCase, [FromUri]string teamName)
		{
			return _caseRepository.SaveTestCase(testCase, teamName);
		}
	}
}