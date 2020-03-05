using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyIoC
{
    [TestClass]
    class TestClass
    {
        [TestMethod]
        public void Sample()
        {
            var container = new Container();
            container.AddAssembly(Assembly.GetExecutingAssembly());
            container.AddType(typeof(CustomerBLL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));
            var customerBLL = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
            Assert.IsNotNull(customerBLL);
        }

        [TestMethod]
        public void Sample2()
        {
            var container = new Container();
            container.AddAssembly(Assembly.GetExecutingAssembly());
            var customerBLL2 = container.CreateInstance<CustomerBLL>();
            Assert.IsNotNull(customerBLL2);
        }
    }
}
