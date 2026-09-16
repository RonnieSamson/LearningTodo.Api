using LearningTodo.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Xunit;


namespace LearningTodo.Api.Tests
{
    public class TodoItemsIntegrationTests
    {

        [Fact]
        public async Task GetAll_ShouldReturnOkAndEmptyList()
        {
            //ARRANGE

            await using var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

            //ACT

            var response = await client.GetAsync("/api/todoitems", TestContext.Current.CancellationToken);

            //ASSERT

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

            Assert.Equal("[]", content);
        }

    }
}
