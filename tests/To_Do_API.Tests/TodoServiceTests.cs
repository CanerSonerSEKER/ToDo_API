using Microsoft.Extensions.Logging;
using Moq;
using To_Do_API.Repository;
using To_Do_API.Services;
using To_Do_API.Models;
using To_Do_API.Exceptions;

namespace To_Do_API.Tests
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _mockRepository;
        private readonly Mock<ILogger<TodoService>> _mockLogger;
        private readonly TodoService _todoService;

        public TodoServiceTests()
        {
            _mockRepository = new Mock<ITodoRepository>();
            _mockLogger = new Mock<ILogger<TodoService>>();
            _todoService = new TodoService(_mockLogger.Object, _mockRepository.Object);
        }


        [Fact]
        public async Task GetById_FoundTodoItem_ReturnsTodoItem()
        {
            // Arrange -- 
            var mockOrder = _mockRepository
                .Setup(r => r.GetByIdAsync(1, 1))
                .ReturnsAsync(new TodoItem { Id = 1, Name = "Test", UserId = 1 });

            // Act
            var result = await _todoService.GetByIdAsync(1, 1);

            // Assert -- Dökümantasyonda önerilmiyor.
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.Name);
        }


        [Fact]
        public async Task GetById_RepositoryReturnsNull_ThrowsNotFoundException() 
        {
            // Arrange -- 
            var mockOrder = _mockRepository
                .Setup(r => r.GetByIdAsync(1, 1))
                .ReturnsAsync((TodoItem?)null);

            // Act

            // Assert 
            await Assert.ThrowsAsync<NotFoundException>(() => _todoService.GetByIdAsync(1,1));
        }






    }
}
