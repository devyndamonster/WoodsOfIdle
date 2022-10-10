using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodsOfIdle
{
    public class DependancyContainer
    {
        Dictionary<Type, object> _dependancies = new Dictionary<Type, object>();
        
        public void Bind<TMatch, TResult>()
        {
            var parameterTypes = typeof(TResult).GetConstructors()[0].GetParameters().Select(parameter => parameter.ParameterType).ToList();
            var parameters = parameterTypes.Select(parameterType => _dependancies[parameterType]).ToArray();
            TResult instance = (TResult)Activator.CreateInstance(typeof(TResult), parameters);
            _dependancies.Add(typeof(TMatch), instance);
        }
        
        public void Bind<TResult>()
        {
            var parameterTypes = typeof(TResult).GetConstructors()[0].GetParameters().Select(parameter => parameter.ParameterType).ToList();
            var parameters = parameterTypes.Select(parameterType => _dependancies[parameterType]).ToArray();
            TResult instance = (TResult)Activator.CreateInstance(typeof(TResult), parameters);
            _dependancies.Add(typeof(TResult), instance);
        }

        public void Bind<TMatch, TResult>(TResult instance)
        {
            _dependancies.Add(typeof(TMatch), instance);
        }

        public void Bind<TResult>(TResult instance)
        {
            _dependancies.Add(typeof(TResult), instance);
        }

    }
}
