﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Syringe.Core.Http;
using Syringe.Core.Tests;
using Syringe.Core.Tests.Results;
using Syringe.Core.Tests.Results.Repositories;

namespace Syringe.Tests.Integration.Core.Repository.LiteDb
{
	public class TestFileResultRepositoryTests
	{
		private LiteDbTestFileRepository _repository;

		[SetUp]
		public void SetUp()
		{
			_repository = new LiteDbTestFileRepository();
			_repository.Wipe();
		}

		[TearDown]
		public void TearDown()
		{
			_repository.Dispose();	
		}

		private TestFileResult GetDummyTestFileResult()
		{
			string seed = "-" + Guid.NewGuid();

			return new TestFileResult()
			{
				Environment = $"dev{seed}",
				TestResults = new List<TestResult>()
				{
					new TestResult()
					{
						ActualUrl = $"url{seed}",
						ExceptionMessage = $"message{seed}",
						HttpContent = $"content{seed}",
						Test = new Test(),
						HttpLog = $"http log{seed}",
						HttpResponse = new HttpResponse()
						{
							Content = $"content{seed}",
							ResponseTime = TimeSpan.FromMinutes(1),
							StatusCode = HttpStatusCode.OK,
							Headers = new List<HttpHeader>()
							{
								new HttpHeader("key1",$"value1{seed}"),
								new HttpHeader("key2",$"value2{seed}")
							}
						},
						AssertionResults = new List<Assertion>()
						{
							new Assertion()
							{
								AssertionMethod = AssertionMethod.CSQuery,
								AssertionType = AssertionType.Negative,
								Description = $"desc{seed}",
								Log = $"log{seed}",
								Success = true,
								TransformedValue = $"tran{seed}",
								Value = $"value{seed}"
							}
						}
					}
				}
			};
		}

		[Test]
		public async Task Add_should_save_testfileresult()
		{
			// given
			TestFileResult testFileResult = GetDummyTestFileResult();

			// when
			await _repository.AddAsync(testFileResult);

			// then
			IEnumerable<TestFileResult> allItems =  _repository.GetAll();
			Assert.That(allItems.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task Delete_should_remove_the_testfileresult()
		{
			// given
			TestFileResult testFileResult = GetDummyTestFileResult();

			await _repository.AddAsync(testFileResult);

			// when
			await _repository.DeleteAsync(testFileResult.Id);

			// then
			IEnumerable<TestFileResult> allItems = _repository.GetAll();
			Assert.That(allItems.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task GetById_should_return_testfileresult()
		{
			// given
			TestFileResult testFileResult = GetDummyTestFileResult();

			await _repository.AddAsync(testFileResult);

			// when
			TestFileResult actualFileResult = _repository.GetById(testFileResult.Id);

			// then
			Assert.That(actualFileResult, Is.Not.Null, "couldn't find the test file result");
			string actual = GetAsJson(actualFileResult);
			string expected = GetAsJson(testFileResult);

			Assert.That(actual, Is.EqualTo(expected), actual);
		}

		[Test]
		public async Task GetSummaries_should_return_testfileresults()
		{
			// given
			TestFileResult testFileResult1 = GetDummyTestFileResult();
			TestFileResult testFileResult2 = GetDummyTestFileResult();

			await _repository.AddAsync(testFileResult1);
			await _repository.AddAsync(testFileResult2);

			// when
			TestFileResultSummaryCollection summaries = await _repository.GetSummaries(It.IsAny<DateTime>());

			// then
			Assert.That(summaries.TotalFileResults, Is.EqualTo(2));

			IEnumerable<Guid> ids = summaries.PagedResults.Select(x => x.Id);
			Assert.That(ids, Contains.Item(testFileResult1.Id));
			Assert.That(ids, Contains.Item(testFileResult2.Id));
		}

		[Test]
		public async Task GetSummaries_should_return_testfileresult_objects_for_today_only()
		{
			// given
			TestFileResult todayResult1 = GetDummyTestFileResult();
			TestFileResult todayResult2 = GetDummyTestFileResult();
			TestFileResult otherTestResult1 = GetDummyTestFileResult();
			TestFileResult otherTestResult2 = GetDummyTestFileResult();

			todayResult1.StartTime = DateTime.Today;
			todayResult1.EndTime = todayResult1.StartTime.AddMinutes(5);

			todayResult2.StartTime = DateTime.Today.AddHours(1);
			todayResult2.EndTime = todayResult2.StartTime.AddMinutes(5);

			otherTestResult1.StartTime = DateTime.Today.AddDays(-2);
			otherTestResult1.EndTime = otherTestResult1.StartTime.AddMinutes(10);

			otherTestResult2.StartTime = DateTime.Today.AddDays(-2);
			otherTestResult2.EndTime = otherTestResult2.StartTime.AddMinutes(10);

			await _repository.AddAsync(todayResult1);
			await _repository.AddAsync(todayResult2);
			await _repository.AddAsync(otherTestResult1);
			await _repository.AddAsync(otherTestResult2);

			// when
			TestFileResultSummaryCollection summaries = await _repository.GetSummaries(DateTime.Today);

			// then
			Assert.That(summaries.TotalFileResults, Is.EqualTo(2));

			IEnumerable<Guid> ids = summaries.PagedResults.Select(x => x.Id);
			Assert.That(ids, Contains.Item(todayResult1.Id));
			Assert.That(ids, Contains.Item(todayResult2.Id));
		}

		// JSON.NET customisations

		private string GetAsJson(object o)
		{
			return JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings
			{
				// Stop JSON.NET from serializing getter properties
				ContractResolver = new WritablePropertiesOnlyResolver(),

				// Stop JSON.NET putting dates as UTC, as the Z breaks the asserts
				Converters = new List<JsonConverter>() { new JavaScriptDateTimeConverter() }
			});
		}

		private class WritablePropertiesOnlyResolver : DefaultContractResolver
		{
			protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
			{
				IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
				return props.Where(p => p.Writable).ToList();
			}
		}
	}
}