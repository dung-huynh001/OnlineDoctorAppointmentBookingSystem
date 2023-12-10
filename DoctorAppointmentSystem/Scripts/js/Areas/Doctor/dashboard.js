var STATUS = "a";
var calendar;
function initCalendar() {
    

    var calendarEl = document.getElementById('calendar');
    calendar = new FullCalendar.Calendar(calendarEl, {
        
        themeSystem: 'bootstrap',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },
        initialView: 'listWeek',
        initialDate: Date.now(),
        navLinks: true, // can click day/week names to navigate views
        selectable: false,
        selectMirror: true,
        editable: false,
        dayMaxEvents: true, // allow "more" link when too many events
        firstDay: 1, //The day that each week begins (Monday=1)
        eventSources: [
            {
                url: '/Doctor/Home/GetDoctorSchedule',
                method : 'POST',
                extraParams: function() { // a function that returns an object
                                    return {
                                        status: $("#appt-status-filter").val(),
                                    };
                                }
                   
                ,
                dataType: "json",
                contentType: false,
                processData: false,
                color: 'blue',   // a non-ajax option
                textColor: '#f1f1fb' // a non-ajax option
            }

        ],
        eventDidMount: function (info) {
            if (typeof (info.event.extendedProps.description) != "undefined" && info.event.extendedProps.description != "") {
                $(info.el).popover({
                    title: info.event.title,
                    content: info.event.extendedProps.description,
                    html: true,
                    placement: 'top',
                    container: "body",
                    delay: { "show": 300, "hide": 200 },
                    trigger: 'hover',
                    template: '<div class="popover fc-med-popover ' + info.event.classNames[0] + '" role="tooltip"><div class="arrow"></div> <h3  class="popover-header"></h3><div class="popover-body"></div></div>'
                });
            }
        },


    });

    calendar.render();
}


function setEventStatusOnChange() {
    $("#appt-status-filter").on("change", function () {
        STATUS = $("#appt-status-filter").val();
        console.log(STATUS);

        calendar.refetchEvents();
    });

}


function calendarResponsive() {
    $('.fc-header-toolbar,.fc-toolbar').addClass('d-flex flex-wrap');
}

$(document).ready(function () {
    initCalendar();
    setEventStatusOnChange();
    calendarResponsive();

});
