using System;

namespace Schematics.UnitTests.Utils
{
    public class NullServiceProvider : IServiceProvider
    {
        public static readonly IServiceProvider Instance = new NullServiceProvider();
        
        private NullServiceProvider()
        {
            
        }
        
        public object GetService(Type serviceType)
        {
            return null;
        }
    }
    
    public class SingleServiceProvider : IServiceProvider
    {
        private object Service { get; }

        public SingleServiceProvider(object service)
        {
            Service = service;
        }
        
        public object GetService(Type serviceType)
        {
            return Service;
        }
    }
}