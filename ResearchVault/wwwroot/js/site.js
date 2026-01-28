// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function FormStatusFunction(String) {
    if (String == "Add") {
        HttpContext.Session.SetString("FormStatus", "Add");
    }
    else if (String == "Edit") {
        HttpContext.Session.SetString("FormStatus", "Edit");
    }
    else if (String == "Delete") {
        HttpContext.Session.SetString("FormStatus", "Delete");
    }
}