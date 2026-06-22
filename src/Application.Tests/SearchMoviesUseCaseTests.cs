using Application.UseCases.Movies;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests;

public class SearchMoviesUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnSuccess_WhenRepositoryReturnsData()
    {
        // Arrange
        var mockRepository = new Mock<IMovieRepository>();
        mockRepository
            .Setup(r => r.SearchAsync(It.IsAny<MovieFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Movie>
            {
                new Movie { ImdbId = "tt0372784", Title = "Batman Begins", Year = "2005", Type = "movie", Poster = "N/A" }
            }, 1));

        var useCase = new SearchMoviesUseCase(mockRepository.Object);
        var filter = new MovieFilter { Text = "batman" };

        // Act
        var result = await useCase.ExecuteAsync(filter);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value!);
        Assert.Equal("Batman Begins", result.Value![0].Title);
    }
}