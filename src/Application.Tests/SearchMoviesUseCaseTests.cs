using Application.UseCases.SearchMovies;
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
            .Setup(r => r.SearchAsync(It.IsAny<MovieSearchFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Movie>
            {
                new Movie { ImdbId = "tt0372784", Title = "Batman Begins", Year = "2005", Type = "movie", Poster = "N/A" }
            }, 1));

        var useCase = new SearchMoviesUseCase(mockRepository.Object);
        var filter = new MovieSearchFilter { Query = "batman" };

        // Act
        var result = await useCase.ExecuteAsync(filter);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value!);
        Assert.Equal("Batman Begins", result.Value![0].Title);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFailure_WhenQueryIsEmpty()
    {
        // Arrange
        var mockRepository = new Mock<IMovieRepository>();
        var useCase = new SearchMoviesUseCase(mockRepository.Object);
        var filter = new MovieSearchFilter { Query = "" };

        // Act
        var result = await useCase.ExecuteAsync(filter);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Search query cannot be empty", result.Error);
    }
}