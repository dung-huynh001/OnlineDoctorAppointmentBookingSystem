//show on off form
function setButtonOnOffForm() {
    $("#create-doctor-schedule-page").hide();
    $("#update-doctor-schedule-page").hide();
    $("#list-doctor-schedule-page").show();

    //close form create
    $("#btn-close-form").on("click", function () {
        $("#create-doctor-schedule-page").hide();
        $("#update-doctor-schedule-page").hide();
        $("#list-doctor-schedule-page").show();

        $('#form-doctor-schedule').find('input').removeClass('border-2 border-danger');
        $('#form-doctor-schedule').find('textarea').removeClass('border-2 border-danger');
        $('#form-doctor-schedule').find('select').removeClass('border-2 border-danger');
        $('#form-doctor-schedule').find('span').removeClass('border-2 border-danger');
        $('#form-doctor-schedule').find('label').remove();

    });
    // close from update
    $("#btn-close-form-update").on("click", function () {
        $("#create-doctor-schedule-page").hide();
        $("#update-doctor-schedule-page").hide();
        $("#list-doctor-schedule-page").show();

    });
    //open from create
    $("#btn-open-form-create").on("click", function () {
        $("#create-doctor-schedule-page").show();
        $("#update-doctor-schedule-page").hide();
        $("#list-doctor-schedule-page").hide();

    });
    //open from update
    $(".btn-update-schedule").on("click", function () {
        $("#create-doctor-schedule-page").hide();
        $("#update-doctor-schedule-page").show();
        $("#list-doctor-schedule-page").hide();

    });   
}

//create doctor schedule
function setSubmitFormByAjax() {
    $("#form-doctor-schedule").submit(function (event) {

        if ($("#form-doctor-schedule").valid()) {

            var formData = {


                DOCTORID: $("#doctor").val(),
                WORKINGDAY: $("#scheduledate").val(),
                SHIFTTIME: $("#starttime").val(),
                BREAKTIME: $("#endtime").val(),
                CONSULTANTTIME: $("#consultanttime").val(),
             
            };
           
            $.ajax({
                type: "POST",
                url: "/Admin/DoctorScheduleManage/CreateDoctorSchedule",
                data: formData,
                dataType: "json",
                encode: true,
            }).done(function (data) {
                
                if (data.error == 1) {
                   
                    Swal.fire(
                        'Failed!',
                        data.msg,
                        'error'
                    )
                }
                if (data.error == 0) {
                    
                    $('#doctor-schedule-table').DataTable().ajax.reload();
                    $("#create-doctor-schedule-page").hide();
                    $("#update-doctor-schedule-page").hide();
                    $("#list-doctor-schedule-page").show();

                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Success !',
                        text: 'Create doctor schedule is success !',
                        showConfirmButton: false,
                        timer: 2000
                    });

                    $("#form-doctor-schedule").trigger('reset');
                    $("#doctor").trigger('change');
                }
            });
        }


        event.preventDefault();
    });
}

//Load data to form update
function loadDataToForm(Scheduleid) {

    var formData = {
        scheduleid: Scheduleid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/DoctorScheduleManage/LoadDoctorScheduleInfo",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
       
        if (data.error == 1) {
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {

            
            $("#uscheduleid").val(data.schedule.SCHEDULEID);
            $("#udoctor").val(data.schedule.DOCTORID);
            $("#udoctor").trigger('change');
            $("#uscheduledate").val(data.schedule.WORKINGDAY);
            $("#ustarttime").val(data.schedule.SHIFTTIME);
            $("#uendtime").val(data.schedule.BREAKTIME);
            $("#uconsultanttime").val(data.schedule.CONSULTANTTIME);
        }
    });

}

//Update doctor schedule
function setSubmitFormUdateByAjax() {
    $("#from-update-doctor-schedule").submit(function (event) {
        if ($("#from-update-doctor-schedule").valid()) {
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
                confirmButtonText: 'Yes, update it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    var formData = {
                        SCHEDULEID: $("#uscheduleid").val(),
                        DOCTORID: $("#udoctor").val(),
                        WORKINGDAY: $("#uscheduledate").val(),
                        SHIFTTIME: $("#ustarttime").val(),
                        BREAKTIME: $("#uendtime").val(),
                        CONSULTANTTIME: $("#uconsultanttime").val()
                    };
                
                    

                    $.ajax({
                        type: "POST",
                        url: "/Admin/DoctorScheduleManage/UpdateDoctorSchedule",
                        data: formData,
                        dataType: "json",
                        encode: true,
                    }).done(function (data) {
                        
                        if (data.error == 1) {

                            Swal.fire(
                                'Failed!',
                                data.msg,
                                'error'
                            )
                        }
                        if (data.error == 0) {
                            $('#doctor-schedule-table').DataTable().ajax.reload();
                            $("#create-doctor-schedule-page").hide();
                            $("#update-doctor-schedule-page").hide();
                            $("#list-doctor-schedule-page").show();
                           
                            Swal.fire({
                                position: 'top',
                                icon: 'success',
                                title: 'Success !',
                                text: 'Update Schedule is success !',
                                showConfirmButton: false,
                                timer: 3000
                            });
                            $("#from-update-doctor-schedule").trigger('reset');

                        }
                    });
                } else if (result.dismiss === Swal.DismissReason.cancel) {/* Read more about handling dismissals below */ }
            })
        }
        event.preventDefault();
    });
}

//show update from
function setEventUpdateDoctorScheduleForBtn() {
    var table = $('#doctor-schedule-table').DataTable();

    table.on('draw', function () {

        $(".btn-update-schedule").on("click", function () {
            $("#create-doctor-schedule-page").hide();
            $("#update-doctor-schedule-page").show();
            $("#list-doctor-schedule-page").hide();

            var Button = $(this);
            var id = Button.data("id");
            
            loadDataToForm(id);
        });

    });

}

//delete dialog
function setEventDeleteScheduleBtn() {
    var table = $('#doctor-schedule-table').DataTable();

    table.on('draw', function () {

        $(".btn-delete-schedule").on("click", function () {
            var Button = $(this);
            var id = Button.data("id");
           

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
                text: "This will delete the appointments contained in this schedule, You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {
                    deleteScheduleByAjax(id) // delete schedule by  id

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        });
    });

}

//delete schedule
function deleteScheduleByAjax(sheduleid) {

    var formData = {
        scheduleid: sheduleid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/DoctorScheduleManage/DeleteSchedule",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
       
        if (data.error == 1) {
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {
            $('#doctor-schedule-table').DataTable().ajax.reload();
            Swal.fire({
                position: 'top',
                icon: 'success',
                title: 'Delete !',
                text: 'Delete schedule is success !',
                showConfirmButton: false,
                timer: 3000
            });
        }
    });
}

// Jquery datatable
function initJqueryDatatable() {
    var table = $('#doctor-schedule-table').DataTable({

        "sAjaxSource": "/Admin/DoctorScheduleManage/LoadDoctorScheduleData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "There are no schedule in the list.",
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
                "data": "SCHEDULEID",
                "title": 'ID',
                "searchable": true

            },
            {
                "data": "DOCTORID",
                "title": 'Doctor ID',
                "searchable": true

            },
            {
                "data": "DOCTORNAME",
                "title": 'Doctor Name',
                "searchable": true

            },
            {
                "data": "WORKINGDAY",
                "title": 'Working Day',
                "searchable": true

            },
            {
                "data": "SHIFTTIME",
                "title": 'Start Time',
                "searchable": true

            },
            {
                "data": "BREAKTIME",
                "title": 'End Time',
                "searchable": true

            },
            {
                "data": "CONSULTANTTIME",
                "title": 'Consultant Time',
                "searchable": true

            },
            {
                "data": "APPOINTMENTNUM",
                "title": 'Appointments',
                "searchable": true

            },
            {
                "data": "CREATEDBY",
                "title": "Created By",
                "searchable": true
            },
            {
                "data": "CREATEDDATE",
                "title": "Created Date",
                "searchable": true
            },
            {
                "data": "UPDATEDBY",
                "title": "Updated By",
                "searchable": true
            },
            {
                "data": "UPDATEDDATE",
                "title": "Updated Date",
                "searchable": true
            },
            {
                "data": null,
                "title": "Action",
                "responsivePriority": 1,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                   
                    return "<btn class=\"btn-update-schedule btn btn-sm btn-outline-primary btn-action\" data-id=\"" + row.SCHEDULEID + "\"  data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Edit schedule\" > <i class=\"fa-solid fa-user-pen\"></i> </btn>"
                        + "<btn class=\"btn-delete-schedule btn btn-sm btn-outline-danger btn-action  ml-2\" data-id=\"" + row.SCHEDULEID + "\"  data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Delete schedule\" > <i class=\"fa-solid fa-trash\"></i> </btn> "

                }

            },
        ]

    });

    $(window).trigger('resize');

}



//Validate form
function validateForm() {

    jQuery.validator.addMethod("greaterThan",
        function (value, element, params) {
            if (value < $(params).val()) {
                return false;
            } else if (value > $(params).val()) {
                return true;
            } else {
                return false;
            }
        }, 'Must be greater than {0}.');

    //Validate form create
    $("#form-doctor-schedule").validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        ignore: ".ignore, .select2-input",
        rules: {
            "Doctor": {
                required: true
            },
            "scheduledate": {
                required: true
            },
            "starttime": {
                required: true
            },
            "endtime": {
                required: true,
                greaterThan: "#starttime"
            },
            "Consultanttime": {
                required: true
            }
        },
        messages: {
            "Doctor": {
                required: "Doctor is required"
            },
            "scheduledate": {
                required: "Schedule date is required"
            },
            "starttime": {
                required: "Start time is required"
            },
            "endtime": {
                required: "End time is required",
                greaterThan: "Start time is smaller than end time"
            },
            "Consultanttime": {
                required: "Consultant time is required"
            }

        },
        highlight: function (element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $(".select2-selection");

                element.addClass('border-2 border-danger')
            } else {
                elem.addClass('border-2 border-danger ')
            }

        },
        unhighlight: function (element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $(".select2-selection");

                element.removeClass('border-2 border-danger')
            } else {
                elem.removeClass('border-2 border-danger')
            }

        },
        errorPlacement: function (error, element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $("#doctor").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }

    });

    //Validate form Update
    $("#from-update-doctor-schedule").validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        ignore: ".ignore, .select2-input",
        rules: {
            "uDoctor": {
                required: true
            },
            "uscheduledate": {
                required: true
            },
            "ustarttime": {
                required: true
            },
            "uendtime": {
                required: true,
                greaterThan: "#ustarttime"
            },
            "uConsultanttime": {
                required: true
            }
        },
        messages: {
            "uDoctor": {
                required: "Doctor is required"
            },
            "uscheduledate": {
                required: "Schedule date is required"
            },
            "ustarttime": {
                required: "Start time is required"
            },
            "uendtime": {
                required: "End time is required",
                greaterThan: "Start time is smaller than end time"
            },
            "uConsultanttime": {
                required: "Consultant time is required"
            }

        },
        highlight: function (element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $(".select2-selection");

                element.addClass('border-2 border-danger')
            } else {
                elem.addClass('border-2 border-danger ')
            }

        },
        unhighlight: function (element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $(".select2-selection");

                element.removeClass('border-2 border-danger')
            } else {
                elem.removeClass('border-2 border-danger')
            }

        },
        errorPlacement: function (error, element) {
            var elem = $(element);
            if (elem.hasClass("select2-hidden-accessible")) {
                element = $("#udoctor").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }

    });
}

    $("document").ready(function () {
        setButtonOnOffForm();
        initJqueryDatatable();
        setSubmitFormByAjax();
        setEventUpdateDoctorScheduleForBtn()
        setSubmitFormUdateByAjax();
        setEventDeleteScheduleBtn();
        validateForm();
        

        $("#doctor").select2();
        $("#doctor").on('select2:close', function (e) {
            $(this).valid()
        });

        $("#udoctor").select2();
        $("#udoctor").on('select2:close', function (e) {
            $(this).valid()
        });
    });