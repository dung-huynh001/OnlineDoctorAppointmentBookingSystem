
 //show on off form
function setButtonOnOffForm() {
    $("#create-patient-page").hide();
    $("#update-patient-page").hide();
    $("#list-patient-page").show();

    //close form create
    $("#btn-close-form").on("click", function () {
        $("#create-patient-page").hide();
        $("#update-patient-page").hide();
        $("#list-patient-page").show();
        $("#form-create-patient").trigger('reset');
        $(".paswd-on-off").each(function () {
            var inp = this;
            inp.type = "password";
        });

        $('#form-create-patient').find('input').removeClass('border-2 border-danger');
        $('#form-create-patient').find('textarea').removeClass('border-2 border-danger');
        $('#form-create-patient').find('select').removeClass('border-2 border-danger');
        $('#form-create-patient').find('span').removeClass('border-2 border-danger');
        $('#form-create-patient').find('label').remove();
     
    });
    // close from update
    $("#btn-close-form-update").on("click", function () {
        $("#create-patient-page").hide();
        $("#update-patient-page").hide();
        $("#list-patient-page").show();

        $('#form-edit-doctor').find('input').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('textarea').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('select').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('span').removeClass('border-2 border-danger');
        $('#form-edit-doctor').find('label').remove();
       
    });
    
    //open from create
    $("#btn-open-form").on("click", function () {
        $("#create-patient-page").show();
        $("#update-patient-page").hide();
        $("#list-patient-page").hide();
       
    });
    //open from update
    $(".btn-update-patient").on("click", function () {
        $("#create-patient-page").hide();
        $("#create-patient-page").hide();
        $("#list-patient-page").show();
     

    });
   
}

//create patient
function setSubmitFormByAjax() {
    $("#form-create-patient").submit(function (event) {
        if ($("#form-create-patient").valid()) {
            var formData = {

                PATIENTNAME: $("#patientname").val(),
                USERNAME: $("#username").val(),
                PASSWORD: $("#password").val(),
                EMAIL: $("#email").val(),
                PATIENTNATIONALID: $("#nationalid").val(),
                PATIENTGENDER: $("#gender").val(),
                PATIENTMOBILENO: $("#mobile").val(),
                PATIENTDATEOFBIRTH: $("#brithofdate").val(),
                PATIENTADDRESS: $("#address").val(),


            };
          
            $.ajax({
                type: "POST",
                url: "/Admin/PatientManage/CreatePatient",
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

                    $('#PatientTable').DataTable().ajax.reload();
                    $("#create-patient-page").hide();
                    $("#list-patient-page").show();
                    
                   
                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Success !',
                        text: 'Create patient is success !',
                        showConfirmButton: false,
                        timer: 3000
                    });
                    $("#form-create-patient").trigger('reset');
                    $('#password').get(0).type = 'password';

                }
            });
        }
        

        event.preventDefault();
    });
}

//show update from
function setEventUpdatePatientFoBtn() {
    var table = $('#PatientTable').DataTable();

    table.on('draw', function () {

        $(".btn-update-patient").on("click", function () {
            $("#create-patient-page").hide();
            $("#list-patient-page").hide();
            $("#update-patient-page").show();

            var Button = $(this);
            var id = Button.data("id");
          
            loadDataToForm(id);

        });

    });

}

//load data to form update
function loadDataToForm(patientid) {

    var formData = {
        PatientId: patientid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/PatientManage/LoadPatientInfo",
        data: formData,
        dataType: "json",
        encode: true,
    }).done(function (data) {
       
        if (data.error == 1) {
            //showMessageFormUpdate(data.msg);
            Swal.fire(
                'Failed!',
                data.msg,
                'error'
            )
        }
        if (data.error == 0) {

            $("#upatientid").val(data.patient.PATIENTID);
            $("#upatientname").val(data.patient.PATIENTNAME);
            $("#uusername").val(data.patient.USERNAME);
            $("#unationalid").val(data.patient.PATIENTNATIONALID);
            $("#uaddress").val(data.patient.PATIENTADDRESS);
            $("#ubrithofdate").val(data.patient.PATIENTDATEOFBIRTH);
            $("#umobile").val(data.patient.PATIENTMOBILENO);
            $("#ugender").val([data.patient.PATIENTGENDER]);
            $("#uemail").val(data.patient.EMAIL);

        }
    });
}

//Update patient
function setSubmitFormUdateByAjax() {
    $("#form-update-patient").submit(function (event) {
        if ($("#form-update-patient").valid()) {
            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-success mr-2',
                    cancelButton: 'btn btn-danger'
                },
                buttonsStyling: false
            })
            swalWithBootstrapButtons.fire({
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

                        PATIENTID: $("#upatientid").val(),
                        PATIENTNAME: $("#upatientname").val(),
                        EMAIL: $("#uemail").val(),
                        PATIENTNATIONALID: $("#unationalid").val(),
                        PATIENTGENDER: $("#ugender").val(),
                        PATIENTMOBILENO: $("#umobile").val(),
                        PATIENTDATEOFBIRTH: $("#ubrithofdate").val(),
                        PATIENTADDRESS: $("#uaddress").val(),
                    };

                    $.ajax({
                        type: "POST",
                        url: "/Admin/PatientManage/UpdatePatient",
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
                            $('#PatientTable').DataTable().ajax.reload();

                            $("#create-patient-page").hide();
                            $("#update-patient-page").hide();
                            $("#list-patient-page").show();
                            
                            Swal.fire({
                                position: 'top',
                                icon: 'success',
                                title: 'Success !',
                                text: 'Update patient is success!',
                                showConfirmButton: false,
                                timer: 3000
                            });
                            $("#form-update-patient").trigger('reset');
                        }
                    });

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {

                }
            })
        }
        
        
       

        event.preventDefault();
    });
}

//show delete dialog
function setEventDeletePatientFoBtn() {
    var table = $('#PatientTable').DataTable();

    table.on('draw', function () {

        $(".btn-delete-patient").on("click", function () {
            var Button = $(this);
            var id = Button.data("id");
            var username = Button.data("partientname");
          

            const swalWithBootstrapButtons = Swal.mixin({
                customClass: {
                    confirmButton: 'btn btn-success mr-2',
                    cancelButton: 'btn btn-danger'
                },
                buttonsStyling: false
            })
            swalWithBootstrapButtons.fire({
                title: 'Are you sure delete patient '+ username+ ' ?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: false
            }).then((result) => {
                if (result.isConfirmed) {

                    deletePatientByAjax(id);// delete patient by id

                } else if (
                    /* Read more about handling dismissals below */
                    result.dismiss === Swal.DismissReason.cancel
                ) {
                    
                }
            })
        });
    });
    
}

// delete patient
function deletePatientByAjax(patientid) {

    var formData = {
        PatientId: patientid,
    };

    $.ajax({
        type: "POST",
        url: "/Admin/PatientManage/DeletePatient",
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
            $('#PatientTable').DataTable().ajax.reload();
            //showMessage("Delete patient  is success ", "Success !")
            Swal.fire(
                'Deleted!',
                'Delete Patient is success!',
                'success'
            )
        }
    });
}


// Jquery datatable
function initJqueryDatatable() {
    var table = $('#PatientTable').DataTable({
        
        "sAjaxSource": "/Admin/PatientManage/LoadPatientData",
        "sServerMethod": "POST",
        "bServerSide": true,
        "bProcessing": true,
        "responsive": true,
        "bSearchable": true,
        "order": [[1, 'asc']],
        "language": {
            "emptyTable": "There are no patient in the list.",
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
                "data": "PATIENTID",
                "title": 'ID',

                "searchable": true

            },
            {
                "data": "PATIENTNAME",
                "title": "Name",

                "searchable": true

            },
            {
                "data": "USERNAME",
                "title": "Username",

                "searchable": true

            },
           
            {
                "data": "PATIENTGENDER",
                "title": "Gender",

                "searchable": true
            },
           
            {
                "data": "PATIENTDATEOFBIRTH",
                "title": "Date Of Birth",

                "searchable": true
            },
            {
                "data": "PATIENTNATIONALID",
                "title": "National ID",

                "searchable": true
            },
            {
                "data": "PATIENTMOBILENO",
                "title": "Mobile",

                "searchable": true
            },
            {
                "data": "EMAIL",
                "title": "Email",

                "searchable": true
            },
            {
                "data": "PATIENTADDRESS",
                "title": "Address",

                "searchable": true
            },
            
            {
                "data": "LOGINLOCKDATE",
                "title": "Login Lock Date",

                "searchable": true
            },
            {
                "data": "LOGINRETRYCOUNT",
                "title": "Login Retry Count",

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
                "searchable": true,
                "orderable": false,
                "render": function (data, type, row) {
                   
                    return "<btn class=\"btn-update-patient btn btn-sm btn-outline-primary btn-action\" data-id=\"" + row.PATIENTID + "\" data-partientname=\"" + row.PATIENTNAME + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Edit user\"> <i class=\"fa-solid fa-user-pen\"></i> </btn>"
                        + "<btn class=\"btn-delete-patient btn btn-sm btn-outline-danger btn-action  ml-2\" data-id=\"" + row.PATIENTID + "\" data-partientname=\"" + row.PATIENTNAME + "\" data-toggle=\"popover\" data-trigger=\"hover\" data-placement=\"top\"  data-content=\"Delete user\" > <i class=\"fa-solid fa-trash\"></i></btn> "
                        
                }

            },
        ]

    });

    $(window).trigger('resize');

}

// show hihe pass
function showPass() {
    
      
        $(".paswd-on-off").each(function () {

             var inp = this;

            if (inp.type == "password"){
                inp.type = "text";
            }
            else {
                inp.type = "password";
            }

         });

    }
        
//Validate form
function validateFormPatient() {
    jQuery.validator.addMethod('valid_phone', function (value) {
        var regex = /^(84|0[3|5|7|8|9])+([0-9]{8})\b$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod('valid_username', function (value) {
        var regex = /^[a-z0-9-]*$/;
        return value.trim().match(regex);
    });

    jQuery.validator.addMethod('valid_password', function (value) {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod('valid_email', function (value) {
        var regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/;
        return value.trim().match(regex);
    });
    jQuery.validator.addMethod("greaterThan",
        function (value, element, params) {

            if (!/Invalid|NaN/.test(new Date(value))) {
                return new Date(value) > new Date($(params).val());
            }

            return isNaN(value) && isNaN($(params).val())
                || (Number(value) > Number($(params).val()));
        }, 'Must be greater than {0}.');


    //Validate form create
    $("#form-create-patient").validate({
        onfocusout: function (element) {
            $(element).valid()
        },
       
        rules: {
            "patientname": {
                required: true,
                maxlength: 50,
                
            },
            "username": {
                required: true,
                maxlength: 50,
                minlength: 3,
                valid_username: true
            },
            "password": {
                required: true,
                maxlength: 50,
                minlength: 6,
                valid_password: true

            },
            "nationalid": {
                required: true,
                maxlength: 20

            },
            "brithofdate": {
                required: true,


            },
            "mobile": {
                required: true,
                valid_phone: true

            },
            "address": {
                required: true,
                maxlength: 256,

            }
            
            ,
            "email": {
                required: true,
                email: true,
                valid_email: true
            }
            ,
            "Gender": {
                required: true

            }
        },
        messages: {
            "patientname": {
                required: "Patient name is required",
                maxlength: "Patient name charater max lenght is 50!",

            },
            "username": {
                required: "User name is required",
                maxlength: "Username charater lenght is 3 to 50!",
                minlength: "Username charater lenght is 3 to 50!",
                valid_username: "Username does not contain any special characters"
            },
            "password": {
                required: "Password is required",
                maxlength: "Maximum 30 characters",
                minlength: "Minimum 8 characters",
                valid_password: "Password charater at least one uppercase letter, one lowercase letter, one number and one special character"
            },
            "nationalid": {
                required: "National ID is required",
                maxlength: "National ID charater max lenght is 20!"
            },
            "brithofdate": {
                required: "Date of Birth is required",

            },
            "mobile": {
                required: "Mobile is required",
                valid_phone: "wrong phone number format"

            },
            "address": {
                required: "Address is required",
                maxlength: "Doctor address charater max lenght is 256 !"
            }
            ,
            "email": {
                required: "Email is required",
                email: "wrong email format",
                valid_email: "wrong email format"
            },
            "Gender": {
                required: "Gender is required"

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
                element = $("#department").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }

    });

    //Validate form update
    $("#form-update-patient").validate({
        onfocusout: function (element) {
            $(element).valid()
        },

        rules: {
            "upatientname": {
                required: true,
                maxlength: 50
            },
            "unationalid": {
                required: true,
                maxlength: 20

            },
            "ubrithofdate": {
                required: true,


            },
            "umobile": {
                required: true,
                valid_phone: true

            },
            "uaddress": {
                required: true,
                maxlength: 256,

            }
            ,
            "uemail": {
                required: true,
                email: true,
                valid_email: true
            },
            "uGender": {
                required: true

            }
        },
        messages: {
            "upatientname": {
                required: "Patient name is required",
                maxlength: "Patient name charater max lenght is 50!",

            },

            "unationalid": {
                required: "National ID is required",
                maxlength: "National ID charater max lenght is 20!"
            },
            "ubrithofdate": {
                required: "Data of Birth is required",

            },
            "umobile": {
                required: "Mobile is required",
                valid_phone: "wrong phone number format"

            },
            "uaddress": {
                required: "Address is required",
                maxlength: "Doctor address charater max lenght is 256 !"
            }
            ,
            "uemail": {
                required: "Email is required",
                email: "wrong email format",
                valid_email: "wrong email format"
            },
            "uGender": {
                required: "Gender is required"

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
                element = $("#udepartment").parent();

                error.insertAfter(element);
            } else {
                error.insertAfter(element);
            }
        }

    });
}

function isNumberKey(evt) { // accept number 
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}  

function isSpaceKey(evt) { // accept number 
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 32)
        return false;
    return true;
}

      


$("document").ready(function () {
    setButtonOnOffForm();
    setSubmitFormByAjax();
    setSubmitFormUdateByAjax();
    initJqueryDatatable();
    setEventUpdatePatientFoBtn();
    setEventDeletePatientFoBtn();
    validateFormPatient();
    
    
    
});