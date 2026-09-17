using LearningTodo.Api.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Xunit;
using LearningTodo.Api.Models;
using System.Net.Http.Json;


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

        [Fact]

        public async Task GetById_ShouldReturnCorrectTodoItem()
        {
            //ARRANGE

            await using var factory = new CustomWebApplicationFactory();
            var client = factory.CreateClient();

            using var scope = factory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.EnsureCreatedAsync(TestContext.Current.CancellationToken);

            var todoItem = new TodoItem
            {
                Title = "Test GetById",
                IsCompleted = false
            };

            dbContext.TodoItems.Add(todoItem);
            await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);


            //ACT
            var response = await client.GetAsync($"/api/todoitems/{todoItem.Id}", TestContext.Current.CancellationToken);

            //ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var returnedTodoItem = await response.Content.ReadFromJsonAsync<TodoItem>(TestContext.Current.CancellationToken);

            Assert.NotNull(returnedTodoItem);

            Assert.Equal(todoItem.Id, returnedTodoItem!.Id);
            Assert.Equal(todoItem.Title, returnedTodoItem.Title);
            Assert.Equal(todoItem.IsCompleted, returnedTodoItem.IsCompleted);
        }



    }
}
