Feature: Adopting multiple puppies
  In order to have the puppies chew my furniture
  As a pet owner,
  I want to adopt two puppies

@Wip
  Scenario: Adopting multiple puppies from the website.
    Given that I am at the website "http://localhost:3001"
    When I adopt for these pets, 
	  | petName    |
      | Brook      |
      | Hanna      |
      | Maggie Mae |
    And I pay for the adoption using this type of payment:
      | paymentType | orderName   | orderAddress    | orderEmail         |
      | Check       | Mr. Baloney | 123 Main Street | baloney@sleezy.com |
    Then I should be back at the main page with a thank you note, "Thank you for adopting a puppy!" 

