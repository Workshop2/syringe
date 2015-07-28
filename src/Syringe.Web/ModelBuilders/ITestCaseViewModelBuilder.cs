﻿using System.Collections.Generic;
using Syringe.Core;
using Syringe.Web.Models;

namespace Syringe.Web.ModelBuilders
{
    public interface ITestCaseViewModelBuilder
    {
        TestCaseViewModel BuildTestCase(Case testCase);
        IEnumerable<TestCaseViewModel> BuildTestCases(CaseCollection caseCollection);
    }
}