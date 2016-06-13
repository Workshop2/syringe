﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Syringe.Core.Configuration;
using Syringe.Core.Extensions;
using Syringe.Core.Services;
using Syringe.Core.Tests;
using Syringe.Web.Mappers;
using Syringe.Web.Models;


namespace Syringe.Web.Controllers
{
    [AuthorizeWhenOAuth]
    public class TestController : Controller
    {
        private readonly ITestService _testsClient;
        private readonly ITestFileMapper _testFileMapper;

        public TestController(ITestService testsClient, ITestFileMapper testFileMapper)
        {
            _testsClient = testsClient;
            _testFileMapper = testFileMapper;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AddPagingDataForBreadCrumb();
            base.OnActionExecuting(filterContext);
        }

        [HttpGet]
        [EditableTestsRequired]
        public ActionResult Edit(string filename, int position)
        {
            TestFile testFile = _testsClient.GetTestFile(filename);
            TestViewModel model = _testFileMapper.BuildTestViewModel(testFile, position);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [EditableTestsRequired]
        public ActionResult Edit(TestViewModel model)
        {
            if (ModelState.IsValid)
            {
                Test test = _testFileMapper.BuildCoreModel(model);
                _testsClient.EditTest(model.Filename, model.Position, test);
                return RedirectToAction("View", "TestFile", new { filename = model.Filename });
            }

            return View("Edit", model);
        }

        [HttpGet]
        [EditableTestsRequired]
        public ActionResult Add(string filename)
        {
            TestFile testFile = _testsClient.GetTestFile(filename);
            var model = new TestViewModel
            {
                Filename = filename,
                AvailableVariables = _testFileMapper.BuildVariableViewModel(testFile),
                Method = MethodType.GET,
                ExpectedHttpStatusCode = HttpStatusCode.OK
            };

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [EditableTestsRequired]
        public ActionResult Add(TestViewModel model)
        {
            if (ModelState.IsValid)
            {
                Test test = _testFileMapper.BuildCoreModel(model);
                _testsClient.CreateTest(model.Filename, test);
                return RedirectToAction("View", "TestFile", new { filename = model.Filename });
            }

            return View("Edit", model);
        }

        [HttpPost]
        [EditableTestsRequired]
        public ActionResult Delete(int position, string fileName)
        {
            _testsClient.DeleteTest(position, fileName);

            return RedirectToAction("View", "TestFile", new { filename = fileName });
        }

        [HttpPost]
        [EditableTestsRequired]
        public ActionResult Copy(int position, string fileName)
        {
            _testsClient.CopyTest(position, fileName);

            return RedirectToAction("View", "TestFile", new { filename = fileName });
        }

        [EditableTestsRequired]
        public ActionResult AddAssertion()
        {
            return PartialView("EditorTemplates/AssertionViewModel", new AssertionViewModel());
        }

        [EditableTestsRequired]
        public ActionResult AddCapturedVariableItem()
        {
            return PartialView("EditorTemplates/CapturedVariableItem", new CapturedVariableItem());
        }

        [EditableTestsRequired]
        public ActionResult AddHeaderItem()
        {
            return PartialView("EditorTemplates/HeaderItem", new Models.HeaderItem());
        }

        public ActionResult ViewRawFile(string fileName)
        {
            var model = new TestFileViewModel { Filename = fileName, RawFile = _testsClient.GetRawFile(fileName) };
            return View("ViewRawFile", model);
        }

        private void AddPagingDataForBreadCrumb()
        {
            // Paging support for the breadcrumb trail
            ViewBag.PageNumber = Request.QueryString["pageNumber"];
            ViewBag.NoOfResults = Request.QueryString["noOfResults"];
        }
    }
}