// Autogenerated by SignalrTypescriptGenerator.exe at 12/04/2016 21:33:16
// Get signalr.d.ts.ts from https://github.com/borisyankov/DefinitelyTyped (or delete the reference)
/// <reference path="signalr/signalr.d.ts" />
/// <reference path="jquery/jquery.d.ts" />

// Hubs
interface SignalR
{
	taskMonitorHub : Syringe.Service.Api.Hubs.TaskMonitorHub;
}

// Service contracts

declare module Syringe.Service.Api.Hubs
{

	interface TaskMonitorHub
	{
		server : Syringe.Service.Api.Hubs.TaskMonitorHubServer;
		client : Syringe.Service.Api.Hubs.ITaskMonitorHubClient;
	}

	interface TaskMonitorHubServer
	{
		startMonitoringTask(taskId : number) : JQueryPromise<Syringe.Service.Api.Hubs.TaskState>;
	}
}


// Clients

declare module Syringe.Service.Api.Hubs
{
	interface ITaskMonitorHubClient
	{
		onTaskCompleted : (taskInfo : Syringe.Service.Api.Hubs.CompletedTaskInfo) => void;
	}
}


// Data contracts

declare module Syringe.Service.Api.Hubs
{
	interface CompletedTaskInfo
	{
		ActualUrl : string;
		ResultId : number;
		Success : boolean;
		HttpResponse : Syringe.Core.Http.HttpResponse;
		Position : number;
		ExceptionMessage : string;
		Verifications : Syringe.Core.Tests.Assertion[];
	}
}


declare module Syringe.Core.Tests
{
	interface Assertion
	{
		Description : string;
		Regex : string;
		TransformedRegex : string;
		Success : boolean;
		AssertionType : Syringe.Core.Tests.AssertionType;
		Log : string;
	}
}


declare module Syringe.Core.Http
{
	interface HttpResponse
	{
		StatusCode : System.Net.HttpStatusCode;
		Content : string;
		Headers : System.Collections.Generic.KeyValuePair_String_String_[];
		ResponseTime : System.TimeSpan;
	}
}


declare module System
{
	interface TimeSpan
	{
		Ticks : number;
		Days : number;
		Hours : number;
		Milliseconds : number;
		Minutes : number;
		Seconds : number;
		TotalDays : number;
		TotalHours : number;
		TotalMilliseconds : number;
		TotalMinutes : number;
		TotalSeconds : number;
	}
}


declare module System.Collections.Generic
{
	interface KeyValuePair_String_String_
	{
		Key : string;
		Value : string;
	}
}


declare module Syringe.Service.Api.Hubs
{
	interface TaskState
	{
		TotalTests : number;
	}
}


// Enums

declare module System.Net
{
	enum HttpStatusCode
	{
		Continue = 100,
		SwitchingProtocols = 101,
		OK = 200,
		Created = 201,
		Accepted = 202,
		NonAuthoritativeInformation = 203,
		NoContent = 204,
		ResetContent = 205,
		PartialContent = 206,
		MultipleChoices = 300,
		Ambiguous = 300,
		MovedPermanently = 301,
		Moved = 301,
		Found = 302,
		Redirect = 302,
		SeeOther = 303,
		RedirectMethod = 303,
		NotModified = 304,
		UseProxy = 305,
		Unused = 306,
		TemporaryRedirect = 307,
		RedirectKeepVerb = 307,
		BadRequest = 400,
		Unauthorized = 401,
		PaymentRequired = 402,
		Forbidden = 403,
		NotFound = 404,
		MethodNotAllowed = 405,
		NotAcceptable = 406,
		ProxyAuthenticationRequired = 407,
		RequestTimeout = 408,
		Conflict = 409,
		Gone = 410,
		LengthRequired = 411,
		PreconditionFailed = 412,
		RequestEntityTooLarge = 413,
		RequestUriTooLong = 414,
		UnsupportedMediaType = 415,
		RequestedRangeNotSatisfiable = 416,
		ExpectationFailed = 417,
		UpgradeRequired = 426,
		InternalServerError = 500,
		NotImplemented = 501,
		BadGateway = 502,
		ServiceUnavailable = 503,
		GatewayTimeout = 504,
		HttpVersionNotSupported = 505,
	}
}


declare module Syringe.Core.Tests
{
	enum AssertionType
	{
		Negative = 0,
		Positive = 1,
	}
}


