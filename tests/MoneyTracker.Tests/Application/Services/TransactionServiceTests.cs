using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MoneyTracker.Application.Services;
using MoneyTracker.Domain.Interfaces.Repositories;

namespace MoneyTracker.Tests.Application.Services
{ 
  public class TransactionServiceTests
  {
    private readonly Mock<ITransactionRepository> _repoMock;
    private readonly Mock<ICategoryRepository> _categoryRepoMock;
    private readonly TransactionService _service;

    public TransactionServiceTests()
    {
      _repoMock = new Mock<ITransactionRepository>();
      _categoryRepoMock = new Mock<ICategoryRepository>();
      
      var fakeProducer = new FakeProducerWrapper();

      _service = new TransactionService(_repoMock.Object, _categoryRepoMock.Object, fakeProducer);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenRepositoryReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repoMock
            .Setup(r => r.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }
  }
}