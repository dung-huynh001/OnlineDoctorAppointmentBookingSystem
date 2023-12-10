var table = $('#historyTbl').DataTable({
    language: {
        emptyTable: "You haven't booked any appointments yet."
    },
    responsive: true,
    ordering: true,
    searching: true,
    autoWidth: true,
    columnDefs: [
        { "width": "165px", "targets": [2, 3] },
        { "width": "100px", "targets": [4, 5] }
    ],
    order: [[6, 'desc']],
    ajax: {
        url: '/Appointment/ViewHistory',
        type: 'GET'
    },
    columns: [
        {
            orderable: false,
            className: 'dt-control',
            data: null,
            defaultContent: '',
        },
        {
            data: 'appointmentID',
            title: 'No.',
        },
        {
            data: 'doctorName',
            title: 'Doctor Name',
            className: 'text-nowrap',
        },
        {
            data: 'dateOfConsultation',
            title: 'Date of Consultation',
            className: 'text-nowrap',
        },
        {
            data: 'consultationTime',
            title: 'Time',
            className: 'text-nowrap',
        },
        {
            data: 'consultationDay',
            title: 'Day',
            className: 'text-nowrap',
        },
        {
            data: 'appointmentStatus',
            title: 'Status',
            className: 'text-center',
            render: function (data, type, row) {
                return '<span class="column-status">' + data + '</span>';
            }
        },
        {
            responsivePriority: 1,
            data: 'appointmentID',
            title: "Action",
            orderable: false,
            render: function (data, type, row) {
                return type === 'display' ?
                    '<div class="d-flex justify-content-round">'
                    + '<a data-appointmentid="' + data + '" class="btn btn-outline-info btn-sm btn-action btn-viewAppt" role="button" data-toggle="modal" data-target="#makeAppointmentModal">'
                    + '<i class="fa-solid fa-eye"  data-toggle="popover" data-trigger="hover" data-placement="top" data-content="View Appointment"></i></a>'
                    + '<a data-appointmentid="' + data + '" class="btn btn-outline-danger btn-sm btn-action btn-cancelAppt" role="button"  data-toggle="popover" data-trigger="hover" data-placement="top"  data-content="Cancel Appointment">'
                    + '<i class="fa-solid fa-circle-xmark"></i></a>'
                    + '</div>'
                    : data;
            }
        }
    ]
});

function loadData() {
    table.ajax.reload();
}

$(document).ready(function () {
    loadData();
});


table.on('draw', function () {
    styleForStatus();
});

function styleForStatus() {
    $('.column-status').each(function () {
        var statusText = this.textContent.toLowerCase();
        switch (statusText) {
            case 'pending':
                $(this).addClass('column-status--pending');
                break;
            case 'confirm':
                $(this).addClass('column-status--process');
                break;
            case 'completed':
                $(this).addClass('column-status--completed');
                break;
            default:
                $(this).addClass('column-status--cancel');
                break;
        }
    });
}

$(document).on('click', '.btn-viewAppt', function () {
    var btnViewAppt = $(this);
    var appointmentID = btnViewAppt.data('appointmentid');
    $.getJSON('/Appointment/ViewAppointment', { appointmentID: appointmentID })
        .done(function (res) {
            var data = res.data;
            console.log(res.data);
            $('#doctorName').text(data.doctorName);
            $('#doctorGender').text(data.doctorGender);
            $('#speciality').text(data.doctorSpeciality);
            $('#patientName').text(data.patientName);
            $('#dateOfBirth').text(data.patientDateOfBirth);
            $('#patientGender').text(data.patientGender);
            $('#modeOfConsultant').val(data.modeOfConsultant);
            $('#consultantType').val(data.consultantType);
            $('#dateOfConsultation').text(data.dateOfConsultation);
            $('#consultationTime').text(data.consultationTime);
            $('#appointmentDate').text(data.appointmentDate);
            $('#appointmentTime').text(data.appointmentTime);
            $('#symtoms').val(data.symtoms);
            $('#existingIllness').val(data.existingIllness);
            $('#drugAlergies').val(data.drugAlergies);
        })
        .fail(function (err) {
            console.log(err.responseText);
        });
});

$(document).on('click', '.btn-cancelAppt', function () {
    var btnCancelAppt = $(this);
    var appointmentID = btnCancelAppt.data('appointmentid');
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success mr-2',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    });

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        position: 'top',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, cancel it!'
    }).then(function (result) {
        if (result.isConfirmed) {
            $.post('/Appointment/CancelAppointment', { appointmentID: appointmentID })
                .done(function (res) {
                    if (res.success) {
                        Swal.fire({
                            title: 'Cancelled!',
                            text: 'Your appointment has been cancelled.',
                            icon: 'success',
                            position: 'top',
                            showConfirmButton: false,
                            timer: 3000
                        });
                        loadData();
                    } else {
                        Swal.fire({
                            title: 'Failed!',
                            text: res.message,
                            icon: 'warning',
                            position: 'top',
                            showConfirmButton: false,
                            timer: 3000
                        });
                    }
                })
                .fail(function (err) {
                    console.log(err.responseText);
                });
        }
    });
});
