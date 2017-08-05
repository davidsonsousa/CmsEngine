﻿using AutoMapper;
using CmsEngine.Data.AccessLayer;
using CmsEngine.Data.EditModels;
using CmsEngine.Data.Models;
using CmsEngine.Data.ViewModels;
using CmsEngine.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CmsEngine.Tests.Fixtures
{
    public class WebsiteFixture
    {
        private Mock<IRepository<Website>> moqRepository;
        public Mock<IRepository<Website>> MoqRepository
        {
            get { return moqRepository; }
        }

        private Mock<IUnitOfWork> moqUnitOfWork;
        public Mock<IUnitOfWork> MoqUnitOfWork
        {
            get { return moqUnitOfWork; }
        }

        private WebsiteService service;
        public WebsiteService Service
        {
            get { return service; }
        }

        private Mock<IMapper> moqMapper;
        public Mock<IMapper> MoqMapper
        {
            get { return moqMapper; }
        }

        public WebsiteFixture()
        {
            SetupRepository();
            SetupUnitOfWork();
            SetupMapper();

            service = new WebsiteService(moqUnitOfWork.Object, moqMapper.Object);
        }

        /// <summary>
        /// Returns a list of websites
        /// </summary>
        public List<Website> GetTestWebsites()
        {
            return new List<Website>
                {
                    new Website { Id = 1, VanityId = new Guid("278c0380-bdd2-45bb-869b-b94659bc2b89"), Name = "Website1", Culture="en-US", Description="Welcome to website 1", IsDeleted = false },
                    new Website { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture="pt-BR", Description="Welcome to website 2", IsDeleted = false }
                };
        }

        /// <summary>
        /// Returns the EditModel of Id 2
        /// </summary>
        /// <returns></returns>
        public WebsiteEditModel GetEditModel()
        {
            return new WebsiteEditModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture = "pt-BR", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Returns the ViewModel of Id 2
        /// </summary>
        /// <returns></returns>
        public WebsiteViewModel GetViewModel()
        {
            return new WebsiteViewModel { Id = 2, VanityId = new Guid("8633a850-128f-4425-a2ec-30e23826b7ff"), Name = "Website2", Culture = "pt-BR", Description = "Welcome to website 2" };
        }

        /// <summary>
        /// Setup the Repository instance and its returning values
        /// </summary>
        /// <returns></returns>
        private void SetupRepository()
        {
            moqRepository = new Mock<IRepository<Website>>();
            moqRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Website, bool>>>())).Returns(GetTestWebsites().AsQueryable());
            moqRepository.Setup(x => x.GetReadOnly(It.IsAny<Expression<Func<Website, bool>>>())).Returns(GetTestWebsites());
        }

        /// <summary>
        /// Setup the UnitOfWork instance
        /// </summary>
        /// <param name="moqRepository"></param>
        /// <returns></returns>
        private void SetupUnitOfWork()
        {
            moqUnitOfWork = new Mock<IUnitOfWork>();
            moqUnitOfWork.Setup(x => x.GetRepository<Website>()).Returns(MoqRepository.Object);
        }

        /// <summary>
        /// Setup Mapper instance
        /// </summary>
        private void SetupMapper()
        {
            moqMapper = new Mock<IMapper>();
            moqMapper.Setup(x => x.Map<Website, WebsiteEditModel>(It.IsAny<Website>())).Returns(GetEditModel());
            moqMapper.Setup(x => x.Map<Website, WebsiteViewModel>(It.IsAny<Website>())).Returns(GetViewModel());
        }
    }
}