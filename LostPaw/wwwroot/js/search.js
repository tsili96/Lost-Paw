document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchInput");
    const clearSearchButton = document.getElementById("clearSearch");

    if (!searchInput || !clearSearchButton) return;

    // Show the clear button if there's text initially
    if (searchInput.value) {
        clearSearchButton.style.display = "inline-block";
    }

    // Toggle clear button based on input changes
    searchInput.addEventListener("input", function () {
        clearSearchButton.style.display = searchInput.value ? "inline-block" : "none";
    });

    // 👇 This is where you put it
    clearSearchButton.addEventListener("click", function (e) {
        e.preventDefault();             // prevent default behavior
        e.stopImmediatePropagation();   // stop event from reaching other elements

        console.log("Clear button clicked");

        searchInput.value = "";
        clearSearchButton.style.display = "none";

        // 🔥 Submit only the search form using its name or ID
        document.forms["searchForm"].submit();
        // or if using ID:
        // document.getElementById("searchForm").submit();
    });
});
