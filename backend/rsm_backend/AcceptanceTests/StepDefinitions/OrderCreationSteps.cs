using AcceptanceTests.Support;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using rsm_backend.Application.DTO;
using rsm_backend.Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests.StepDefinitions
{
    [Binding]
    public class OrderCreationSteps
    {
        private readonly AcceptanceScenarioContext _scenario;

        public OrderCreationSteps(AcceptanceScenarioContext scenario)
        {
            _scenario = scenario;
        }

        [Given("I have a product in my guest cart")]
        public async Task GivenTheCustomerHasAProductInTheirGuestCart()
        {
            await using var db = AcceptanceTestInfrastructure.Database.CreateDbContext();
            var testCart = TestData.CreateValidGuestCart();
            var token = Guid.NewGuid().ToString();

            testCart.GuestCartToken=token;

            await db.Carts.AddAsync(testCart);

            await db.SaveChangesAsync();

            _scenario.GuestCartToken = token;

        }

        [When("I create an order using email {string}")]
        public async Task WhenTheCustomerCreatesAnOrder(string email)
        {
            // Send the request / create the order

            var placeOrderDTO = TestData.CreateValidPlaceOrderDTO();
            placeOrderDTO.Email=email;
            _scenario.Email = email;


            using var request = new HttpRequestMessage(HttpMethod.Post, "/api/Order");

            request.Headers.Add("Cookie", $"GuestCartToken={_scenario.GuestCartToken}");
            request.Content = JsonContent.Create(placeOrderDTO);

            _scenario.Response = await _scenario.Client.SendAsync(request);

            var placeOrderResponse =
             await _scenario.Response.Content.ReadFromJsonAsync<PlaceOrderResponseDTO>();

            _scenario.OrderNumber = placeOrderResponse!.OrderNumber;
        }

        [Then("the response status should be {int}")]
        public void ThenTheResponseStatusShouldBe(int statusCode)
        {
            // Check that the actual status code == statusCode

            var response = _scenario.Response
                        ?? throw new InvalidOperationException("No response was received.");

            ((int)response.StatusCode).Should().Be(statusCode);
        }

        [Then("an order should exist for {string}")]
        public async Task ThenAnOrderShouldExistFor(string email)
        {
            await using var db= AcceptanceTestInfrastructure.Database.CreateDbContext();

           var order= await db.Orders.SingleAsync(o => o.Customer.Email == email);

            order.Should().NotBeNull();
        }

        [Then("a confirmation email should have been sent")]
        public void ThenAConfirmationEmailShouldHaveBeenSent()
        {
            // Check that an email was sent
            AcceptanceTestInfrastructure.ApiFactory._emailService.SentEmails.Should().Contain(x => x.To == _scenario.Email && x.Body.Contains(_scenario.OrderNumber));
        }
    }
}
