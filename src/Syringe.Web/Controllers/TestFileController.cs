﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Syringe.Core.Security;
using Syringe.Core.Services;
using Syringe.Core.TestCases;
using Syringe.Web.Models;

namespace Syringe.Web.Controllers
{
    public class TestFileController : Controller
    {
        private readonly ICaseService _casesClient;
        private readonly IUserContext _userContext;

        public TestFileController(ICaseService casesClient, IUserContext userContext)
        {
            _casesClient = casesClient;
            _userContext = userContext;
        }

        // GET: TestFile
        public ActionResult Add()
        {
            TestFileViewModel model = new TestFileViewModel();

            return View(model);
        }

        // GET: TestFile
        [HttpPost]
        public ActionResult Add(TestFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var caseCollection = new CaseCollection
                {
                    Filename = model.Filename,
                    Variables = model.Variables != null ? model.Variables.ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>()
                };

                var createdTestFile = _casesClient.CreateTestFile(caseCollection, _userContext.TeamName);
                if (createdTestFile)
                    return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: TestFile
        public ActionResult Update(string fileName)
        {
            var testCaseCollection = _casesClient.GetTestCaseCollection(fileName, _userContext.TeamName);

            TestFileViewModel model = new TestFileViewModel
            {
                Filename = fileName,
                Variables =
                    testCaseCollection.Variables.Select(x => new VariableItem {Key = x.Key, Value = x.Value}).ToList()
            };

            return View(model);
        }

        // GET: TestFile
        [HttpPost]
        public ActionResult Update(TestFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var caseCollection = new CaseCollection
                {
                    Filename = model.Filename,
                    Variables = model.Variables != null ? model.Variables.ToDictionary(x => x.Key, x => x.Value) : new Dictionary<string, string>()
                };

                var updateTestFile = _casesClient.UpdateTestFile(caseCollection, _userContext.TeamName);
                if (updateTestFile)
                    return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public ActionResult AddVariableItem(VariableItem model)
        {
            var item = new VariableItem
            {
                Key = model.Key,
                Value = model.Value
            };

            return PartialView("EditorTemplates/VariableItem", item);
        }
    }
}