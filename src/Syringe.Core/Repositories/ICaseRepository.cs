﻿using System.Collections.Generic;
using Syringe.Core.TestCases;

namespace Syringe.Core.Repositories
{
    public interface ICaseRepository
    {
	    IEnumerable<string> ListCasesForTeam(string teamName);
		CaseCollection GetTestCaseCollection(string filename, string teamName);
        CaseCollection GetPagedTestCaseCollection(string filename, string teamName, int pageNumber, int take);
        Case GetTestCase(string filename, string teamName, int caseId);
		bool SaveTestCase(Case testCase, string teamName);
    }
}