﻿// Copyright 2007-2016 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Pipeline.Pipes
{
    using System;
    using System.Threading.Tasks;
    using Filters;


    public class ManagementPipe :
        IManagementPipe
    {
        readonly MessageTypeConsumeFilter _filter;
        readonly IPipe<ConsumeContext> _pipe;

        public ManagementPipe()
        {
            _filter = new MessageTypeConsumeFilter();
            _pipe = Pipe.New<ConsumeContext>(x => x.UseFilter(_filter));
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            var scope = context.CreateScope("managementPipe");

            _pipe.Probe(scope);
        }

        Task IPipe<ConsumeContext>.Send(ConsumeContext context)
        {
            return _pipe.Send(context);
        }

        ConnectHandle IConsumePipeConnector.ConnectConsumePipe<T>(IPipe<ConsumeContext<T>> pipe)
        {
            return _filter.ConnectConsumePipe(pipe);
        }

        ConnectHandle IRequestPipeConnector.ConnectRequestPipe<T>(Guid requestId, IPipe<ConsumeContext<T>> pipe)
        {
            return _filter.ConnectRequestPipe(requestId, pipe);
        }
    }
}