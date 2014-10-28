/**
 * Copyright (C) 2014 Microsoft Corporation
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package org.apache.reef.client.parameters;

import org.apache.reef.client.FailedJob;
import org.apache.reef.runtime.common.client.defaults.DefaultFailedJobHandler;
import org.apache.reef.tang.annotations.Name;
import org.apache.reef.tang.annotations.NamedParameter;
import org.apache.reef.wake.EventHandler;

/**
 * Client EventHandler triggered on remote job failure.
 */
@NamedParameter(doc = "Client EventHandler triggered on remote job failure.",
    default_classes = DefaultFailedJobHandler.class)
public final class JobFailedHandler implements Name<EventHandler<FailedJob>> {
  private JobFailedHandler() {
  }
}