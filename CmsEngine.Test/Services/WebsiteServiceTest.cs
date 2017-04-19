using CmsEngine.Data;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.Models;
using CmsEngine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsEngine.Test.Services
{
    [TestClass]
    public class WebsiteServiceTest
    {
        [TestMethod]
        public void Get_All_Websites()
        {
            // Arrange
            var moqWebsiteService = this.SetupWebsiteService();

            // Act
            var response = moqWebsiteService.GetAll();

            // Assert
            Assert.IsTrue(response.Count() == 2);
        }

        [TestMethod]
        public void Get_Website_By_Id()
        {
            // Arrange
            var moqWebsiteService = this.SetupWebsiteService();

            // Act
            var response = moqWebsiteService.GetById(1);

            // Assert
            Assert.IsTrue(response.Name == "Website1");
        }

        /// <summary>
        /// Setup the WebsiteService
        /// </summary>
        /// <returns></returns>
        private WebsiteService SetupWebsiteService()
        {
            var listWebsites = new List<Website>
            {
                new Website { Id = 1, Name = "Website1", Culture="en-US", Description="Welcome to website 1", IsDeleted = false },
                new Website { Id = 2, Name = "Website2", Culture="pt-BR", Description="Welcome to website 2", IsDeleted = false }
            };

            var moqRepository = new Mock<IRepository<Website>>();
            moqRepository.Setup(x => x.Get(q => q.IsDeleted == false)).Returns(listWebsites.AsQueryable());

            var moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Website>()).Returns(moqRepository.Object);

            return new WebsiteService(moqUnitOfWork.Object);
        }
    }
}
