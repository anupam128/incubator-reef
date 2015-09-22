﻿// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.Apache.REEF.Client.Yarn.RestClient;
using Org.Apache.REEF.Tang.Implementations.Tang;

namespace Org.Apache.REEF.Client.Tests
{
    [TestClass]
    public class WindowsHadoopEmulatorYarnClientTests
    {
        /// <summary>
        /// TestInit here checks if the required hadoop services are running on the local machine.
        /// If the the services are not available, tests will be marked inconclusive.
        /// </summary>
        [TestInitialize]
        public void TestInit()
        {
            ServiceController[] serviceControllers = ServiceController.GetServices();
            IEnumerable<string> actualServices = serviceControllers.Select(x => x.ServiceName);

            string[] expectedServices = {"datanode", "namenode", "nodemanager", "resourcemanager"};

            bool allServicesExist = expectedServices.All(expectedService => actualServices.Contains(expectedService));

            if (!allServicesExist)
            {
                Assert.Inconclusive(
                    "At least some required windows services not installed. " +
                    "Two possible ways: HDInsight Emulator or HortonWorks Data Platform for Windows. " +
                    "Required services: " + string.Join(", ", expectedServices));
            }

            bool allServicesRunning = expectedServices.All(
                expectedService =>
                {
                    ServiceController controller = serviceControllers.First(x => x.ServiceName == expectedService);
                    return controller.Status == ServiceControllerStatus.Running;
                });

            if (!allServicesRunning)
            {
                Assert.Inconclusive("At least some required windows services are not running. " +
                                    "Required services: " + string.Join(", ", expectedServices));
            }
        }

        [TestMethod]
        [TestCategory("Functional")]
        public void TestGetClusterInfo()
        {
            var client = TangFactory.GetTang().NewInjector().GetInstance<IYarnRMClient>();

            var clusterInfo = client.GetClusterInfoAsync().GetAwaiter().GetResult();

            Assert.IsNotNull(clusterInfo);
            Assert.AreEqual("STARTED", clusterInfo.State);
            Assert.IsFalse(string.IsNullOrEmpty(clusterInfo.HaState));
            Assert.IsTrue(clusterInfo.StartedOn > 0);
        }

        [TestMethod]
        [TestCategory("Functional")]
        public void TestGetClusterMetrics()
        {
            var client = TangFactory.GetTang().NewInjector().GetInstance<IYarnRMClient>();

            var clusterMetrics = client.GetClusterMetricsAsync().GetAwaiter().GetResult();

            Assert.IsNotNull(clusterMetrics);
            Assert.IsTrue(clusterMetrics.TotalMB > 0);
            Assert.IsTrue(clusterMetrics.ActiveNodes > 0);
        }

        //// TODO: [REEF-757] Once submit API is in place, submit an app then get the details
        ////[TestMethod]
        ////[TestCategory("Functional")]
        ////public void TestGetApplication()
        ////{
        ////    const string applicationName = @"application_1440795762187_0001";

        ////    var client = TangFactory.GetTang().NewInjector().GetInstance<IYarnRMClient>();

        ////    var application = client.GetApplicationAsync(applicationName).GetAwaiter().GetResult();

        ////    Assert.IsNotNull(application);
        ////}

        [TestMethod]
        [TestCategory("Functional")]
        public void TestErrorResponse()
        {
            const string WrongApplicationName = @"Something";

            var client = TangFactory.GetTang().NewInjector().GetInstance<IYarnRMClient>();

            try
            {
                client.GetApplicationAsync(WrongApplicationName).GetAwaiter().GetResult();
                Assert.Fail("Should throw YarnRestAPIException");
            }
            catch (YarnRestAPIException)
            {
            }
        }
    }
}