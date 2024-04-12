using Castle.Core.Logging;
using CleanArch.Application.DTOs;
using CleanArch.Application.Interfaces;
using CleanArch.Application.Mappings;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Interfaces;
using CleanArch.Domain.Validation;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Xml.Linq;

namespace CleanArch.Tests.Application
{
    public class CategoryServiceUnitTest1
    {
        private readonly ServiceProvider _serviceProvider;
        Mock<ICategoryRepository> mockRepository = new Mock<ICategoryRepository>();
        public CategoryServiceUnitTest1()
        {
            // Configurar o serviço e o contêiner de injeção de dependência para os testes
            var services = new ServiceCollection();

            // Mock do repository


            services.AddSingleton(mockRepository.Object);
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void GetCategories_WithValidState_ResultObjectValidState()
        {
            var categories = new List<Category>{
              new Category(1, "Material Escolar"),
              new Category(2, "Eletrônicos")
            };
            mockRepository.Setup(repo => repo.GetCategories()).ReturnsAsync(categories); // Simula o retorno de uma entidade
            // Resolução de dependência no escopo do teste
            using (var scope = _serviceProvider.CreateScope())
            {
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                var entities = myService.GetCategories();
                Assert.NotNull(entities);
            }
        }

        [Fact]
        public async void GetCategories_WithEmptyList_ResultObjectValidState()
        {
            int countExpected = 0;
            var categories = new List<Category>
            {
                //new Category(1, "Material Escolar"),
                //new Category(2, "Eletrônicos")
            };
            mockRepository.Setup(repo => repo.GetCategories()).ReturnsAsync(categories); // Simula o retorno de uma entidade
            // Resolução de dependência no escopo do teste
            using (var scope = _serviceProvider.CreateScope())
            {
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                var entities = await myService.GetCategories();
                Assert.Equal(countExpected, entities.Count());
            }
        }


        [Fact]
        public async Task CreateCategory_WithValidParameter_ResultObjectValidState()
        {
            // Arrange
            var expected = new CategoryDTO { Id = 1, Name = "Test Success" };
            var category = new Category(1, "Test Success");


            // Act
            using (var scope = _serviceProvider.CreateScope())
            {
                mockRepository.Setup(repo => repo.Create(category)).ReturnsAsync(category);
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                var actual = await myService.Add(expected);

                // Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async void CreateCategory_InvalidEntity_returnNull()
        {
            CategoryDTO categoryInsert = null;
            CategoryDTO expected = null;
            var category = new Category(1, "Test Success");


            // Act
            using (var scope = _serviceProvider.CreateScope())
            {
                mockRepository.Setup(repo => repo.Create(category)).ReturnsAsync(category);
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                var actual = await myService.Add(categoryInsert);

                // Assert
                actual.Should().BeNull();
            }
        }

        [Fact]
        public async void GetById_WithValidParameter__ResultObjectValidState()
        {
            int? idToSerch = 1;
            CategoryDTO expected = new CategoryDTO { Id = 1, Name = "Teste" };
            Category category = new Category(1, "Teste");


            // Act
            using (var scope = _serviceProvider.CreateScope())
            {
                mockRepository.Setup(repo => repo.GetById(idToSerch)).ReturnsAsync(category);
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                var actual = await myService.GetById(idToSerch);

                // Assert
                actual.Should().BeEquivalentTo(expected);
            }
        }

        [Fact]
        public async void GetById_WithInvalidParameter_ResultNull()
        {
            int? idToSerch = null;
            CategoryDTO expected = null;
            Category category = null;


            // Act
            using (var scope = _serviceProvider.CreateScope())
            {
                mockRepository.Setup(repo => repo.GetById(idToSerch)).ReturnsAsync(category);
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                var actual = await myService.GetById(idToSerch);

                // Assert
                actual.Should().BeNull();
            }
        }

        [Fact]
        public async Task Update_WithValidParameter_ResultObjectValidState()
        {
            // Arrange
            CategoryDTO expected = new CategoryDTO { Id = 1, Name = "Test Success" };
            Category category = new Category(1, "Test Success");


            // Act
            using (var scope = _serviceProvider.CreateScope())
            {
                mockRepository.Setup(repo => repo.Update(category)).ReturnsAsync(category);
                mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(category);
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                await myService.Update(expected);
                var actual = await myService.GetById(1);

                // Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

        [Fact]
        public async Task Update_WithInvalidParameter_ResultObjectValidState()
        {

            ServiceProvider _serviceProvider;
            Mock<ICategoryRepository> mockRepository = new Mock<ICategoryRepository>();
            // Configurar o serviço e o contêiner de injeção de dependência para os testes
            var services = new ServiceCollection();

            // Mock do repository


            services.AddSingleton(mockRepository.Object);
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            _serviceProvider = services.BuildServiceProvider();



            // Arrange
            CategoryDTO expected = new CategoryDTO { Id = 99, Name = "Test Success" };
            Category category = null;// new Category(1, "Test Success");


            // Act
            using (var scope = _serviceProvider.CreateScope())
            {
                mockRepository.Setup(repo => repo.Update(category)).ReturnsAsync(category);
                mockRepository.Setup(repo => repo.GetById(99)).ReturnsAsync(category);
                var myService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

                // Obter uma entidade
                await myService.Update(expected);
                mockRepository.Verify(m => m.Update(category), Times.Once);
                var actual = await myService.GetById(99);

                // Assert
                actual.Should().BeEquivalentTo(expected);

            }
        }

    }
}
