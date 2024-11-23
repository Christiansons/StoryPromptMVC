// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
        // JavaScript for handling form submission
        const form = document.getElementById('promptForm');
        const successMessage = document.getElementById('successMessage');

        form.addEventListener('submit', function (event) {
            event.preventDefault();
            // Show success message
            successMessage.style.display = 'block';
            // Optionally, you can clear the textarea
            document.getElementById('promptContent').value = '';
        });
