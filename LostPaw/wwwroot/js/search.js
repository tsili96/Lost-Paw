document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const clearSearchButton = document.getElementById("clearSearch");
    const searchForm = document.querySelector("form");

    // check if the input has a value
    if (searchInput.value) {
        clearSearchButton.style.display = "inline-block"; // Show the clear button if there is a search term
    }

    // changes in the search input
    searchInput.addEventListener("input", function () {
        // visibility of the clear button based on the input value
        if (searchInput.value) {
            clearSearchButton.style.display = "inline-block"; // Show the clear button if there's input
        } else {
            clearSearchButton.style.display = "none"; // Hide it when the input is empty
        }
    });

    // Clear the search input and reload the page when the 'X' is clicked
    clearSearchButton.addEventListener("click", function () {
        searchInput.value = ""; // Clear the input field
        searchForm.submit();    // Submit the form to reload the page without the search term
    });
});
