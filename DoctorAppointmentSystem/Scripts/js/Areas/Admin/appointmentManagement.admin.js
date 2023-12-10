//show on off form
function setButtonOnOffForm() {
    $("#appointment-info").hide();
    $("#list-appointment-page").show();

    //close form 
    $("#btn-close-form").on("click", function () {
        $("#appointment-info").hide();
        $("#list-appointment-page").show();

    });
   
    //open from 
    $("#btn-open-form-create").on("click", function () {
        $("#appointment-info").show();
        $("#list-appointment-page").hide();

    });
    
}

//delete dialog
function setEventDeleteAppointment() {
    var table = $('#appointment-table').DataTable();

    table.on('draw', function () {

        $(".btn-delete-appointment").on("click", function () {
            var Button = $(this);
            var patientname = Button.data("patientname");
            var id = Button.data("id");
            console.log("db=>" + id);

            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-success mr-2',
                    cancelButton: 'btn btn-danger'
                },
                buttonsStyling: false
            })
            swalWithBootstrapButtons.fire({
                position: 'top',
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    deleteAppointment(id);


                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
    });
}

//delete doctor
function deleteAppointment(apmid) {

    var formData = {
        AppointmentId: apmid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/AppointmentManage/DeleteAppointment",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
        console.log(data);
        if (data.error == 1) {
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {
            $('#appointment-table').DataTable().ajax.reload();

            Swal.fire({
                position: 'top',
                icon: 'success',
                title: 'Deleted !',
                text: 'Delete appointment is success !',
                showConfirmButton: false,
                timer: 3000
            });
        }
    });
}


// Jquery datatable
function initJqueryDatatable() {
    var table = $('#appointment-table').DataTable({

        "sAjaxSource": "/Admin/AppointmentManage/LoadAppointmentData",
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
                "title": 'ID',
                "searchable": true

            },
            {
                "data": "PATIENTNAME",
                "title": 'Patient Name',
                "searchable": true

            },
            {
                "data": "DOCTORNAME",
                "title": 'Doctor Name',
                "searchable": true

            },
            {
                "data": "APPOINTMENTDATE",
                "title": 'Appointment Date',
                "searchable": true

            },
            {
                "data": "DATEOFCONSUITATION",
                "title": 'Date Of Consultation',
                "searchable": true
            },
            {
                "data": "CLOSEDBY",
                "title": 'Closed By',
                "searchable": true,
                "orderable": false,
            },
            {
                "data": "CLOSEDDATE",
                "title": 'Closed Date',
                "searchable": true,
                "orderable": false,
            },
            {
                "data": "CREATEDBY",
                "title": "Created By",
                "searchable": true,
                "orderable": false,
            },
            {
                "data": "CREATEDDATE",
                "title": "Created Date",
                "searchable": true,
                "orderable": false,
            },
            {
                "data": "UPDATEDBY",
                "title": "Updated By",
                "searchable": true,
                "orderable": false,
            },
            {
                "data": "UPDATEDDATE",
                "title": "Updated Date",
                "searchable": true,
                "orderable": false,
            }
            ,
            {
                "data": "APPOIMENTSTATUS",
                "title": 'Status',
                "className": 'text-center',
                "responsivePriority": 1,
                "searchable": true,
                "render": function (data, type, row) {
                    if (data.toLowerCase() == 'pending') {
                        return '<span class="column-status column-status--pending">' + data + '</span>';
                        
                    } else if (data.toLowerCase() == 'confirm') {
                        return '<span class="column-status column-status--process">' + data + '</span>';
                       
                    } else if (data.toLowerCase() == 'completed') {
                        

                        return '<span class="column-status column-status--completed">' + data + '</span>';
                        
                    } else {
                        return '<span class="column-status column-status--cancel">' + data + '</span>';
                       
                    }

                }
            },
            {
                "data": null,
                "title": "Action",
                "responsivePriority": 1,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                   
                    return "<btn class=\"btn-view-appointment btn btn-sm btn-outline-primary btn-action\" data-appointmentid=\"" + row.APPOINTMENTID + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"View Appointment\"    > <i class=\"fa-solid fa-eye\"></i> </btn>"
                        + "<btn class=\"btn-delete-appointment btn btn-sm btn-outline-danger btn-action  ml-2\" data-id=\"" + row.APPOINTMENTID + "\" data-patientname=\"" + row.PATIENTNAME + "\"   data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Delete Appointment\" > <i class=\"fa-solid fa-trash\"></i> </btn> "

                }

            },
        ]

    });

    $(window).trigger('resize');

}




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
    setButtonOnOffForm();
    initJqueryDatatable();
    setEventDeleteAppointment();
    setEventViewAppointment();
  

});
