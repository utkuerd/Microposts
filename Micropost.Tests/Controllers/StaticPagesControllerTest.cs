using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Micropost;
using Micropost.Controllers;

namespace Micropost.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Home()
        {
            // Arrange
            StaticPagesController controller = new StaticPagesController();

            // Act
            ViewResult result = controller.Home() as ViewResult;

            // Assert
            Assert.IsNotNull(result);            
        }

        [TestMethod]
        public void Help()
        {
            // Arrange
            StaticPagesController controller = new StaticPagesController();

            // Act
            ViewResult result = controller.Help() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            StaticPagesController controller = new StaticPagesController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        //[TestMethod]
        //public void Contact()
        //{
        //    // Arrange
        //    StaticPagesController controller = new StaticPagesController();

        //    // Act
        //    ViewResult result = controller.Contact() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}
    }
}
