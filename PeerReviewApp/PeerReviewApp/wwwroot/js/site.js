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

function toggleElementById(id)
{
    //get reference to div containing form
    let element = document.getElementById(id);

    if(element.classList.contains("formHidden"))
    {
        //if elements class list already contains formHidden, remove it so it is visible
        element.classList.remove("formHidden");
    } else {
        //otherwise hide form
        element.classList.add("formHidden");
    }
}

/*
    For use in View Submissions page for instructors for expanding the row options.
    Inputs table rowId and buttonId
    Toggles visibility of row and changes button icon to reflect direction
*/
function hideElementById(rowId, buttonId)
{
    //get reference to div containing extra options
    let element = document.getElementById(rowId);
    let button = document.getElementById(buttonId);

    if(element.classList.contains("hidden"))
    {
        //if elements class list already contains hidden, remove it so it is visible
        button.classList.remove("fa-chevron-down");
        button.classList.add("fa-chevron-up");
        element.classList.remove("hidden");
    } else {
        //otherwise hide form
        button.classList.remove("fa-chevron-up");
        button.classList.add("fa-chevron-down");
        element.classList.add("hidden");
        $("rowId").slideUp("slow");
    }
}

function showTooltip(targetElement, message) {
    // Create tooltip container
    const tooltip = document.createElement('div');
    tooltip.style.position = 'absolute';
    tooltip.style.background = '#333';
    tooltip.style.color = '#fff';
    tooltip.style.padding = '6px 10px';
    tooltip.style.borderRadius = '4px';
    tooltip.style.fontSize = '14px';
    tooltip.style.zIndex = 1000;
    tooltip.style.transition = 'opacity 1s ease';
    tooltip.style.opacity = '1';
    tooltip.style.pointerEvents = 'none';

    // Set initial position temporarily (will adjust after DOM insert)
    tooltip.style.top = '0';
    tooltip.style.left = '0';

    // Add tooltip message
    tooltip.innerText = message;

    // Create arrow
    const arrow = document.createElement('div');
    arrow.style.position = 'absolute';
    arrow.style.top = '100%';
    arrow.style.left = '50%';
    arrow.style.transform = 'translateX(-50%)';
    arrow.style.width = '0';
    arrow.style.height = '0';
    arrow.style.borderLeft = '6px solid transparent';
    arrow.style.borderRight = '6px solid transparent';
    arrow.style.borderTop = '6px solid #333';

    // Append arrow to tooltip
    tooltip.appendChild(arrow);
    document.body.appendChild(tooltip);

    // Position tooltip relative to the target element
    const rect = targetElement.getBoundingClientRect();
    const tooltipRect = tooltip.getBoundingClientRect();
    tooltip.style.top = `${window.scrollY + rect.top - tooltipRect.height - 10}px`;
    tooltip.style.left = `${window.scrollX + rect.left + rect.width / 2}px`;
    tooltip.style.transform = 'translateX(-50%)';

    // Start fade out after 4 seconds
    setTimeout(() => {
        tooltip.style.opacity = '0';
    }, 4000);

    // Remove from DOM after 5 seconds
    setTimeout(() => {
        tooltip.remove();
    }, 5000);
}

