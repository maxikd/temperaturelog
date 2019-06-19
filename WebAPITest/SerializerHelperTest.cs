using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebAPI.Helpers;

namespace WebAPITest
{
    [TestClass]
    public class SerializerHelperTest
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Serialize_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            SerializerHelper.Serialize(null);

            //Assert
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void Deserialize_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            SerializerHelper.Deserialize<object>(string.Empty);

            //Assert
        }
    }
}
