using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTrackerTests.Helpers
{
    public class ObjectWithMockedDependency<TObjectType, TMockType>
        where TObjectType : class 
        where TMockType : class
    {
        public ObjectWithMockedDependency()
        {
            var mock = new Mock<TMockType>();
            var objectType = typeof(TObjectType);
            var obj = (TObjectType)Activator.CreateInstance(objectType, mock.Object);

            Object = obj;
            Mock = mock;
        }

        public TObjectType Object { get; set; }
        public Mock<TMockType> Mock { get; set; }
    }
}
