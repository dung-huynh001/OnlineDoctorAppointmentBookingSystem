/*---------------------------------------------------------------------------------------------------*/
/*                                     Schedule of Doctors                                           */
/*---------------------------------------------------------------------------------------------------*/

$(document).ready(function () {
    loadSchedule();
});

/*Using AJAX to display data of DEPARTMENT TABLE*/
var table = $("#scheduleTbl").DataTable({
    "ajax": {
        "responsive": true,
        "url": '/ScheduleOfDoctors/LoadAllSchedule',
        "type": "GET",
        "datatype": "json"
    },
    "order": [[1, 'asc']],
    "columns": [
        {
            "className": 'dt-control',
            "orderable": false,
            "data": null,
            "defaultContent": '',
        },
        {
            "data": "workingDay",
            "title": 'Working Day',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "doctorName",
            "title": 'Doctor Name',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "speciality",
            "title": 'Speciality',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "gender",
            "title": 'Gender',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "availableTime",
            "title": 'Available Time',
            "autoWidth": true,
            "searchable": true
        },
        {
            "data": "consultantTime",
            "title": 'Consultant Time',
            "autoWidth": true,
            "searchable": true
        },
        {
            "responsivePriority": 1,
            "data": "scheduleID",
            "title": "Action",
            "orderable": false,
            "autoWidth": true,
            "searchable": true,
            "render": function (data, type, row) {
                return type === 'display' ? '<a data-doctorid="' + row.doctorID + '" data-scheduleid="' + data + '" class="btn btn-outline-info btn-sm ml-1 btn-viewDoctor" role="button" data-toggle="modal" data-target="#makeAppointmentModal" '
                    + '><i class="fa-solid fa-eye"  data-toggle="popover" data-trigger="hover" data-placement="top"  data-content="View Doctor"></i></div>' : data;
            }
        }
    ]
});

// Get schedule of doctors to display on screen
function loadSchedule() {
    table.ajax.reload();
}


/*---------------------------------------------------------------------------------------------------*/
/*                                           View Doctor                                             */
/*---------------------------------------------------------------------------------------------------*/

// Get doctorID and scheduleID to handle
var selectedDoctorID = 0;
var selectedScheduleID = 0;
$(document).on('click', '.btn-viewDoctor', function () {
    var btnAppointment = $(this);
    selectedDoctorID = btnAppointment.data('doctorid');
    selectedScheduleID = btnAppointment.data('scheduleid');
    loadAppointment(selectedDoctorID, selectedScheduleID)
})



// Get data to display on appointment form
function loadAppointment(selectedDoctorID, selectedScheduleID) {
    $.ajax({
        url: '/ScheduleOfDoctors/LoadDoctorInfo',
        method: 'GET',
        data: {
            selectedDoctorID,
            selectedScheduleID
        },
        dataType: 'JSON',
        success: function (res) {
            $('#fullName').text(res.data.doctorName);
            $('#gender').text(res.data.gender);
            $('#speciality').text(res.data.speciality);
            $('#phoneNumber').text(res.data.phoneNumber);
            $('#shiftTime').text(res.data.shiftTime);
            $('#dateOfBirth').text(res.data.dateOfBirth);
            $('#consultantTime').text(res.data.consultantTime);
            $('#workingDay').text(res.data.workingDay);
            $('#breakTime').text(res.data.breakTime);
            $('#workingDay').text(res.data.workingDay);
            $('#email').text(res.data.email);
            console.log(res);
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
}