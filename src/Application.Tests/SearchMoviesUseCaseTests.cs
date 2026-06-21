using Application.DTOs;
using Application.UseCases.SearchMovies;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace Application.Tests;

public class SearchMoviesUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldReturnMovies_WhenRepositoryReturnsData()
    {
        // Arrange
        var mockRepository = new Mock<IMovieRepository>();
        mockRepository
            .Setup(r => r.SearchAsync("batman", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Movie>
            {
                new Movie { ImdbId = "tt0372784", Title = "Batman Begins", Year = "2005", Type = "movie", Poster = "N/A" }
            });

        var useCase = new SearchMoviesUseCase(mockRepository.Object);
        var query = new SearchMoviesQuery("batman");

        // Act
        var result = await useCase.ExecuteAsync(query);

        // Assert
        Assert.Single(result);
        Assert.Equal("Batman Begins", result[0].Title);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnEmptyList_WhenQueryIsEmpty()
    {
        // Arrange
        var mockRepository = new Mock<IMovieRepository>();
        var useCase = new SearchMoviesUseCase(mockRepository.Object);
        var query = new SearchMoviesQuery("");

        // Act
        var result = await useCase.ExecuteAsync(query);

        // Assert
        Assert.Empty(result);
        mockRepository.Verify(r => r.SearchAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}