# MovieCatalogProject

## Overview
MovieCatalogProject is a test automation project built using NUnit and Selenium WebDriver to test the Movie Catalog web application. The project validates functionalities such as adding, editing, marking, and deleting movies in the catalog.

## Technologies Used
- C#
- NUnit
- Selenium WebDriver
- ChromeDriver

## Test Cases Implemented
### 1. Add Movie Without Title Test
- Attempts to add a movie without providing a title.
- Validates that an appropriate error message is displayed.

### 2. Add Movie Without Description Test
- Attempts to add a movie without providing a description.
- Ensures that an error message appears as expected.

### 3. Add Random Title Movie With Random Description Test
- Generates random title and description.
- Adds a new movie with generated details.
- Navigates to the last page and verifies that the movie appears in the list.

### 4. Edit Last Added Movie Test
- Navigates to the last added movie.
- Edits the movie title and verifies that changes are successfully saved.

### 5. Mark Last Added Movie as Watched Test
- Navigates to the last added movie.
- Marks it as watched.
- Ensures that the movie appears in the watched section.

### 6. Delete Last Movie Test
- Navigates to the last added movie.
- Deletes the movie.
- Confirms that a success message appears.

## Setup and Execution
### Prerequisites
- Install [ChromeDriver](https://sites.google.com/chromium.org/driver/)
- Install NUnit framework
- Install Selenium WebDriver package

### How to Run
1. Clone the repository.
2. Open the `MovieCatalogProject.sln` in Visual Studio.
3. Build the solution to restore dependencies.
4. Run the NUnit tests using Test Explorer.

## Future Enhancements
- Implement Page Object Model (POM) for better maintainability.
- Expand test coverage to include negative test cases.
- Add support for running tests in multiple browsers.

## Contributors
- **Martin** (Maintainer & Developer)

---
This project ensures the reliability of the Movie Catalog web application by automating its core functionalities through robust test cases. 🚀

