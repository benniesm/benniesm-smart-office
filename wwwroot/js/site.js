// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/**
 * Filter button value toggle
 */

//console.log('search initiated');
const urlParams = new URLSearchParams(window.location.search);
const searchString = urlParams.get('SearchString');
const urlPath = window.location.pathname;

if (searchString && searchString !== '' && searchString !== null) {
    let filterName = document.getElementById('taskFilter').value;
    let filterNewName = '';
    if (urlPath === '/TaskReport') {
    filterNewName =
        filterName === 'Filter reports' ? 'Clear filter': 'Filter reports';
    }

    if (urlPath === '/TaskAssignment') {
    filterNewName =
            filterName === 'Filter assignments' ? 'Clear filter': 'Filter assignments';
    }
    document.getElementById('taskFilter').value = filterNewName;
}
