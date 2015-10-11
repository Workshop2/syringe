﻿using System.Collections.Generic;
using Syringe.Core.TestCases;

namespace Syringe.Core.Services
{
	public interface ICaseService
	{
		IEnumerable<string> ListFilesForTeam(string teamName);
		Case GetTestCase(string filename, string teamName, int caseId);
		CaseCollection GetTestCaseCollection(string filename, string teamName);
	    CaseCollection GetPagedTestCaseCollection(string filename, string teamName, int pageNumber, int take);
        bool AddTestCase(Case testCase, string teamName);
	}
}