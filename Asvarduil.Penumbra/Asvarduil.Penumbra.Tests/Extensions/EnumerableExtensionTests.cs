using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asvarduil.Penumbra.Services.Extensions;

namespace Asvarduil.Penumbra.Tests.Extensions
{
    [TestClass]
    public class EnumerableExtensionTests
    {
        [TestMethod]
        public void IsNullOrEmptyCorrectlyDetectsEmpty()
        {
            var set = new List<int>();
            Assert.IsTrue(set.IsNullOrEmpty());
        }
    }
}
