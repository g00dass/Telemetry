/*
 * Statistics API
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using RestSharp;
using Xunit;

using Telemetry.Client.Client;
using Telemetry.Client.Api;
using Telemetry.Client.Model;

namespace Telemetry.Client.Test
{
    /// <summary>
    ///  Class for testing StatisticsApi
    /// </summary>
    /// <remarks>
    /// This file is automatically generated by OpenAPI Generator (https://openapi-generator.tech).
    /// Please update the test case below to test the API endpoint.
    /// </remarks>
    public class StatisticsApiTests : IDisposable
    {
        private StatisticsApi instance;

        public StatisticsApiTests()
        {
            instance = new StatisticsApi("https://localhost:32775/");
        }

        public void Dispose()
        {
            // Cleanup when everything is done.
        }

        /// <summary>
        /// Test an instance of StatisticsApi
        /// </summary>
        [Fact]
        public void InstanceTest()
        {
            // TODO uncomment below to test 'IsType' StatisticsApi
            //Assert.IsType<StatisticsApi>(instance);
        }

        /// <summary>
        /// Test StatisticsApiAppInfoAllGet
        /// </summary>
        [Fact]
        public void StatisticsApiAppInfoAllGetTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            var response = instance.StatisticsApiAppInfoAllGet();
            Assert.IsType<List<AppInfo>>(response);
        }

        /// <summary>
        /// Test StatisticsApiAppInfoIdEventsHistoryGet
        /// </summary>
        [Fact]
        public void StatisticsApiAppInfoIdEventsHistoryGetTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //Guid id = null;
            //var response = instance.StatisticsApiAppInfoIdEventsHistoryGet(id);
            //Assert.IsType<List<StatisticsEvent>>(response);
        }

        /// <summary>
        /// Test StatisticsApiAppInfoIdGet
        /// </summary>
        [Fact]
        public void StatisticsApiAppInfoIdGetTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //Guid id = null;
            //var response = instance.StatisticsApiAppInfoIdGet(id);
            //Assert.IsType<AppInfo>(response);
        }

        /// <summary>
        /// Test StatisticsApiAppInfoPost
        /// </summary>
        [Fact]
        public void StatisticsApiAppInfoPostTest()
        {
            // TODO uncomment below to test the method and replace null with proper value
            //AppInfoRequest appInfoRequest = null;
            //instance.StatisticsApiAppInfoPost(appInfoRequest);
        }
    }
}
