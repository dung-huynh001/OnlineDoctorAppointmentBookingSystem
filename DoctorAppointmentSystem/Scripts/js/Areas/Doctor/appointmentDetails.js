
$(document).on('click', '#btnChangeDate', function () {
    var date = $('#appointmentDate').val();
    var time = $('#appointmentTime').val();

    var datetime = date + ' ' + time;
    var id = $(this).data('appointmentid');

    $.ajax({
        url: '/Doctor/ConfirmedAppt/ChangeAppointmentDate',
        method: 'POST',
        data: {
            datetime: datetime,
            id: id
        },
        success: function (res) {
            if (res.success) {
                Swal.fire({
                    title: 'Updated!',
                    text: res.message,
                    icon: 'success',
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2000
                })
            } else {
                Swal.fire({
                    title: 'Failed!',
                    text: res.message,
                    icon: 'warning',
                    position: 'top',
                    showConfirmButton: false,
                    timer: 3000
                })
            }
        },
        error: function (err) {
            console.log(err.responseText);
        }
    })
})

$(document).on('click', '#btnCancel', function () {
    var btnCancel = $(this);
    var appointmentID = btnCancel.data('appointmentid');

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
        position: 'top',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, cancel it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Doctor/Appointments/CancelAppointment',
                method: 'POST',
                data: { id: appointmentID },
                dataType: 'JSON',
                success: function (res) {
                    if (res.success) {
                        Swal.fire({
                            title: 'Cancelled!',
                            text: 'Appointment has been cancelled.',
                            icon: 'success',
                            position: 'top',
                            showConfirmButton: false,
                            timer: 3000
                        })
                        setTimeout(function () {
                            window.location.href = '/Doctor/CancelledAppt/Index';
                        }, 2000)
                    } else {
                        console.log(res.message)
                    }

                },
                error: function (err) {
                    console.log(err.responseText);
                }
            })
        }
    })
})



$(document).ready(function () {
    var appointmentID = $('#btnCompleted').data('appointmentid');
    var table = $('#mediacationTbl').DataTable({
        "autoWidth": false,
        "responsive": true,
        "ordering": false,
        "info": true,
        "searching": false,
        "paging": false,
        "columns": [
            {
                "title": "Drug",
                "data": "drug",
                "width": "15%"
            },
            {
                "responsivePriority": 1,
                "title": "Frequency",
                "data": "frequency",
                "className": "frequency",
                "width": "18%"
            },
            {
                "title": "Medication Days",
                "data": "medicationDays",
                "width": "5%"
            },
            {
                "title": "Quantity",
                "data": "quantity",
                "width": "10%"
            },
            {
                "responsivePriority": 1,
                "title": "Unit",
                "data": "unit",
                "className": "unit",
                "width": "15%"
            },
            {
                "title": "Note",
                "data": "note",
                "width": "25%"
            },
            {
                "responsivePriority": 1,
                "title": "Action",
                "data": null,
                "orderable": false,
                "searchable": false,
                "width": "5%",
                "render": function (data, type, row) {
                    return '<button class="btn btn-danger btn-sm btnDelete">Delete</button>';
                }
            }
        ]
    });

    var freqSelect = $('<select>').addClass('custom-select');
    freqSelect.attr('name', 'frequency');

    var unitSelect = $('<select>').addClass('custom-select');
    unitSelect.attr('name', 'unit');
    $('#medication-tab').on('click', function () {
        $.ajax({
            url: '/Doctor/ConfirmedAppt/GetFreqAndUnit',
            method: 'GET',
            dataType: 'JSON',
            success: function (res) {
                var freq = res.frequency;
                var unit = res.unit;
                freqSelect.empty();
                unitSelect.empty();
                for (var item of freq) {
                    var option = $('<option>', {
                        value: item.ID,
                        text: item.PARAVAL
                    });
                    freqSelect.append(option);
                }

                for (var item of unit) {
                    var option = $('<option>', {
                        value: item.ID,
                        text: item.PARAVAL
                    });
                    unitSelect.append(option);
                }
            },
            error: function (err) {
                console.log(err.responseText);
            }
        });
    })

    $('#btnAddRow').on('click', function (event) {
        event.preventDefault();

        var numOrder = table.rows().count() + 1;
        var object = {
            no: '<span class="numOrder">' + numOrder + '.</span>',
            drug: '<input required autocomplete="off" type="text" name="drug" class="drug-input"><div class="error-container"></div>',
            frequency: '',
            unit: '',
            medicationDays: '<input required autocomplete="off" type="text" onkeypress="return validateNumber(event)" name="medicationDays" class="medicationDays-input"><div class="error-container"></div>',
            quantity: '<input required autocomplete="off" type="text" onkeypress="return validateNumber(event)" name="quantity" class="quantity-input"><div class="error-container"></div>',
            note: '<input required autocomplete="off" type="text" name="note" class="note-input"><div class="error-container"></div>',
        }
        $('.error-container').empty();
        table.row.add(object).draw(false);

        var frequency = $('.frequency').last();
        freqSelect.clone().appendTo(frequency);
        
        var unit = $('.unit').last();
        unitSelect.clone().appendTo(unit);
    });


    $('#mediacationTbl tbody').on('click', '.btnDelete', function () {
        var row = table.row($(this).closest('tr'));
        row.remove().draw(false);
    });

    function markAsCompleted() {
        var data = [];
        var isValid = true;

        $('#medication input, #medication select').each(function () {
            if (!$(this).valid()) {
                isValid = false;
            }
        });

        if (isValid) {
            $('.error-container').empty();
            if (table.rows().count() == 0) {
                Swal.fire({
                    position: 'top',
                    title: 'Failed!',
                    icon: 'warning',
                    text: 'Please add prescription before pressing Save.',
                    showConfirmButton: false,
                    timer: 3000
                });
            } else {
                $('#mediacationTbl tbody tr').each(function () {
                    var rowData = [];
                    var cells = $(this).find('input');
                    var numberOrder = $(this).find('span').first().text();
                    var frequency = $(this).find('select').first().val();
                    var unit = $(this).find('select').last().val();
                    var rowData = {
                        "appointmentID": appointmentID,
                        "no": numberOrder,
                        "drug": cells.eq(0).val(),
                        "note": cells.eq(1).val(),
                        "frequency": frequency,
                        "unit": unit,
                        "medicationDays": cells.eq(2).val(),
                        "quantity": cells.eq(3).val()
                    };
                    data.push(rowData);
                });
            }

            if (data.length != 0) {
                var notes = {
                    appointmentID: appointmentID,
                    diagnosis: $('#diagnosis').val(),
                    caseNote: $('#caseNote').val(),
                    adviceToPatient: $('#adviceToPatient').val()
                }
                $.ajax({
                    url: '/Doctor/ConfirmedAppt/MarkAsCompleted',
                    method: 'POST',
                    data: {
                        id: appointmentID,
                        data: data,
                        notes: notes
                    },
                    dataType: 'JSON',
                    success: function (res) {
                        if (res.success) {
                            Swal.fire({
                                position: 'top',
                                icon: 'success',
                                title: 'Saved!',
                                text: 'Saved medication successfully.',
                                showConfirmButton: false,
                                timer: 3000
                            });
                        } else {
                            console.log('failed.');
                        }
                    },
                    error: function (err) {
                        console.log(err.responseText);
                    }
                });
            }
        }
    }



    $('#btnResetMedication').on('click', function () {
        table.clear().draw();
    });

    $('#btnResetNotes').on('click', function () {
        $('#diagnosis').val('');
        $('#adviceToPatient').val('');
        $('#caseNote').val('');
    });


    $('#btnCompleted').on('click', function (event) {
        event.preventDefault();
        var isValid = true;

        $('#medication input, #medication select').each(function () {
            if (!$(this).valid()) {
                isValid = false;
            }
        });

        // Change status of the appointment to 'Completed'
        if (isValid && isEmptyDoctorNotes(table)) {
            markAsCompleted();
            $('.error-container').empty();
            console.log("completed.");
        } else {
            Swal.fire({
                position: 'top',
                title: 'Failed!',
                icon: 'warning',
                text: 'Please complete all required information in Notes and Medication.',
                timer: 3000,
                showConfirmButton: false
            })
        }
    });

});


function isEmptyDoctorNotes(table) {
    var diagnosis = $('#diagnosis').val();
    var adviceToPatient = $('#adviceToPatient').val();
    var rows = table.rows().count();

    if (diagnosis && adviceToPatient && rows) {
        return true;
    }
    return false;
}


var validData = function () {
    $('#medication').validate({
        onfocusout: function (element) {
            $(element).valid()
        },
        rules: {
            "drug-input": {
                required: true
            },
            "frequency-select": {
                required: true
            },
            "medicationDays-input": {
                required: true
            },
            "quantity-input": {
                required: true
            },
            "note-input": {
                required: true
            }
        },
        messages: {
            "drug-input": {
                required: 'This value is required.'
            },
            "frequency-select": {
                required: 'This value is required.'
            },
            "medicationDays-input": {
                required: 'This value is required.'
            },
            "quantity-input": {
                required: 'This value is required.'
            },
            "note-input": {
                required: 'This value is required.',
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
            var container = $(element).parent().find('.error-container');
            var errorClone = error.clone();
            container.empty();
            container.append(errorClone);
        },
    })
}


$(document).ready(function () {
    validData();


})

function responsiveButton() {
    if (window.innerWidth <= 550) {
        document.getElementById("btnChangeDate").textContent = "Change Date";
        document.getElementById("btnCancel").textContent = "Cancel";
    } else {
        document.getElementById("btnChangeDate").textContent = "Change Appointment Date";
        document.getElementById("btnCancel").textContent = "Cancel Appointment";
    }
}

window.addEventListener("resize", responsiveButton);
window.addEventListener("load", responsiveButton);
