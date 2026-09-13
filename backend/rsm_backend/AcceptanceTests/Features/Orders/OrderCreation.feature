Feature: Order creation

    As a guest customer
    I want to create an order
    So that I can purchase the products in my cart

    Scenario: Creating an order with products in the cart
        Given I have a product in my guest cart
        When I create an order using email "customer@test.com"
        Then the response status should be 201
        And an order should exist for "customer@test.com"
        And a confirmation email should have been sent