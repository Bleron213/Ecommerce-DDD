﻿using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Ecommerce.API.Common.Base
{
    public class ResponseResult
    {
        public readonly bool IsSuccessStatusCode;
        public readonly HttpStatusCode StatusCode;
        public readonly string Content;
        public readonly ProblemDetails ProblemDetails;
        public ResponseResult(bool isSuccessStatusCode, HttpStatusCode statusCode, string httpContent)
        {
            IsSuccessStatusCode = isSuccessStatusCode;
            StatusCode = statusCode;
            Content = httpContent;

            if (!isSuccessStatusCode)
            {

                try
                {
                    if (!string.IsNullOrEmpty(httpContent))
                    {
                        ProblemDetails = JsonSerializer.Deserialize<ProblemDetails>(httpContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })!;
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("Failed to deserialize json: ", ex);
                }

            }
        }
    }

    public class ResponseResult<T1> : ResponseResult where T1 : new()
    {
        public readonly T1 Data;
        public ResponseResult(bool isSuccessStatusCode, HttpStatusCode statusCode, string httpContent) : base(isSuccessStatusCode, statusCode, httpContent)
        {
            if (isSuccessStatusCode)
            {
                Data = JsonSerializer.Deserialize<T1>(httpContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
        }
    }
}
