// Helper function for the date ranges (Start Date => End Date).
// Restricts available dates based on the selected date in the other datepicker.
// E.g. If Start Date is set to "January 1st", when End Date is selected the first available
// option will be January 2nd, and vice versa.
$(document).on('focusin', 'input.datepicker', function () {
    var minDate = "";
    var maxDate = "";

    // If this is a start date
    if ($(this).hasClass('startDate')) {
        var $edEl = $(this).closest('.date-range').find('input.enddate');
        var edVal = $edEl.val();

        if (edVal) {
            var edObj = new Date(edVal);
            maxDate = addDays(edObj, -1);
        }
    }
        // Else if this is an end date
    else if ($(this).hasClass('endDate')) {
        var $sdEl = $(this).closest('.date-range').find('input.start-date');
        var sdVal = $sdEl.val();

        if (sdVal) {
            var sdObj = new Date(sdVal);
            minDate = addDays(sdObj, 1);
        }
    }

    // Destroy any existing datepickers
    if ($(this).hasClass('datepicker'))
        $(this).datepicker("destroy");

    // Set up datepicker
    $(this).datepicker({
        dateFormat: 'D, d MM, yy',
        changeMonth: true,
        changeYear: true,
        duration: 0,
        defaultDate: new Date($(this).val() || new Date())
    });

    // Set necessary restrictions
    if (minDate)
        $(this).datepicker('option', 'minDate', minDate);

    if (maxDate)
        $(this).datepicker('option', 'maxDate', maxDate);
});

// add days to date object
function addDays(d, numDays) {
    var newDate = new Date(d);
    newDate.setDate(d.getDate() + Number(numDays));
    return newDate;
}

function humanDate(d) {
    return $.datepicker.formatDate('D, d MM, yy', new Date(d));
}