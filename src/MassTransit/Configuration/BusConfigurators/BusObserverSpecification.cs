﻿// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace MassTransit.BusConfigurators
{
    using System;
    using System.Collections.Generic;
    using Builders;
    using Configurators;


    public class BusObserverSpecification :
        IBusFactorySpecification
    {
        readonly Func<IBusObserver> _observerFactory;

        public BusObserverSpecification(Func<IBusObserver> observerFactory)
        {
            _observerFactory = observerFactory;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            if (_observerFactory == null)
                yield return this.Failure("Observer", "must not be null");
        }

        public void Apply(IBusBuilder builder)
        {
            var observer = _observerFactory();

            builder.ConnectBusObserver(observer);
        }
    }
}