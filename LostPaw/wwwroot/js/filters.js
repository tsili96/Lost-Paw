document.addEventListener('DOMContentLoaded', function () {
    const clearFiltersButton = document.getElementById('clearFilters');

    // Clear the filters and submit the form
    if (clearFiltersButton) {
        clearFiltersButton.addEventListener('click', function () {
            // Reset the filter form values (select inputs)
            document.querySelector('select[name="postType"]').value = '';
            document.querySelector('select[name="dateFilter"]').value = '';

            // Submit the form with empty filters
            const filtersForm = document.getElementById('filtersForm');
            filtersForm.submit();
        });
    }
});
