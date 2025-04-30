// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function toggleFilters()
{
    //get reference to div containing form
    let element = document.getElementById("filters")

    if(element.classList.contains("formHidden"))
    {
        //if elements class list already contains formHidden, remove it so it is visible
        element.classList.remove("formHidden");
    } else {
        //otherwise hide form
        element.classList.add("formHidden");
    }
}