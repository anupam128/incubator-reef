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

using Org.Apache.REEF.Tang.Annotations;

namespace Org.Apache.REEF.Client.YARN
{
    /// <summary>
    /// Provide the command to be submitted to RM for execution of .NET driver.
    /// </summary>
    [DefaultImplementation(typeof(WindowsYarnJobCommandBuilder))]
    public interface IYarnJobCommandBuilder
    {
        /// <summary>
        /// Sets the max memory for Java driver in MegaBytes
        /// </summary>
        /// <returns></returns>
        IYarnJobCommandBuilder SetMaxDriverAllocationPoolSizeMB(int sizeMB);

        /// <summary>
        /// Sets the maximum permgen size for Java driver in MegaBytes
        /// </summary>
        /// <param name="sizeMB"></param>
        /// <returns></returns>
        IYarnJobCommandBuilder SetDriverMaxPermSizeMB(int sizeMB);

        /// <summary>
        /// Builds the command to be submitted to YARNRM
        /// </summary>
        /// <returns>Command</returns>
        string GetJobSubmissionCommand();
    }
}