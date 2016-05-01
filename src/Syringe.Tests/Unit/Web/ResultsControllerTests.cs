﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Syringe.Core.Helpers;
using Syringe.Core.Services;
using Syringe.Core.Tasks;
using Syringe.Core.Tests.Results;
using Syringe.Web.Controllers;
using Syringe.Web.Models;

namespace Syringe.Tests.Unit.Web
{
    [TestFixture]
    public class ResultsControllerTests
    {
        private int _id;
        private Mock<ITasksService> _tasksServiceMock;
        private Mock<IUrlHelper> _urlHelperMock;
        private Mock<ITestService> _testsClient;

        private ResultsController _resultsController;

        [SetUp]
        public void Setup()
        {
            _id = 0;
            _tasksServiceMock = new Mock<ITasksService>();
            _urlHelperMock = new Mock<IUrlHelper>();

            _testsClient = new Mock<ITestService>();
            _testsClient.Setup(x => x.GetResultById(It.IsAny<Guid>())).Returns(new TestFileResult());
            _testsClient.Setup(x => x.GetSummaries(It.IsAny<DateTime>(),It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(new TestFileResultSummaryCollection()));

            _tasksServiceMock.Setup(x => x.GetRunningTaskDetails(It.IsAny<int>())).Returns((new TaskDetails { Results = new List<TestResult> { new TestResult { ActualUrl = "syringe.com", Position = _id } } }));
            _resultsController = new ResultsController(_tasksServiceMock.Object, _urlHelperMock.Object, _testsClient.Object);
        }

        [Test]
        public void Html_should_return_404_not_found_if_test_doesnt_exist()
        {
            // given
            _tasksServiceMock.Setup(x => x.GetRunningTaskDetails(It.IsAny<int>())).Returns((new TaskDetails { Results = new List<TestResult>() }));
            
            // when
            var actionResult = _resultsController.Html(It.IsAny<int>(), It.IsAny<int>()) as HttpStatusCodeResult;

            // then
            Assert.AreEqual((int)HttpStatusCode.NotFound, actionResult.StatusCode);
            Assert.AreEqual("Could not locate the specified test.", actionResult.StatusDescription);
        }

        [Test]
        public void Html_should_return_correct_model()
        {
            // given + when
            var actionResult = _resultsController.Html(It.IsAny<int>(), _id) as ViewResult;

            // then
            Assert.IsInstanceOf<ResultsViewModel>(actionResult.Model);
            _tasksServiceMock.Verify(x=>x.GetRunningTaskDetails(It.IsAny<int>()),Times.Once);
        }

        [Test]
        public void Raw_should_return_404_not_found_if_test_doesnt_exist()
        {
            // given
            _tasksServiceMock.Setup(x => x.GetRunningTaskDetails(It.IsAny<int>())).Returns((new TaskDetails { Results = new List<TestResult>() }));

            // when
            var actionResult = _resultsController.Raw(It.IsAny<int>(), It.IsAny<int>()) as HttpStatusCodeResult;

            // then
            Assert.AreEqual((int)HttpStatusCode.NotFound, actionResult.StatusCode);
            Assert.AreEqual("Could not locate the specified test.", actionResult.StatusDescription);
        }

        [Test]
        public void Raw_should_return_correct_model()
        {
            // given + when
            var actionResult = _resultsController.Raw(It.IsAny<int>(), _id) as ViewResult;

            // then
            Assert.IsInstanceOf<ResultsViewModel>(actionResult.Model);
            _tasksServiceMock.Verify(x => x.GetRunningTaskDetails(It.IsAny<int>()), Times.Once);
        }


        [Test]
        public void Index_should_return_correct_view_and_model()
        {
            // given + when
            var viewResult = _resultsController.Index().Result as ViewResult;

            // then
            _testsClient.Verify(x => x.GetSummaries(It.IsAny<DateTime>(),It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.IsInstanceOf<TestFileResultSummaryCollection>(viewResult.Model);
        }

        [Test]
        public void Today_should_return_correct_view_and_model()
        {
            // given + when
            var viewResult = _resultsController.Today().Result as ViewResult;

            // then
            _testsClient.Verify(x => x.GetSummaries(It.IsAny<DateTime>(),It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.AreEqual("Index", viewResult.ViewName);
            Assert.IsInstanceOf<TestFileResultSummaryCollection>(viewResult.Model);
        }


        [Test]
        public void ViewResult_should_return_correct_view_and_model()
        {
            // given + when
            var viewResult = _resultsController.ViewResult(It.IsAny<Guid>()) as ViewResult;

            // then
            _testsClient.Verify(x => x.GetResultById(It.IsAny<Guid>()), Times.Once);
            Assert.AreEqual("ViewResult", viewResult.ViewName);
            Assert.IsInstanceOf<TestFileResult>(viewResult.Model);
        }

        [Test]
        public async void DeleteResult_should_call_delete_methods_and_redirect_to_correct_action()
        {
            // given + when
            var redirectToRouteResult = await _resultsController.Delete(It.IsAny<Guid>()) as RedirectToRouteResult;

            // then
            _testsClient.Verify(x => x.GetResultById(It.IsAny<Guid>()), Times.Once);
            _testsClient.Verify(x => x.DeleteResultAsync(It.IsAny<Guid>()), Times.Once);
            Assert.AreEqual("Index", redirectToRouteResult.RouteValues["action"]);
        }
    }
}
