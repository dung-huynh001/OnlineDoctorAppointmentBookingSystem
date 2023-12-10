


// Jquery datatable
function initJqueryDatatable() {
    var table = $('#appointment-table').DataTable({

        "sAjaxSource": "/Doctor/CompletedAppt/LoadAppointmentData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "There are no appointments in the list.",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "columns": [
            {
                "className": 'dt-control',
                "orderable": false,
                "data": null,
                "defaultContent": '',
            },
            {
                "data": "APPOINTMENTID",
                "title": 'No.',
                "searchable": true

            },
            {
                "data": "PATIENTNAME",
                "title": 'Patient Name',
                "searchable": true,
                "searchable": true,
                "render": function (data, type, row) {
                    return '<a href="#' + row.APPOINTMENTID + '" class="">' + data + '</a>';
                }
            },
            {
                "data": "DATEOFCONSULTANT",
                "title": 'Consultation Date',
                "searchable": true

            },
            {
                "data": "APPOINTMENTDATE",
                "title": 'Appointment Date',
                "searchable": true
            },
            
            {
                "data": "CONSULTANTTIME",
                "title": 'Consultant Time',
                "searchable": true
            }
            ,
            {
                "data": "CLOSEDBY",
                "title": 'Closed By',
                "orderable": false,
                "searchable": true
            }
            ,
            {
                "data": "CLOSEDDATE",
                "title": 'Closed Date',
                "orderable": false,
                "searchable": true
            },
            {
                "data": "APPOIMENTSTATUS",
                "title": 'Status',
                "className": 'text-center',
                "responsivePriority": 1,
                "searchable": true,
                "orderable": false,
                "render": function (data, type, row) {
                    if (data.toLowerCase() == 'pending') {
                        return '<span class="column-status column-status--pending">' + data + '</span>';

                    } else if (data.toLowerCase() == 'in process') {
                        return '<span class="column-status column-status--process">' + data + '</span>';

                    } else if (data.toLowerCase() == 'completed') {
                        return '<span class="column-status column-status--completed">' + data + '</span>';
                    } else {
                        return '<span class="column-status column-status--cancel">' + data + '</span>';

                    }
                }
            }
           


        ]

    });

    $(window).trigger('resize');

}
//view detail appointment
function setEventViewAppointment() {
    var table = $('#appointment-table').DataTable();

    table.on('draw', function () {

        $(".btn-view-appointment").on("click", function () {
            var Button = $(this);

            var appointmentID = Button.data('appointmentid');
            $.ajax({
                url: '/AppointmentManage/AppointViewDetail',
                method: 'GET',
                data: { appointmentID: appointmentID },
                dataType: 'JSON',
                success: function (res) {
                    var data = res.data;
                    
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
                },
                error: function (err) {
                    console.log(err.responseText);
                }
            })

            $('#makeAppointmentModal').modal('show')


        });
    });

}




$("document").ready(function () {
 
    initJqueryDatatable();
    setEventViewAppointment();

    

});
$(document).on('click', '#btnGoToDashboard', function () {
    var previousPage = document.referrer;
    
    $('#btnGoToDashboard').attr('href', previousPage);
})