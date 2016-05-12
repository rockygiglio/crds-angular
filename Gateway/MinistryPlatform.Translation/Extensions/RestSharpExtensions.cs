using System;
using System.Net;
using RestSharp;

namespace MinistryPlatform.Translation.Extensions
{
    public static class RestSharpExtensions
    {
        public static bool IsError(this IRestResponse response, bool errorNotFound = false)
        {
            return (response.ResponseStatus != ResponseStatus.Completed
                    || (errorNotFound && response.StatusCode == HttpStatusCode.NotFound)
                    || response.StatusCode == HttpStatusCode.Unauthorized
                    || response.StatusCode == HttpStatusCode.BadRequest
                    || response.StatusCode == HttpStatusCode.PaymentRequired);
        }

        public static void CheckForErrors(this IRestResponse response, string errorMessage, bool errorNotFound = false)
        {
            if (!IsError(response, errorNotFound))
            {
                return;
            }

            throw new RestResponseException(errorMessage, response);
        }
    }

    public class RestResponseException : Exception
    {
        public IRestResponse Response { get; }

        public RestResponseException(string message, IRestResponse response) : base($"{message} - Status: {response.ResponseStatus}, Status Code: {response.StatusCode}, Error: {response.ErrorMessage}, Content: {response.Content}", response.ErrorException)
        {
            Response = response;
        }
    }
}
