using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebAPI.Helpers;

namespace WebAPITest
{
    [TestClass]
    public class HttpHelperTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_Empty_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.GetAsync(string.Empty);

            //Assert
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_Null_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.GetAsync(null);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Post_EmptyUrl_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.PostAsync(string.Empty, new object());

            //Assert
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Post_NullUrl_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.PostAsync(null, new object());

            //Assert
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Post_NullObject_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.PostAsync<object>("PRETEND_THIS_IS_A_URL", null);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Put_EmptyUrl_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.PutAsync(string.Empty, new object());

            //Assert
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Put_NullUrl_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.PutAsync(null, new object());

            //Assert
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Put_NullObject_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.PutAsync<object>("PRETEND_THIS_IS_A_URL", null);

            //Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Empty_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.DeleteAsync(string.Empty);

            //Assert
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Null_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            HttpHelper.Client.DeleteAsync(null);

            //Assert
        }
    }
}
